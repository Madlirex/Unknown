using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class MainMenuManager : MonoBehaviour
{
    public MainMenuActions InputControls;
    
    public GameObject activeMenu;

    [SerializeField] private GameObject introMenu;
    [SerializeField] private GameObject mainMenu;

    public void Awake()
    {
        InputControls = new MainMenuActions();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnEnable()
    {
        InputControls.Enable();
    }

    public void OnDisable()
    {
        InputControls.Disable();
    }
    
    public void SetMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    public void StartMenu(TypingSequence source)
    {
        StartCoroutine(ExitStartMenu(source));
    }
    
    public IEnumerator ExitStartMenu(TypingSequence source)
    {
        source.DisableTyping();
        yield return source.DelayDone();
        
        introMenu.SetActive(false);
        mainMenu.SetActive(true);
        
        ThemeManager.Instance.ToggleDarkMode(!ThemeManager.Instance.IsDarkMode);
    }

    public void SwitchLocale(string localeCode)
    {
        StartCoroutine(SwitchLocaleCoroutine(localeCode));
    }
    
    public static IEnumerator SwitchLocaleCoroutine(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        Locale targetLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        
        if(targetLocale)
        {
            LocalizationSettings.SelectedLocale = targetLocale;
        }
        else
        {
            Debug.LogWarning($"Locale code {localeCode} was not found in project settings.");
        }
    }
}

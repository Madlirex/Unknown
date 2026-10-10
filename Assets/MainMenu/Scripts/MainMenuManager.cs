using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class MainMenuManager : MonoBehaviour
{
    public MainMenuActions InputControls;
    
    public GameObject activeMenu;

    public void Awake()
    {
        InputControls = new MainMenuActions();
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

    public static IEnumerator SwitchLocale(string localeCode)
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

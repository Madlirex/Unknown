using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UI;

public class IntroTypingSequence : MonoBehaviour
{
    [SerializeField] private GameObject introMenu;
    [SerializeField] private GameObject mainMenu;
    
    [SerializeField] private Image panelImage;
    [SerializeField] private TMP_Text typingText;

    [SerializeField] private LocalizedString targetString;
    private string _targetString;
    private string _currentString = "";

    [SerializeField] private float delay = 5f;
    public void Awake()
    {
        _targetString = typingText.text.ToLower();
    }

    public void OnEnable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += ProgressStartMenu;
        }
        
        targetString.StringChanged += ChangeTargetString;
    }

    public void OnDisable()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= ProgressStartMenu;
        }
        
        targetString.StringChanged -= ChangeTargetString;
    }

    private void ChangeTargetString(string value)
    {
        _targetString = value.ToLower();
        ProgressStartMenu('}');
    }
    
    public void SwitchToEnglish()
    {
        StartCoroutine(MainMenuManager.SwitchLocale("en"));
    }
    
    public void ProgressStartMenu(char key)
    {
        if (_currentString.Length >= _targetString.Length) return;
        
        key = char.ToLower(key);
        
        char expected = _targetString[_currentString.Length];
        char? secondExpected = _currentString.Length < _targetString.Length - 1
            ? _targetString[_currentString.Length + 1]
            : null;

        if (expected == key)
        {
            _currentString += expected;
        }
        else if (expected == ' ' && secondExpected == key)
        {
            _currentString += expected;
            _currentString += secondExpected;
        }
        else
        {
            _currentString = "";
        }
        
        string hexColor = ColorUtility.ToHtmlStringRGB(ThemeManager.Instance.ActiveTheme.typingText);
        
        if (_currentString.Length == _targetString.Length)
        {
            typingText.text = $"<color=#{hexColor}>{_currentString}</color>";
            StartCoroutine(ExitStartMenu());
        }
        else
        {
            string remainingString = _targetString[_currentString.Length..];
            typingText.text = $"<color=#{hexColor}>{_currentString}</color>{remainingString}";
        }
    }

    public IEnumerator ExitStartMenu()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= ProgressStartMenu;
        }

        targetString.StringChanged -= ChangeTargetString;
        
        typingText.color = ThemeManager.Instance.ActiveTheme.correctText;
        typingText.text = _targetString;

        yield return new WaitForSecondsRealtime(delay);
        
        introMenu.SetActive(false);
        mainMenu.SetActive(true);
        
        ThemeManager.Instance.ToggleDarkMode(!ThemeManager.Instance.IsDarkMode);
    }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    public MainMenuActions InputControls;

    public GameObject startMenu;
    public GameObject mainMenu;
    public GameObject activeMenu;

    [SerializeField] private Image panelImage;
    [SerializeField] private TMP_Text startText;
    
    private string _targetString;
    private string _currentString = "";

    [SerializeField] private float delay = 5f;
    
    [Header("Colors")] 
    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private Color correctColor = Color.red;
    [SerializeField] private Color endColor = Color.green;
    [SerializeField] private Color darkColor = Color.black;

    public void Awake()
    {
        InputControls = new MainMenuActions();

        _targetString = startText.text.ToLower();
    }

    public void OnEnable()
    {
        InputControls.Enable();

        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += ProgressStartMenu;
        }
    }

    public void OnDisable()
    {
        InputControls.Disable();

        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= ProgressStartMenu;
        }
    }

    public void SetTargetString(string value)
    {
        _targetString = value;
    }
    
    public void SetMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    public void ProgressStartMenu(char key)
    {
        if (_currentString.Length >= _targetString.Length) return;

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
        
        string hexColor = ColorUtility.ToHtmlStringRGB(correctColor);
        
        if (_currentString.Length == _targetString.Length)
        {
            startText.text = $"<color=#{hexColor}>{_currentString}</color>";
            StartCoroutine(ExitStartMenu());
        }
        else
        {
            string remainingString = _targetString[_currentString.Length..];
            startText.text = $"<color=#{hexColor}>{_currentString}</color>{remainingString}";
        }
    }

    public IEnumerator ExitStartMenu()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= ProgressStartMenu;
        }

        startText.color = endColor;
        startText.text = _targetString;

        yield return new WaitForSecondsRealtime(delay);
        
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        panelImage.color = lightColor;
    }
}

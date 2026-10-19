using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Events;

public class TypingSequence : MonoBehaviour
{
    [SerializeField] private TMP_Text typingText;

    [SerializeField] private LocalizedString targetString;
    private string _targetString;
    private string _currentString = "";

    [SerializeField] private float doneDelay = 2f;

    [Header("Color Roles")]
    [SerializeField] private ColorRole baseRole = ColorRole.Text;
    [SerializeField] private ColorRole typingRole = ColorRole.TypingText;
    [SerializeField] private ColorRole doneRole = ColorRole.CorrectText;
    
    [SerializeField] private UnityEvent<TypingSequence> doneEvent;
    
    public void Awake()
    {
        _targetString = typingText.text.ToLower();
    }

    public void OnEnable()
    {
        EnableTyping();
    }

    public void OnDisable()
    {
        DisableTyping();
    }

    public void EnableTyping()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput += ProgressStartMenu;
        }
        
        targetString.StringChanged += ChangeTargetString;
    }

    public void DisableTyping()
    {
        if (Keyboard.current != null)
        {
            Keyboard.current.onTextInput -= ProgressStartMenu;
        }
        
        targetString.StringChanged -= ChangeTargetString;
    }

    public void ResetText()
    {
        typingText.color = ThemeManager.GetColor(baseRole);
        typingText.text = _targetString;
        _currentString = "";
    }
    
    public void StartDelayDone()
    {
        StartCoroutine(DelayDone());
    }
    
    public void StartDelayDone(float time)
    {
        StartCoroutine(DelayDone(time));
    }

    public IEnumerator DelayDone()
    {
        typingText.color = ThemeManager.GetColor(doneRole);
        typingText.text = _targetString;

        yield return new WaitForSeconds(doneDelay);
        ResetText();
    }
    
    public IEnumerator DelayDone(float time)
    {
        typingText.color = ThemeManager.GetColor(doneRole);
        typingText.text = _targetString;

        yield return new WaitForSeconds(time);
        ResetText();
    }
    
    private void ChangeTargetString(string value)
    {
        _targetString = value.ToLower();
        ProgressStartMenu('\0');
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
        
        string hexColor = ColorUtility.ToHtmlStringRGB(ThemeManager.GetColor(typingRole));
        
        if (_currentString.Length == _targetString.Length)
        {
            typingText.text = $"<color=#{hexColor}>{_currentString}</color>";
            doneEvent.Invoke(this);
        }
        else
        {
            string remainingString = _targetString[_currentString.Length..];
            typingText.text = $"<color=#{hexColor}>{_currentString}</color>{remainingString}";
        }
    }
}

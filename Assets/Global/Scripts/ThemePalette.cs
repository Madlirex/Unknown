using UnityEngine;

public enum ColorRole
{
    Background,
    Title,
    Text,
    TypingText,
    CorrectText
}

[CreateAssetMenu(fileName = "Theme Palette", menuName = "Colors/Theme Palette")]
public class ThemePalette : ScriptableObject
{
    [Header("Core Colors")] 
    public Color background = Color.white;
    public Color title = Color.black;
    
    [Header("Text Colors")]
    public Color text = Color.black;
    public Color typingText = Color.red;
    public Color correctText = Color.green;
}

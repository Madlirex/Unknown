using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    [Header("Themes")]
    public ThemePalette lightTheme;
    public ThemePalette darkTheme;
    
    public ThemePalette ActiveTheme { get; private set; }
    
    public System.Action OnThemeChanged;

    public bool IsDarkMode => darkTheme == ActiveTheme;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (ActiveTheme == null && darkTheme != null)
        {
            ActiveTheme = darkTheme;
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeThemeSystem()
    {
        if (Instance != null) return;
        
        ThemeManager prefab = Resources.Load<ThemeManager>("Managers/ThemeManager");

        if (prefab != null)
        {
            ThemeManager instance = Instantiate(prefab);
            instance.name = "ThemeManager";
        }
        else
        {
            Debug.LogError("Failed to initialize Theme System! Ensure a 'ThemeManager' prefab is placed in a 'Resources' folder.");
        }
    }


    public void ToggleDarkMode(bool useDark)
    {
        ActiveTheme = useDark ? darkTheme : lightTheme;
        OnThemeChanged?.Invoke();
    }

    public static Color GetColor(ColorRole role)
    {
        if (!Instance) return Color.white;
        
        return role switch
        {
            ColorRole.Background => Instance.ActiveTheme.background,
            ColorRole.Text => Instance.ActiveTheme.text,
            ColorRole.Title => Instance.ActiveTheme.title,
            ColorRole.CorrectText => Instance.ActiveTheme.correctText,
            ColorRole.TypingText => Instance.ActiveTheme.typingText,
            _ => Instance.ActiveTheme.text
        };
    }
}
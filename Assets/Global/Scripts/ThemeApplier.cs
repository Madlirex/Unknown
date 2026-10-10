using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ColorTargetGroup
{
    [Tooltip("The color role this group should fetch from the active theme.")]
    public ColorRole role;

    [Header("Target Components")]
    public List<Image> targetImages = new();
    public List<TextMeshProUGUI> targetTexts = new();
}

public class ThemeApplier : MonoBehaviour
{
    [Header("Theme Binding Configuration")]
    [SerializeField] private List<ColorTargetGroup> colorBindings = new();

    private void OnEnable()
    {
        if(ThemeManager.Instance != null)
            ThemeManager.Instance.OnThemeChanged += RefreshAllVisuals;
        
        RefreshAllVisuals();
    }

    private void OnDisable()
    {
        if(ThemeManager.Instance != null)
            ThemeManager.Instance.OnThemeChanged -= RefreshAllVisuals;
    }

    public void RefreshAllVisuals()
    {
        if (ThemeManager.Instance.ActiveTheme == null) return;
        
        foreach (var group in colorBindings)
        {
            Color targetColor = ThemeManager.GetColor(group.role);
            
            foreach (var img in group.targetImages.Where(img => img != null))
            {
                img.color = targetColor;
            }

            foreach (var txt in group.targetTexts.Where(txt => txt != null))
            {
                txt.color = targetColor;
            }
        }
    }
}

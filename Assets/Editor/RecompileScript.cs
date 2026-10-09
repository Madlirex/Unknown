using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class ManualCompileShortcut
    {
        // ReSharper disable once Unity.DuplicateShortcut
        [MenuItem("Tools/Compile & Reload Domain &c")] // Alt + C
        public static void Compile()
        {
            AssetDatabase.Refresh();
        }
    }

    public class ManualCompileWindow : EditorWindow
    {
        [MenuItem("Tools/Manual Compile Window")]
        public static void Open()
        {
            var window = GetWindow<ManualCompileWindow>();
            window.titleContent = new GUIContent("Compile");
            window.minSize = new Vector2(220, 70);
            window.maxSize = window.minSize;
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            if (GUILayout.Button("Compile Scripts (Alt + C)", GUILayout.Height(40)))
            {
                AssetDatabase.Refresh();
            }
        }
    }
}
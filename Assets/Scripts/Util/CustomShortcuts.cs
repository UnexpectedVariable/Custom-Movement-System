using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class CustomShortcuts
    {
        // Alt + C
        [Shortcut("Clear Console", KeyCode.C, ShortcutModifiers.Alt)]
        public static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}

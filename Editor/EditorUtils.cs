using UnityEngine;
using UnityEditor;

public static class EditorUtils
{
    public static void DrawHeader(string title, SerializedProperty activeField)
    {
        var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);
        
        var labelRect = backgroundRect;
        labelRect.xMin += 16f;
        labelRect.xMax -= 20f;

        var foldoutRect = backgroundRect;
        foldoutRect.y += 1f;
        foldoutRect.width = 13f;
        foldoutRect.height = 13f;

        backgroundRect.xMin = 0f;
        backgroundRect.width += 4f;

        float backgroundTint = EditorGUIUtility.isProSkin ? 0.1f : 1f;
        EditorGUI.DrawRect(backgroundRect, new Color(backgroundTint, backgroundTint, backgroundTint, 0.2f));
        EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);
        activeField.boolValue = GUI.Toggle(foldoutRect, activeField.boolValue, GUIContent.none, new GUIStyle("ShurikenCheckMark"));
    }

    public static void DrawHeader(string title)
    {
        var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);
        
        var labelRect = backgroundRect;
        labelRect.xMin += 16f;
        labelRect.xMax -= 20f;

        var foldoutRect = backgroundRect;
        foldoutRect.y += 1f;
        foldoutRect.width = 13f;
        foldoutRect.height = 13f;

        backgroundRect.xMin = 0f;
        backgroundRect.width += 4f;

        float backgroundTint = EditorGUIUtility.isProSkin ? 0.1f : 1f;
        EditorGUI.DrawRect(backgroundRect, new Color(backgroundTint, backgroundTint, backgroundTint, 0.2f));
        EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);
    }
}
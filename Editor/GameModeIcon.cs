using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class GameModeIcon
{
    private static readonly Texture2D icon;

    static GameModeIcon()
    {
        icon = AssetDatabase.LoadAssetAtPath("Assets/Gizmos/GameMode Icon.png", typeof(Texture2D)) as Texture2D;
        EditorApplication.hierarchyWindowItemOnGUI += DrawIconOnWindowItem;
    }

    private static void DrawIconOnWindowItem(int instanceID, Rect rect)
    {
        if (icon == null)
        {
            return;
        }
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject == null)
        {
            return;
        }
        GameMode gameMode = gameObject.GetComponent<GameMode>();
        if (gameMode == null)
        {
            return;
        }
        Color defaultColor = GUI.color;
        GUI.color = gameMode.IsValide ? Color.white : Color.red;
        DrawIcon(rect);
        GUI.color = defaultColor;
    }

    private static void DrawIcon(Rect rect)
    {
        float iconWidth = 15;
        Vector2 padding = new Vector2(5, 0);
        EditorGUIUtility.SetIconSize(new Vector2(iconWidth, iconWidth));
        var iconDrawRect = new Rect(
                                   rect.xMax - (iconWidth + padding.x), 
                                   rect.yMin, 
                                   rect.width, 
                                   rect.height);
        var iconGUIContent = new GUIContent(icon);
        EditorGUI.LabelField(iconDrawRect, iconGUIContent);
        EditorGUIUtility.SetIconSize(Vector2.zero);
    }
}
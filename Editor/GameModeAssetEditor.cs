using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameModeAsset))]
public class GameModeAssetEditor : Editor
{
    private SerializedProperty _prefabPlayer;
    private SerializedProperty _spawnPlayer;
    private SerializedProperty _fadeIn;
    private SerializedProperty _fadeOut;

    private void OnEnable() 
    {
        _prefabPlayer = serializedObject.FindProperty("player");
        _spawnPlayer = serializedObject.FindProperty("spawnPlayerOnSceneLoad");
        _fadeIn = serializedObject.FindProperty("fadeIn");
        _fadeOut = serializedObject.FindProperty("fadeOut");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorUtils.DrawHeader("Player");
        EditorGUILayout.PropertyField(_prefabPlayer, new GUIContent ("Prefab", "Prefab of player. Don't forget player tag or mainCamera tag."));
        EditorGUILayout.PropertyField(_spawnPlayer, new GUIContent ("Spawn on start", "Instantiate player prefab on start"));
        FadeGroup(_fadeIn, "Fade in");
        FadeGroup(_fadeOut, "Fade out");
        serializedObject.ApplyModifiedProperties();
    }

    public static void FadeGroup(SerializedProperty serializedProperty, string title)
    {
        SerializedProperty enable = serializedProperty.FindPropertyRelative("enable");
        SerializedProperty time = serializedProperty.FindPropertyRelative("time");
        SerializedProperty sound = serializedProperty.FindPropertyRelative("globalSound");
        SerializedProperty screen = serializedProperty.FindPropertyRelative("globalScreen");
        EditorUtils.DrawHeader(title, enable);
        bool oldEnableGUI = GUI.enabled;
        GUI.enabled = enable.boolValue;
        EditorGUILayout.PropertyField(time, new GUIContent("Time"));
        EditorGUILayout.PropertyField(sound, new GUIContent("Sound"));
        EditorGUILayout.PropertyField(screen, new GUIContent("Screen"));
        GUI.enabled = oldEnableGUI;
    }
}
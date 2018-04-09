using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameMode))]
public class GameModeEditor : Editor 
{
    SerializedProperty gameModeAsset;
    SerializedProperty fadeOnLoadScene;
    SerializedProperty fadeOnExitScene;
    SerializedProperty fadeIn;
    SerializedProperty fadeOut;


    private void OnEnable() 
    {
        gameModeAsset = serializedObject.FindProperty("_gameMode");
        fadeOnLoadScene = serializedObject.FindProperty("fadeOnLoadScene");
        fadeOnExitScene = serializedObject.FindProperty("fadeOnExitScene");

        fadeIn = serializedObject.FindProperty("fadeIn");
        fadeOut = serializedObject.FindProperty("fadeOut");
        
    }

    private void FadeOptionsGui(SerializedProperty serializedProperty)
    {
        SerializedProperty camera = serializedProperty.FindPropertyRelative("camera");
        SerializedProperty sound = serializedProperty.FindPropertyRelative("sound");
        SerializedProperty delay = serializedProperty.FindPropertyRelative("delay");
        SerializedProperty time = serializedProperty.FindPropertyRelative("time");
        SerializedProperty color = serializedProperty.FindPropertyRelative("color");

        GUILayout.BeginVertical("Box");
        EditorGUILayout.PropertyField (delay, new GUIContent("Delay"));
        EditorGUILayout.PropertyField (time, new GUIContent("Time"));
        EditorGUILayout.PropertyField (sound, new GUIContent("Sound"));
        EditorGUILayout.PropertyField (camera, new GUIContent("Camera"));
        if (camera.boolValue == true)
        {
            EditorGUILayout.PropertyField (color, new GUIContent("Fade camera color"));
        }
        GUILayout.EndVertical();
    }

    public override void OnInspectorGUI() 
    {
        EditorGUILayout.PropertyField (gameModeAsset, new GUIContent ("Game mode profil"));
        if (gameModeAsset.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Game mode profil is require", MessageType.Error);
        }
        else
        {
            GameModeAsset gameMode = gameModeAsset.objectReferenceValue as GameModeAsset;
            if (gameMode.player == null)
            {
                EditorGUILayout.HelpBox("Player prefab is require", MessageType.Error);
            }
            else
            {
                if (GameMode.FindMainCameraTag(gameMode.player) == null && GameMode.FindPlayerTag(gameMode.player) == null)
                {
                    EditorGUILayout.HelpBox("Player tag not found in player prefab", MessageType.Warning);
                }
            }
        }
        GUILayout.Space(10);
        EditorGUILayout.PropertyField (fadeOnLoadScene, new GUIContent ("Fade-in when scene load"));
        if (fadeOnLoadScene.boolValue == true)
        {
            FadeOptionsGui(fadeIn);
        }
        GUILayout.Space(10);
        EditorGUILayout.PropertyField (fadeOnExitScene, new GUIContent ("Fade-out when scene unload"));
        if (fadeOnExitScene.boolValue == true)
        {
            FadeOptionsGui(fadeOut);
        }
        GUILayout.Space(10);
        
        serializedObject.ApplyModifiedProperties();
    }
}

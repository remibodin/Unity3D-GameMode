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

        EditorGUILayout.PropertyField (delay, new GUIContent("Delay"));
        EditorGUILayout.PropertyField (time, new GUIContent("Time"));
        EditorGUILayout.PropertyField (sound, new GUIContent("Sound"));
        EditorGUILayout.PropertyField (camera, new GUIContent("Camera"));
        if (camera.boolValue == true)
        {
            EditorGUILayout.PropertyField (color, new GUIContent("Fade camera color"));
        }
    }

    

    bool _fadeInEditor = false;
    bool _fadeOutEditor = false;

    public override void OnInspectorGUI() 
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField (gameModeAsset, new GUIContent ("Game mode profil"));
        // if (gameModeAsset.objectReferenceValue == null)
        // {
        //     EditorGUILayout.HelpBox("Game mode profil is require", MessageType.Error);
        // }
        // else
        // {
        //     GameModeAsset gameMode = gameModeAsset.objectReferenceValue as GameModeAsset;
        //     if (gameMode.player == null)
        //     {
        //         EditorGUILayout.HelpBox("Player prefab is require", MessageType.Error);
        //     }
        //     else
        //     {
        //         if (GameMode.FindMainCameraTag(gameMode.player) == null && GameMode.FindPlayerTag(gameMode.player) == null)
        //         {
        //             EditorGUILayout.HelpBox("Player tag not found in player prefab", MessageType.Warning);
        //         }
        //     }
        // }
        // GUILayout.Space(10);
        
        // _fadeInEditor = DrawHeader("Fade in", fadeOnLoadScene, _fadeInEditor);
        // if (_fadeInEditor)
        // {
        //     bool oldEnableGUI = GUI.enabled;
        //     GUI.enabled = fadeOnLoadScene.boolValue;
        //     FadeOptionsGui(fadeIn);
        //     GUI.enabled = oldEnableGUI;
        // }
        // _fadeOutEditor = DrawHeader("Fade out", fadeOnExitScene, _fadeOutEditor);
        // if (_fadeOutEditor)
        // {
        //     bool oldEnableGUI = GUI.enabled;
        //     GUI.enabled = fadeOnExitScene.boolValue;
        //     FadeOptionsGui(fadeOut);
        //     GUI.enabled = oldEnableGUI;
        // }        
        serializedObject.ApplyModifiedProperties();
    }
}

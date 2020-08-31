using UnityEngine;
using UnityEditor;

namespace Nrtx
{
    [CustomEditor(typeof(GameMode))]
    public class GameModeEditor : Editor 
    {
        SerializedProperty gameModeAsset;

        private void OnEnable() 
        {
            gameModeAsset = serializedObject.FindProperty("_gameMode");
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(gameModeAsset, new GUIContent ("Game mode profil"));     
            serializedObject.ApplyModifiedProperties();
        }
    }
}

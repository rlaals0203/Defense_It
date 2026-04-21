/*using UnityEditor;
using UnityEngine;

namespace _01_Script._05_GameSystem.Wave.Editor
{
    [CustomEditor(typeof(WaveSO))]
    public class WaveEditor : UnityEditor.Editor
    {
        SerializedProperty enemiesProp;

        private void OnEnable()
        {
            enemiesProp = serializedObject.FindProperty("enemies");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("웨이브 패턴", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            for (int i = 0; i < enemiesProp.arraySize; i++)
            {
                SerializedProperty element = enemiesProp.GetArrayElementAtIndex(i);
                SerializedProperty enemySO = element.FindPropertyRelative("enemySO");

                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.PropertyField(enemySO);
                EditorGUILayout.PropertyField(element.FindPropertyRelative("startDelay"));
                EditorGUILayout.PropertyField(element.FindPropertyRelative("spawnDelay"));
                EditorGUILayout.PropertyField(element.FindPropertyRelative("count"));
                EditorGUILayout.PropertyField(element.FindPropertyRelative("isRepeat"));
                EditorGUILayout.PropertyField(element.FindPropertyRelative("isAppend"));
                EditorGUILayout.PropertyField(element.FindPropertyRelative("isJoin"));

                if (GUILayout.Button("Remove Enemy"))
                {
                    enemiesProp.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add New Enemy Pattern"))
            {
                enemiesProp.InsertArrayElementAtIndex(enemiesProp.arraySize);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}*/
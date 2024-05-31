using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(AudioManager.Tag))]
public class AudiTagDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.serializedObject.Update();

        SerializedProperty nameProperty = property.FindPropertyRelative("_name");
        SerializedProperty activeProperty = property.FindPropertyRelative("IsActive");
        SerializedProperty volumeProperty = property.FindPropertyRelative("Volume");
        SerializedProperty pitchProperty = property.FindPropertyRelative("Pitch");

        if (activeProperty.boolValue)
            GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        activeProperty.boolValue = EditorGUILayout.Toggle(activeProperty.boolValue, GUILayout.MaxWidth(20));
        EditorGUI.BeginDisabledGroup(true);

        if (EditorGUIUtility.currentViewWidth > 300)
            GUILayout.Label("Name Tag -> ", GUILayout.MaxWidth(80));

        nameProperty.stringValue = EditorGUILayout.TextField(nameProperty.stringValue);
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (activeProperty.boolValue)
        {
            if (EditorGUIUtility.currentViewWidth > 300)
            {
                EditorGUILayout.PropertyField(volumeProperty);
                EditorGUILayout.PropertyField(pitchProperty);
            }
            else if (EditorGUIUtility.currentViewWidth > 150)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("V", GUILayout.MaxWidth(20));
                EditorGUILayout.PropertyField(volumeProperty, new GUIContent(""));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("P", GUILayout.MaxWidth(20));
                EditorGUILayout.PropertyField(pitchProperty, new GUIContent(""));
                GUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.PropertyField(volumeProperty, new GUIContent(""));
                EditorGUILayout.PropertyField(pitchProperty, new GUIContent(""));
            }

            GUILayout.EndVertical();
        }

        property.serializedObject.ApplyModifiedProperties();
    }
}

#endif
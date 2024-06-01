using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ScriptableEvent))]
public class ScriptableEventDrawer : PropertyDrawer
{
    const uint widthButton = 90;

    ScriptableEvent script;
    bool toggleGroup = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        script = (ScriptableEvent)property.objectReferenceValue;

        Rect propertyRect = new Rect(position.x, position.y, position.width - widthButton, position.height);

        EditorGUI.BeginProperty(position, label, property);

        if (property.objectReferenceValue != null && property.objectReferenceValue is ScriptableEvent)
        {
            // Draw default property field (Object field)
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }
        else
        {
            // Draw default property field (Object field)
            EditorGUI.PropertyField(position, property, label, true);
        }

        // Check if the property is a ScriptableEvent
        if (property.objectReferenceValue != null && property.objectReferenceValue is ScriptableEvent)
        {
            string[] methodsNames = new string[0];

            if (script == null) { return; }

            bool getNames = script.TryGetEventMethodsNames(out methodsNames);

            // Draw a button to launch the event
            Rect buttonRect = new Rect(position.x + position.width - widthButton, position.y, widthButton, position.height);
            if (GUI.Button(buttonRect, (getNames) ? "Launch Event" : "Event Empty"))
            {
                // Launch the event
                script.LaunchEvent();
                Debug.Log(property.objectReferenceValue.name + " -> LaunchEvent manually");
            }

            // ---- DISPLAY SET METHODS ---- //

            if (getNames)
            {
                GUILayout.BeginVertical(GUI.skin.box);

                // Display the names of methods in the event
                toggleGroup = EditorGUILayout.BeginFoldoutHeaderGroup(toggleGroup, "Methods in Event");

                if (toggleGroup)
                {
                    bool alt = false;
                    foreach (var name in methodsNames)
                    {
                        GUI.backgroundColor = (alt) ? (Color.white * 1.5f) : Color.white;
                        GUILayout.BeginHorizontal(GUI.skin.box);
                        EditorGUILayout.LabelField(name);
                        GUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        alt = !alt;
                    }
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                GUILayout.EndVertical();
            }

        }

        EditorGUI.EndProperty();
    }

}
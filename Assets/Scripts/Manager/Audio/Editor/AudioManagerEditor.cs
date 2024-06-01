using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    AudioManager script;

    List<string> pathSounds = new List<string>();
    int selectedPath = 0;

    string nameChangedTag;
    string nameTag;

    int indexPageSounds = 0;
    int indexPageTags = 0;

    private Vector2 scrollPosition;
    private List<string> myList = new List<string>();  // List of elements

    private int itemsPerPage = 10;  // Number of page display on screen
    private int currentPage = 0;

    private SerializedProperty serializedSounds;
    private SerializedProperty serializedTags;

    private void OnEnable()
    {
        if (script == null)
        {
            script = (AudioManager)target;
        }

        Dictionary<string, uint> keys = new Dictionary<string, uint>();

        foreach (var audio in AssetDatabase.FindAssets("t:AudioClip"))
        {
            string[] directory = audio.Split('/');
            string path = "";

            for (int i = 0; i < directory.Length - 1; i++)
            {
                path += directory[i];
            }

            keys = IncrementDictionary(keys, path);

            foreach (var item in keys)
            {
                pathSounds.Add(item.Key);
            }

        }

        serializedSounds = serializedObject.FindProperty("_sounds");
        serializedTags = serializedObject.FindProperty("_tags");

    }

    private void OnValidate()
    {
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        serializedSounds = serializedObject.FindProperty("_sounds");
        serializedTags = serializedObject.FindProperty("_tags");

        serializedObject.Update();

        GUILayout.BeginVertical();

        if (script == null)
        {
            script = (AudioManager)target;
        }
        else
        {

            if (GUILayout.Button("Add all audio of folders"))
            {
                Debug.Log("Add all assets audio of project");
                foreach (var audio in AssetDatabase.FindAssets("t:AudioClip"))
                {
                    string path = AssetDatabase.GUIDToAssetPath(audio);
                    Debug.Log(path + "-" + audio);
                    script.AddSound(new Sound(AssetDatabase.LoadAssetAtPath<AudioClip>(path)));

                }
                AssetDatabase.SaveAssets();
            }

            GUILayout.Space(20);

            nameChangedTag = EditorGUILayout.TextField("RecherchedTag", nameChangedTag);
            nameTag = EditorGUILayout.TextField("ChangeTagFor", nameTag);
            if (GUILayout.Button("Change Tag of all audio contains this word"))
            {
                Debug.Log("Changed tag for " + nameTag + " because " + nameChangedTag);
                if (nameTag != string.Empty && nameChangedTag != string.Empty)
                {

                    foreach (var sound in script.Sounds)
                    {

                        if (sound.Name.Contains(nameChangedTag))
                        {
                            Debug.Log(sound.Name);
                            sound.ChangeTag(nameTag);
                        }
                    }

                }
                AssetDatabase.SaveAssets();

                script.SetupTags();

            }

        }

        GUILayout.EndVertical();

        GUILayout.Space(20);

        if (GUILayout.Button("Add Sound"))
        {
            script.AddSound(new Sound(null));
        }

        indexPageSounds = DisplayPage(serializedSounds, 10, indexPageSounds, " -- Sounds - Audio Clip -- ");
        indexPageTags = DisplayPage(serializedTags, 5, indexPageTags, true, " -- Tags -- ");

        serializedObject.ApplyModifiedProperties();

    }

    private int DisplayPage(SerializedProperty array, int nbrElementsPage, int indexPage = 0, string nameList = "")
    {
        return DisplayPage(array, nbrElementsPage, indexPage, false, nameList);
    }

    private int DisplayPage(SerializedProperty array, int nbrElementsPage, int indexPage = 0, bool isHorizontal = false, string nameList = "")
    {
        serializedObject.Update();

        int nbrPages = 0;
        int index = indexPage;

        if (array != null)
            nbrPages = Mathf.RoundToInt(array.arraySize / nbrElementsPage);
        else
            return index;

        GUILayout.BeginVertical(GUI.skin.box);

        GUIStyle title = new GUIStyle(GUI.skin.box);
        title.alignment = TextAnchor.MiddleCenter;
        title.fontStyle = FontStyle.Bold;
        GUILayout.Label(nameList, title);

        GUILayout.BeginVertical(GUI.skin.box);
        int nbrElementSub = 0;
        for (int i = (indexPage * nbrElementsPage); i < (indexPage * nbrElementsPage) + nbrElementsPage; i++)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label(i.ToString() + " - ", GUILayout.MaxWidth((i >= array.arraySize) ? 20 : 40));

            if (i >= array.arraySize)
            {
                GUILayout.Label("Empty");
                GUILayout.EndHorizontal();
                nbrElementSub++;
                continue;
            }

            EditorGUILayout.PropertyField(array.GetArrayElementAtIndex(i), (isHorizontal) ? GUILayout.MaxWidth(EditorGUIUtility.fieldWidth - 60) : GUILayout.MinWidth(30));
            GUILayout.EndHorizontal();

        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginHorizontal(GUI.skin.box);

        EditorGUI.BeginDisabledGroup(indexPage <= 0);
        if (GUILayout.Button("Previous")) { index--; }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(indexPage >= nbrPages);
        if (GUILayout.Button("Next")) { index++; }
        EditorGUI.EndDisabledGroup();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.Label("Pages " + indexPage + " / " + nbrPages + " | Elements in this page " + (nbrElementsPage - nbrElementSub) + " / " + array.arraySize, GUI.skin.box);

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();

        return index;
    }

    private static Dictionary<string, uint> IncrementDictionary(Dictionary<string, uint> dictionary, string typeString)
    {
        if (dictionary.ContainsKey(typeString)) // If this type exist in dictionnary add +1 to count
            dictionary[typeString]++;
        else // Add new element to dictionnary and setup the count to 1
            dictionary.Add(typeString, 1);

        return dictionary;
    }

    private void BeautifulSlider(float valueMin, float valueActual, float valueMax)
    {
        // Create Style min GUIStyle
        GUIStyle styleMin = new GUIStyle();
        styleMin.alignment = TextAnchor.MiddleRight;
        styleMin.fontSize = 12;
        styleMin.normal.textColor = Color.white;

        // Create Style max GUIStyle
        GUIStyle styleMax = new GUIStyle();
        styleMax.alignment = TextAnchor.MiddleLeft;
        styleMax.fontSize = 12;
        styleMax.normal.textColor = Color.white;

        Color baseColor = GUI.color;

        GUILayout.BeginHorizontal();
        GUILayout.Label(valueMin.ToString(), styleMin, GUILayout.Width(20));
        GUILayout.Space(5f);
        GUI.color = Color.white;
        GUILayout.HorizontalSlider(valueActual, valueMin, valueMax);
        GUI.color = baseColor;
        GUILayout.Space(5f);
        GUILayout.Label(Mathf.Round(valueMax).ToString(), styleMax, GUILayout.Width(20));
        GUILayout.EndHorizontal();

        // Create Style actual GUIStyle
        GUIStyle styleActual = new GUIStyle();
        styleActual.alignment = TextAnchor.MiddleCenter;
        styleActual.fontSize = 14;
        styleActual.normal.textColor = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.Label(valueActual.ToString(), styleActual);
        GUILayout.EndHorizontal();
    }

    private void BeautifulSlider(float valueMin, float valueActual, float valueMax, bool isColored)
    {
        // Create Style min GUIStyle
        GUIStyle styleMin = new GUIStyle();
        styleMin.alignment = TextAnchor.MiddleRight;
        styleMin.fontSize = 12;

        if (isColored) { styleMin.normal.textColor = Color.red; }
        else { styleMin.normal.textColor = Color.white; }

        // Create Style max GUIStyle
        GUIStyle styleMax = new GUIStyle();
        styleMax.alignment = TextAnchor.MiddleLeft;
        styleMax.fontSize = 12;

        if (isColored) { styleMax.normal.textColor = Color.green; }
        else { styleMax.normal.textColor = Color.white; }

        Color baseColor = GUI.color;

        GUILayout.BeginHorizontal();
        GUILayout.Label(valueMin.ToString(), styleMin, GUILayout.Width(20));
        GUILayout.Space(5f);
        GUI.color = Color.white;
        GUILayout.HorizontalSlider(valueActual, valueMin, valueMax);
        GUI.color = baseColor;
        GUILayout.Space(5f);
        GUILayout.Label(Mathf.Round(valueMax).ToString(), styleMax, GUILayout.Width(20));
        GUILayout.EndHorizontal();

        // Create Style actual GUIStyle
        GUIStyle styleActual = new GUIStyle();
        styleActual.alignment = TextAnchor.MiddleCenter;
        styleActual.fontSize = 14;

        if (isColored) { styleActual.normal.textColor = Color.blue; }
        else { styleActual.normal.textColor = Color.white; }

        GUILayout.BeginHorizontal();
        GUILayout.Label(valueActual.ToString(), styleActual);
        GUILayout.EndHorizontal();
    }


}

#endif
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    #region SINGLETON

    /// <summary>
    /// Force a avoir qu'un seul LevelManager
    /// </summary>
    [SerializeField] private static AudioManager _instance = null;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioManager>();
                // if true, force instantiate of AudioManager
                if (_instance == null)
                {
                    var newObjectInstance = new GameObject();
                    newObjectInstance.name = typeof(AudioManager).ToString();
                    _instance = newObjectInstance.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    [System.Serializable]
    public struct Tag
    {
        [SerializeField] private string _name;

        [SerializeField] public bool IsActive;

        [SerializeField][Range(0, 1)] public float Volume;
        [SerializeField][Range(-3, 3)] public float Pitch;

        public string Name
        {
            get { return _name; }
        }

        public Tag(string name)
        {
            _name = name;

            IsActive = false;

            Volume = 1f;
            Pitch = 1f;
        }
    }

    /// <summary>
    /// Active debugLog of this script for debug
    /// </summary>
    [SerializeField] private bool _debugLog;

    [SerializeField] private string _prefixRandomSounds = "-00";

    /// <summary>
    /// List of sounds in audioManager
    /// </summary>
    [SerializeField] private Sound[] _sounds = new Sound[0];

    [SerializeField] private Tag[] _tags = new Tag[0];

    public Sound[] Sounds { get { return _sounds; } }

    void Awake()
    {
        Generate();
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        SetupTags();

        UpdateTagSound();
    }

    public void SetupTags()
    {
        Dictionary<string, uint> tagCount = new Dictionary<string, uint>();

        foreach (var sound in _sounds)
        {
            tagCount = IncrementDictionary(tagCount, sound.Tag.ToString());

            sound.UpdateName();
        }

        // Setup tab of tags in audioManager
        List<Tag> temp = new List<Tag>();
        foreach (var name in tagCount) // Pass on all type of assets
        {
            if (name.Value > 0)
            {

                bool contains = false;
                for (int i = 0; i < _tags.Length; i++)
                {
                    if (_tags[i].Name == name.Key)
                    {
                        temp.Add(_tags[i]);
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    // Get only name of tag
                    temp.Add(new Tag(name.Key));
                }

            }

        }

        bool modify = false;

        if (_tags.Length != temp.Count) { modify = true; }
        else
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                if (_tags[i].Name != temp[i].Name) { modify = true; break; }
            }
        }

        if (modify) { _tags = temp.ToArray(); }
    }

    private void UpdateTagSound()
    {
        foreach (var sound in _sounds)
        {
            if (sound.Tag != "")
            {
                foreach (var tag in _tags)
                {
                    if (tag.Name == sound.Tag && tag.IsActive)
                    {
                        sound.Volume = tag.Volume;
                        sound.Pitch = tag.Pitch;
                    }
                }
            }

            sound.UpdateSource();
        }
    }

    private void Update()
    {
        UpdateTagSound();
    }

#endif
    /// <summary>
    /// Get the 'Sound' of this 'AudioManager'
    /// </summary>
    /// <param name="name">'String' Name of the sound in array</param>
    /// <returns>Return the 'Sound'</returns>
    public Sound GetSound(string name)
    {
        if (name == null) { name = "NULL"; }
        if (_sounds.Length == 0) { return null; }

        return Array.Find(_sounds, sound => sound.Name == name);
    }

    public void Generate()
    {
        foreach (var component in gameObject.GetComponents<AudioSource>())
        {
            if (component == null) { continue; }

#if UNITY_EDITOR
            DestroyImmediate(component);
#else
                Destroy(component);
#endif
        }

        foreach (Sound instance in _sounds)
        {
            instance.SetSource(gameObject.AddComponent<AudioSource>());
            instance.Source.clip = instance.Clip;

            instance.Source.volume = instance.Volume;
            instance.Source.pitch = instance.Pitch;
            instance.Source.loop = instance.Loop;
        }
    }

    /// <summary>
    /// Get number of sound share the name
    /// </summary>
    /// <param name="name">Name shared of sounds</param>
    /// <returns>Return number of sounds with this name</returns>
    private int GetNbrSound(string name)
    {
        // If Name is null dont execute the method
        if (name == null) { name = "NULL"; }

        if (AudioManager.Instance != null)
        {
            int nbrIterationSound = 1;

            while (AudioManager.Instance.IsExist(name + _prefixRandomSounds + nbrIterationSound))
            {
                nbrIterationSound++;
            }

            if (nbrIterationSound <= 1) { return 0; }
            else return nbrIterationSound;
        }
        else
        {
            return 0;
        }

    }

    /// <summary>
    /// Get random sound who shared the name
    /// </summary>
    /// <param name="name">Name shared in sound</param>
    /// <returns>Return name of the sound selected</returns>
    private string GetRandomSoundFromName(string name)
    {
        if (name == null) { name = "NULL"; }

        int nbrSound = GetNbrSound(name);

        if (nbrSound > 0)
        {
            return name + _prefixRandomSounds + UnityEngine.Random.Range(1, nbrSound);
        }
        else
        {
            return name;
        }

    }

    /// <summary>
    /// Play the sound if is exist
    /// </summary>
    /// <param name="name">The name of sound seek</param>
    public Sound GetAndPlay(string name)
    {
        Debug.Log("Play() Function of AudioManager : " + gameObject.name);

        if (name == null)
        {
            name = "NULL";
            Debug.Log("Name is null");
            return null;
        }

        if (_sounds.Length == 0)
        {
            Debug.Log("Have noting sound");
            return null;
        }

        Sound instance = Array.Find(_sounds, sound => sound.Name == name);
        if (instance != null)
        {
            if (_debugLog) { Debug.Log("Sound " + instance.Name + " is played"); }
            instance.Source.Play();
            return instance;
        }
        else
        {
            if (_debugLog) { Debug.Log("Sound " + name + " is not found"); }
            return null;
        }

    }

    public void Play(string name)
    {
        GetAndPlay(name);
    }

    /// <summary>
    /// Play sound random on shared name
    /// </summary>
    /// <param name="name">The name shared with sound</param>
    public void PlayRandom(string name)
    {
        Play(GetRandomSoundFromName(name));
    }

    public Sound GetAndPlayRandom(string name)
    {
        return GetAndPlay(GetRandomSoundFromName(name));
    }

    /// <summary>
    /// Verify if this 'Sound' exist in 'AudioManager'
    /// </summary>
    /// <param name="name">Name of the sound</param>
    /// <returns>Return true if exist in 'AudioManager' else return false</returns>
    public bool IsExist(string name)
    {
        if (name == null) { name = "NULL"; }
        if (_sounds.Length == 0) { return false; }

        Sound instance = Array.Find(_sounds, sound => sound.Name == name);
        if (instance != null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Increment value on Dictionary
    /// </summary>
    /// <param name="dictionary">this dictionary to modify</param>
    /// <param name="value">string of key</param>
    /// <returns></returns>
    private static Dictionary<string, uint> IncrementDictionary(Dictionary<string, uint> dictionary, string value)
    {
        if (value == string.Empty) { return dictionary; }

        if (dictionary.ContainsKey(value)) // If this type exist in dictionnary add +1 to count
            dictionary[value]++;
        else // Add new element to dictionnary and setup the count to 1
            dictionary.Add(value, 1);

        return dictionary;
    }

#if UNITY_EDITOR
    public void AddSound(Sound sound)
    {
        List<Sound> list = new List<Sound>();

        for (int i = 0; i < _sounds.Length; i++)
        {
            list.Add(_sounds[i]);
        }

        sound.UpdateSource();
        sound.UpdateName();
        list.Add(sound);
        _sounds = list.ToArray();

    }

#endif

}
using UnityEngine;

[System.Serializable]
public class Sound
{
    [Header("SOUND")]
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private string _tag;

    [Header("VALUES")]
    [Range(0f, 1f)]
    [SerializeField] private float _volume = 1;
    [Range(.1f, 3f)]
    [SerializeField] private float _pitch = 0.5f;

    /// <summary>
    /// If is true sound loop on end of this
    /// </summary>
    [Header("STATES")]
    [SerializeField] private bool _loop = false;

    /// <summary>
    /// Audio Source of this sound egal instance of this sound
    /// </summary>
    [HideInInspector]
    [SerializeField] private AudioSource _source;

    /// <summary>
    /// Name assigned from this sound
    /// </summary>
    public string Name { get { return _name; } }
    /// <summary>
    /// Tag assigned from this sound
    /// </summary>
    public string Tag { get { return _tag; } }
    /// <summary>
    /// The audio of this sound
    /// </summary>
    public AudioClip Clip { get { return _clip; } }

    public float RestTime
    {
        get
        {
            return TimeSound - ActualTIme;
        }
    }

    public float ActualTIme
    {
        get { return _source.time; }
    }

    public float TimeSound
    {
        get { return _source.clip.length; }
    }

    /// <summary>
    /// If is loop
    /// </summary>
    public bool Loop { get { return _loop; } set { _loop = value; } }
    /// <summary>
    /// Actual volume of the sound
    /// </summary>
    public float Volume
    {
        get { return _volume; }
        set
        {
            if (value < 0f) { _volume = 0f; }
            else if (value > 1f) { _volume = 1f; }
            else { _volume = value; }
        }
    }
    /// <summary>
    /// The degree of highness or lowness of a tone.
    /// </summary>
    public float Pitch
    {
        get { return _pitch; }
        set
        {
            if (value < -3f) { _pitch = 0f; }
            else if (value > 3f) { _pitch = 3f; }
            else { _pitch = value; }
        }
    }
    /// <summary>
    /// Object of this sound in scene
    /// </summary>
    public AudioSource Source { get { return _source; } }

    public Sound(AudioClip audio)
    {
        _name = string.Empty;
        _volume = 1f;
        _pitch = 1f;
        _loop = false;
        _source = null;

        _clip = audio;
    }

    /// <summary>
    /// Stop SourceAudio
    /// </summary>
    public void Stop()
    {
        if (_source != null)
        {
            _source.Stop();
        }
    }

    /// <summary>
    /// Set the AudioSource
    /// </summary>
    /// <param name="source">the source set</param>
    public void SetSource(AudioSource source)
    {
        if (source == null) { return; }

        _source = source;
    }

    /// <summary>
    /// Update the name if is empty with name of Sound Clip
    /// </summary>
    public void UpdateName()
    {
        if (Clip != null && Name == "")
        {
            string name = Clip.name;

            string[] splitName = name.Split('_');
            if (splitName[0].ToUpper() == "SFX")
            {
                name = splitName[0].ToUpper();

                for (int i = 1; i < splitName.Length; i++)
                {
                    name += '_' + splitName[i];
                }

            }

            _name = name;
        }
    }

    /// <summary>
    /// If Source is not null update volume and pitch value with define value
    /// </summary>
    public void UpdateSource()
    {
        if (Source != null)
        {
            Source.volume = _volume;
            Source.pitch = _pitch;
        }
    }


#if UNITY_EDITOR
    public void ChangeTag(string tag)
    {
        _tag = tag.ToLower();
    }
#endif
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ")]
[CreateAssetMenu(fileName = "NewUnit", menuName = "Scriptable/Unit")]
public class ScriptableUnit : ScriptableObject
{
    [System.Serializable]
    public class UnitInfo
    {
        /// <summary>
        /// Name of unit
        /// </summary>
        [SerializeField] private string _name;
        /// <summary>
        /// Description of this unit or variant
        /// </summary>
        [SerializeField] private LocalizationAsset _description;
        /// <summary>
        /// Display unit
        /// </summary>
        [SerializeField] private Sprite _sprite;
        /// <summary>
        /// Prefabs for generate unit
        /// </summary>
        [SerializeField] private GameObject _prefabs;

        /// <summary>
        /// Used sounds of unit
        /// </summary>
        [SerializeField] private ScriptableSoundUnit _soundNames;

        public GameObject Prefab { get { return _prefabs; } }

        public ScriptableSoundUnit Sound { get { return _soundNames; } }

        public Sprite Sprite { get { return _sprite; } }

#if UNITY_EDITOR
        /// <summary>
        /// for editor detected if is a variant or not
        /// </summary>
        [SerializeField] private bool _isVariant = false; 

        public bool IsVariant { 
            get { return _isVariant; }
            set { _isVariant = value; }
        }
#endif

    }

    /// <summary>
    /// All units
    /// </summary>
    [SerializeField] private UnitInfo[] _units = new UnitInfo[1];

    public int NbrVariant
    {
        get { return _units.Length; }
    }

    private void OnValidate()
    {
        if (_units != null)
        {
            for (int i = 0; i < _units.Length; i++)
            {
                if (_units[i] == null) { continue; }

                if (i > 0)
                {
                    _units[i].IsVariant = true;
                }
                else
                {
                    _units[i].IsVariant = false;
                }
            }
        }

    }

    /// <summary>
    /// Get the prefab of unit for battle
    /// </summary>
    /// <param name="skinIndex">define the skin used</param>
    /// <returns></returns>
    public GameObject GetPrefab(int skinIndex)
    {
        if (_units.Length <= 0)
        {
            Debug.LogError(name + " is not usable, dont have units");
            return null;
        }

        if (skinIndex >= 0 && skinIndex < _units.Length)
        {
            return _units[skinIndex].Prefab;
        }
        else
        {
            return _units[0].Prefab;
        }
    }
}

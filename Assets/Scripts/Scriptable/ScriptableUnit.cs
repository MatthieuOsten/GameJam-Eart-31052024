using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnValidate()
    {
        if (_units != null)
        {
            for (int i = 0; i < _units.Length; i++)
            {
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
}

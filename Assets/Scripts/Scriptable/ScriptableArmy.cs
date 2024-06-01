using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmy", menuName = "Scriptable/Army")]
public class ScriptableArmy : ScriptableObject
{
    [System.Serializable]
    public class SoldierInfo
    {
        /// <summary>
        /// Name of this soldier
        /// </summary>
        [SerializeField] private string _name;
        /// <summary>
        /// Index of skin to unit
        /// </summary>
        [SerializeField] private uint _indexVariant;

        /// <summary>
        /// Health of soldier
        /// </summary>
        [SerializeField] private float _health;

        /// <summary>
        /// Health of this soldier, update by others scripts
        /// </summary>
        public float Health
        {
            get { return _health; }
            set { 
                if (value < 0) { _health = 0; }
                else { _health = value; }
                }
        }

        public SoldierInfo()
        {
            _name = "Smith";
            _indexVariant = 0;
            _health = 0;
        }

        public SoldierInfo(string name, uint indexSkin, float health)
        {
            _name = name;
            _indexVariant = indexSkin;
            _health = health;
        }

    }

    /// <summary>
    /// Name of this army
    /// </summary>
    [SerializeField] private string _name;
    /// <summary>
    /// Unit used by this army
    /// </summary>
    [SerializeField] private ScriptableUnit _unit;
    /// <summary>
    /// Soldiers of army
    /// </summary>
    [SerializeField] private SoldierInfo[] _soldiers;

    /// <summary>
    /// Sprite of dead soldier
    /// </summary>
    [SerializeField] private Sprite _spriteDead;

    /// <summary>
    /// Soldiers of army
    /// </summary>
    public SoldierInfo[] Soldiers
    {
        get { return _soldiers; }
        set { _soldiers = value; }
    }

    /// <summary>
    /// Selected type of unit
    /// </summary>
    public ScriptableUnit Unit 
    { 
        get { return _unit; } 
        set
        {
            if (value == null) {
                Debug.Log(name + " | Unit is not set scriptable is null");
                return; 
            }
            _unit = value;
        }
    }

    /// <summary>
    /// Set soldiers default
    /// </summary>
    /// <param name="numbers"></param>
    public void SetSoldiers(int numbers)
    {
        if (_unit == null) {
            Debug.Log(name + " | SetSoldiers is not set because Unit is null");
            return; 
        }

        _soldiers = new SoldierInfo[numbers];
    }
}

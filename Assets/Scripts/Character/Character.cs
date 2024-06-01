using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _healthMaximum;
    [SerializeField] private ScriptableFloat _scriptableHealth;
    [SerializeField] private Walk _moveWalk;

    /// <summary>
    /// Health of this character
    /// </summary>
    public float Health
    {
        get { return _health; } 
        private set
        {
            if (value <= 0)
            {
                _health = 0;
            }
            else if (value > HealthMax && HealthMax > 0)
            {
                _health = HealthMax;
            }
            else
            {
                _health = value;
            }

            if (_scriptableHealth != null) { _scriptableHealth.Value = _health; }

        }
    }

    /// <summary>
    /// Maximum of health 
    /// </summary>
    public float HealthMax
    {
        get { return _healthMaximum; }
    }

    private void FixedUpdate()
    {
        _moveWalk.FixedUpdateMovement();
    }
}

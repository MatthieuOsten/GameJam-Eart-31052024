using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Movement
{
    [SerializeField] private bool _isActive = true;

    [SerializeField] private GameObject _target;

    [SerializeField] private float _speed;

    [SerializeField] private Vector2 _limits;

    /// <summary>
    /// Target of the movement
    /// </summary>
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    /// <summary>
    /// Speed of action
    /// </summary>
    public float Speed
    {
        get { return _speed; } 
        protected set { 

            if (value >= Limits.x && value <= Limits.y) 
            { 
                _speed = value; 
            }
            else if(value < Limits.x) 
            { 
                _speed = Limits.x; 
            }
            else if (value > Limits.y) 
            { 
                _speed = Limits.y; 
            }

        }
    }

    /// <summary>
    /// Limits of this action
    /// </summary>
    public Vector2 Limits
    {
        get { return _limits; }
    }

    public bool IsActive { 
        get { return _isActive; } 
        set { _isActive = value; }
    }

    public virtual void OnDrawGizmos() { }

    public virtual void OnEnable() { }
    public virtual void OnDisable() { }

    public virtual void FixedUpdateMovement() { }

    public virtual void InvokeMovement() { }
}

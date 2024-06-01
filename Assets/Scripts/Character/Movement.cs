using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Movement
{
    [SerializeField] private float _speed;

    [SerializeField] private Vector2 _limits;

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

    public Vector2 Limits
    {
        get { return _limits; }
    }

    public virtual void FixedUpdateMovement() { }

    public virtual void InvokeMovement() { }
}

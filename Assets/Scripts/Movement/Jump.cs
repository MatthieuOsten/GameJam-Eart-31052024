using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MoveJump : Movement
{

    [SerializeField] private bool _isJumping = false;

    [SerializeField] private ScriptableEvent _scriptableEvent;

    [SerializeField] private UnityEvent _eventJump, _eventTouchGround;

    [SerializeField] private Vector2 _direction;
    [SerializeField] private ScriptableVector2 _scriptableDirection;

    [SerializeField,Min(0.1f)] private float _groundRaycast = 1f;

    public bool IsJumping
    {
        get { return _isJumping; }
        set { 
            if (value != _isJumping)
            {
                _isJumping = value;

                if (value)
                {
                    _eventJump?.Invoke();
                }
                else
                {
                    _eventTouchGround?.Invoke();
                }

            }
        }
    }

    public Vector2 Direction
    {
        get { return _direction; }
        set
        {
            _direction = new Vector2(value.x, (value.y < 0) ? _direction.y : value.y);
        }
    }

    public override void OnEnable()
    {
        if (_scriptableEvent != null)
        {
            _scriptableEvent.OnEvent += Jump;
        }
    }

    public override void OnDisable()
    {
        if (_scriptableEvent != null)
        {
            _scriptableEvent.OnEvent -= Jump;
        }
    }

    public override void FixedUpdateMovement()
    {
        base.FixedUpdateMovement();

        _direction = new Vector2(Mathf.Clamp(_scriptableDirection.Value.x,-1,1), _direction.y);

        // Verifie si l'objet touche le sol ou non
        _isJumping = !Physics.Raycast(Target.transform.position, new Vector3(0, -1), _groundRaycast);
    }

    public override void OnDrawGizmos()
    {
        if (Target != null)
        {
            Gizmos.DrawLine(Target.transform.position,Target.transform.position + new Vector3(0, -1 * _groundRaycast));
        }
    }

    private void Jump()
    {
        if (!IsActive)
        {
            return;
        }

        Debug.Log("JUMP " + IsJumping.ToString());

        if (!IsJumping) {

            ThrowHandler.LaunchObject(Target, _direction, Speed) ;

        }
    }
}

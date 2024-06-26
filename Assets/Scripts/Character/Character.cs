using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[HelpURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ")]
public class Character : MonoBehaviour
{

    [SerializeField] private float _health;
    [SerializeField] private float _healthMaximum;

    [SerializeField] private bool _activeMovements = false;

    [SerializeField] private MoveWalk _walk;
    [SerializeField] private MoveJump _jump;
    [SerializeField] private Shoot _shoot;

    [SerializeField] private GameObject _body;

    /// <summary>
    /// Active or disable all movements of this character
    /// </summary>
    public bool ActiveMovements
    {
        get { return _activeMovements; }
        private set {

            if (_activeMovements != value)
            {
                if (value)
                {
                    _walk.IsActive = true;
                    _jump.IsActive = true;
                }
                else
                {
                    _walk.IsActive = false;
                    _jump.IsActive = false;
                }
            }

            _activeMovements = value; 
        }
    }

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

        }
    }

    /// <summary>
    /// Maximum of health 
    /// </summary>
    public float HealthMax
    {
        get { return _healthMaximum; }
    }

    private void OnValidate()
    {
        if (_walk != null)
        {
            if (_walk.Target == null) { _walk.Target = gameObject; }
        }

        if (_jump != null)
        {
            if (_jump.Target == null) { _jump.Target = gameObject; }
        }

        if (_shoot != null)
        {
            SetTags(tag, _shoot.gameObject);
        }

    }

    private void Start()
    {
        if (_shoot != null)
        {
            SetTags(tag,_shoot.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        _jump.OnDrawGizmos();
        _walk.OnDrawGizmos();
    }

    private void OnEnable()
    {
        _walk.OnEnable();
        _jump.OnEnable();
    }

    private void OnDisable()
    {
        _walk.OnDisable();
        _jump.OnDisable();
    }

    private void FixedUpdate()
    {
        if (_walk.IsMoving)
        {
            _jump.IsActive = false;
        }
        else
        {
            _jump.IsActive = true;
        }

        if (_jump.IsJumping)
        {
            _walk.IsActive = false;
        }
        else
        {
            _walk.IsActive = true;
        }

        _walk.FixedUpdateMovement();
        _jump.FixedUpdateMovement();
    }

    private void SetTags(string tag, GameObject tagedObject)
    {
        tagedObject.tag = tag;

        for (int i = 0; i < tagedObject.transform.childCount; i++)
        {
            SetTags(tag, tagedObject.transform.GetChild(i).gameObject);
        }

    }

    public void TakeDamage(float damage)
    {
        if (Health == 0) { return; }

        _body.transform.localScale = (_body.transform.localScale) * ((1.2f) * (Health - damage));
        Health -= damage;
    }
}

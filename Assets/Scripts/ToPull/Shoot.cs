using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shoot : MonoBehaviour
{
    #region VARIABLE DISPLAY
    [SerializeField] private GameObject _prefabProjectile;
    [SerializeField] private GameObject _projectileStored;

    [SerializeField] private Viewfinder _displayRay;
    [SerializeField] private Transform _shootPosition;

    [SerializeField] private ScriptableEvent _scriptableAimded;
    [SerializeField] private ScriptableEvent _scriptableShoot;
    [SerializeField] private ScriptableEvent _scripatbleCancel;
    #endregion

    [Header("VALUE")]
    [SerializeField] ScriptableVector2 _scriptableDirection;
    [SerializeField] ScriptableFloat _scriptableForce;
    [SerializeField] Vector2 _limitForce = new Vector2(3, 5);
    [SerializeField] float _speedForce = 1f;

    [Header("EVENT")]
    [SerializeField] private UnityEvent _eventShooting;
    [SerializeField] private UnityEvent _eventAimded;
    [SerializeField] private UnityEvent _eventCancel;

    public bool IsStateAimded
    {
        get
        {
            if (_displayRay == null) { Debug.LogError(name + " | Dont find Viewfinder"); }

            return _displayRay.Display;
        }
    }

    private void Awake()
    {
        InstantiateProjectiles();

        if (_displayRay == null)
        {
            _displayRay = gameObject.GetComponent<Viewfinder>();
        }
    }

    public void OnEnable()
    {
        if (_scriptableShoot == null || _scripatbleCancel == null) { return; }

        _scriptableShoot.OnEvent += ShootProjectile;
        _scripatbleCancel.OnEvent += HideAimded;
    }

    public void OnDisable()
    {
        if (_scriptableShoot == null || _scripatbleCancel == null) { return; }

        _scriptableShoot.OnEvent -= ShootProjectile;
        _scripatbleCancel.OnEvent -= HideAimded;
    }

    private void SeekAimded()
    {
        if (_displayRay == null) { return; }

        // Invoke if modify
        if (_displayRay.Display != true)
        {
            _eventAimded.Invoke();
        }

        // Modify value
        _displayRay.Display = true;

    }

    private void HideAimded()
    {
        if (_displayRay == null) { return; }

        // Invoke if modify
        if (_displayRay.Display != false)
        {
            _eventCancel.Invoke();
        }

        // Modify value
        _displayRay.Display = false;

    }

    private void InstantiateProjectiles()
    {
        if (_prefabProjectile != null && _projectileStored == null)
        {
            _projectileStored = Instantiate<GameObject>(_prefabProjectile, transform);
            _projectileStored.transform.position = Vector3.zero;
            _projectileStored.SetActive(false);
            SetTags(tag, _projectileStored);
        }
    }

    private void SetTags(string tag, GameObject tagedObject)
    {
        tagedObject.tag = tag;

        for (int i = 0; i < tagedObject.transform.childCount; i++)
        {
            SetTags(tag, tagedObject.transform.GetChild(i).gameObject);
        }

    }

    /// <summary>
    /// Shoot projectile
    /// </summary>
    public void ShootProjectile()
    {
        // If is not on display ray of shoot is not usable
        if (!IsStateAimded) { return; }

        // Else if all is set, launch projectile
        if (_scriptableForce != null && _scriptableDirection != null)
        {
            if (_projectileStored != null)
            {

                if (_shootPosition != null)
                {
                    _projectileStored.transform.position = _shootPosition.position;
                }

                _projectileStored.SetActive(true);
                HideAimded();

                SetTags(tag, _projectileStored);

                ThrowHandler.LaunchObject(_projectileStored, _scriptableDirection.Value, _scriptableForce.Value);

                _eventShooting.Invoke();
            }
        }
    }

    public void AddForce()
    {
        if (_scriptableForce.Value < _limitForce.y)
        {
            _scriptableForce.Value += Time.deltaTime * _speedForce;
            Debug.Log("LOG | GetKey of add Force : " + _scriptableForce.Value);
        }
    }

    public void SubForce()
    {
        if (_scriptableForce.Value > _limitForce.y)
        {
            _scriptableForce.Value += Time.deltaTime * _speedForce;
            Debug.Log("LOG | GetKey of add Force : " + _scriptableForce.Value);
        }
    }

}

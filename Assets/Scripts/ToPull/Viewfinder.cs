using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewfinder : MonoBehaviour
{

    [Header("VALUE")]
    [SerializeField] ScriptableVector2 _scriptableDirection;
    [SerializeField] ScriptableFloat _scriptableForce;
    [SerializeField] ScriptableBool _scriptablePower;

    [SerializeField] Transform _origin;
    [SerializeField] Vector2 _direction;
    [SerializeField] float _force;

    [Header("DISPLAY")]
    [SerializeField] private Vector3 _size;
    [SerializeField] private bool _display;
    [SerializeField] private int _nbrPoints = 10;
    [SerializeField] private List<GameObject> _points;
    [SerializeField] private GameObject _prefabPoint;

    public bool Display
    {
        get { return _display; }

        set {
            _display = value; 
        }
    }

    public Vector2 Direction
    {
        get
        {
            if (_scriptableDirection != null) { return _scriptableDirection.Value; }
            else { Debug.Log(name + " -> Direction scriptable not setup "); return _direction; }
        }

        set
        {
            _direction = value;
        }
    }

    public float Force
    {
        get
        {
            if (_scriptableForce != null) { return _scriptableForce.Value; }
            else { Debug.Log(name + " -> Force scriptable not setup "); return _force; }
        }

        set
        {
            _force = value;
        }
    }

    private void Awake()
    {
        if (_prefabPoint != null)
        {
            if (_points != null) { Clear(); }

            InstantiatePoints();

            if (_prefabPoint != null) { _size = _prefabPoint.transform.localScale;}
        }
    }

    private void FixedUpdate()
    {
        if (_scriptablePower != null) { Display = _scriptablePower.Value; }

        UpdatePoints(_points);
        DisplayPoints(_points, Display);
    }

    private void Start()
    {
        Display = false;

        DisplayPoints(_points,Display);
    }

    private void Clear()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            #if UNITY_EDITOR
                DestroyImmediate(_points[i]);
            #else
                Destroy(_points[i]);
            #endif
        }

        _points.Clear();
    }

    private void InstantiatePoints()
    {
        if (_prefabPoint == null) {
            Debug.LogWarning(name + " | Prefab not set");
            return; 
        }

        for (int i = 0; i < _nbrPoints; i++)
        {
            GameObject instance = Instantiate(_prefabPoint,transform);

            _points.Add(instance);
        }
    }

    IEnumerator HidePoint(GameObject point)
    {
        float step = 0;
        while (step < 1)
        {
            point.transform.position = Vector3.Lerp(point.transform.position, Vector3.zero, step += Time.deltaTime);
            point.transform.localScale = Vector3.Lerp(point.transform.localScale, Vector3.zero, step += Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SeekPoint(GameObject point, Vector3 positionTarget, Vector3 sizeTarget)
    {
        float step = 0;
        while (step < 1)
        {
            point.transform.position = Vector3.Lerp(point.transform.position, positionTarget, step += Time.deltaTime);
            point.transform.localScale = Vector3.Lerp(point.transform.localScale, sizeTarget, step += Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void DisplayPoints(List<GameObject> points, bool display)
    {
        Vector3[] pointsPos = ThrowHandler.GetPoints(_origin.position,Direction,Force, _nbrPoints);

        for (int i = 0; i < points.Count; i++)
        {
            if (display)
            {
                StartCoroutine(SeekPoint(points[i], pointsPos[i], _size));
            }
            else
            {
                StartCoroutine(HidePoint(points[i]));
            }
        }
    }

    private void UpdatePoints(List<GameObject> points)
    {
        Vector3[] pointsPos = ThrowHandler.GetPoints(_origin.position, Direction, Force, _nbrPoints);

        for (int i = 0; i < _points.Count; i++)
        {
            if (pointsPos.Length > i)
            {
                _points[i].transform.position = pointsPos[i];
            }
            else
            {
                StartCoroutine(HidePoint(_points[i]));
            }

        }
    }

}

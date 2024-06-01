using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVector2", menuName = "Scriptable/Default/Vector2")]
public class ScriptableVector2 : ScriptableObject
{
    [SerializeField] private Vector2 _Vector2;

    public Vector2 Value {
        get { 
            return _Vector2; 
        }
        set {
            Debug.Log("Value " + Value + " changer for " + value);
            _Vector2 = value; 
        } 
    }

    public void ReceiveInput(Vector2 value)
    {
        Value = value;
    }
}

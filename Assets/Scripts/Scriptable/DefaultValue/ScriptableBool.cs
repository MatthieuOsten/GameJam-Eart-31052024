using UnityEngine;

[CreateAssetMenu(fileName = "NewBool", menuName = "Scriptable/Default/Bool")]
public class ScriptableBool : ScriptableObject
{
    [SerializeField] private bool _value;
    public bool Value { 
        get { return _value; }
        set { _value = value; } 
    }

    public void ReceiveInput(bool value)
    {
        Value = value;
    }
}

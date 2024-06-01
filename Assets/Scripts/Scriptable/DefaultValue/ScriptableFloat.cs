using UnityEngine;

[CreateAssetMenu(fileName = "NewFloat", menuName = "Scriptable/Default/Float")]
public class ScriptableFloat : ScriptableObject
{
    [SerializeField] private float _value;

    public float Value {
        get { return _value; } 
        set { _value = value; } 
    }

    public void ReceiveInput(float value)
    {
        Value = value;
    }
}

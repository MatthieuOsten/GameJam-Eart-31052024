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

    public void ReceiveInput(float value,float limit)
    {
        if (value > limit) { Value = 0; return; }

        Value = value;
    }

    public void ReceiveInput(float value, Vector2 limit)
    {
        if (value < limit.x) { Value = limit.x; return; }
        if (value > limit.y) { Value = limit.y; return; }

        Value = value;
    }
}

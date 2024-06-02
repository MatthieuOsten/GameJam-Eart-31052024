using UnityEngine;

[CreateAssetMenu(fileName = "NewInt", menuName = "Scriptable/Default/Int")]
public class ScriptableInt : ScriptableObject
{
    public int Value { get; set; }

    public void ReceiveInput(int value)
    {
        Value = value;
    }
}

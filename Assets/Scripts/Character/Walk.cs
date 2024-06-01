using UnityEngine;

[System.Serializable]
public class Walk : Movement
{

    [SerializeField] private ScriptableVector2 _scriptableMove;

    [SerializeField] private CharacterController _controller;

    [SerializeField] private bool _isMoving;

    public bool IsMoving
    {
        get { return _isMoving; }
    }

    public override void FixedUpdateMovement()
    {
        if (_scriptableMove != null)
        {
            if (_scriptableMove.Value.x != 0 || _scriptableMove.Value.y != 0)
            {
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }

            if (_controller != null)
            {
                _controller.SimpleMove((Speed == 0) ? Vector3.zero : new Vector3(_scriptableMove.Value.x * Speed, 0));
            }

        }

    }
}

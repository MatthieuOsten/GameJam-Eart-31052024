using UnityEngine;

[System.Serializable]
public class MoveWalk : Movement
{

    [SerializeField] private ScriptableVector2 _scriptableMove;
    [SerializeField] private ScriptableVector2 _scriptableDirection;

    [SerializeField] private bool _isMoving;

    [SerializeField] private CharacterController _targetController = null;
    [SerializeField] private Rigidbody _targetRigidbody = null;
    [SerializeField] private Rigidbody2D _targetRigidbody2D = null;

    public bool IsMoving
    {
        get { return _isMoving; }
    }

    public override void FixedUpdateMovement()
    {
        if (!IsActive)
        {
            return;
        }

        if (_scriptableDirection != null && _scriptableMove != null) { 
            if (_scriptableMove.Value.x != 0)
            {
                if (_scriptableMove.Value.x > 0)
                {
                    _scriptableDirection.Value = new Vector2(1, _scriptableDirection.Value.y);
                }
                else
                {
                    _scriptableDirection.Value = new Vector2(-1, _scriptableDirection.Value.y);
                }
            }
        }

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

            if (Target != null)
            {
                float positionY = Target.transform.position.y;

                if (_targetController != null || Target.GetComponent<CharacterController>() != null)
                {
                    if (_targetController == null) { _targetController = Target.GetComponent<CharacterController>(); }

                    _targetController.SimpleMove((Speed == 0) ? Vector3.zero : new Vector3(_scriptableMove.Value.x * Speed, positionY));
                }
                else if (_targetRigidbody != null || Target.GetComponent<Rigidbody>() != null)
                {
                    if (_targetRigidbody == null) { _targetRigidbody = Target.GetComponent<Rigidbody>(); }

                    _targetRigidbody.velocity += ((Speed == 0) ? Vector3.zero : new Vector3(_scriptableMove.Value.x * Speed, 0));
                }
                else if (_targetRigidbody2D != null || Target.GetComponent<Rigidbody2D>() != null)
                {
                    if (_targetRigidbody2D == null) { _targetRigidbody2D = Target.GetComponent<Rigidbody2D>(); }

                    _targetRigidbody.velocity += ((Speed == 0) ? Vector3.zero : new Vector3(_scriptableMove.Value.x * Speed, 0));
                }

            }

        }

    }
}

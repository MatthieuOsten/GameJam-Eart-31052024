using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    [Header("SETTINGS")]
    [SerializeField] private Controller _controller;
    [SerializeField] private bool _debug;

    #region SCRIPTABLES

    [Header("ON CONTROLS")]
    [SerializeField] private ScriptableVector2 _moveDirection;
    [SerializeField] private ScriptableEvent _eventJump;

    [Header("ON SHOOTING")]
    [SerializeField] private ScriptableEvent _eventShoot;

    [Header("ON TARGET")]
    [SerializeField] private ScriptableFloat _moveHeight;
    [SerializeField] private ScriptableEvent _eventCancel;
    [SerializeField] private ScriptableBool _eventPower;

    [Header("IN GAME")]
    [SerializeField] private ScriptableEvent _eventPause;

    #endregion

    public Controller Inputs { 
        get 
        { 
            if (_controller == null) { 
                _controller = new Controller(); 

            }

            if (_debug) { Debug.Log("Controller is " + ((_controller == null) ? "NULL" : "Create")); }
            return _controller; 
        } 
    }

    private void Awake()
    {
        _controller = new Controller();
    }

    private void OnEnable()
    {
        if (Inputs == null) { return; }

        // Enabled controllers
        Inputs.InGame.Enable();
        Inputs.OnControls.Enable();
        Inputs.OnShooting.Enable();
        Inputs.OnTarget.Enable();

        #region CONTROLS ENABLE
        // If _moveDirection is not null set MoveDirection event input
        if (_moveDirection != null)
        {
            Inputs.OnControls.Movement.performed += ctx => _moveDirection.ReceiveInput(ctx.ReadValue<Vector2>());
            Inputs.OnControls.Movement.canceled += ctx => _moveDirection.ReceiveInput(Vector2.zero);
        }

        if (_eventJump != null)
        {
            Inputs.OnControls.Jump.performed += ctx => _eventJump.LaunchEvent();
        }
        #endregion

        #region SHOOT ENABLE
        if (_eventShoot != null)
        {
            Inputs.OnShooting.Shoot.performed += ctx => _eventShoot.LaunchEvent();
        }
        #endregion

        #region TARGET ENABLE
        if (_moveHeight != null)
        {
            Inputs.OnTarget.Movement.performed += ctx => _moveHeight.ReceiveInput(ctx.ReadValue<float>());
            Inputs.OnTarget.Movement.canceled += ctx => _moveHeight.ReceiveInput(0f);
        }

        if (_eventCancel != null)
        {
            Inputs.OnTarget.Cancel.performed += ctx => _eventCancel.LaunchEvent();
        }

        if (_eventJump != null)
        {
            Inputs.OnTarget.Power.started += ctx => _eventPower.ReceiveInput(true);
            Inputs.OnTarget.Power.canceled += ctx => _eventPower.ReceiveInput(false);
        }
        #endregion

        #region INGAME ENABLE
        if (_eventPause != null)
        {
            Inputs.InGame.Pause.performed += ctx => _eventPause.LaunchEvent();
        }
        #endregion

    }

    private void OnDisable()
    {
        if (Inputs == null) { return; }

        // Disable controllers
        Inputs.InGame.Disable();
        Inputs.OnControls.Disable();
        Inputs.OnShooting.Disable();
        Inputs.OnTarget.Disable();

        #region CONTROLS DISABLE
        // If _moveDirection is not null set MoveDirection event input
        if (_moveDirection != null)
        {
            Inputs.OnControls.Movement.performed -= ctx => _moveDirection.ReceiveInput(ctx.ReadValue<Vector2>());
            Inputs.OnControls.Movement.canceled -= ctx => _moveDirection.ReceiveInput(Vector2.zero);
        }

        if (_eventJump != null)
        {
            Inputs.OnControls.Jump.performed -= ctx => _eventJump.LaunchEvent();
        }
        #endregion

        #region SHOOT DISABLE
        if (_eventShoot != null)
        {
            Inputs.OnShooting.Shoot.performed -= ctx => _eventShoot.LaunchEvent();
        }
        #endregion

        #region TARGET DISABLE
        if (_moveHeight != null)
        {
            Inputs.OnTarget.Movement.performed -= ctx => _moveHeight.ReceiveInput(ctx.ReadValue<float>());
            Inputs.OnTarget.Movement.canceled -= ctx => _moveHeight.ReceiveInput(0f);
        }

        if (_eventCancel != null)
        {
            Inputs.OnTarget.Cancel.performed -= ctx => _eventCancel.LaunchEvent();
        }

        if (_eventJump != null)
        {
            Inputs.OnTarget.Power.started -= ctx => _eventPower.ReceiveInput(true);
            Inputs.OnTarget.Power.canceled -= ctx => _eventPower.ReceiveInput(false);
        }
        #endregion

        #region INGAME DISABLE
        if (_eventPause != null)
        {
            Inputs.InGame.Pause.performed -= ctx => _eventPause.LaunchEvent();
        }
        #endregion
    }

}

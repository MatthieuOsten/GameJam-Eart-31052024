using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    [Header("SETTINGS")]
    [SerializeField] private Controller _controller;
    [SerializeField] private bool _debug;
    [SerializeField] private Vector2 _limitForce = new Vector2(3f,20f);
    [SerializeField] private bool _pressPowerAdd = false, _pressPowerSub = false;
    [SerializeField] private float _pressSpees = 10f;

    #region SCRIPTABLES

    [Header("ON CONTROLS")]
    [SerializeField] private ScriptableVector2 _moveDirection;
    [SerializeField] private ScriptableEvent _eventJump;

    [Header("ON SHOOTING")]
    [SerializeField] private ScriptableEvent _eventShoot;

    [Header("ON TARGET")]
    [SerializeField] private ScriptableVector2 _moveDirectionTarget;
    [SerializeField] private ScriptableFloat _floatLaunchPower;
    [SerializeField] private ScriptableEvent _eventCancel;
    [SerializeField] private ScriptableBool _eventToPull;

    [Header("IN GAME")]
    [SerializeField] private ScriptableBool _eventPause;

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

        if (_eventPause != null) { _eventPause.Value = false; }
        if (_eventToPull != null) { _eventToPull.Value = false; }
        if (_floatLaunchPower != null) { _floatLaunchPower.Value = 0f; }
        if (_moveDirectionTarget != null) { _moveDirectionTarget.Value = Vector2.zero; }
        if (_moveDirection != null) { _moveDirection.Value = Vector2.zero; }

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

        if (_eventToPull != null)
        {
            Inputs.OnControls.Attack.started += ctx => _eventToPull.ReceiveInput(true);
            Inputs.OnControls.Attack.started += ctx => ChangeStateOnTarget();
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
            Inputs.OnShooting.Shoot.performed += ctx => _eventToPull.ReceiveInput(false);
        }
        #endregion

        #region TARGET ENABLE

        if (_moveDirectionTarget != null)
        {
            Inputs.OnTarget.Movement.performed += ctx => _moveDirectionTarget.ReceiveInput(_moveDirectionTarget.Value + ctx.ReadValue<Vector2>());
        }

        if (_floatLaunchPower != null)
        {
            Inputs.OnTarget.PowerAdd.started += ctx => _pressPowerAdd = true;
            Inputs.OnTarget.PowerAdd.canceled += ctx => _pressPowerAdd = false;

            Inputs.OnTarget.PowerSub.started += ctx => _pressPowerSub = true;
            Inputs.OnTarget.PowerSub.canceled += ctx => _pressPowerSub = false;
        }

        if (_eventCancel != null)
        {
            Inputs.OnTarget.Cancel.performed += ctx => _eventCancel.LaunchEvent();
            Inputs.OnTarget.Cancel.performed += ctx => ChangeStateOnControls();
            if (_floatLaunchPower != null)
            {
                Inputs.OnTarget.Cancel.performed += ctx => _floatLaunchPower.ReceiveInput(0f);
            }
        }

        if (_eventToPull != null)
        {
            Inputs.OnTarget.Cancel.started += ctx => _eventToPull.ReceiveInput(false);
        }
        #endregion

        #region INGAME ENABLE
        if (_eventPause != null)
        {
            Inputs.InGame.Pause.started += ctx => _eventPause.ReceiveInput(!_eventPause.Value);
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

        #region CONTROLS ENABLE
        // If _moveDirection is not null set MoveDirection event input
        if (_moveDirection != null)
        {
            Inputs.OnControls.Movement.performed -= ctx => _moveDirection.ReceiveInput(ctx.ReadValue<Vector2>());
            Inputs.OnControls.Movement.canceled -= ctx => _moveDirection.ReceiveInput(Vector2.zero);
        }

        if (_eventToPull != null)
        {
            Inputs.OnControls.Attack.started -= ctx => _eventToPull.ReceiveInput(true);
            Inputs.OnControls.Attack.started -= ctx => ChangeStateOnTarget();
        }

        if (_eventJump != null)
        {
            Inputs.OnControls.Jump.performed -= ctx => _eventJump.LaunchEvent();
        }
        #endregion

        #region SHOOT ENABLE

        if (_eventShoot != null)
        {
            Inputs.OnShooting.Shoot.performed += ctx => _eventShoot.LaunchEvent();
            Inputs.OnShooting.Shoot.performed += ctx => _eventToPull.ReceiveInput(false);
        }
        #endregion

        #region TARGET ENABLE

        if (_moveDirectionTarget != null)
        {
            Inputs.OnTarget.Movement.performed -= ctx => _moveDirectionTarget.ReceiveInput(_moveDirectionTarget.Value + ctx.ReadValue<Vector2>());
        }

        if (_floatLaunchPower != null)
        {
            Inputs.OnTarget.PowerAdd.started -= ctx => _pressPowerAdd = true;
            Inputs.OnTarget.PowerAdd.canceled -= ctx => _pressPowerAdd = false;

            Inputs.OnTarget.PowerSub.started -= ctx => _pressPowerSub = true;
            Inputs.OnTarget.PowerSub.canceled -= ctx => _pressPowerSub = false;
        }

        if (_eventCancel != null)
        {
            Inputs.OnTarget.Cancel.performed -= ctx => _eventCancel.LaunchEvent();
            Inputs.OnTarget.Cancel.performed -= ctx => ChangeStateOnControls();
            if (_floatLaunchPower != null)
            {
                Inputs.OnTarget.Cancel.performed -= ctx => _floatLaunchPower.ReceiveInput(0f);
            }
        }

        if (_eventToPull != null)
        {
            Inputs.OnTarget.Cancel.started -= ctx => _eventToPull.ReceiveInput(false);
        }
        #endregion

        #region INGAME ENABLE
        if (_eventPause != null)
        {
            Inputs.InGame.Pause.started -= ctx => _eventPause.ReceiveInput(!_eventPause.Value);
        }
        #endregion

    }

    private void Update()
    {
        if (_floatLaunchPower != null && _pressPowerAdd)
        {
            _floatLaunchPower.ReceiveInput(_floatLaunchPower.Value + (_pressSpees * Time.deltaTime), _limitForce);
        }
        else if (_floatLaunchPower != null && _pressPowerSub)
        {
            _floatLaunchPower.ReceiveInput(_floatLaunchPower.Value - (_pressSpees * Time.deltaTime), _limitForce);
        }

    }

    private void ChangeStateOnControls()
    {

        Debug.Log("Change for controls");

        Inputs.OnControls.Enable();
        Inputs.OnShooting.Disable();
        Inputs.OnTarget.Disable();

        Debug.Log("InGame is " + Inputs.InGame.enabled + " | OnControls is " + Inputs.OnControls.enabled + " | OnTarget is " + Inputs.OnTarget.enabled + " | OnShooting is " + Inputs.OnShooting.enabled);

    }

    private void ChangeStateOnTarget()
    {
        Debug.Log("Change for targets");

        Inputs.OnControls.Disable();
        Inputs.OnTarget.Enable();
        Inputs.OnShooting.Enable();

        Debug.Log("InGame is " + Inputs.InGame.enabled + " | OnControls is " + Inputs.OnControls.enabled + " | OnTarget is " + Inputs.OnTarget.enabled + " | OnShooting is " + Inputs.OnShooting.enabled);

    }

    private void ChangeStatePause(bool active)
    {
        if (active)
        {
            Inputs.OnControls.Disable();
            Inputs.OnShooting.Disable();
            Inputs.OnTarget.Disable();

            Cursor.visible = true;
        }
        else
        {
            Inputs.OnControls.Enable();
            Inputs.OnShooting.Enable();
            Inputs.OnTarget.Enable();

            Cursor.visible = false;
        }

    }

}

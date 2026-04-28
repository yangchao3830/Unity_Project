using UnityEngine.InputSystem;

public class InputSysTemController : Singleton<InputSysTemController>
{
    private CharacterInputActions _inputActions;

    public CharacterInputActions InputActions => _inputActions;

    private bool _isInitialized = false;

    void Awake()
    {
        if (!_isInitialized)
        {
            _inputActions ??= new CharacterInputActions();
            _isInitialized = true;
        }
    }

    void Onable()
    {
        _inputActions.Player.Enable();   
    }

    void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    void OnDestroy()
    {
        _inputActions.Disable();
    }

    public Vector2 GetMovementInpt()
    {
        return _inputActions.Player.Move.ReadValue<Vector2>();
    }

    public bool GetPlayerConfirmPressed()
    {
        return _inputActions.Player.Confirm.WasPressedThisFrame();
    }

}

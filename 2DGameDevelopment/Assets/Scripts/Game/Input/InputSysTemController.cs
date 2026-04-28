using MFrameWork.Evnet;
using UnityEngine.InputSystem;

public class InputSysTemController : Singleton<InputSysTemController>, IEventReceiver<GameModeChangeEvent>
{
    private CharacterInputActions _inputActions;

    public CharacterInputActions InputActions => _inputActions;

    private bool _isInitialized = false;

    private ActiveMap _currentMap = ActiveMap.None;

    protected override void Awake()
    {
        base.Awake();
        if (!_isInitialized)
        {
            _inputActions ??= new CharacterInputActions();
            _isInitialized = true;
        }
    }

    void OnEnable()
    {
        EvnetBus.Subscribe<GameModeChangeEvent>(this);
    }


    void OnDisable()
    {
        EvnetBus.Unsubscribe<GameModeChangeEvent>(this);
    }

    void OnDestroy()
    {
        _inputActions.Disable();
    }

    public Vector2 GetMovementInpt()
    {
        if (!_isInitialized || _currentMap != ActiveMap.Player) return Vector2.zero;

        return _inputActions.Player.Move.ReadValue<Vector2>();
    }

    public bool GetPlayerConfirmPressed()
    {
           if (!_isInitialized || _currentMap != ActiveMap.Player) return false;
        return _inputActions.Player.Confirm.WasPressedThisFrame();
    }

    #region  事件实现
    public void OnEvent(GameModeChangeEvent evt)
    {
        // print(evt.newMode);
        _currentMap = GetMapFromGameMode(evt.newMode);

        switch (_currentMap)
        {
            case ActiveMap.Player:
                _inputActions.Player.Enable();
                _inputActions.UI.Disable();
                break;
            case ActiveMap.UI:
                _inputActions.Player.Disable();
                _inputActions.UI.Enable();
                break;
            case ActiveMap.None:
            default:
                break;
        }
    }

    private ActiveMap GetMapFromGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Battle:
            case GameMode.InteractionMenu:
            case GameMode.Puase:
                return ActiveMap.UI;
            case GameMode.Explore:
            default:
                return ActiveMap.Player;
        }

    }
}
    #endregion
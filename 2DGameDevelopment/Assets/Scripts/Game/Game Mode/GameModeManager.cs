
public class GameModeManager : Singleton<GameModeManager>
{
   public GameMode currentGameMode;

   [SerializeField] private GameMode defaultMode = GameMode.Explore;

    protected override void Awake()
    {
        base.Awake();
        currentGameMode = defaultMode;
    }

    void Start()
    {
      AppleMode(currentGameMode);       
    }

/// <summary>
/// 外部请求入口
/// </summary>
/// <param name="newMode"></param>
    public void RequesChangeMode(GameMode newMode)
    {
        if(Instance != this) return;
        AppleMode(newMode);        
    }

    public bool CanSwitchMode(GameMode newMode)
    {
        if(currentGameMode == GameMode.Battle) return false;
        return true;
    }

    private void AppleMode(GameMode newMode)
    {
        currentGameMode = newMode;
         EvnetBus.Publish(new GameModeChangeEvent(currentGameMode));
    }
}


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
       EvnetBus.Publish(new GameModeChangeEvent(currentGameMode));
    }
}

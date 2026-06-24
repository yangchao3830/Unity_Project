using MFrameWork.Evnet;
public class CameraModeController : MonoBehaviour, IEventReceiver<GameModeChangeEvent>
{
    [Header("Cameras")]
    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject battleCamera;

    void OnEnable()
    {
       EvnetBus.Subscribe<GameModeChangeEvent>(this);
    }

    void OnDisable()
    {
        EvnetBus.Unsubscribe<GameModeChangeEvent>(this);
    }

    public void OnEvent(GameModeChangeEvent evt)
    {
        switch (evt.newMode)
        {
            case GameMode.Explore:
            SetCameraView(CameraView.Explore);
                break;
            case GameMode.Battle:
             SetCameraView(CameraView.Battle);
                break;
        }
    }
 
    private void SetCameraView(CameraView view)
    {
        bool followActive = false;
        bool battleActive = false;
        switch (view) 
        {
            case CameraView.Explore:
                followActive = true;
                break;
            case CameraView.Battle:
                battleActive = true;
                break;
        }
        followCamera.SetActive(followActive);
        battleCamera.SetActive(battleActive);
    }
}

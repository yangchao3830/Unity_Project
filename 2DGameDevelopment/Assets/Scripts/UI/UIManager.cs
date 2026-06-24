using System;
using MFrameWork.Evnet;

public class UIManager : MonoBehaviour,
IEventReceiver<PanelRequestEvent>
{
    [Header("根节点与特殊面板引用")]
    [SerializeField, Tooltip("探索模式下显示的总体 UI 根节点")]
    private GameObject fieldUIRoot;
    private Dictionary<Type, PanelController> _panelControllerDict = new Dictionary<Type, PanelController>();
    private List<PanelController> _allPanelList = new();

    private void Awake()
    {
        _panelControllerDict.Clear();
        _allPanelList.Clear();

        GetPanelsFromRoot(transform);
    }

    void OnEnable()
    {
        EvnetBus.Subscribe<PanelRequestEvent>(this);
    }

    void OnDisable()
    {
        EvnetBus.Unsubscribe<PanelRequestEvent>(this);
    }

    void Update()
    {
        var mode = GameModeManager.Instance.currentGameMode;
        var input = InputSysTemController.Instance;

        if (mode is GameMode.Battle) return;
        if (mode is GameMode.InteractionMenu)
        {
            if (IsAnyPanelActive() && input.GetUICancelPressed())
            {
                TryHandleCancelByActivePanel();
                GameModeManager.Instance.RequesChangeMode(GameMode.Explore);
                return;
            }
        }
        if (InputSysTemController.Instance.GetUICancelPressed())
        {
            CloseAllPanels();
            GameModeManager.Instance.RequesChangeMode(GameMode.Explore);
        }

    }

    private void GetPanelsFromRoot(Transform root)
    {
        var panels = root.GetComponentsInChildren<PanelController>(true);
        foreach (var panel in panels)
        {
            _allPanelList.Add(panel);
            if (panel.PanelActionType == null) return;
            _panelControllerDict.Add(panel.PanelActionType, panel);
        }
    }

    /// <summary>
    /// 尝试通过活动面板处理取消操作
    /// 历遍所有可能的控制面板，如果处于激活状态，则关闭该面板
    /// </summary>
    private void TryHandleCancelByActivePanel()
    {
        foreach (var panel in _allPanelList)
        {
            if (panel.gameObject.activeSelf)
            {
                panel.gameObject.SetActive(false);
                return;
            }
        }
    }

    /// <summary>
    /// 检查是否有任何面板处于活动状态
    /// </summary>
    /// <returns></returns>
    private bool IsAnyPanelActive()
    {
        foreach (var panel in _allPanelList)
        {
            if (panel.gameObject.activeSelf) return true;
        }
        return false;
    }

    private void CloseAllPanels()
    {
        foreach (var panel in _allPanelList)
        {
            panel.gameObject.SetActive(false);
        }
    }

    #region  事件函数
    public void OnEvent(PanelRequestEvent evt)
    {
        var panelType = evt.actionBase.GetType();

        _panelControllerDict.TryGetValue(panelType, out var panelController);
        panelController?.gameObject.SetActive(true);
        panelController?.SetupPanel(evt.actionBase);
    }
    #endregion

}

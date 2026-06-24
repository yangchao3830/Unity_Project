using UnityEngine.UI;
using UnityEngine.Pool;
using MFrameWork.Evnet;
using UnityEngine.EventSystems;

public class InteractionUIContraoller : MonoBehaviour,
IEventReceiver<InteractionChangedEvent>,
IEventReceiver<InteractionMenuRequesEvent>,
IEventReceiver<GameModeChangeEvent>
{
    [Header("Head Icon")]
    [SerializeField] private RectTransform actionIconHolder;
    [SerializeField] private GameObject actionIconPrefab;

    [Header("Menu Button")]
    [SerializeField] private RectTransform actionMenuHolder;
    [SerializeField] private GameObject actionMenuButtonPrefab;

    private ObjectPool<GameObject> _iconPool;
    private ObjectPool<GameObject> _menuButtonPool;
    private readonly List<GameObject> _actionIcons = new(8);
    private readonly List<GameObject> _actionButtons = new(8);

    private IReadOnlyList<ActionCommandInfo> _currentCommandList;

    private Transform _headAnchor;
    #region  周期函数
    void Awake()
    {
        InitPool();
        actionIconHolder.gameObject.SetActive(false);
        actionMenuHolder.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EvnetBus.Subscribe<InteractionChangedEvent>(this);
        EvnetBus.Subscribe<InteractionMenuRequesEvent>(this);
        EvnetBus.Subscribe<GameModeChangeEvent>(this);
    }

    private void OnDisable()
    {
        EvnetBus.Unsubscribe<InteractionChangedEvent>(this);
        EvnetBus.Unsubscribe<InteractionMenuRequesEvent>(this);
        EvnetBus.Unsubscribe<GameModeChangeEvent>(this);
    }

    void Update()
    {
        if (GameModeManager.Instance.currentGameMode != GameMode.InteractionMenu) return;
        var input = InputSysTemController.Instance;
        if (input.GetUICancelPressed())
        {
            CloseMenu(true);            
        }
    }


    private void LateUpdate()
    {
        if (!actionIconHolder.gameObject.activeSelf || _headAnchor == null) return;
        UpdateHeadIconPosition();
    }
    #endregion
    private void InitPool()
    {

        _iconPool = new ObjectPool<GameObject>(
           createFunc: () => Instantiate(actionIconPrefab, actionIconHolder),
           actionOnGet: (obj) =>
           {
               obj.SetActive(true);
               obj.transform.SetAsLastSibling();
           },
           actionOnRelease: (obj) => obj.SetActive(false),
           actionOnDestroy: (obj) => Destroy(obj),
           defaultCapacity: 8,
           maxSize: 20
       );

        _menuButtonPool = new ObjectPool<GameObject>(
           createFunc: () => Instantiate(actionMenuButtonPrefab, actionMenuHolder),
           actionOnGet: (obj) =>
           {
               obj.SetActive(true);
               obj.transform.SetAsLastSibling();
           },
           actionOnRelease: (obj) => obj.SetActive(false),
           actionOnDestroy: (obj) => Destroy(obj),
           defaultCapacity: 8,
           maxSize: 20
       );
    }

    private void UpdateHeadIconPosition()
    {
        var worldPos = _headAnchor.position;
        var screeenPos = Camera.main.WorldToScreenPoint(worldPos);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screeenPos, null, out var localPoint))
        {
            actionIconHolder.anchoredPosition = localPoint;
        }
    }

    #region 对象池相关方法
    private void SyncPool(List<GameObject> activeList, ObjectPool<GameObject> pool, int tragetCount)
    {
        while (activeList.Count > tragetCount)
        {
            int lastIndex = activeList.Count - 1;
            GameObject item = activeList[lastIndex];
            pool.Release(item);
            activeList.RemoveAt(lastIndex);
        }

        while (activeList.Count < tragetCount)
        {
            GameObject item = pool.Get();
            activeList.Add(item);
        }

    }
    private void ReleaseAll(List<GameObject> activeList, ObjectPool<GameObject> pool)
    {
        for (int i = 0; i < activeList.Count; i++)
        {
            pool.Release(activeList[i]);
        }
        activeList.Clear();
    }

    #endregion

    #region 事件相关方法

    /// <summary>
    /// 启动头顶Icon
    /// </summary>
    /// <param name="evt"></param>Icon
    public void OnEvent(InteractionChangedEvent evt)
    {
        if (!evt.inRange || evt.target is null)
        {
            actionIconHolder.gameObject.SetActive(false);
            ReleaseAll(_actionIcons, _iconPool);
            return;
        }
        //启动显示头顶
        _currentCommandList = evt.target.CachedCommandInfo;
        _headAnchor = evt.target.HeadAnchor;

        ShowHeadIcons();
    }


    /// <summary>
    /// 启动菜单
    /// </summary>
    /// <param name="evt"></param>Icon
    public void OnEvent(InteractionMenuRequesEvent evt)
    {
        //关闭显示头顶Icon
        actionIconHolder.gameObject.SetActive(false);
        ReleaseAll(_actionIcons, _iconPool);

        actionMenuHolder.gameObject.SetActive(true);
        OpenMenu(evt.target);
    }

    public void OnEvent(GameModeChangeEvent evt)
    {
        if(evt.newMode == GameMode.InteractionMenu) return;

        if(evt.newMode == GameMode.Explore)
        {
            if(_currentCommandList is not null && _currentCommandList.Count > 0)
            {
                ShowHeadIcons();
            }
        }

    }

    #endregion


    private void ShowHeadIcons()
    {
        if (_currentCommandList.Count == 0) return;
        actionIconHolder.gameObject.SetActive(true);
        SyncPool(_actionIcons, _iconPool, _currentCommandList.Count);

        for (int i = 0; i < _actionIcons.Count; i++)
        {
            var obj = _actionIcons[i];
            var cmd = _currentCommandList[i];

            obj.GetComponent<Image>().sprite = cmd.Icon;
        }
    }

    /// <summary>
    /// 打开交互菜单，设置菜单按钮并处理用户交互
    /// </summary>
    /// <param name="target"></param>
    private void OpenMenu(InteractionBase target)
    {
        //通知GameMode 切换
        GameModeManager.Instance.RequesChangeMode(GameMode.InteractionMenu);

        SyncPool(_actionButtons, _menuButtonPool, _currentCommandList.Count);

        Button firstButton = null;

        for (int i = 0; i < _actionButtons.Count; i++)
        {
            var btn = _actionButtons[i];
            var cmd = _currentCommandList[i];

            int commanIndex = i;
            btn.GetComponent<ActionMenuButton>().SetButton(cmd, () =>
            {
                target.ExecuteCommandFromUI(commanIndex);
                CloseMenu(false);
            });

            if (firstButton is null)
            {
                firstButton = btn.GetComponent<Button>();
            }
        }
        if (firstButton is not null)
        {
            firstButton.Select();
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
    }

    private void CloseMenu(bool restorHeadIcons)
    {
        HideActionMenu();
        if (restorHeadIcons)
        {
            ShowHeadIcons();
        }
        else
        {
            HideHeadIcons();
        }
    }

    private void HideHeadIcons()
    {
        actionIconHolder?.gameObject.SetActive(false);
        ReleaseAll(_actionIcons, _iconPool);
    }

    private void HideActionMenu()
    {
        actionMenuHolder?.gameObject.SetActive(false);
        ReleaseAll(_actionButtons, _menuButtonPool);
    }


}

using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PanelController : MonoBehaviour
{
    [Header("Action")]
    public ActionBase CurrentAction;

    [Header("Focus Navigation")]
    public Button FirstButton;

    [Header("Focus Navigation")]
    [SerializeField] private Image actionIcon;

    public virtual Type PanelActionType => null;

    public virtual void SetupPanel(ActionBase actionBase)
    {
        CurrentAction = actionBase;
        actionIcon.sprite = actionBase.ConmmandInfo.Icon;
    }

    public virtual void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    protected void OnCancel()
    {
        GameModeManager.Instance.RequesChangeMode(GameMode.Explore);
        ClosePanel();
    }

    protected void OnConfirm()
    {
        CurrentAction.Execute();
        ClosePanel();
    }

    /// <summary>
    /// 设置默认选中的UI元素
    /// 此方法用于界面初始化自动选择第一个按钮
    /// </summary>
    protected void SetDefaultSelection()
    {
        FirstButton.Select();
        EventSystem.current.SetSelectedGameObject(FirstButton.gameObject);
    }

    protected void RebindButtons(Button button,UnityAction unityAction)
    {
       if(button is null) return;
       button.onClick.RemoveAllListeners();
       button.onClick.AddListener(unityAction);
    }
}

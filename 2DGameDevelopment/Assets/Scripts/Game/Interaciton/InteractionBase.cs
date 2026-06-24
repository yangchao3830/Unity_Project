
using System;

public class InteractionBase : MonoBehaviour
{
  [Header("Sign Trans")]
  public Transform HeadAnchor;
  private AllyDefinitionSO _currentInteractor;
  private ActionBase[] _actionsCache;

  private readonly List<ActionCommandInfo> _cachedCommandInfo = new(8);
  private readonly List<VisibleActionEntry> _visibleEntries = new(8);
  public IReadOnlyList<ActionCommandInfo> CachedCommandInfo => _cachedCommandInfo;


  private struct VisibleActionEntry
  {
    public ActionBase Action;
    public ActionCommandInfo CommandInfo;
  }

  void Awake()
  {
    CacheActions();
    HeadAnchor = transform.GetChild(0);
  }
  public void Interact(AllyDefinitionSO interactor)
  {
    EvnetBus.Publish(new InteractionMenuRequesEvent(this));
  }

  public void OnFocus(AllyDefinitionSO interactor)
  {
    CacheActions();
    _currentInteractor = interactor;
    RebuildCommands();
    PublishEvent(true);
  }

  public void OnLoseFocus(AllyDefinitionSO interactor)
  {
    _currentInteractor = null;
    _cachedCommandInfo.Clear();
    PublishEvent(false);

    HeadAnchor.gameObject.SetActive(true);
  }

  private void CacheActions() => _actionsCache = GetComponents<ActionBase>();

  private void RebuildCommands()
  {
    _cachedCommandInfo.Clear();
    _visibleEntries.Clear();
    for (int i = 0; i < _actionsCache.Length; i++)
    {
      var action = _actionsCache[i];
      if (!action.CanShow(_currentInteractor))
      {
        continue;
      }
      _visibleEntries.Add(new VisibleActionEntry
      {
        Action = action,
        CommandInfo = action.ConmmandInfo
      });
    }
    if (_visibleEntries.Count > 1)
    {
      _visibleEntries.Sort((a, b) => a.CommandInfo.Order.CompareTo(b.CommandInfo.Order));
    }
    for (int i = 0; i < _visibleEntries.Count; i++)
    {
      _cachedCommandInfo.Add(_visibleEntries[i].CommandInfo);
    }

    if (_visibleEntries.Count > 0)
    {
      HeadAnchor.gameObject.SetActive(false);
    }
  }

  private void PublishEvent(bool inRange)
  {
    EvnetBus.Publish(new InteractionChangedEvent(this, inRange));
  }

  #region UI 回调入口
  /// <summary>
  /// UI调用
  /// </summary>
  /// <param name="target"></param>
  public bool ExecuteCommandFromUI(int commandIndex)
  {
    if (commandIndex >= _visibleEntries.Count || commandIndex < 0) return false;

    var action = _visibleEntries[commandIndex].Action;
    if (!action.CanExecute(_currentInteractor))
    {
      return false; 
    }
    action.TriggerAction(_currentInteractor);
    return true;

  }
  #endregion

}

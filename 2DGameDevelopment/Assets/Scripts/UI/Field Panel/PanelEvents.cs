using MFrameWork.Evnet;

public readonly struct PanelRequestEvent : IEvent
{
    public readonly ActionBase actionBase;

    public PanelRequestEvent(ActionBase actionBase)
    {
        this.actionBase = actionBase;
    }
}
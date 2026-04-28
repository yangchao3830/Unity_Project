using MFrameWork.Evnet;

public readonly struct GameModeChangeEvent : IEvent
{
    public readonly GameMode newMode;
    public GameModeChangeEvent(GameMode newMode)
    {
        this.newMode = newMode;
    }
}
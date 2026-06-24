using MFrameWork.Evnet;

public readonly struct InteractionChangedEvent : IEvent
{
    public readonly InteractionBase target;
    public readonly bool inRange;

    /// <summary>
    /// 交互提示变化事件
    /// </summary>
    /// <param name="target"></param>
    /// <param name="inRange">inRange=fase 或无可用命令:隐藏头顶 icon + 关闭菜单</param>
    public InteractionChangedEvent(InteractionBase target, bool inRange)
    {
        this.target = target;
        this.inRange = inRange;
    }
}

public readonly struct InteractionMenuRequesEvent : IEvent
{
    public readonly InteractionBase target;
    public InteractionMenuRequesEvent(InteractionBase target)
    {
        this.target = target;
    }

}
using System;
using MFrameWork.Evnet;

public static class EvnetBus
{
    private static Dictionary<Type, List<object>> EventDict = new();

    public static void Subscribe<TEvent>(IEventReceiver<TEvent> receuver) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        if (!EventDict.TryGetValue(eventType, out var receivers))
        {
            receivers = new List<object>();
            EventDict[eventType] = receivers;
        }

        if (!receivers.Contains(receuver))
        {
            receivers.Add(receuver);
        }
    }

    public static void Unsubscribe<TEvent>(IEventReceiver<TEvent> receiver) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

        if (EventDict.TryGetValue(eventType, out var receivers))
        {
            receivers.Remove(receiver);

            if (receivers.Count == 0)
            {
                EventDict.Remove(eventType);
            }
        }
    }

    public static void Publish<TEvent>(TEvent evt) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        if (EventDict.TryGetValue(eventType, out var receivers))
        {
            for (int i = 0; i < receivers.Count; i++)
            {
                var obj = receivers[i];

                if(obj is UnityEngine.Object unityObj && unityObj ==null)
                {
                    receivers.RemoveAt(i);
                    continue;
                }
                ((IEventReceiver<TEvent>)receivers[i]).OnEvent(evt);
            }
        }
    }

}

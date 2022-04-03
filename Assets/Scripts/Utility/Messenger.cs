using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TypedEvent : UnityEvent<object> { }

public class Messenger : MonoBehaviour
{
    private Dictionary <MessageIDs, UnityEvent> eventDictionary;
    private Dictionary<MessageIDs, TypedEvent> typedEventDictionary;

    private static Messenger messenger;
    public static Messenger instance
    {
        get
        {
            if (!messenger)
            {
                messenger = FindObjectOfType(typeof (Messenger)) as Messenger;

                if (!messenger)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    messenger.Init(); 
                }
            }

            return messenger;
        }
    }

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<MessageIDs, UnityEvent>();
            typedEventDictionary = new Dictionary<MessageIDs, TypedEvent>();
        }
    }

    public static void Subscribe(MessageIDs eventID, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.AddListener(listener);
        } 
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventID, thisEvent);
        }
    }

    public static void Subscribe(MessageIDs eventID, UnityAction<object> listener)
    {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TypedEvent();
            thisEvent.AddListener(listener);
            instance.typedEventDictionary.Add(eventID, thisEvent);
        }
    }

    public static void Unsubscribe(MessageIDs eventID, UnityAction listener)
    {
        if (messenger == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    public static void Unsubscribe(MessageIDs eventID, UnityAction<object> listener)
    {
        if (messenger == null) return;
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void SendMessage(MessageIDs eventID)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void SendMessage(MessageIDs eventID, object data)
    {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventID, out thisEvent))
        {
            thisEvent.Invoke(data);
        }
    }
}
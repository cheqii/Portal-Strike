using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Nf_CustomGameEvent : UnityEvent<Component>{}

public class Nf_EventListener : MonoBehaviour
{
    public Nf_GameEvent GameEvent;
    public Nf_CustomGameEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        GameEvent.UnRegisterListener(this);
    }
    public void OnEventRaise(Component sender)
    {
        Response.Invoke(sender);
    }
}

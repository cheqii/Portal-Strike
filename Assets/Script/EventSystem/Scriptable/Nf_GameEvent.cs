using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Nefer_GameEvent")]
public class Nf_GameEvent : ScriptableObject
{
    [HideInInspector]
    public List<Nf_EventListener> Listeners = new List<Nf_EventListener>();

    //use this method if you want to raise event.
    public void Raise()
    {
        foreach (var item in Listeners)
        {
            item.OnEventRaise(null);
        }
    }
    
    //use this method if you want to raise event and use some sender variable.
    public void Raise(Component sender)
    {
        foreach (var item in Listeners)
        {
            item.OnEventRaise(sender);
        }
    }
    
    
    public void RegisterListener(Nf_EventListener Listener)
    {
        if (!Listeners.Contains(Listener))
        {
            Listeners.Add(Listener);
        }
    }
    
    public void UnRegisterListener(Nf_EventListener Listener)
    {
        if (Listeners.Contains(Listener))
        {
            Listeners.Remove(Listener);
        }
    }
    
}

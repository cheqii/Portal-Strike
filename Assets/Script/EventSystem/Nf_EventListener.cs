using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

[System.Serializable]
public class Nf_CustomGameEvent : UnityEvent<Component, object>{} 

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
    public void OnEventRaise(Component sender, Object data)
    {
        Response.Invoke(sender, data);
    }
}

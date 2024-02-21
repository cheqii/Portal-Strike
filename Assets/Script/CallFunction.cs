using UnityEngine;

public class CallFunction : MonoBehaviour
{
    public MonoBehaviour script;

    public void Call(string name)
    {
        script.Invoke(name,0.1f);
    }
}
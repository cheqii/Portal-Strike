using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QE_Collision_3d : MonoBehaviour
{
    [HideInInspector]
    public EventList OnColEnter, OnColExit,OnColStay;

    
    private void OnCollisionEnter(Collision collision)
    {
        OnColEnter.Response.Invoke();
    }
    private void OnCollisionExit(Collision other)
    {
        OnColExit.Response.Invoke();

    }
    private void OnCollisionStay(Collision collisionInfo)
    {
        OnColStay.Response.Invoke();

    }
}

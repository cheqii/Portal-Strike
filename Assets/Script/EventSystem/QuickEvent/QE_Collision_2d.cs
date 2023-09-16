using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QE_Collision_2d : MonoBehaviour
{
    [HideInInspector]
    public EventList OnColEnter, OnColExit,OnColStay;

    

    private void OnCollisionEnter2D(Collision2D col)
    {
        OnColEnter.Response.Invoke();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        OnColExit.Response.Invoke();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnColStay.Response.Invoke();
    }
}

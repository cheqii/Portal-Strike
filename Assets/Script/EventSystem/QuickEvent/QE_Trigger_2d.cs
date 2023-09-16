using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QE_Trigger_2d : MonoBehaviour
{
    public EventList _OnTriggerEnter, _OnTriggerExit, _OnTriggerStay;

    

    private void OnTriggerEnter2D(Collider2D col)
    {
        _OnTriggerEnter.Response.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _OnTriggerExit.Response.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _OnTriggerStay.Response.Invoke();
    }
    
}

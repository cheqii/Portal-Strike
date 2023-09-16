using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QE_Trigger_3d : MonoBehaviour
{
    public EventList _OnTriggerEnter, _OnTriggerExit, _OnTriggerStay;


    private void OnTriggerEnter(Collider other)
    {

        _OnTriggerEnter.Response.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _OnTriggerExit.Response.Invoke();

    }

    private void OnTriggerStay(Collider other)
    {
        _OnTriggerStay.Response.Invoke();
    }
}

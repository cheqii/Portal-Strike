using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPortal : MonoBehaviour
{
    public MiniPortal portalOut;

    private void OnTriggerEnter(Collider other)
    {
        if (portalOut == null)
        {
            return;
        }
        
        switch (other.transform.tag)
        {
            case "Bullet":

                WarpObject(other.gameObject);

                break;
        }
        
    }

    private void WarpObject(GameObject go)
    {
        go.transform.position = portalOut.transform.position;
        go.transform.eulerAngles = -portalOut.transform.eulerAngles;

        if (go.CompareTag("Bullet"))
        {
            go.GetComponent<Rigidbody>().velocity = -go.GetComponent<Rigidbody>().velocity;
        }
    }
}

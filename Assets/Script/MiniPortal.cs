using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPortal : MonoBehaviour
{
    public MiniPortal portalOut;

    private float moveSpeed;
    private Movement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<Movement>();
        
        moveSpeed = playerMovement.moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                PlayerDash();
                break;
        }
        
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

    private void PlayerDash()
    {
        playerMovement.moveSpeed *= 6;
        Invoke("ResetSpeed", 0.15f);
    }

    void ResetSpeed()
    {
        playerMovement.moveSpeed = moveSpeed;
    }
}

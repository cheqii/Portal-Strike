using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MiniPortal : MonoBehaviour
{
    public MiniPortal portalOut;
    
    private float moveSpeed;
    private Movement playerMovement;

    private BuildPortal portalBuild;

    private void Start()
    {
        portalBuild = FindObjectOfType<BuildPortal>();
        
        playerMovement = FindObjectOfType<Movement>();
        
        moveSpeed = playerMovement.moveSpeed;
    }

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
            case "Player":
                if (portalBuild.MyPortalOut == null) PlayerDash();
                // if (portalBuild.MyPortalOut == null) return;
                PlayerWarp(other.gameObject);
                break;
        }
        
    }

    private void WarpObject(GameObject go)
    {
        go.transform.position = portalOut.transform.position;
        go.transform.eulerAngles = -portalOut.transform.eulerAngles;
        Debug.Log("Warp Object");
        
        switch (go.gameObject.tag)
        {
            case "Bullet":
                go.GetComponent<Rigidbody>().velocity = -go.GetComponent<Rigidbody>().velocity;
                break;
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
    void PlayerWarp(GameObject go)
    {
        // Debug.Log("Player Warp");
        // var Player = FindObjectOfType<Player>().transform;
        go.GetComponent<CharacterController>().enabled = false;
        go.transform.position = portalOut.gameObject.transform.position;
        go.GetComponent<CharacterController>().enabled = true;


    }
}

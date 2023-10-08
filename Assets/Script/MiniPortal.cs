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
            switch (other.transform.tag)
            {
                case "Player":
                    if (portalBuild.MyPortalIn != null && portalBuild.MyPortalOut != null)
                    {
                        Debug.Log("Destroy Portal");
                        GameObject blood = Instantiate(ParticleManager.Instance.data.BloodBomb_particle
                            , portalBuild.MyPortalIn.transform.position, Quaternion.identity);
                        Destroy(portalBuild.MyPortalIn.gameObject);
                        other.gameObject.GetComponent<TraumaInducer>().HardShake();
                    }
                    break;
            }
            return;
        }
        
        switch (other.transform.tag)
        {
            case "Bullet":
                WarpObject(other.gameObject);
                break;
            case "Player":
                if (portalBuild.MyPortalOut == null) PlayerDash();
                if (portalBuild.MyPortalIn != null && portalBuild.MyPortalOut != null) PlayerWarp(other.gameObject);
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
        FindObjectOfType<PlayerAnimation>().SetAnim("Hurt");
        Debug.Log("Dash");
        playerMovement.moveSpeed *= 6;
        GameObject Dash_Effect = Instantiate(ParticleManager.Instance.data.Dash_particle, playerMovement.transform.position,
            playerMovement.transform.rotation);
        Dash_Effect.transform.SetParent(playerMovement.gameObject.transform);
        
        Destroy(Dash_Effect,0.5f);
        
        Invoke("ResetSpeed", 0.15f);
    }

    void ResetSpeed()
    {
        playerMovement.moveSpeed = moveSpeed;
    }
    void PlayerWarp(GameObject go)
    {
        FindObjectOfType<PlayerAnimation>().SetAnim("Hurt");

        Debug.Log("Warp");
        go.GetComponent<CharacterController>().enabled = false;
        go.transform.position = portalOut.gameObject.transform.position + new Vector3(0,0, 1.5f);
        go.GetComponent<CharacterController>().enabled = true;
    }
}

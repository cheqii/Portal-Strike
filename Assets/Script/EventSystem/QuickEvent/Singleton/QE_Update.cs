using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QE_Update : MonoBehaviour
{
    [HideInInspector]
    public List<EventList> OnUpdate, OnFixedUpdate;
    
    
    public static QE_Update Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (OnUpdate.Count > 0)
        {
            foreach (var item in OnUpdate)
            {
                item.Response.Invoke();
            }
        }
       
    }

    private void FixedUpdate()
    {
        if (OnFixedUpdate.Count > 0)
        {
            foreach (var item in OnFixedUpdate)
            {
                item.Response.Invoke();
            }
        }
    }

}

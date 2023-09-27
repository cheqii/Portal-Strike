using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;

    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float detectRadius;
    // [SerializeField] private bool playerTrigger;
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        if (col != null) 
        {
            col.radius = detectRadius; // Set the new value
            Debug.Log(col.radius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if (target != null)
        {
            // Calculate the direction from the enemy to the player.
            Vector3 moveDirection = (target.position - transform.position).normalized;

            // Move the enemy in the calculated direction.
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}

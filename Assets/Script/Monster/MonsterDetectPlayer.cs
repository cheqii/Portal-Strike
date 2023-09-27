using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDetectPlayer : MonoBehaviour
{
    [Header("Monster")] 
    [SerializeField] private MonsterData monData;
    [SerializeField] private NavMeshAgent monster;
    [SerializeField] private float detectRadius;
    [SerializeField] private float stopFollow;
    
    private Player target;
    
    [Header("Distance between player & monster")]
    private float awayFromPlayer;
    private Vector2 beginPos;
    private Vector2 targetPos;
    

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * detectRadius, Color.red);
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if (target == null) return;
        
        Ray ray = new Ray(transform.position, transform.forward * detectRadius);
        RaycastHit hit;
        CalculateTargetDistance();
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(transform.position, transform.forward * detectRadius, Color.cyan);
            Debug.Log("Attack Player!");
        }

        // if distance between player and monster is lower than stop following var then monster will follow the player
        switch (monData.monsterType)
        {
            case MonsterData.MonsterType.Melee:
                if (awayFromPlayer <= stopFollow)
                {
                    monster.stoppingDistance = monData.attackRange;
                    monster.SetDestination(target.transform.position);
                }
                break;
            case MonsterData.MonsterType.Range:
                if (awayFromPlayer <= stopFollow)
                {
                    monster.stoppingDistance = monData.attackRange;
                    monster.SetDestination(target.transform.position);
                }
                break;
        }
    }

    void CalculateTargetDistance()
    {
        beginPos = transform.position;
        targetPos = target.transform.position;
        
        // Calculate distance between monster and player
        awayFromPlayer = Mathf.Sqrt(Mathf.Pow(targetPos.x - beginPos.x, 2) + Mathf.Pow(targetPos.y - beginPos.y, 2));
        Debug.Log(awayFromPlayer);
    }
}

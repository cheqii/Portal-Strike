using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] private MonsterData mondata;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<ITakeDamage>().TakeDamage(mondata.atkDamage);
            Destroy(gameObject);
        }
    }
}

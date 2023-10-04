using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<ITakeDamage>().TakeDamage(10);
        }
        
    }
}
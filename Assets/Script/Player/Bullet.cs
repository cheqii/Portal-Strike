using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            GameObject blood = Instantiate(ParticleManager.Instance.data.BloodSplash_particle, transform.position, Quaternion.identity);
            blood.transform.SetParent(col.transform);
            col.gameObject.GetComponent<ITakeDamage>().TakeDamage(10);
        }
        
    }
}
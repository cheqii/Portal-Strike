using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    
    private void OnCollisionEnter(Collision col)
    {
        var enemy = col.transform.GetComponent<Enemy>();
        if (col.gameObject.CompareTag("Enemy"))
        {
            // DynamicTextData data = enemy.TextData
            // int damage = enemy.
            GameObject blood = Instantiate(ParticleManager.Instance.data.BloodSplash_particle, transform.position, Quaternion.identity);
            blood.transform.SetParent(col.transform);
            col.gameObject.GetComponent<ITakeDamage>().TakeDamage(damage);
        }
        
    }

    public void ChangeDamage(int dmg)
    {
        damage = dmg;
    }
}
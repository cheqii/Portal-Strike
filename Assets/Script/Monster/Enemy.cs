using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,ITakeDamage
{
    [SerializeField] private int hp = 100;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        
        if (hp <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        GameObject xp = Instantiate(ParticleManager.Instance.data.Xp_particle,transform.position,Quaternion.identity);
        xp.AddComponent<SmoothCameraFollow>();
        xp.GetComponent<SmoothCameraFollow>().target = FindObjectOfType<Player>().transform;
        xp.GetComponent<SmoothCameraFollow>().offset = Vector3.zero;
        xp.GetComponent<SmoothCameraFollow>().smoothSpeed = 2.5f;

        Destroy(this.gameObject);
    }
}

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
            Destroy(this.gameObject);
        }
    }
}

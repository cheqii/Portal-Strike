using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;

public class Enemy : MonoBehaviour,ITakeDamage
{
    [SerializeField] private MonsterData mondata;
    [SerializeField] private int hp;
    [SerializeField] private Transform shootPoint;
    public Player target;
    
    [SerializeField] private MicroBar _microBar;
    
    void Awake()
    {
        target = FindObjectOfType<Player>();
    }

    private void Start()
    {
        hp = mondata.hp;
        _microBar.Initialize(mondata.hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        
        _microBar.UpdateHealthBar(hp);
        
        if (hp <= 0)
        {
            Dead();
        }
    }

    #region -Monster Attack Behavior-

    // Melee monster attack
    public void MeleeAttack()
    {
        if (target == null) return;
        Ray ray = new Ray(transform.position, transform.forward * mondata.attackRange);
        
    }
    
    // Range monster attack
    public IEnumerator RemoteAttack()
    {
        while (true)
        {
            if (target != null)
            {
                GameObject bullet = Instantiate(mondata.bullet, shootPoint.position, shootPoint.transform.rotation);
                bullet.transform.LookAt(target.gameObject.transform);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = bullet.transform.forward * mondata.atkSpeed;

                yield return new WaitForSeconds(mondata.atkCoolDown);
            }
            else
            {
                yield break;
            }
        }
    }

    #endregion

    public void Dead()
    {
        GameObject xp = Instantiate(ParticleManager.Instance.data.Xp_particle,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
}

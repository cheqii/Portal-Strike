using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,ITakeDamage
{
    [SerializeField] private MonsterData mondata;
    [SerializeField] private int hp;
    [SerializeField] private Transform shootPoint;
    public Player target;
    
    void Awake()
    {
        target = FindObjectOfType<Player>();
    }

    private void Start()
    {
        hp = mondata.hp;
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

    #region -Monster Attack Behavior-

    public IEnumerator RemoteAttack()
    {
        while (true)
        {
            if (target != null)
            {
                if (mondata.monsterType == MonsterData.MonsterType.Range)
                {
                    GameObject bullet = Instantiate(mondata.bullet, shootPoint.position, shootPoint.transform.rotation);
                    bullet.transform.LookAt(target.gameObject.transform);
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    rb.velocity = bullet.transform.forward * mondata.atkSpeed;
                    Debug.Log("Enemy Shot");

                    yield return new WaitForSeconds(mondata.atkCoolDown);
                }
            }
            else
            {
                Debug.Log("Break coroutine");
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

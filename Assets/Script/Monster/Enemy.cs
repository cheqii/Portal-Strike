using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;

public class Enemy : MonoBehaviour,ITakeDamage
{
    [SerializeField] private MonsterData mondata;
    [SerializeField] private int hp;
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform shootPoint;
    private bool isAttack;
    private Player target;
    
    [Header("Enemy Health Bar")]
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
        Debug.DrawRay(weapon.transform.position, weapon.transform.forward * mondata.attackRange, Color.red);
    }

    public void TakeDamage(int dmg)
    {
        StartCoroutine(WhiteFlash());
        
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
        Ray ray = new Ray(weapon.transform.position, weapon.transform.forward * mondata.attackRange);
        RaycastHit hit;
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        rb.AddTorque(new Vector3(0f, 30f, 0f));
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name != "Player") return;
            rb.velocity = Vector3.zero;
        }
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
        Instantiate(ParticleManager.Instance.data.BloodBomb_particle, transform.position, Quaternion.identity);
        GameObject xp = Instantiate(ParticleManager.Instance.data.Xp_particle,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator WhiteFlash()
    {
        Material bodyMat = GetComponent<Renderer>().material;
        bodyMat.SetColor("_EmissionColor", Color.white * 2);
        
        yield return new WaitForSeconds(0.5f);
        bodyMat.SetColor("_EmissionColor", Color.white * 0);
    }
}

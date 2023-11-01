using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Microlight.MicroBar;
using TMPro;

public class Enemy : MonoBehaviour,ITakeDamage
{
    [SerializeField] private MonsterData mondata;
    [SerializeField] private int hp;
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform shootPoint;
    
    private Player target;
    
    [Header("Enemy Health Bar")]
    [SerializeField] private MicroBar _microBar;

    [Header("Float Text")] 
    [SerializeField] private GameObject floatingTextPrefab;

    [Header("Animator")] 
    private EnemyAnimations animation;
    
    void Awake()
    {
        target = FindObjectOfType<Player>();
    }

    private void Start()
    {
        animation = GetComponent<EnemyAnimations>();
        
        hp = mondata.hp;
        _microBar.Initialize(hp);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(weapon.transform.position, weapon.transform.up * mondata.attackRange, Color.red);
    }

    public void TakeDamage(int dmg)
    {
        animation.TriggerGetHitAnim();
        
        GetComponent<TraumaInducer>().Shake();

        StartCoroutine(WhiteFlash());
        
        hp -= dmg;
        
        _microBar.UpdateHealthBar(hp);

        if (floatingTextPrefab)
        {
            ShowFloatingText();
        }
        
        if (hp <= 0)
        {
            Dead();
        }
    }

    void ShowFloatingText()
    {
        var text = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = target.AtkDamage.ToString();
    }
    
    #region -Monster Attack Behavior-

    // Melee monster attack
    public void MeleeAttack()
    {
        if (target == null) return;
        
        Ray ray = new Ray(weapon.transform.position, weapon.transform.up * mondata.attackRange);
        RaycastHit hit;
        transform.LookAt(target.gameObject.transform);
        animation.TriggerAttackAnim();
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag != "Player") return;
            target.GetComponent<ITakeDamage>().TakeDamage(mondata.atkDamage);
        }
    }
    
    // Range monster attack
    public void RemoteAttack()
    {
        if (target != null)
        {
            GameObject bullet = Instantiate(mondata.bullet, shootPoint.position, shootPoint.transform.rotation);
            bullet.transform.LookAt(target.gameObject.transform);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bullet.transform.forward * mondata.atkSpeed;
        }
    }

    #endregion

    public void Dead()
    {
        animation.TriggerDieAnim();
        GetComponent<TraumaInducer>().HardShake();
        GameObject blood = Instantiate(ParticleManager.Instance.data.BloodBomb_particle, transform.position, Quaternion.identity);
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

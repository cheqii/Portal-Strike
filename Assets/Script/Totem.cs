using System;
using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;
using TMPro;

public class Totem : MonoBehaviour, ITakeDamage
{
    [Header("Totem Attack")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootingPoint;

    private Transform player;
    
    [Header("Totem distance")]
    [SerializeField] private float distance = 100;
    [SerializeField]  private float delay = 4;
    [SerializeField] private float shootDistance = 5f;

    [Header("Totem Hp Bar")]
    [SerializeField] private MicroBar microBar;
    [SerializeField] private int hp;

    [SerializeField] private GameObject floatingText;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>().gameObject.transform;
    }

    private void Start()
    {
        microBar.Initialize(hp);
        StartCoroutine(IEnu_TotemShoot());
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        distance = Vector3.Distance (this.transform.position, player.position);
    }
    

    IEnumerator IEnu_TotemShoot()
    {
        while (true)
        {
            if (distance < shootDistance)
            {
                GameObject totem_bullet = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
                totem_bullet.GetComponent<BossBullet>().target = player;
            }
            yield return new WaitForSeconds(delay);
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        
        microBar.UpdateHealthBar(hp);

        if (floatingText) ShowFloatingText();
        
        if (hp <= 0) Destroy(this.gameObject);
    }

    void ShowFloatingText()
    {
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = player.GetComponent<Player>().AtkDamage.ToString();
    }
}

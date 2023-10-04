using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class Player : MonoBehaviour, ITakeDamage
{
    #region -Declared Variables-

    [Header("Player Status")]
    [SerializeField] private float hp;
    public float Hp
    {
        get => hp;
        set => hp = value;
    }
    [SerializeField] private float maxHp;
    public float MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }
    [SerializeField] private float moveSpeed;
    [SerializeField] private int def;
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;
    [SerializeField] private float dodgeRate;

    [SerializeField] private WeaponData weaponData;

    [Header("Event System")]
    [SerializeField] private Nf_GameEvent takeDamageEvent;
    private Nf_GameEvent playerHealthEvent;
    
    #endregion

    #region -Unity Medthod Function-

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region -TakeDamage & Healing-

    public void TakeDamage(int dmg)
    {
        takeDamageEvent.Raise(this, dmg);
    }

    public void DealDamage(Component sender,Object data)
    {
        if (data is int)
        {
            int damage = (int) data;
            hp = Mathf.Clamp(hp - ((damage - def) + 1), 0, maxHp);
        }

        if (hp < 1) Debug.Log("Player ded");
    }

    public void Healing(float heal)
    {
        hp = Mathf.Clamp(hp + heal, 0, maxHp);
    }

    #endregion
}

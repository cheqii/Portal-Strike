using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class Player : MonoBehaviour, ITakeDamage
{
    #region -Declared Variables-

    [Header("Player Status")]
    [SerializeField] private int hp;
    public int Hp
    {
        get => hp;
        set => hp = value;
    }
    [SerializeField] private int maxHp;
    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    private int level;
    public int Level
    {
        get => level;
        set => level = value;
    }
    
    [SerializeField] private int xp;
    public int Xp
    {
        get => xp;
        set => xp = value;
    }
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private int def;
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;
    [SerializeField] private float dodgeRate;

    [SerializeField] private WeaponData weaponData;

    [Header("Event System")]
    [SerializeField] private Nf_GameEvent takeDamageEvent;
    [SerializeField] private Nf_GameEvent playerHealthEvent;
    [SerializeField] private Nf_GameEvent playerLevelUpEvent;

    
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

    public void IncreaseXp(Component sender,Object data)
    {
        if (data is int)
        {
            xp += (int)data;
        }


        if (xp > 100)
        {
            xp = 0;
            level++;
            playerLevelUpEvent.Raise();
        }
    }
    

    public void Healing(int heal)
    {
        hp = Mathf.Clamp(hp + heal, 0, maxHp);
    }

    #endregion

    #region -All ability-
    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
        hp = Mathf.Clamp(hp, 0, maxHp);
    }
    public void IncreaseMoveSpeed(float amount)
    {
        float increasePercentage = amount / 100f;
        float speedIncrease = moveSpeed * increasePercentage;
        moveSpeed += speedIncrease;
    }

    public void IncreaseDef(int amount)
    {
        def += amount;
    }
    #endregion
}

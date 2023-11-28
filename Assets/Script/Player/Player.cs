using UnityEngine;
using Object = System.Object;

public class Player : MonoBehaviour, ITakeDamage
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject clearPortal;
    private bool oneDie = false;

    #region -Declared Variables-

    [Header("Player HP & EXP")]
    [SerializeField] private int hp = 1000;
    public int Hp {
        get => hp;
        set => hp = value;
    }
    [SerializeField] private int maxHp = 1000;
    public int MaxHp {
        get => maxHp;
        set => maxHp = value;
    }
    private int level = 1;
    public int Level {
        get => level;
        set => level = value;
    }
    [SerializeField] private int xp = 0;
    public int Xp {
        get => xp;
        set => xp = value;
    }
    [SerializeField] private int maxXp = 100;
    public int MaxXp {
        get => maxXp;
        set => maxXp = value;
    }
    [Header("Player Stats")]
    [SerializeField] private int def = 0;
    public int Def {
        get => def;
        set => def = value;
    }
    [SerializeField] private int atkDamage = 10;
    public int AtkDamage
    {
        get => atkDamage;
        set => atkDamage = value;
    }
    [SerializeField] private float critDamage = 10.0f;
    public float CritDamage {
        get => critDamage;
        set => critDamage = value;
    }
    [SerializeField] private float critRate = 10.0f;
    public float CritRate {
        get => critRate;
        set => critRate = value;
    }
    [SerializeField] private float moveSpeed = 10.0f;
    public float MoveSpeed {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    [SerializeField] private float dodgeRate = 10.0f;
    public float DodgeRate {
        get => dodgeRate;
        set => dodgeRate = value;
    }
    [SerializeField] private int enemiesCount = 0;
    public int EnemyCounnt {
        get => enemiesCount;
        set => enemiesCount = value;
    }

    [Header("Another")]
    [SerializeField] private WeaponData weaponData;

    [Header("Event System")]
    [SerializeField] private Nf_GameEvent takeDamageEvent;
    [SerializeField] private Nf_GameEvent playerLevelUpEvent;
    [SerializeField] private Nf_GameEvent skipableAdsEvent;
    [SerializeField] private Nf_GameEvent enemiesCountUpdate;
    [SerializeField] private Nf_GameEvent healthBarUpdate;

    #endregion

    #region -Unity Medthod Function-

    private void Awake()
    {
        hp = maxHp;
    }

    public void EnemiesCount()
    {
        enemiesCount += 1;
        enemiesCountUpdate.Raise();
        if(enemiesCount >= 95)
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(0, -2, 5);
            Instantiate(clearPortal, newPosition, Quaternion.identity);
        }
    }

    #endregion

    #region -TakeDamage & Healing-

    public void TakeDamage(int dmg)
    {
        takeDamageEvent.Raise(this, dmg -def);
    }

    public void DealDamage(Component sender, Object data)
    {
        if (data is int)
        {
            int damage = (int)data;
            hp = Mathf.Clamp(hp - (damage - def), 0, maxHp);
        }

        if (hp < 1)
        {
            oneDie = true;
            if(oneDie == true)
            {
                hp = 5;
                oneDie = false;

                skipableAdsEvent.Raise();
                gameOverPanel.SetActive(true);

                Time.timeScale = 0;
            }
        }
    }

    public void IncreaseXp(Component sender,Object data)
    {
        if (data is int)
        {
            xp += (int)data;
        }


        if (xp > maxXp)
        {
            xp = 0;
            level++;
            playerLevelUpEvent.Raise();
        }
    }
    

    public void Healing(int heal)
    {
        hp += heal;

        if(hp > maxHp)
        {
            hp = maxHp;
        }
        healthBarUpdate.Raise();
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
        float increasePercentage = amount / 100.0f;
        float speedIncrease = moveSpeed * increasePercentage;
        moveSpeed += speedIncrease;
    }
    public void IncreaseDef(int amount)
    {
        def += amount;
    }
    public void IncreaseCritRate(float amount)
    {
        float increasePercentage = amount / 100.0f;
        float critRateIncrease = critRate * increasePercentage;
        critRate += critRateIncrease;
    }
    public void IncreaseCritDamage(float amount)
    {
        float increasePercentage = amount / 100.0f;
        float critDamageIncrease = critDamage * increasePercentage;
        critDamage += critDamageIncrease;
    }
    public void IncreaseDodgeRate(float amount)
    {
        float increasePercentage = amount / 100.0f;
        float dodgeRateIncrease = dodgeRate * increasePercentage;
        dodgeRate += dodgeRateIncrease;
    }
    #endregion
}
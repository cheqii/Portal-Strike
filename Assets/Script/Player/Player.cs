using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ITakeDamage
{
    #region -Declared Variables-

    [Header("Player Status")]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int def;
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;
    [SerializeField] private float dodgeRate;

    [SerializeField] private WeaponData weaponData;

    [Header("Player UI Text")] 
    [SerializeField] private Image currentHp;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text currentHpIndex;

    private float hpLerpSpeed;

    private Nf_GameEvent takeDamageEvent;
    
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
        HealthUpdate();
    }

    #endregion

    #region -TakeDamage & Healing-

    public void TakeDamage(int dmg)
    {
        hp = Mathf.Clamp(hp - ((dmg - def) + 1), 0, maxHp);

        if (hp < 1) Debug.Log("Player ded");
    }

    public void Healing(float heal)
    {
        hp = Mathf.Clamp(hp + heal, 0, maxHp);
    }

    void ColorChanger()
    {
        /* Create new color
         * color8CFF41 is green
         * colorFF4040 is red */
        Color green = new (140f / 255f, 1f, 65f / 255f);
        Color red = new (1f, 64f / 255f, 64f / 255f);

        Color healthColor = Color.Lerp(red, green, (hp / maxHp));
        currentHp.color = healthColor;
        hpText.color = healthColor;
    }

    void HealthUpdate()
    {
        hpLerpSpeed = 3f * Time.deltaTime;
        currentHp.fillAmount = Mathf.Lerp(currentHp.fillAmount, hp / maxHp, hpLerpSpeed);
        currentHpIndex.text = $"{hp} / {maxHp}";
        ColorChanger();
    }

    #endregion
}

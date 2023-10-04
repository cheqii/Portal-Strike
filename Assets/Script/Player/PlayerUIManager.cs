using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    
    [Header("Player UI Text")] 
    [SerializeField] private Image currentHpBar;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text currentHpIndex;

    private float hpLerpSpeed;
    private Player player;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthUpdate();
    }
    
    void ColorChanger()
    {
        /* Create new color
         * color8CFF41 is green
         * colorFF4040 is red */
        Color green = new (140f / 255f, 1f, 65f / 255f);
        Color red = new (1f, 64f / 255f, 64f / 255f);

        Color healthColor = Color.Lerp(red, green, (player.Hp / player.MaxHp));
        currentHpBar.color = healthColor;
        hpText.color = healthColor;
    }

    void HealthUpdate()
    {
        hpLerpSpeed = 3f * Time.deltaTime;
        
        currentHpBar.fillAmount = Mathf.Lerp(currentHpBar.fillAmount, player.Hp / player.MaxHp, hpLerpSpeed);
        currentHpIndex.text = $"{player.Hp} / {player.MaxHp}";
        ColorChanger();
    }
}

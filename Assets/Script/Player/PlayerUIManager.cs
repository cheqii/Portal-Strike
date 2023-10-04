using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microlight.MicroBar;

public class PlayerUIManager : MonoBehaviour
{

    [Header("Player UI Text")] 
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private MicroBar xp_Bar;
    
    private float hpLerpSpeed;
    private Player player;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        
        xp_Bar.Initialize(100);
        xp_Bar.UpdateHealthBar(0);

        
        healthBar.Initialize(player.MaxHp);
        healthBar.UpdateHealthBar(player.Hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void ColorChanger()
    {
        /* Create new color
         * color8CFF41 is green
         * colorFF4040 is red */
        Color green = new (140f / 255f, 1f, 65f / 255f);
        Color red = new (1f, 64f / 255f, 64f / 255f);

        Color healthColor = Color.Lerp(red, green, (player.Hp / player.MaxHp));
        
        // healthColor.color = healthColor;
        // hpText.color = healthColor;
    }

    public void HealthUpdate()
    {
        healthBar.UpdateHealthBar(player.Hp);
        hpText.text = $"{player.MaxHp} / {player.Hp}";
    }

    public void XpBarUpdate()
    {
        StartCoroutine(DelayXpBarUpdate());
    }
    
    IEnumerator DelayXpBarUpdate()
    {
        yield return new WaitForSeconds(0.2f);
        xp_Bar.UpdateHealthBar(player.Xp);
    }
}

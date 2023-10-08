using System.Collections;
using UnityEngine;
using TMPro;
using Microlight.MicroBar;

public class PlayerUIManager : MonoBehaviour
{

    [Header("Player UI Text")] 
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private MicroBar xp_Bar;
    
    private Player player;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        
        xp_Bar.Initialize(100);
        xp_Bar.UpdateHealthBar(0);

        
        healthBar.Initialize(player.MaxHp);
        healthBar.UpdateHealthBar(player.Hp);
        
        hpText.text = $"{player.Hp} / {player.MaxHp}";
    }

    public void HealthUpdate()
    {
        healthBar.UpdateHealthBar(player.Hp);
        hpText.text = $"{player.Hp} / {player.MaxHp}";
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

using System.Collections;
using UnityEngine;
using TMPro;
using Microlight.MicroBar;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Player UI Text")] 
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private TextMeshProUGUI xp_Text;
    [SerializeField] private MicroBar xp_Bar;

    [Header("Player Controller UI")]
    [SerializeField] private GameObject playerUI;
    
    private Player player;

    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOverPanel;
    
    [Header("Portal Event")]
    [SerializeField] private Nf_GameEvent PortalIn;
    [SerializeField] private Nf_GameEvent PortalOut;
    
    void Start()
    {
        player = FindObjectOfType<Player>();
        
        xp_Bar.Initialize(100);
        xp_Bar.UpdateHealthBar(0);

        
        healthBar.Initialize(player.MaxHp);
        healthBar.UpdateHealthBar(player.Hp);
        
        hpText.text = $"{player.Hp} / {player.MaxHp}";
        xp_Text.text = $"{player.Xp} / { player.MaxXp }";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDead();
    }

    public void HealthUpdate()
    {
        healthBar.UpdateHealthBar(player.Hp);
        hpText.text = $"{player.Hp} / {player.MaxHp}";
    }

    public void XpBarUpdate()
    {
        xp_Text.text = $"{player.Xp} / {player.MaxXp}";
        StartCoroutine(DelayXpBarUpdate());
    }
    
    IEnumerator DelayXpBarUpdate()
    {
        yield return new WaitForSeconds(0.2f);
        xp_Bar.UpdateHealthBar(player.Xp);
    }

    public void CreatePortalIn()
    {
        PortalIn.Raise();
    }
    public void CreatePortalOut()
    {
        PortalOut.Raise();
    }

    void PlayerDead()
    {
        if(player != null) return;
        gameOverPanel.SetActive(true);
        if (gameOverPanel.gameObject.activeInHierarchy)
        {
            playerUI.SetActive(false);
        }
    }
}
using System.Collections;
using UnityEngine;
using TMPro;
using Microlight.MicroBar;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp_Text;
    [SerializeField] private MicroBar hp_Bar;
    [SerializeField] private TextMeshProUGUI xp_Text;
    [SerializeField] private MicroBar xp_Bar;
    [Header("Player Stats UI")]
    [SerializeField] private TextMeshProUGUI level_Text;
    [SerializeField] private TextMeshProUGUI atk_Text;
    [SerializeField] private TextMeshProUGUI moveSpeed_Text;
    [SerializeField] private TextMeshProUGUI def_Text;
    [SerializeField] private TextMeshProUGUI critRate_Text;
    [SerializeField] private TextMeshProUGUI criteDamate_Text;
    [SerializeField] private TextMeshProUGUI dodgeRate_Text;
    [SerializeField] private TextMeshProUGUI enemiesCount_Text;

    [Header("Player Controller UI")]
    [SerializeField] private GameObject playerUI;

    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOverPanel;
    
    [Header("Portal Event")]
    [SerializeField] private Nf_GameEvent PortalIn;
    [SerializeField] private Nf_GameEvent PortalOut;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        
        xp_Bar.Initialize(100);
        xp_Bar.UpdateHealthBar(0);

        
        hp_Bar.Initialize(player.MaxHp);
        hp_Bar.UpdateHealthBar(player.Hp);

        hp_Text.text = $"{player.Hp} / {player.MaxHp}";
        xp_Text.text = $"{player.Xp} / { player.MaxXp }";

        StatsUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDead();
    }

    public void HealthUpdate()
    {
        hp_Bar.UpdateHealthBar(player.Hp);
        hp_Text.text = $"{player.Hp} / {player.MaxHp}";
    }

    public void XpBarUpdate()
    {
        xp_Text.text = $"{player.Xp} / {player.MaxXp}";
        StartCoroutine(DelayXpBarUpdate());
    }

    public void StatsUpdate()
    {
        level_Text.text = $"lv.{player.Level}";
        atk_Text.text = $"ATK : {player.AtkDamage}";
        moveSpeed_Text.text = $"Move Speed : {player.MoveSpeed:F2}%";
        def_Text.text = $"DEF : {player.Def}";
        critRate_Text.text = $"CRIT RATE : {player.CritRate:F2}%";
        criteDamate_Text.text = $"CRIT DAMAGE : {player.CritDamage:F2}%";
        dodgeRate_Text.text = $"DODGE RATE : {player.DodgeRate:F2}%";
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

    public void EnemiesCountUpdate()
    {
        enemiesCount_Text.text = $"{player.EnemyCounnt}/95\nEnemies Left";
    }
}
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilitySelection : MonoBehaviour
{
    [SerializeField] private GameObject abilitySelectionPanel;
    [SerializeField] private TMP_Text Button1Description;
    [SerializeField] private TMP_Text Button2Description;
    [SerializeField] private TMP_Text Button3Description;
    [SerializeField] private Image Button1Image;
    [SerializeField] private Image Button2Image;
    [SerializeField] private Image Button3Image;

    private Player player;
    private PlayerUIManager playerUIManager;

    private List<System.Action> availableAbilities = new List<System.Action>();
    private System.Action[] selectedAbilities = new System.Action[3];

    // Define constants for abilies values
    private const int MaxHpIncrease = 20;
    private const float MoveSpeedIncrease = 0.2f; // 0.2f = 20%
    private const int DefIncrease = 2;
    private const float CritRateIncrease = 0.1f; // 0.1f = 10%
    private const float CritDamageIncrease = 0.1f; // 0.1f = 10%
    private const float DodgeRateIncrease = 0.1f; // 0.1f = 10%

    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();

        AvailableAbilities();
    }
    #region -Abilities Data-
    private void AvailableAbilities()
    {
        // All 9 abilities in the list that have not yet been selected.
        availableAbilities.Add(IncreaseMaxHp);
        availableAbilities.Add(IncreaseMoveSpeed);
        availableAbilities.Add(IncreaseDef);
        availableAbilities.Add(IncreaseCritRate);
        availableAbilities.Add(IncreaseCritDamage);
        availableAbilities.Add(IncreaseDodgeRate);
    }

    //
    private void SetAbilityColorAndDescription(Image buttonImage, System.Action ability, TMP_Text descriptionText)
    {
        Color myBlue = new Color(0x5E / 255.0f, 0x6E / 255.0f, 0xE2 / 255.0f); // 5E6EE2 in HEX
        Color myPurple = new Color(0x7D / 255.0f, 0x1E / 255.0f, 0xF5 / 255.0f); // 7D1EF5 in HEX
        Color myRed = new Color(0xFF / 255.0f, 0x11 / 255.0f, 0x50 / 255.0f); // FF1150 in HEX

        // Here's the description and things that will be displayed on the UI before you select an ability.
        if (ability == IncreaseMaxHp)
        {
            descriptionText.text = $"Max HP\n+{MaxHpIncrease}";
            buttonImage.color = myBlue;
        }
        else if (ability == IncreaseMoveSpeed)
        {
            descriptionText.text = $"Move Speed\n+{(MoveSpeedIncrease * 100).ToString("0")}%";
            buttonImage.color = myPurple;
        }
        else if (ability == IncreaseDef)
        {
            descriptionText.text = $"Defense\n+{DefIncrease}";
            buttonImage.color = myBlue;
        }
        else if (ability == IncreaseCritRate)
        {
            descriptionText.text = $"Increase Critical Rate\n+{(CritRateIncrease * 100).ToString("0")}%";
            buttonImage.color = myRed;
        }
        else if (ability == IncreaseCritDamage)
        {
            descriptionText.text = $"Increase Critical Damage\n+{(CritDamageIncrease * 100).ToString("0")}%";
            buttonImage.color = myRed;
        }
        else if (ability == IncreaseDodgeRate)
        {
            descriptionText.text = $"Increase Dodge Rate\n+{(DodgeRateIncrease * 100).ToString("0")}%";
            buttonImage.color = myRed;
        }
        else
        {
            descriptionText.text = "Unknown Ability";
            buttonImage.color = Color.gray;
        }
    }

    // This is what happens after pressing the select ability button.
    // Increase Max HP .. unit
    private void IncreaseMaxHp()
    {
        Debug.Log("Max HP +" + MaxHpIncrease);
        player.IncreaseMaxHp(MaxHpIncrease);
        playerUIManager.HealthUpdate();
    }
    // Increase Move Speed ..%
    private void IncreaseMoveSpeed()
    {
        Debug.Log("Move Speed +" + (MoveSpeedIncrease * 100).ToString("0") + "%");
        player.IncreaseMoveSpeed(MoveSpeedIncrease);
    }

    // Increase Defense .. unit
    private void IncreaseDef()
    {
        Debug.Log("Defense +" + DefIncrease);
        player.IncreaseDef(DefIncrease);
    }
    // Increase Critical Rate ..%
    private void IncreaseCritRate()
    {
        Debug.Log("Increase Critical Rate +" + (CritRateIncrease * 100).ToString("0") + "%");
        player.IncreaseCritRate(CritRateIncrease);
    }
    // Increase Critical Damage ..%
    private void IncreaseCritDamage()
    {
        Debug.Log("Increase Critical Damage +" + (CritDamageIncrease * 100).ToString("0") + "%");
        player.IncreaseCritDamage(CritDamageIncrease);
    }
    // Increase Dodge Rate ..%
    private void IncreaseDodgeRate()
    {
        Debug.Log("Increase Dodge Rate +" + (DodgeRateIncrease * 100).ToString("0") + "%");
        player.IncreaseDodgeRate(DodgeRateIncrease);
    }
    #endregion

    #region -Ability Selection Work Progress-
    // Stop/Resume time of game
    public void TimeFreeze()
    {
        Time.timeScale = 0;
    }

    public void UnFreeze()
    {
        Time.timeScale = 1;
    }

    public void ShowUI()
    {
        GetComponent<Image>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);

        // Create a list of indices from 0 to the number of available abilities - 1
        List<int> indices = new List<int>();
        for (int i = 0; i < availableAbilities.Count; i++)
        {
            indices.Add(i);
        }

        // Randomly select 3 unique indices
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            int selectedIndex = indices[randomIndex];
            selectedAbilities[i] = availableAbilities[selectedIndex];
            indices.RemoveAt(randomIndex);
        }

        // Reset indices for the next round
        indices.Clear();

        SetAbilityColorAndDescription(Button1Image, selectedAbilities[0], Button1Description);
        SetAbilityColorAndDescription(Button2Image, selectedAbilities[1], Button2Description);
        SetAbilityColorAndDescription(Button3Image, selectedAbilities[2], Button3Description);
    }

    public void Button1()
    {
        if (selectedAbilities[0] != null)
        {
            selectedAbilities[0]();
            UnFreeze();
        }
    }

    public void Button2()
    {
        if (selectedAbilities[1] != null)
        {
            selectedAbilities[1]();
            UnFreeze();
        }
    }

    public void Button3()
    {
        if (selectedAbilities[2] != null)
        {
            selectedAbilities[2]();
            UnFreeze();
        }
    }

    public void HideUI()
    {
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
    #endregion
}
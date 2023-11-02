using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilitySelection : MonoBehaviour
{
    [SerializeField] private GameObject abilitySelectionPanel;
    [SerializeField] private Button abilityButton1;
    [SerializeField] private Button abilityButton2;
    [SerializeField] private Button abilityButton3;
    [SerializeField] private Image abilityImage1;
    [SerializeField] private Image abilityImage2;
    [SerializeField] private Image abilityImage3;
    [SerializeField] private TMP_Text abilityDescription1;
    [SerializeField] private TMP_Text abilityDescription2;
    [SerializeField] private TMP_Text abilityDescription3;
    [SerializeField] private GameObject getRewardPanel;

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

    // =======================================================================
    // Use this method when you want to select abilities
    public void StartSelectingAbilities()
    {
        // Set card button to active
        SetButtonActive();

        // Show ui image of "AbilitySelectionPanel" and 3 abilities card
        ShowUI();

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

        // Set ability card as desired [Ability color and description]
        SetAbilityColorAndDescription(abilityImage1, selectedAbilities[0], abilityDescription1);
        SetAbilityColorAndDescription(abilityImage2, selectedAbilities[1], abilityDescription2);
        SetAbilityColorAndDescription(abilityImage3, selectedAbilities[2], abilityDescription3);
    }

    // =======================================================================
    // button card (1)
    public void Button1()
    {
        if (selectedAbilities[0] != null)
        {
            selectedAbilities[0](); // Select ability in array 2 of SelectedAbilities
            SetButtonInactive(); // Set card button interactable inactive to prevents cheating by pressing multiple times !!
            UnFreeze(); // UnFreeze game time
        }
    }

    // button card (2)
    public void Button2()
    {
        if (selectedAbilities[1] != null)
        {
            selectedAbilities[1](); // Select ability in array 2 of SelectedAbilities
            SetButtonInactive(); // Set card button interactable inactive to prevents cheating by pressing multiple times !!
            UnFreeze(); // UnFreeze game time
        }
    }

    // button card (3)
    public void Button3()
    {
        if (selectedAbilities[2] != null)
        {
            selectedAbilities[2](); // Select ability in array 2 of SelectedAbilities
            SetButtonInactive(); // Set card button to inactive to prevents cheating by pressing multiple times !!
            UnFreeze(); // UnFreeze game time
        }
    }

    // =======================================================================
    // Show ui image of "AbilitySelectionPanel" and 3 abilities card
    public void ShowUI()
    {
        GetComponent<Image>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    // Hide ui image of "AbilitySelectionPanel" and 3 abilities card
    public void HideUI() 
    {
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    // =======================================================================
    // Set button interactable active in ShowUI() When level up!
    public void SetButtonActive()
    {
        abilityButton1.interactable = true;
        abilityButton2.interactable = true;
        abilityButton3.interactable = true;
    }

    // Set button interactable inactive in all 3 buttons card When not in use
    // Prevents cheating by pressing multiple times !!
    public void SetButtonInactive()
    {
        abilityButton1.interactable = false;
        abilityButton2.interactable = false;
        abilityButton3.interactable = false;
    }

    // =======================================================================
    // Reset Abilities card when player watching ads
    public void ResetAbility()
    {
        getRewardPanel.SetActive(true); // Notification panel
        ShowUI(); // Random ability again
    }
    #endregion
}
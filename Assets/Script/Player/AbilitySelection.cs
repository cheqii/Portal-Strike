using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilitySelection : MonoBehaviour
{
    private Player player;
    public GameObject abilitySelectionPanel;

    public TMP_Text Button1Description;
    public TMP_Text Button2Description;
    public TMP_Text Button3Description;

    public Image Button1Image;
    public Image Button2Image;
    public Image Button3Image;

    private List<System.Action> availableAbilities = new List<System.Action>();
    private System.Action[] selectedAbilities = new System.Action[3];

    private void Start()
    {
        player = FindObjectOfType<Player>();

        // All 9 abilities in the list that have not yet been selected.
        availableAbilities.Add(IncreaseMaxHp);
        availableAbilities.Add(IncreaseMoveSpeed);
        availableAbilities.Add(IncreaseDef);
        availableAbilities.Add(IncreaseCritRate);
        availableAbilities.Add(IncreaseCritDamage);
        availableAbilities.Add(IncreaseDodgeRate);
    }

    private void RandomAbility()
    {
        // Check if there are still unselected abilities.
        if (availableAbilities.Count > 0)
        {
            // Random ability from list that has not been selected yet.
            int randomIndex = Random.Range(0, availableAbilities.Count);
            System.Action selectedAbility = availableAbilities[randomIndex];

            // Triggers a random ability
            selectedAbility();

            // Removes the selected ability from the List.
            availableAbilities.RemoveAt(randomIndex);
        }
    }

    private void SetAbilityColorAndDescription(Image buttonImage, System.Action ability, TMP_Text descriptionText)
    {
        Color myBlue = new Color(0x5E / 255.0f, 0x6E / 255.0f, 0xE2 / 255.0f); // 5E6EE2 in HEX
        Color myPurple = new Color(0x64 / 255.0f, 0x1E / 255.0f, 0xF5 / 255.0f); // 641EF5 in HEX
        Color myRed = new Color(0xFF / 255.0f, 0x11 / 255.0f, 0x50 / 255.0f); // FF1150 in HEX

        if (ability == IncreaseMaxHp)
        {
            descriptionText.text = "Max HP\n+20";
            buttonImage.color = myBlue;
        }
        else if (ability == IncreaseMoveSpeed)
        {
            descriptionText.text = "Move Speed\n+20%";
            buttonImage.color = myPurple;
        }
        else if (ability == IncreaseDef)
        {
            descriptionText.text = "Def\n+2";
            buttonImage.color = myBlue;
        }
        else if (ability == IncreaseCritRate)
        {
            descriptionText.text = "Increase Critical Rate\n+10%";
            buttonImage.color = myRed;
        }
        else if (ability == IncreaseCritDamage)
        {
            descriptionText.text = "Increase Critical Damage\n+10%";
            buttonImage.color = myRed;
        }
        else if (ability == IncreaseDodgeRate)
        {
            descriptionText.text = "Increase Dodge Rate\n+10%";
            buttonImage.color = myRed;
        }
        else
        {
            // Default description if the ability doesn't match any condition
            descriptionText.text = "Unknown Ability";
            buttonImage.color = Color.gray;
        }
    }


    public void Button1()
    {
        // Use random ability for Button1
        if (selectedAbilities[0] != null)
        {
            selectedAbilities[0]();
            
            UnFreeze();
        }
    }

    public void Button2()
    {
        // Use random ability for Button2
        if (selectedAbilities[1] != null)
        {
            selectedAbilities[1]();
            UnFreeze();

        }
    }

    public void Button3()
    {
        // Use random ability for Button3
        if (selectedAbilities[2] != null)
        {
            selectedAbilities[2]();
            UnFreeze();
        }
    }



    /* Ability List */
    private void IncreaseMaxHp()
    {
        Debug.Log("Max HP +20");
        player.IncreaseMaxHp(20);
    }

    private void IncreaseMoveSpeed()
    {
        Debug.Log("Move Speed +20%");
        player.IncreaseMoveSpeed(20.0f);
    }

    private void IncreaseDef()
    {
        Debug.Log("Def +2");
        player.IncreaseDef(2);
    }

    private void IncreaseCritRate()
    {
        Debug.Log("Increase Critical Rate +10%");
        player.IncreaseCritRate(10.0f);
    }

    private void IncreaseCritDamage()
    {
        Debug.Log("Increase Critical Damage +10%");
        player.IncreaseCritDamage(10.0f);
    }

    private void IncreaseDodgeRate()
    {
        Debug.Log("Increase Dodge Rate +10%");
        player.IncreaseDodgeRate(10.0f);
    }

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

        // Randomly 3 abilities and store them in selectedAbilities
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, availableAbilities.Count);
            selectedAbilities[i] = availableAbilities[randomIndex];
            availableAbilities.RemoveAt(randomIndex);
        }

        // Set ability colors and descriptions based on the selected abilities
        SetAbilityColorAndDescription(Button1Image, selectedAbilities[0], Button1Description);
        SetAbilityColorAndDescription(Button2Image, selectedAbilities[1], Button2Description);
        SetAbilityColorAndDescription(Button3Image, selectedAbilities[2], Button3Description);
    }


    public void HideUI()
    {
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
}
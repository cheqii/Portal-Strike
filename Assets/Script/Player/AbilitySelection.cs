using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilitySelection : MonoBehaviour
{
    public Player player;
    public GameObject abilitySelectionPanel;
    public TMP_Text abilityDescription;
    public TMP_Text Button2Description;
    public TMP_Text Button3Description;


    private List<System.Action> availableAbilities = new List<System.Action>();
    private System.Action[] selectedAbilities = new System.Action[3];

    private void Start()
    {
        // All 9 abilities in the list that have not yet been selected.
        availableAbilities.Add(IncreaseMaxHp);
        availableAbilities.Add(IncreaseMoveSpeed);
        availableAbilities.Add(IncreaseDef);
        availableAbilities.Add(IncreaseTest1);
        availableAbilities.Add(IncreaseTest2);
        availableAbilities.Add(IncreaseTest3);
        availableAbilities.Add(IncreaseTest4);
        availableAbilities.Add(IncreaseTest5);
        availableAbilities.Add(IncreaseTest6);
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

    private string GetAbilityDescription(System.Action ability)
    {
        if (ability == IncreaseMaxHp)
        {
            return "Max HP +20";
        }
        else if (ability == IncreaseMoveSpeed)
        {
            return "Move Speed +20%";
        }
        else if (ability == IncreaseDef)
        {
            return "Def +2";
        }
        else if (ability == IncreaseTest1)
        {
            return "IncreaseTest1";
        }
        else if (ability == IncreaseTest2)
        {
            return "IncreaseTest2";
        }
        else if (ability == IncreaseTest3)
        {
            return "IncreaseTest3";
        }
        else if (ability == IncreaseTest4)
        {
            return "IncreaseTest4";
        }
        else if (ability == IncreaseTest5)
        {
            return "IncreaseTest5";
        }
        else if (ability == IncreaseTest6)
        {
            return "IncreaseTest6";
        }
        return "Unknown Ability";
    }

    public void Button1()
    {
        // Use random ability for Button1
        if (selectedAbilities[0] != null)
        {
            selectedAbilities[0]();
        }
    }

    public void Button2()
    {
        // Use random ability for Button2
        if (selectedAbilities[1] != null)
        {
            selectedAbilities[1]();
        }
    }

    public void Button3()
    {
        // Use random ability for Button3
        if (selectedAbilities[2] != null)
        {
            selectedAbilities[2]();
        }
    }



    /* Ability List */
    private void IncreaseMaxHp()
    {
        abilityDescription.text = "Max HP +20";
        Debug.Log("Max HP +20");
        player.IncreaseMaxHp(20);
        HideUI();
    }

    private void IncreaseMoveSpeed()
    {
        abilityDescription.text = "Move Speed +20%";
        Debug.Log("Move Speed +20%");
        player.IncreaseMoveSpeed(20.0f);
        HideUI();
    }

    private void IncreaseDef()
    {
        abilityDescription.text = "Def +2";
        Debug.Log("Def +2");
        player.IncreaseDef(2);
        HideUI();
    }

    private void IncreaseTest1()
    {
        Debug.Log("Test1");
        HideUI();
    }

    private void IncreaseTest2()
    {
        Debug.Log("Test2");
        HideUI();
    }

    private void IncreaseTest3()
    {
        Debug.Log("Test3");
        HideUI();
    }
    private void IncreaseTest4()
    {
        Debug.Log("Test4");
        HideUI();
    }
    private void IncreaseTest5()
    {
        Debug.Log("Test5");
        HideUI();
    }
    private void IncreaseTest6()
    {
        Debug.Log("Test6");
        HideUI();
    }

    public void ShowUI()
    {
        Time.timeScale = 0;
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

        // Show random ability name in abilityDescription
        abilityDescription.text = "1. " + GetAbilityDescription(selectedAbilities[0]);
        Button2Description.text = "2. " + GetAbilityDescription(selectedAbilities[1]);
        Button3Description.text = "3. " + GetAbilityDescription(selectedAbilities[2]);
    }


    public void HideUI()
    {
        Time.timeScale = 1;
        GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }
}
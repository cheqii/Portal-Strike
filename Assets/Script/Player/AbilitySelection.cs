using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilitySelection : MonoBehaviour
{
    public Player player;
    public GameObject abilitySelectionPanel;
    public TMP_Text abilityDescription;

    private List<System.Action> availableAbilities = new List<System.Action>();

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

    private void UseRandomAbilitySlot()
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

    public void AbilitySlot1()
    {
        UseRandomAbilitySlot();
    }

    public void AbilitySlot2()
    {
        UseRandomAbilitySlot();
    }

    public void AbilitySlot3()
    {
        UseRandomAbilitySlot();
    }


    /* Ability List */
    private void IncreaseMaxHp()
    {
        abilityDescription.text = "Max HP +20";
        Debug.Log("Max HP +20");
        player.IncreaseMaxHp(20);
        abilitySelectionPanel.SetActive(false);
    }

    private void IncreaseMoveSpeed()
    {
        abilityDescription.text = "Move Speed +20%";
        Debug.Log("Move Speed +20%");
        player.IncreaseMoveSpeed(20.0f);
        abilitySelectionPanel.SetActive(false);
    }

    private void IncreaseDef()
    {
        abilityDescription.text = "Def +2";
        Debug.Log("Def +2");
        player.IncreaseDef(2);
        abilitySelectionPanel.SetActive(false);
    }

    private void IncreaseTest1()
    {
        Debug.Log("Test1");
        abilitySelectionPanel.SetActive(false);
    }

    private void IncreaseTest2()
    {
        Debug.Log("Test2");
        abilitySelectionPanel.SetActive(false);
    }

    private void IncreaseTest3()
    {
        Debug.Log("Test3");
        abilitySelectionPanel.SetActive(false);
    }
    private void IncreaseTest4()
    {
        Debug.Log("Test4");
        abilitySelectionPanel.SetActive(false);
    }
    private void IncreaseTest5()
    {
        Debug.Log("Test5");
        abilitySelectionPanel.SetActive(false);
    }
    private void IncreaseTest6()
    {
        Debug.Log("Test6");
        abilitySelectionPanel.SetActive(false);
    }
}
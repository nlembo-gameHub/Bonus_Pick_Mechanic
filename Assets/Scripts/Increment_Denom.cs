using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Increment_Denom : MonoBehaviour
{
    //Scriptable Objects
    [SerializeField] private Increment_Value valueScriptable;
    //Game Manager and its Script
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Chest_Manager managerScript;
    //Button text
    [SerializeField] public TextMeshProUGUI denomText;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        managerScript = gameManager.GetComponent<Chest_Manager>();

        denomText.text = "$" + valueScriptable.value.ToString();
    }

    public void SubtractBalance()
    {
        Debug.Log("Running SubtractBalance");
        managerScript.CurrentBalanceCheck(-valueScriptable.value);
    }

    private void FixBalance()
    {
        Debug.Log("Running FixBalance");
        managerScript.CurrentBalanceCheck(managerScript.currentDenomination);
    }

    public void AddDenomination()
    {
        if(valueScriptable.value > managerScript.currentBalance)
        {
            FixBalance();
            SubtractBalance();
            managerScript.DenominationCheck(valueScriptable.value);
        }
        else
        {
            Debug.Log("Denomination exceeds the balance!");
        }
        
    }
}

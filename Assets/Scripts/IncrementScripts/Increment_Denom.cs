using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Increment_Denom : MonoBehaviour
{
    //Game Object Arrays
    [Header("Game Object Arrays")]
    //[SerializeField] private Increment_Value valueScriptable;
    [SerializeField] private Increment_Value[] value;

    //Game Manager and its Script
    [Header("Game Objects")]
    [SerializeField] private GameObject gameManager;
    [SerializeField] private BonusManager managerScript;

    //UI Elements
    [Header("UI Variables")]
    [SerializeField] public TextMeshProUGUI decreaseText;
    [SerializeField] public TextMeshProUGUI increaseText;

    //Counter Variables
    [Header("Counter Variables")]
    [SerializeField] public int currentDenom;
    [SerializeField] public int previousDenom;
    [SerializeField] public bool newRound = true;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        managerScript = gameManager.GetComponent<BonusManager>();

        DenomTextUpdate();
    }

    public void SubtractBalance()
    {
        managerScript.CurrentBalanceCheck(-value[currentDenom].value);
    }

    private void FixBalance()
    {
        managerScript.CurrentBalanceCheck(managerScript.currentDenomination);
    }

    public void AddDenomination()
    {
        FixBalance();
        if (value[currentDenom].value <= managerScript.currentBalance)
        {
            DenomTextUpdate();
            SubtractBalance();
            managerScript.DenominationCheck(value[currentDenom].value);
        }
        else
        {
            //Debug.Log("Denomination exceeds the balance!");
        }
        
    }

    public void NextDenom()
    {
        if(currentDenom < value.Length-1) 
        {
            currentDenom += 1;
            previousDenom = currentDenom - 1;
            AddDenomination();
        }
    }

    public void PreviousDenom()
    {
        if(previousDenom > 0) 
        {
            currentDenom -= 1;
            previousDenom = currentDenom - 1;
            AddDenomination();
        }
        else
        {
            currentDenom = 0;
            previousDenom = 0;
            AddDenomination();
        }
        
    }

    public void DenomTextUpdate()
    {
        if (newRound == true)
        {
            decreaseText.text = "$" + value[currentDenom].value.ToString();
            increaseText.text = "$" + value[currentDenom + 1].value.ToString();
            newRound = false;
        }
        else
        {
            decreaseText.text = "$" + value[previousDenom].value.ToString();
            if(currentDenom < value.Length - 1)
            {
                increaseText.text = "$" + value[currentDenom + 1].value.ToString();
            }
            else
            {
                increaseText.text = "$" + value[currentDenom].value.ToString();
            }
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TreasureChestScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float value;
    [SerializeField] private float winsAmount;
    [SerializeField] private int percent;

    [SerializeField] public bool isPooper;
    [SerializeField] public bool isModified;
    [SerializeField] public bool wasClicked;

    [SerializeField] private GameObject gameManager;
    [SerializeField] private BonusManager managerScript;
    [SerializeField] private TreasureChestScript otherChests;

    [SerializeField] public TextMeshProUGUI chestText;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        managerScript = gameManager.GetComponent<BonusManager>();
    }

    public void CheckPercentValue(int _percent)
    {
        percent = _percent;
    }
    public void CheckChest()
    {
        if (percent == 0)
        {
            //chestText.text = "$" + value.ToString();
            managerScript.AcceptWinsAmount(value);
            RoundOver();
        }
        else
        {
            if (value != 0)
            {
                chestText.text = "$" + value.ToString();
                managerScript.AcceptWinsAmount(value);
                value = 0;
                wasClicked = true;
            }
            else if (value == 0 && !wasClicked)
            {
                SwapChestValue();
            }
            else if(value == 0 && wasClicked)
            {
                isPooper = true;
                //Will call the 'RoundOver' func since the pooper was found
                chestText.text = "Pooper";
                RoundOver();
            }
            
        }
    }

    private void SwapChestValue()
    {
        Debug.Log("Swap Chest Func Fires");
        for (int i = 0; i < managerScript.TreasureChestScripts.Length; i++)
        {
            if (managerScript.TreasureChestScripts[i].wasClicked == false)
            {
                if (managerScript.TreasureChestScripts[i].value != 0)
                {
                    value = managerScript.TreasureChestScripts[i].value;
                    managerScript.TreasureChestScripts[i].value = 0;
                    wasClicked = true;
                    isModified = true;
                    break;
                }
            }
           
        }
        wasClicked = true;
        CheckChest();

    }

    public void RoundOver()
    {
        Debug.Log("Round is over");
        //chestText.text = "Button";
        managerScript.RoundEnd();
    }
}

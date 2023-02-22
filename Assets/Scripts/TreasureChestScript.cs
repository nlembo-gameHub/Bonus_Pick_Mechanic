using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureChestScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int value;
    [SerializeField] public int winsAmount;

    [SerializeField] public bool isPooper;
    [SerializeField] public bool isModified;

    [SerializeField] private GameObject gameManager;
    [SerializeField] private Chest_Manager managerScript;

    [SerializeField] private TextMeshProUGUI chestText;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        managerScript = gameManager.GetComponent<Chest_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckChest()
    {
        if (isPooper)
        {
            //Will call the 'RoundOver' func since the pooper was found
            chestText.text = "Pooper";
            RoundOver();
        }
        else
        {
            if(value == 0)
            {
                //winsAmount += value * managerScript.currentDenomination;
                chestText.text = "x" + value.ToString();
                managerScript.AcceptWinsAmount(value);
                RoundOver();
            }
            else
            {
                chestText.text = "x" + value.ToString();
                //winsAmount += value * managerScript.currentDenomination;
                managerScript.AcceptWinsAmount(value);
                winsAmount = 0;
            }
        }
    }

    public void RoundOver()
    {
        Debug.Log("Round is over");
        chestText.text = "Button";
        managerScript.RoundEnd();
    }
}

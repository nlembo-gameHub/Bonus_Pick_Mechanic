using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int id;
    [SerializeField] public int value;
    [SerializeField] public int winsAmount;

    [SerializeField] public bool isPooper;
    [SerializeField] public bool isModified;

    [SerializeField] private GameObject gameManager;
    [SerializeField] private Chest_Manager managerScript;
    void Start()
    {
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
            RoundOver();
        }
        else
        {
            if(value == 0)
            {
                
                RoundOver();
            }
            else
            {
                winsAmount += value * managerScript.currentDenomination;
            }
        }
    }

    public void RoundOver()
    {
        Debug.Log("Round is over");
        managerScript.WinsCheck(winsAmount);
        managerScript.CurrentBalanceCheck(winsAmount);
        managerScript.GameEndButtons();
        winsAmount = 0;
        isPooper = false;
        isModified = false;
    }
}

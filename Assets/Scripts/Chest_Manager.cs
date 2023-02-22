using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest_Manager : MonoBehaviour
{
    //Game Object Arrays
    [Header("Game Object Arrays")]
    [SerializeField] private GameObject[] TreasureChests;
    [SerializeField] private TreasureChestScript[] TreasureChestScripts;

    //UI Elements
    [Header("U.I. Variables")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI denominationText;
    [SerializeField] private TextMeshProUGUI winsText;
    [SerializeField] private Button PlayButton;

    [SerializeField] public int currentBalance;
    [SerializeField] public int currentDenomination;
    [SerializeField] public int currentWins;
    // Start is called before the first frame update
    void Start()
    {
        TreasureChestScripts = new TreasureChestScript[TreasureChests.Length];
        //Filling out the TreasureChestScript Array
        for (int i = 0; i < TreasureChests.Length; i++)
        {
            
            TreasureChestScripts[i] = TreasureChests[i].GetComponent<TreasureChestScript>();
            Debug.Log("The Treasure Chest Object: " + TreasureChests[i].name + " = " + TreasureChestScripts[i].name);
            TreasureChests[i].GetComponent<Button>().interactable = false;
        }

        //Setting up the initial values of the text
        CurrentBalanceCheck(currentBalance);
        DenominationCheck(currentDenomination);
        WinsCheck(currentWins);
    }

    #region Text Assigments
    public void CurrentBalanceCheck(int _passedValue)
    {
        currentBalance += _passedValue;
        balanceText.text = _passedValue.ToString();
    }

    public void DenominationCheck(int _passedValue)
    {
        currentDenomination = _passedValue;
        denominationText.text = _passedValue.ToString();
    }

    public void WinsCheck(int _passedValue)
    {
        currentWins = _passedValue;
        winsText.text = _passedValue.ToString(); 
    }
    #endregion

    #region Chest Assignments
    public void BeginChestAssignment()
    {
        Debug.Log("Play button has been pressed");
        //Make buttons interactable and uninteractable.
        GameBeginButtons();
        //With the play button pressed, we will now call the 'AssignPooper' func
        //We can use this to assign one of the chest gameObjects as the pooper
        AssignPooper();

        //We then need to pick out the rest of the chests and assign them a value range.
        AssignValue();
    }

    private void GameBeginButtons()
    {
        PlayButton.interactable = false;
        for (int i = 0; i < TreasureChests.Length; i++)
        {
            TreasureChests[i].GetComponent<Button>().interactable = true;
            TreasureChestScripts[i].value = 0;
        }
    }

    private void AssignPooper()
    {
        //Generate the random element num
        int randomElement = Random.Range(0, TreasureChestScripts.Length);
        Debug.Log("The random element number is: " + randomElement);

        //Set the TreasureChest to have its isPooper bool become true. Then do the same for if it is Modified.
        TreasureChestScripts[randomElement].isPooper= true;
        TreasureChestScripts[randomElement].isModified= true;
        /*
        TreasureChests[randomElement].GetComponent<TreasureChestScript>().isPooper = true;
        TreasureChests[randomElement].GetComponent<TreasureChestScript>().isModified = true;
        */
    }

    private void AssignValue()
    {
        //This is the following Value Range we are looking for:
        /*
        o 0x (instant loss) - 50% of the time.
        o 1x, 2x, 3x, 4x, 5x, 6x, 7x, 8x, 9x, 10x - one of these 30% of the time.
        o 12x, 16x, 24x, 32x, 48x, 64x - one of these 15% of the time.
        o 100x, 200x, 300x, 400x, 500x - one of these 5% of the time.
        */
        
    }

    public void GameEndButtons()
    {
        PlayButton.interactable = true;
        for (int i = 0; i < TreasureChests.Length; i++)
        {
            TreasureChests[i].GetComponent<Button>().interactable = false;
            TreasureChestScripts[i].RoundOver();
        }
    }
    #endregion
}

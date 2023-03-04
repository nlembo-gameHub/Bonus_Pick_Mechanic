using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BonusManager : MonoBehaviour
{
    //Game Object Arrays
    [Header("Game Object Arrays")]
    [SerializeField] public GameObject[] TreasureChests;
    [SerializeField] public TreasureChestScript[] TreasureChestScripts;
    [SerializeField] private Increment_Denom increment;

    //UI Elements
    [Header("U.I. Variables")]
    [SerializeField] private GameObject MainCanvas;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI denominationText;
    [SerializeField] private TextMeshProUGUI winsText;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button[] IncrementButtons;

    //Floats and Ints that determine final values
    [Header("Game Manager Variables")]
    [SerializeField] public float currentBalance;
    [SerializeField] public float currentDenomination;
    [SerializeField] public float currentWins;

    [SerializeField] private float totalWinnings;
    [SerializeField] private float[] winningsSplit;

    [SerializeField] private bool startingBalanceCheck = false; 
    //Multiplier Ranges
    [Header("Multiplier Ranges")]
    [SerializeField] private float[] percentRangeFifty;
    [SerializeField] private float[] percentRangeThirty;
    [SerializeField] private float[] percentRangeFifteen;
    [SerializeField] private float[] percentRangeFive;
    [SerializeField] private int percentValue;

    // Start is called before the first frame update
    void Start()
    {
        //Getting Components
        MainCanvas.GetComponent<UIManager>().PanelFadeOut();
        TreasureChestScripts = new TreasureChestScript[TreasureChests.Length];
        increment = this.GetComponent<Increment_Denom>();
        PlayButton.GetComponent<Button>().interactable = false;

        //Filling out the TreasureChestScript Array
        for (int i = 0; i < TreasureChests.Length; i++)
        {

            TreasureChestScripts[i] = TreasureChests[i].GetComponent<TreasureChestScript>();
            TreasureChests[i].GetComponent<Button>().interactable = false;
        }


        //Setting up the initial values of the text
        CurrentBalanceCheck(currentBalance);
        DenominationCheck(currentDenomination);
        WinsCheck(currentWins);
    }
    #region Text Assigments
    public void CurrentBalanceCheck(float _passedValue)
    {

        if(startingBalanceCheck == false)
        {
            if (_passedValue != currentBalance)
            {
                currentBalance += _passedValue;
            }
            else
            {
                currentBalance = _passedValue;
            }
            startingBalanceCheck = true;
        }
        else
        {
            currentBalance += _passedValue;
        }
        balanceText.text = currentBalance.ToString();
    }

    public void DenominationCheck(float _passedValue)
    {
        
        currentDenomination = _passedValue;
        denominationText.text = currentDenomination.ToString();
        if (currentDenomination > 0)
        {
            PlayButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            PlayButton.GetComponent<Button>().interactable = false;
        }
    }

    public void WinsCheck(float _passedValue)
    {
        currentWins += _passedValue;
        winsText.text = currentWins.ToString();
    }
    #endregion
    #region Chest Assignments
    public void BeginChestAssignment()
    {
        //Debug.Log("Play button has been pressed");
        currentWins = 0;
        WinsCheck(currentWins);
        //Make buttons interactable and uninteractable.
        GameBeginButtons();

        //Determine the frequency of what Multiplier will be chosen
        DetermineMultiplier();
    }

    private void GameBeginButtons()
    {
        PlayButton.interactable = false;
        //Make Chest Buttons Interactable
        for (int i = 0; i < TreasureChests.Length; i++)
        {
            TreasureChests[i].GetComponent<Button>().interactable = true;
            TreasureChestScripts[i].value = 0;
        }
        //Make Increment buttons disabled
        for (int i = 0; i < IncrementButtons.Length; i++)
        {
            IncrementButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    private void DetermineMultiplier()
    {
        //This is the following Value Range we are looking for:
        /*
        o 0x (instant loss) - 50% of the time.
        o 1x, 2x, 3x, 4x, 5x, 6x, 7x, 8x, 9x, 10x - one of these 30% of the time.
        o 12x, 16x, 24x, 32x, 48x, 64x - one of these 15% of the time.
        o 100x, 200x, 300x, 400x, 500x - one of these 5% of the time.
        */
        int rnd = Random.Range(0, 100);
        
        switch (rnd)
        {
            case int n when (n <= 50):
                percentValue = Random.Range(0, percentRangeFifty.Length);
                AssignChestValues(percentValue);
                break;

            case int n when (n > 50 && n <= 80):
                percentValue = Random.Range(0, percentRangeThirty.Length);
                AssignChestValues(percentValue);
                break;

            case int n when (n > 80 && n <= 95):
                percentValue = Random.Range(0, percentRangeFifteen.Length);
                AssignChestValues(percentValue);
                break;

            case int n when (n > 95 && n <= 100):
                percentValue = Random.Range(0, percentRangeFive.Length);
                AssignChestValues(percentValue);
                break;

            default:
                break;
        }
    }

    private void AssignChestValues(int _multiplier)
    {
        totalWinnings = _multiplier * currentDenomination;
        int rndRange = Random.Range(1, TreasureChests.Length);
        winningsSplit = new float[rndRange];
        
        //Send percent multipler to individual chest scripts
        for(int i = 0; i < TreasureChestScripts.Length; i++) 
        {
            TreasureChestScripts[i].CheckPercentValue(_multiplier);
        }

        while(winningsSplit.Sum() < totalWinnings)
        {
            
            int rndTChest = Random.Range(0, winningsSplit.Length);

            winningsSplit[rndTChest] += 0.05f;
            winningsSplit[rndTChest] = Mathf.Round(winningsSplit[rndTChest] * 100f) / 100f;
            TreasureChestScripts[rndTChest].value = winningsSplit[rndTChest];
        }

    }
    #endregion

    #region Calculating Wins & Ending game
    public void AcceptWinsAmount(float value)
    {

        WinsCheck(value);
    }

    public void RoundEnd()
    {
        PlayButton.interactable = true;
        for (int i = 0; i < TreasureChests.Length; i++)
        {
            TreasureChests[i].GetComponent<Button>().interactable = false;
            TreasureChestScripts[i].isModified = false;
            TreasureChestScripts[i].wasClicked = false;
            TreasureChestScripts[i].isPooper = false;
            TreasureChestScripts[i].chestText.text = "Chest";
        }

        CurrentBalanceCheck(currentWins);
        //WinsCheck(currentWins);
        DenominationCheck(0);

        for (int i = 0; i < IncrementButtons.Length; i++)
        {
            IncrementButtons[i].GetComponent<Button>().interactable = true;
        }
        increment.currentDenom = 0;
        increment.previousDenom = 0;
        increment.newRound = true;
        increment.DenomTextUpdate();
        MainCanvas.GetComponent<UIManager>().PanelFadeOut();
    }
    #endregion
}

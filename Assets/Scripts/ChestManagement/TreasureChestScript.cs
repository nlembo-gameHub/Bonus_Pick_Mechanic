using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class TreasureChestScript : MonoBehaviour
{
    //Floats and Ints
    [Header("Float & Int Variables")]
    [SerializeField] public float value;
    [SerializeField] private float winsAmount;
    [SerializeField] private int percent;
    //Bools
    [Header("Bool Variables")]
    [SerializeField] public bool isPooper;
    [SerializeField] public bool isModified;
    [SerializeField] public bool wasClicked;

    //Game Objects
    [Header("Game Objects")]
    [SerializeField] private GameObject gameManager;
    [SerializeField] private BonusManager managerScript;
    [SerializeField] private TreasureChestScript otherChests;
    [SerializeField] private GameObject coinGoToLoc;

    //UI Variables
    [Header("UI Variables")]
    [SerializeField] private Sprite chestSprite1;
    [SerializeField] private Sprite chestSprite2;
    [SerializeField] private GameObject PileOfCoinsParent;
    [SerializeField] public TextMeshProUGUI chestText;
    [SerializeField] private Vector3[] InitialCoinPos;
    [SerializeField] private Quaternion[] InitialCoinRot;
    [SerializeField] private int CoinNum;
    [SerializeField] private Transform _shaker;



    void Start()
    {
        //Get Components
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        managerScript = gameManager.GetComponent<BonusManager>();
        chestSprite1 = GetComponent<Image>().sprite;

        //Coin Transform Array Lengths
        CoinNum = PileOfCoinsParent.transform.childCount;
        InitialCoinPos = new Vector3[CoinNum];
        InitialCoinRot = new Quaternion[CoinNum];

        //Getting Initial Transforms for the Coin Objects
        for(int i = 0; i < CoinNum; i++)
        {
            InitialCoinPos[i] = PileOfCoinsParent.transform.GetChild(i).position;
            InitialCoinRot[i] = PileOfCoinsParent.transform.GetChild(i).rotation;
        }
    }

    #region Chest Checking System
    public void CheckPercentValue(int _percent)
    {
        percent = _percent;
    }
    public void CheckChest()
    {
        if (percent == 0)
        {
            managerScript.AcceptWinsAmount(value);
            RoundOver();
        }
        else
        {
            if (value != 0)
            {
                GetComponent<Image>().sprite = chestSprite2;
                chestText.text = "$" + value.ToString();
                managerScript.AcceptWinsAmount(value);
                value = 0;
                wasClicked = true;
                
                PileOfCoinAnim(PileOfCoinsParent.transform.childCount);
                this.GetComponent<Button>().interactable = false;
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
    #endregion
    public void RoundOver()
    {

        managerScript.RoundEnd();
    }

    #region UI Functions
    private void RestCoinTrans()
    {
        //Is resetting the transform of the coins
        for (int i = 0; i < CoinNum; i++)
        {
            PileOfCoinsParent.transform.GetChild(i).position = InitialCoinPos[i];
            PileOfCoinsParent.transform.GetChild(i).rotation = InitialCoinRot[i];
        }
    }

    private void PileOfCoinAnim(int Num_Coin)
    {
        RestCoinTrans();

        float delay = 0f;
        PileOfCoinsParent.SetActive(true);

        for (int i = 0; i < CoinNum; i++)
        {
           //Scale the coins up
           PileOfCoinsParent.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
           //Have the coins move to the CurrentWins counter location
           PileOfCoinsParent.transform.GetChild(i).GetComponent<RectTransform>().DOLocalMove(new Vector2(coinGoToLoc.transform.localPosition.x, coinGoToLoc.transform.localPosition.y), 1f).SetDelay(delay + 0.5f).SetEase(Ease.InBack);

           PileOfCoinsParent.transform.GetChild(i).DOLocalRotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);
            //Scale the coins back down
           PileOfCoinsParent.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.8f).SetEase(Ease.OutBack);
            
           //Increase the delay so that each coin scales and moves as a differing times
            delay += 0.1f;
        }
    }

    public void ChestShake()
    {
        const float duration = 1f;
        const float strength = 5f;

        var tween = _shaker.DOShakePosition(duration, strength);

        if (tween.IsPlaying()) return;

        _shaker.DOShakeRotation(duration, strength);
        _shaker.DOShakeScale(duration, strength);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

public class DiceRolla : MonoBehaviour
{
    public static DiceRolla DiceRoll;
    [SerializeField] List<Rigidbody> Dice3D;
    [SerializeField] TMP_Text TimeDisplay;
    [SerializeField] TMP_Text ExtraPayIndicator;
    int dice1Value, dice2Value;
    public TMP_Text BalanceDisplay;
    [SerializeField] GameObject Notifications;
    public Button StartRolling;
    [SerializeField] float totalTimeForBets = 10f;
    double totalTimeLeft = 10f;
    public bool isCountdown = false;
    public LayerMask ColliderLayer;
    public GameObject HistoryPanel;
    [SerializeField] List<Sprite> DieFaces;
    [SerializeField] List<Sprite> StateOfRound;
    private List<Vector3> InitialDicePos = new();
    public GameObject ParentAudioSource;
    public bool areRolling = false;
    [SerializeField] TMP_Text Username;
    [SerializeField] DiceRotationHandler DicesHandler;
    [SerializeField] GameObject DiceContainer;
    [SerializeField] AlertPanelHandler ServerPopup;
    public int MultiplayerIndex;
    [SerializeField] TMP_Text DemoIndicator;
    public int GameCount = 0;
    [SerializeField] int Force = 5;
    public GameObject ParentOfDices;
    public String currentMatchToken;
    public Double winProbability;
    public int BetIndex;
    public string CurrentBetID;

    void Awake()
    {
        DiceRoll = this;
    }

    //void ResetExtraPay()
    //{
    //    foreach (ButtonsManager button in BetButtons)
    //    {
    //        button.GetComponent<Image>().color = Color.white;
    //        button.ExtraPay = false;
    //    }
    //    //BetsManager.Bets.isExtraPay = false;
    //    ExtraPayButtons.Clear();
    //    BetsManager.Bets.BetHistory.Clear();
    //    BetsManager.Bets.UndoneMoves.Clear();
    //}

    private void Start()
    {
        foreach (Rigidbody dice in Dice3D) InitialDicePos.Add(dice.transform.position);
        PoolContainer.PoolInstance.CreateContainer();
        MultiplayerIndex = 0;
        isCountdown = true;
        ParentAudioSource.transform.GetChild(5).GetComponent<AudioSource>().Play();
        ParentAudioSource.transform.GetChild(5).GetComponent<AudioSource>().volume = 0.0f;
    }

    /*public void OnTabSwitch(bool OnCurrentTab) //Get rid of it
    {
        if (OnCurrentTab)
        {
            if (BetsManager.Bets.Settings.SoundToggle.isOn)
            {
                AudioListener.volume = 1;
                DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(5).GetComponent<AudioSource>().volume = 1.0f;
            }
        }
        else
        {
            AudioListener.volume = 0;
            DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(5).GetComponent<AudioSource>().volume = 0.0f;
        }
        if (APIController.instance.isClickDeopsit)
        {
            AudioListener.volume = 0;
            DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(5).GetComponent<AudioSource>().volume = 0.0f;
        }
    }*/

    private void Update()
    {
        UpdateUserDetailsInUI();
        UpdateBalanceInUI();
    }

    public void UpdateUserDetailsInUI() //Replace with a non api reliant method
    {
        Username.text = $"Default";
        /*if (APIController.instance.userDetails.isBlockApiConnection)*/ DemoIndicator.text = "7up 7down\r\n<size=35><alpha=#80>DEMO";
        /*else*/ DemoIndicator.text = "7up 7down";
    }

    public void UpdateBalanceInUI() //Replace with a non api reliant method
    {
        BalanceDisplay.text = $"{BetsManager.Bets.Degenerate.Balance.ToString("f2")} <size=40>USD";
    }

    public IEnumerator RollIt()
    {
            if (BetsManager.Bets.Settings.SoundToggle.isOn) ParentAudioSource.transform.GetChild(1).GetComponent<AudioSource>().Play();
            BetsManager.Bets.BetHistory.Clear();
            BetsManager.Bets.UndoneMoves.Clear();
            //ExtraPayIndicator.gameObject.SetActive(true);
            DicesHandler.RollDices();
            //Debug.Log(extraFlag);
            //if (extraFlag == 1)
            //{
            //    ExtraPayIndicator.gameObject.SetActive(true);
            //    BetsManager.Bets.isExtraPay = true;
            //    index = UnityEngine.Random.Range(2, 4);
            //    for (int i = 0; i < index; i++)
            //    {
            //        int element = UnityEngine.Random.Range(0, BetButtons.Count);
            //        ExtraPayButtons.Add(BetButtons[element]);
            //    }
            //    foreach (ButtonsManager button in ExtraPayButtons) button.TriggerExtraPay(true);
            //    yield return new WaitForSeconds(3f);
            //    foreach (ButtonsManager button in ExtraPayButtons) button.TriggerExtraPay(false);
            //    ExtraPayIndicator.gameObject.SetActive(false);
            //}
            yield return new WaitForSeconds(6f);
            //ExtraPayIndicator.gameObject.SetActive(false);
            isCountdown = true;
            BetsManager.Bets.ResetBets(true);
            BetsManager.Bets.HasDoubled.Clear();
            //StartCoroutine("BetCountdown");
            areRolling = false;
            foreach (GameObject range in BetsManager.Bets.BetButtonParents) for (int i = 0; i < range.transform.childCount; i++) range.transform.GetChild(i).GetComponent<Button>().interactable = true;
            BetsManager.Bets.QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = true;
    }

    int Matchstate = 2;

    public IEnumerator Notification(int type, int value)
    {
        switch (type)
        {
            case 0:
                Notifications.transform.GetChild(type).gameObject.SetActive(true);
                Notifications.transform.GetChild(type).GetChild(0).GetComponentInChildren<TMP_Text>().text = $"{value:f2} USD";
                if (BetsManager.Bets.Settings.SoundToggle.isOn) ParentAudioSource.transform.GetChild(3).GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(2f);
                Notifications.transform.GetChild(type).gameObject.SetActive(false);
                break;
            case 1:
                Notifications.transform.GetChild(type).gameObject.SetActive(true);
                Notifications.transform.GetChild(type).GetChild(0).GetComponentInChildren<TMP_Text>().text = "Between Up and Down, you can place your bets on only one of them";
                yield return new WaitForSeconds(2f);
                Notifications.transform.GetChild(type).gameObject.SetActive(false);
                break;
            case 2:
                Notifications.transform.GetChild(1).gameObject.SetActive(true);
                Notifications.transform.GetChild(1).GetChild(0).GetComponentInChildren<TMP_Text>().text = "You are exceeding the betting limit";
                yield return new WaitForSeconds(2f);
                Notifications.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 3:
                Notifications.transform.GetChild(2).gameObject.SetActive(true);
                Notifications.transform.GetChild(2).GetChild(0).GetComponentInChildren<TMP_Text>().text = $"{value:f2} USD";
                if (BetsManager.Bets.Settings.SoundToggle.isOn) ParentAudioSource.transform.GetChild(4).GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(2f);
                Notifications.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 4:
                Notifications.transform.GetChild(1).gameObject.SetActive(true);
                Notifications.transform.GetChild(1).GetChild(0).GetComponentInChildren<TMP_Text>().text = "Bet Cannot Be Doubled Past This Point As The Bet Limit is 100";
                yield return new WaitForSeconds(2f);
                Notifications.transform.GetChild(1).gameObject.SetActive(false);
                break;
        }
    }
    public void UpdateHistory(int dice1Value, int dice2Value, int Sum)
    {
        GameObject HistoryTab;
        HistoryTab = PoolContainer.PoolInstance.GetProjectile();
        HistoryTab.transform.SetParent(HistoryPanel.transform);
        HistoryTab.transform.localPosition = Vector2.zero;
        //Debug.Log($"Has Won: {BetsManager.Bets.HasWon}");
        HistoryPanel.transform.parent.GetChild(1).gameObject.SetActive(true);
        Matchstate = BetsManager.Bets.Results(Sum);
        if (Sum <= 6) HistoryTab.transform.GetChild(0).GetComponent<Image>().sprite = StateOfRound[0];
        else if (Sum == 7) HistoryTab.transform.GetChild(0).GetComponent<Image>().sprite = StateOfRound[2];
        else HistoryTab.transform.GetChild(0).GetComponent<Image>().sprite = StateOfRound[1];
        HistoryTab.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        HistoryTab.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = Sum.ToString();
        int buffer, flag;
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                buffer = dice1Value;
                flag = 1;
            }
            else
            {
                buffer = dice2Value;
                flag = 2;
            }
            switch (buffer)
            {
                case 1:
                    HistoryTab.transform.GetChild(flag).GetComponent<Image>().sprite = DieFaces[0];
                    break;
                case 2:
                    HistoryTab.transform.GetChild(flag).GetComponent<Image>().sprite = DieFaces[1];
                    break;
                case 3:
                    HistoryTab.transform.GetChild(flag).GetComponent<Image>().sprite = DieFaces[2];
                    break;
                case 4:
                    HistoryTab.transform.GetChild(flag).GetComponent<Image>().sprite = DieFaces[3];
                    break;
                case 5:
                    HistoryTab.transform.GetChild(flag).GetComponent<Image>().sprite = DieFaces[4];
                    break;
                case 6:
                    HistoryTab.transform.GetChild(flag).GetComponent<Image>().sprite = DieFaces[5];
                    break;
            }
            HistoryTab.transform.GetChild(flag).GetComponent<Image>().SetNativeSize();
        }
        if (HistoryPanel.transform.childCount > 12) PoolContainer.PoolInstance.ReturnProjectile(HistoryPanel.transform.GetChild(0).gameObject);
        GameCount++;
    }

    public void BetCountdown()
    {
        if (BetsManager.Bets.Degenerate.Balance - BetsManager.Bets.Degenerate.BettingPool < 0)
        {
            BetsManager.Bets.DisplayInsufficientPopup();
            return;
        }
        else
        {
            bool isEmpty = true;
            BetsManager.Bets.QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
            BetsManager.Bets.QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
            BetsManager.Bets.QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
            BetsManager.Bets.QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = false;
            BetsManager.Bets.Degenerate.Balance -= BetsManager.Bets.Degenerate.BettingPool;
            //CallCreateMatch(); //Get rid of it
            foreach (int item in BetsManager.Bets.BettingPools)
            {
                if (item > 0)
                {
                    isEmpty = false;
                    break;
                }
            }
            if (!areRolling && !isEmpty)
            {
                StartRolling.interactable = false;
                isCountdown = false;
                BetsManager.Bets.RedoBettingPools.Clear();
                foreach (int item in BetsManager.Bets.BettingPools) BetsManager.Bets.RedoBettingPools.Add(item);
                foreach (GameObject range in BetsManager.Bets.BetButtonParents) for (int i = 0; i < range.transform.childCount; i++) range.transform.GetChild(i).GetComponent<Button>().interactable = false;
                StartCoroutine("RollIt");
                isCountdown = false;
            }
        }
        //double startTime = (DateTime.UtcNow.Hour * 60 * 60) + (DateTime.UtcNow.Minute * 60) + (DateTime.UtcNow.Second);
        //double currentTime = (DateTime.UtcNow.Hour * 60 * 60) + (DateTime.UtcNow.Minute * 60) + (DateTime.UtcNow.Second);
        //while(isCountdown)
        //{
        //    //Debug.Log(startTime + " " + currentTime);
        //    totalTimeLeft = (startTime + totalTimeForBets) - currentTime;
        //    TimeDisplay.transform.parent.gameObject.SetActive(true);
        //    TimeDisplay.text = $"{(int)totalTimeLeft}.00";
        //    yield return null;
        //    currentTime = (DateTime.UtcNow.Hour * 60 * 60) + (DateTime.UtcNow.Minute * 60) + (DateTime.UtcNow.Second);
        //if (totalTimeLeft <= 0)
        //{
        //totalTimeLeft = totalTimeForBets;
        //TimeDisplay.transform.parent.gameObject.SetActive(false);
        //}
        //}
    }
}
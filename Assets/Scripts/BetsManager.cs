using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Playables;
using DG.Tweening.Core.Easing;

public class BetsManager : MonoBehaviour
{
    public static BetsManager Bets;
    public List<BetInstance> BetHistory;
    public List<BetInstance> UndoneMoves;
    [SerializeField] TMP_Text BalanceDisplay;
    [SerializeField] ConfirmationPanelHandler Panel;
    [SerializeField] AlertPanelHandler InsufficientBrokeBoi;
    public int MatchState = 2;
    [SerializeField] GameObject Cursor;
    [SerializeField] List<RectTransform> CursorPositions;
    [SerializeField] List<Sprite> CoinSpriteList;
    public List<GameObject> BetButtonParents;
    public GameObject QuickAccessParentObject;
    [SerializeField] TMP_Text PoolDisplay;
    int index;
    [SerializeField] List<bool> HasRedone;
    public List<bool> HasDoubled;
    [SerializeField] GameObject TopRange;

    [Header("Bet Storage")]
    public List<int> BettingPools = new();
    public List<int> RedoBettingPools = new();
    public PlayerData Degenerate;
    int BetAmount = 0;

    private void Awake()
    {
        Bets = this;
    }

    private IEnumerator Start()
    {
        ResetBets(true);
        yield return null;
        BetAmountSelection(5);
        index = 0;
    }

    private void Update()
    {
        PoolDisplay.text = $"{Degenerate.BettingPool.ToString("f2")} <size=34>USD</size>";
    }

    public void ResetBets(bool state)
    {
        if (!state)
        {
        }
        Degenerate.BettingPool = 0;
        for (int i = 1; i <= 13; i++) CoinActivation(i, 0, false);
        for (int i = 0; i < 13; i++) BettingPools[i] = 0;
        BetHistory.Clear();
    }

    /*1 4 1 26 12 8 6 5 26 12 8 6 5*/

    public void Highlight(int ButtonIndex)
    {
        if (ButtonIndex == 7)//7
        {
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(1).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(1).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 2)//2
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(0).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(0).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 3)//3
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(1).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(1).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 4)//4
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(2).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(2).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 5)//5
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(3).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(3).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 6)//6
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(4).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(4).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(0).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 8)//8
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(5).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(5).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 9)//9
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(6).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(6).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 10)//10
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(7).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(7).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 11)//11
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(8).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(8).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
            .Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
        if (ButtonIndex == 12)//12
        {
            DOTween.Sequence().Append(BetButtonParents[1].transform.GetChild(9).GetChild(2).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[1].transform.GetChild(9).GetChild(2).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
            DOTween.Sequence().Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(1.0f, 0.25f).From(0))
                .Append(BetButtonParents[0].transform.GetChild(2).GetChild(5).GetComponent<Image>().DOFade(0, 0.25f).From(1)).SetLoops(6);
        }
    }
    public int Results(int Sum)
    {
        MatchState = 2;
        int payout = 0;
        Highlight(Sum);
        if (BettingPools[0] > 0 && Sum <= 6) payout += BettingPools[0] * 2;
        if (BettingPools[1] > 0 && Sum == 7) payout += BettingPools[1] + (BettingPools[1] * 3);
        if (BettingPools[2] > 0 && Sum >= 8) payout += BettingPools[2] * 2;
        if (BettingPools[3] > 0 && Sum == 2) payout += BettingPools[3] + (BettingPools[3] * 8);
        if (BettingPools[4] > 0 && Sum == 3) payout += BettingPools[4] + (BettingPools[4] * 7);
        if (BettingPools[5] > 0 && Sum == 4) payout += BettingPools[5] + (BettingPools[5] * 6);
        if (BettingPools[6] > 0 && Sum == 5) payout += BettingPools[6] + (BettingPools[6] * 5);
        if (BettingPools[7] > 0 && Sum == 6) payout += BettingPools[7] + (BettingPools[7] * 4);
        if (BettingPools[8] > 0 && Sum == 8) payout += BettingPools[8] + (BettingPools[8] * 4);
        if (BettingPools[9] > 0 && Sum == 9) payout += BettingPools[9] + (BettingPools[9] * 5);
        if (BettingPools[10] > 0 && Sum == 10) payout += BettingPools[10] + (BettingPools[10] * 6);
        if (BettingPools[11] > 0 && Sum == 11) payout += BettingPools[11] + (BettingPools[11] * 7);
        if (BettingPools[12] > 0 && Sum == 12) payout += BettingPools[12] + (BettingPools[12] * 8);
        //Debug.Log($"Payout:{payout}");
        if (payout > 0)
        {
            Degenerate.Balance += payout;
            StartCoroutine(DiceRolla.DiceRoll.Notification(0, payout));
            MatchState = 0;
        }
        else if (payout == 0)
        {
            MatchState = 1;
            StartCoroutine(DiceRolla.DiceRoll.Notification(3, Degenerate.BettingPool));
        }
        /*TransactionMetaData metaData = new()
        {
            Amount = BetsManager.Bets.Degenerate.BettingPool,
            Info = "Winnings" +
            " Bet",
        };*/
        /*if (APIController.instance.userDetails.isBlockApiConnection) APIController.instance.WinningsBet(DiceRolla.DiceRoll.BetIndex, payout, Degenerate.BettingPool, metaData);
        else
        {
            APIController.instance.WinningsBetMultiplayerAPI(DiceRolla.DiceRoll.BetIndex, DiceRolla.DiceRoll.CurrentBetID, APIController.instance.userDetails.commission + payout, Degenerate.BettingPool, Degenerate.BettingPool, metaData, (status) => { }, APIController.instance.userDetails.Id, false, true, "7up7down", APIController.instance.userDetails.game_Id.Split("_")[0], APIController.instance.userDetails.gameId, APIController.instance.userDetails.commission, DiceRolla.DiceRoll.currentMatchToken);
        }*/ //Replace the entire if-else clause with non API statements
        HasRedone.Clear();
        return MatchState;
    }

    [SerializeField] List<GameObject> CloseButtons;

    public void DisplayInsufficientPopup()
    {
        if (Degenerate.Balance < 5)
        {
            CloseButtons[0].SetActive(false);
            InsufficientBrokeBoi.ShowMessageWithAction("Oops, you have spent all of your demo money. Go back to lobby to retain your demo money", null, "Insufficient Balance", "BACK TO LOBBY", AlertType.Low);
            CloseButtons[0].SetActive(false);
            Panel.ShowMessageWithAction("You dont have enough money! Kindly add money to your wallet.", null, null, "Insufficient Balance", "ADD CASH", "BACK TO LOBBY", AlertType.Low);
        }
        else
        {
            CloseButtons[0].SetActive(true);
            InsufficientBrokeBoi.ShowMessageWithAction("Oops, you have spent all of your demo money. Go back to lobby to retain your demo money", null, "Insufficient Balance", "BACK TO LOBBY", AlertType.Low);
            if (!InsufficientBrokeBoi.GetComponentInParent<Transform>().GetChild(0).GetChild(2).gameObject.activeSelf) InsufficientBrokeBoi.GetComponentInParent<Transform>().GetChild(0).GetChild(2).gameObject.SetActive(true);
            //Get rid of this condition
        }
    }

    /*
       > 
        -- 3 (UwU)
       >
     
    */

    /*
        .--'''''''''--.
     .'      .---.      '.
    /    .-----------.    \
   /        .-----.        \
   |       .-.   .-.       |
   |      /   \ /   \      |
    \    | .-. | .-. |    /
     '-._| | | | | | |_.-'
         | '-' | '-' |
          \___/ \___/
       _.-'  /   \  `-._
     .' _.--|     |--._ '.
     ' _...-|     |-..._ '
            |     |
            '.___.'
              | |
             _| |_
            /\( )/\
           /  ` '  \
          | |     | |
          '-'     '-'
          | |     | |
          | |     | |
          | |-----| |
       .`/  |     | |/`.
       |    |     |    |
       '._.'| .-. |'._.'
             \ | /
             | | |
             | | |
             | | |
            /| | |\
          .'_| | |_`.
          `. | | | .'
       .    /  |  \    .
      /o`.-'  / \  `-.`o\
     /o o\ .'   `. /o  o\
     `.___.'       `.___.
                          */

    public void QuickAccess(int FunctionIndex)
    {
        if (DiceRolla.DiceRoll.isCountdown)
        {
            switch (FunctionIndex)
            {
                case 1:
                    for (int item = 0; item < BettingPools.Count; item++)
                    {
                        if (BettingPools[item] > 0 && Degenerate.Balance >= Degenerate.BettingPool + BettingPools[item] && Degenerate.BettingPool + BettingPools[item] <= 100)
                        {
                            Degenerate.BettingPool += BettingPools[item];
                            BetInstance buffer = new(item + 1, BettingPools[item]);
                            BetHistory.Add(buffer);
                            BettingPools[item] += BettingPools[item];
                            CoinActivation(item + 1, BettingPools[item], true);
                        }
                        else if (BettingPools[item] > 0 && Degenerate.BettingPool + BettingPools[item] > Degenerate.Balance)
                        {
                            DisplayInsufficientPopup();
                            break;
                        }
                    }
                    if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(0).GetComponent<AudioSource>().Play();
                    HasDoubled.Add(true);
                    HasRedone.Add(false);
                    if (Degenerate.BettingPool > 50)
                    {
                        QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                        StartCoroutine(DiceRolla.DiceRoll.Notification(4, 0));
                        return;
                    }
                    break;
                case 2:
                    if (BetHistory.Count > 0)
                    {
                        if (!HasRedone[^1])
                        {
                            if (!HasDoubled[^1] && HasDoubled.Count >= 1)
                            {
                                if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(0).GetComponent<AudioSource>().Play();
                                switch (BetHistory[^1].BetID)
                                {
                                    case 1:
                                        BettingPools[0] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[0] > 0) CoinActivation(1, BettingPools[0], true);
                                        else CoinActivation(1, BettingPools[0], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 2:
                                        BettingPools[1] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[1] > 0) CoinActivation(2, BettingPools[1], true);
                                        else CoinActivation(2, BettingPools[1], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 3:
                                        BettingPools[2] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[2] > 0) CoinActivation(3, BettingPools[2], true);
                                        else CoinActivation(3, BettingPools[2], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 4:
                                        BettingPools[3] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[3] > 0) CoinActivation(4, BettingPools[3], true);
                                        else CoinActivation(4, BettingPools[3], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 5:
                                        BettingPools[4] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[4] > 0) CoinActivation(5, BettingPools[4], true);
                                        else CoinActivation(5, BettingPools[4], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 6:
                                        BettingPools[5] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[5] > 0) CoinActivation(6, BettingPools[5], true);
                                        else CoinActivation(6, BettingPools[5], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 7:
                                        BettingPools[6] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[6] > 0) CoinActivation(7, BettingPools[6], true);
                                        else CoinActivation(7, BettingPools[6], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 8:
                                        BettingPools[7] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[7] > 0) CoinActivation(8, BettingPools[7], true);
                                        else CoinActivation(8, BettingPools[7], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 9:
                                        BettingPools[8] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[8] > 0) CoinActivation(9, BettingPools[8], true);
                                        else CoinActivation(9, BettingPools[8], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 10:
                                        BettingPools[9] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[9] > 0) CoinActivation(10, BettingPools[9], true);
                                        else CoinActivation(10, BettingPools[9], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 11:
                                        BettingPools[10] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[10] > 0) CoinActivation(11, BettingPools[10], true);
                                        else CoinActivation(11, BettingPools[10], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 12:
                                        BettingPools[11] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[11] > 0) CoinActivation(12, BettingPools[11], true);
                                        else CoinActivation(12, BettingPools[11], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                    case 13:
                                        BettingPools[12] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        if (BettingPools[12] > 0) CoinActivation(13, BettingPools[12], true);
                                        else CoinActivation(13, BettingPools[12], false);
                                        UndoneMoves.Add(BetHistory[^1]);
                                        BetHistory.Remove(BetHistory[^1]);
                                        if ((BetHistory.Count == 0) && (DiceRolla.DiceRoll.StartRolling.interactable))
                                        {
                                            DiceRolla.DiceRoll.StartRolling.interactable = false;
                                            QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                        }
                                        break;
                                }
                                HasDoubled.RemoveAt(HasDoubled.Count - 1);
                            }
                            else if (HasDoubled[^1] && HasDoubled.Count >= 1)
                            {
                                Debug.Log("Positive Condition Encountered");
                                for (int i = BettingPools.Count - 1; i >= 0; i--)
                                {
                                    if (BettingPools[i] > 0 && BettingPools[i] % 2 == 0)
                                    {
                                        BettingPools[i] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        CoinActivation(i + 1, BettingPools[i], true);
                                        BetHistory.Remove(BetHistory[^1]);
                                    }
                                }
                                HasDoubled.RemoveAt(HasDoubled.Count - 1);
                            }
                            HasRedone.RemoveAt(HasRedone.Count - 1);
                        }
                        else if (HasRedone[^1])
                        {
                            Debug.Log("Clean Table Condition Encountered");
                            QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
                            if (RedoBettingPools.Count > 0) QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = true;
                            else QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
                            QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = false;
                            if (HasDoubled[^1] && HasDoubled.Count >= 1)
                            {
                                Debug.Log("Positive Condition Encountered");
                                for (int i = 0; i < BettingPools.Count; i++)
                                {
                                    if (BettingPools[i] > 0 && BettingPools[i] % 2 == 0)
                                    {
                                        BettingPools[i] -= BetHistory[^1].BetAmount;
                                        Degenerate.BettingPool -= BetHistory[^1].BetAmount;
                                        CoinActivation(i + 1, BettingPools[i], true);
                                    }
                                }
                                HasDoubled.RemoveAt(HasDoubled.Count - 1);
                            }
                            goto case 4;
                        }
                        if (Degenerate.BettingPool == 0)
                        {
                            Debug.Log("BettingPool 0 Condition Encountered");
                            QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
                            if (RedoBettingPools.Count > 0) QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = true;
                            else QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
                            QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = false;
                        }
                        else if (Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    }
                    break;
                case 3:
                    int sum = 0;
                    foreach (int item in RedoBettingPools) sum += item;
                    if (sum > Degenerate.Balance) DisplayInsufficientPopup();
                    else if (DiceRolla.DiceRoll.isCountdown && RedoBettingPools.Count > 0)
                    {
                        if (!DiceRolla.DiceRoll.StartRolling.interactable) DiceRolla.DiceRoll.StartRolling.interactable = true;
                        BettingPools.Clear();
                        int count = 1;
                        foreach (int item in RedoBettingPools)
                        {
                            BettingPools.Add(item);
                            Degenerate.BettingPool += item;
                            if (item > 0)
                            {
                                CoinActivation(count, BettingPools[count - 1], true);
                                BetInstance buffer = new(count, BettingPools[count - 1]);
                                BetHistory.Add(buffer);
                                HasDoubled.Add(false);
                            }
                            count++;
                        }
                        if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(0).GetComponent<AudioSource>().Play();
                        HasRedone.Add(true);
                        QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
                        QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                        QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                        QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                    }
                    break;
                case 4:
                    if (DiceRolla.DiceRoll.isCountdown) ResetBets(false);
                    QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = false;
                    if (RedoBettingPools.Count > 0) QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = true;
                    else QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
                    QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    DiceRolla.DiceRoll.StartRolling.interactable = false;
                    if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(0).GetComponent<AudioSource>().Play();
                    HasDoubled.Clear();
                    HasRedone.Clear();
                    break;
            }
        }
    }

    public void CoinActivation(int buttonIndex, int Amount, bool flag)
    {
        switch (buttonIndex)
        {
            case 1:
                BetButtonParents[0].transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(0, 0, Amount);
                break;
            case 2:
                BetButtonParents[0].transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(0, 1, Amount);
                break;
            case 3:
                BetButtonParents[0].transform.GetChild(2).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(0, 2, Amount);
                break;
            case 4:
                BetButtonParents[1].transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 0, Amount);
                break;
            case 5:
                BetButtonParents[1].transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 1, Amount);
                break;
            case 6:
                BetButtonParents[1].transform.GetChild(2).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 2, Amount);
                break;
            case 7:
                BetButtonParents[1].transform.GetChild(3).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 3, Amount);
                break;
            case 8:
                BetButtonParents[1].transform.GetChild(4).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 4, Amount);
                break;
            case 9:
                BetButtonParents[1].transform.GetChild(5).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 5, Amount);
                break;
            case 10:
                BetButtonParents[1].transform.GetChild(6).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 6, Amount);
                break;
            case 11:
                BetButtonParents[1].transform.GetChild(7).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 7, Amount);
                break;
            case 12:
                BetButtonParents[1].transform.GetChild(8).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 8, Amount);
                break;
            case 13:
                BetButtonParents[1].transform.GetChild(9).transform.GetChild(3).gameObject.SetActive(flag);
                if (flag) CoinColorAssignment(1, 9, Amount);
                break;
        }
    }

    void CoinColorAssignment(int ParentID, int ButtonID, int Amount)
    {
        if (Amount >= 1000)
        {
            float buffer = (float)Amount / 1000;
            BetButtonParents[ParentID].transform.GetChild(ButtonID).transform.GetChild(3).GetComponentInChildren<TMP_Text>().text = $"{buffer}k";
        }
        else BetButtonParents[ParentID].transform.GetChild(ButtonID).transform.GetChild(3).GetComponentInChildren<TMP_Text>().text = Amount.ToString();
        if (Amount < 10) BetButtonParents[ParentID].transform.GetChild(ButtonID).transform.GetChild(3).GetComponent<Image>().sprite = CoinSpriteList[0];
        else if (Amount >= 10 && Amount < 25) BetButtonParents[ParentID].transform.GetChild(ButtonID).transform.GetChild(3).GetComponent<Image>().sprite = CoinSpriteList[1];
        else if (Amount >= 25 && Amount < 50) BetButtonParents[ParentID].transform.GetChild(ButtonID).transform.GetChild(3).GetComponent<Image>().sprite = CoinSpriteList[2];
        else if (Amount >= 50 && Amount < 100) BetButtonParents[ParentID].transform.GetChild(ButtonID).transform.GetChild(3).GetComponent<Image>().sprite = CoinSpriteList[3];
        else BetButtonParents[ParentID].transform.GetChild(ButtonID).transform.GetChild(3).GetComponent<Image>().sprite = CoinSpriteList[4];
    }

    public List<int> BetTypes;

    public void StoreBets(int ButtonID)
    {
        if ((Degenerate.BettingPool < 100) && ((Degenerate.BettingPool + BetAmount) <= 100))
        {
            if (!DiceRolla.DiceRoll.StartRolling.interactable && BetAmount <= Degenerate.Balance) DiceRolla.DiceRoll.StartRolling.interactable = true;
            if (DiceRolla.DiceRoll.isCountdown)
            {
                switch (ButtonID)
                {
                    case 1:
                        if (BettingPools[2] == 0)
                        {
                            if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                            {
                                //Degenerate.Balance -= BetAmount;
                                BetInstance buffer = new(1, BetAmount);
                                BetHistory.Add(buffer);
                                Degenerate.BettingPool += BetAmount;
                                BettingPools[0] += BetAmount;
                                HasDoubled.Add(false);
                                HasRedone.Add(false);
                                if (!BetTypes.Contains(0)) BetTypes.Add(0);
                                CoinActivation(1, BettingPools[0], true);
                                if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                                else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                                if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                                if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                            }
                            else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        }
                        else StartCoroutine(DiceRolla.DiceRoll.Notification(1, 0));
                        break;
                    case 2:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            BetInstance buffer = new(2, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[1] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(1)) BetTypes.Add(1);
                            CoinActivation(2, BettingPools[1], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 3:
                        if (BettingPools[0] == 0)
                        {
                            if (Degenerate.Balance >= BetAmount)
                            {
                                //Degenerate.Balance -= BetAmount;
                                BetInstance buffer = new(3, BetAmount);
                                BetHistory.Add(buffer);
                                Degenerate.BettingPool += BetAmount;
                                BettingPools[2] += BetAmount;
                                HasDoubled.Add(false);
                                HasRedone.Add(false);
                                if (!BetTypes.Contains(2)) BetTypes.Add(2);
                                //TotalPoolUp += BetAmount;
                                //APIController.instance.userDetails.balance = Degenerate.Balance;
                                //BalanceDisplay.text = APIController.instance.userDetails.balance.ToString();
                                CoinActivation(3, BettingPools[2], true);
                                if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                                else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                                if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                                if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                                //RedoBettingPools.Clear();
                                if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                            }
                            else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        }
                        else StartCoroutine(DiceRolla.DiceRoll.Notification(1, 0));
                        break;
                    case 4:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(4, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[3] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(3)) BetTypes.Add(3);
                            //TotalPoolTwo += BetAmount;
                            //APIController.instance.userDetails.balance = Degenerate.Balance;
                            //BalanceDisplay.text = APIController.instance.userDetails.balance.ToString();
                            CoinActivation(4, BettingPools[3], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 5:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(5, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[4] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(4)) BetTypes.Add(4);
                            //TotalPoolThree += BetAmount;
                            //APIController.instance.userDetails.balance = Degenerate.Balance;
                            //BalanceDisplay.text = APIController.instance.userDetails.balance.ToString();
                            CoinActivation(5, BettingPools[4], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 6:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(6, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[5] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(5)) BetTypes.Add(5);
                            //TotalPoolFour += BetAmount;
                            //APIController.instance.userDetails.balance = Degenerate.Balance;
                            //BalanceDisplay.text = APIController.instance.userDetails.balance.ToString();
                            CoinActivation(6, BettingPools[5], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 7:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            BetInstance buffer = new(7, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[6] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(6)) BetTypes.Add(6);
                            //TotalPoolFive += BetAmount;
                            //APIController.instance.userDetails.balance = Degenerate.Balance;
                            //BalanceDisplay.text = APIController.instance.userDetails.balance.ToString();
                            CoinActivation(7, BettingPools[6], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 8:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(8, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[7] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(7)) BetTypes.Add(7);
                            //TotalPoolSix += BetAmount;
                            //APIController.instance.userDetails.balance = Degenerate.Balance;
                            //BalanceDisplay.text = APIController.instance.userDetails.balance.ToString();
                            CoinActivation(8, BettingPools[7], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 9:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(9, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[8] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(8)) BetTypes.Add(8);
                            //TotalPoolEight += BetAmount;
                            //APIController.instance.userDetails.balance = Degenerate.Balance;
                            //BalanceDisplay.text = APIController.instance.userDetails.balance.ToString();
                            CoinActivation(9, BettingPools[8], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 10:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(10, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[9] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(9)) BetTypes.Add(9);
                            CoinActivation(10, BettingPools[9], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 11:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(11, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[10] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(10)) BetTypes.Add(10);
                            CoinActivation(11, BettingPools[10], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 12:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(12, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[11] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(11)) BetTypes.Add(11);
                            CoinActivation(12, BettingPools[11], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                    case 13:
                        if (Degenerate.Balance >= Degenerate.BettingPool + BetAmount)
                        {
                            //Degenerate.Balance -= BetAmount;
                            BetInstance buffer = new(13, BetAmount);
                            BetHistory.Add(buffer);
                            Degenerate.BettingPool += BetAmount;
                            BettingPools[12] += BetAmount;
                            HasDoubled.Add(false);
                            HasRedone.Add(false);
                            if (!BetTypes.Contains(12)) BetTypes.Add(12);
                            CoinActivation(13, BettingPools[12], true);
                            if (!QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable && Degenerate.BettingPool <= 50) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = true;
                            else if (Degenerate.BettingPool > 50 || BetAmount == 100) QuickAccessParentObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
                            if (!QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(1).GetComponent<Button>().interactable = true;
                            if (!QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(3).GetComponent<Button>().interactable = true;
                            //RedoBettingPools.Clear();
                            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
                        }
                        else if (Degenerate.Balance <= Degenerate.BettingPool + BetAmount) DisplayInsufficientPopup();
                        break;
                }
                if (QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable) QuickAccessParentObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
            }
        }
        else StartCoroutine(DiceRolla.DiceRoll.Notification(2, 0));

    }
    public SettingsPanelHandler Settings;
    public void BetAmountSelection(int Amount)
    {
        if (DiceRolla.DiceRoll.isCountdown)
        {
            if (!Cursor.activeSelf) Cursor.SetActive(true);
            Vector2 targetPos = CursorPositions[Amount == 5 ? 0 : Amount == 10 ? 1 : Amount == 25 ? 2 : Amount == 50 ? 3 : 4].anchoredPosition;
            targetPos.y = 120;
            Cursor.GetComponent<RectTransform>().DOAnchorPos(targetPos, 0.1f);
            BetAmount = Amount;
            if (Settings.SoundToggle.isOn) DiceRolla.DiceRoll.ParentAudioSource.transform.GetChild(2).GetComponent<AudioSource>().Play();
        }
    }

    [System.Serializable]

    public class BetInstance
    {
        public int BetID;
        public int BetAmount;
        public BetInstance(int BetID, int BetAmount)
        {
            this.BetID = BetID;
            this.BetAmount = BetAmount;
        }
    }

    [System.Serializable]

    public class PlayerData
    {
        public int Balance;
        public int BettingPool;
        public PlayerData(int balance, int bettingPool)
        {
            this.Balance = balance;
            this.BettingPool = bettingPool;
        }
    }
}
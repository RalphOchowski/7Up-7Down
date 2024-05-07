using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using SwipeWire;
using DG.Tweening;
//using Mirror;

public class MatchMakingHandler : UIHandler
{
    public TMP_Text GameNameHeader;
    public TMP_Text GameNameSub;
    public GameObject[] ProfileDisplay;
    public Image[] playerBG;
    public Image[] PlayerScroll;
    public TMP_Text entryAmount;
    public TMP_Text pricePoolAmount;
    public TMP_Text GrandPricePoolAmount;
    public TMP_Text winnerCount;
    private Coroutine ScrollRoutine;
    private Coroutine ScrollRoutine1;
    private Coroutine ScrollRoutine2;
    public Button BackBtn;
    public TMP_Text OpponentName;
    public TMP_Text OpponentName1;
    public TMP_Text OpponentName2;
    public TMP_Text MyName;
    public TMP_Text Amount;
    public static MatchMakingHandler Instance;
    public TMP_Text ConnectingToServer;
    public GameObject CountdownTxt;
    public GameObject Holder1;
    public GameObject Holder2;
    public GameObject line1, line2;
    private void Awake()
    {
        Instance = this;
        BackBtn.onClick.AddListener(() => { OnClickLeaveGame(); });
    }

    private void OnDestroy()
    {
        HideMe();
    }

    bool CallOne;
    bool CallTwo;
    bool CallThree;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {

        StopCoroutine("StartGame");

    }

    public override void ShowMe()
    {

#if UNITY_WEBGL
        //  UIController.instance.loadinHandler.HideMe(); 
#endif


        base.ShowMe();
        if(APIController.instance.userDetails.bootAmount == 0)
        {
            Amount.text = "$" + 0;
            //BackBtn.gameObject.SetActive(false);
        }
        else
        {
            //BackBtn.gameObject.SetActive(true);
            Amount.text = "$" + APIController.instance.userDetails.balance;
        }

        //if (GameController.Instance.CurrentGameMode == GameMode.LUDOTWOPLAYERTURBO || GameController.Instance.CurrentGameMode == GameMode.LUDOTWOPLAYERSUPREME || GameController.Instance.CurrentGameMode == GameMode.LUDOTWOPLAYERNINJA)
        //{
        //    line1.SetActive(true);
        //    line2.SetActive(false);
        //}
        //else
        //{
        //    line2.SetActive(true);
        //    line1.SetActive(false);
        //}

       
        //GameNameHeader.text = GameController.Instance.currentGameData.name;
       // GameNameSub.text = GameController.Instance.currentGameData.name;
        IsRun1 = false;
        IsRun2 = false;
        IsRun3 = false;
        IsRun4 = false;
        CallOne = false;
        CallTwo = false;
        CallThree = false;
        playerBG[1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -215f, 0f);
        playerBG[2].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -215f, 0f);
        playerBG[3].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -215f, 0f);
        playerBG[0].GetComponent<Image>().sprite = null;///GameController.Instance.playerProfileImages[GameController.Instance.my_PlayerData.avatarIndex];
        MyName.text = "";//GameController.Instance.my_PlayerData.playerName;
        OpponentName.gameObject.SetActive(false);
        OpponentName1.gameObject.SetActive(false);
        OpponentName2.gameObject.SetActive(false);
        if (ScrollRoutine != null)
        {
            StopCoroutine(ScrollRoutine);
        }
        if (ScrollRoutine1 != null)
        {
            StopCoroutine(ScrollRoutine1);
        }
        if (ScrollRoutine2 != null)
        {
            StopCoroutine(ScrollRoutine2);
        }
        if (false)
        {
            //ScrollRoutine = StartCoroutine(ScrollOpponentImg());
            //ScrollRoutine1 = StartCoroutine(ScrollOpponentImg1());
            //ScrollRoutine2 = StartCoroutine(ScrollOpponentImg2());
            ProfileDisplay[0].SetActive(true);
            ProfileDisplay[1].SetActive(true);
            ProfileDisplay[2].SetActive(true);
            ProfileDisplay[3].SetActive(true);
        }
        else
        {
            Debug.LogError("Called Scrolling");
            ScrollRoutine = StartCoroutine(ScrollOpponentImg());
            ProfileDisplay[0].SetActive(true);
            ProfileDisplay[1].SetActive(true);
            ProfileDisplay[2].SetActive(false);
            ProfileDisplay[3].SetActive(false);
        }


        StopCoroutine(nameof(StartGame));
        StartCoroutine(nameof(StartGame));
        entryAmount.text = "$";// + //GameController.Instance.EntryAmount.ToString();
        winnerCount.text = "";//GameController.Instance.WinnerCount.ToString();
        pricePoolAmount.text = "";//GameController.Instance.PoolPrize.ToString();
        GrandPricePoolAmount.text = "";//GameController.Instance.PoolPrize.ToString();
        Holder1.SetActive(true);
        Holder2.SetActive(false);
        if (true)///GameController.Instance.PoolPrize.Equals("Free Contest"))
        {
            entryAmount.text = "Free";
            Holder1.SetActive(false);
            Holder2.SetActive(true);
            GrandPricePoolAmount.text = "Free";
        }

        //UIController.instance.settingsPanel.ActivateExitBtn();

        CountdownTxt.SetActive(false);



    }


    public void OnRecallAgain()
    {
        HideMe();
        ShowMe();
    }


    public override void OnBack()
    {
        if (BackBtn.gameObject.activeInHierarchy)
        {
            Debug.LogError("^^^^^^^^^^^^^^^^^^^^^^");
            OnClickLeaveGame();
        }



    }

    public override void HideMe()
    {
        base.HideMe();
        //if (UIController.instance.openedPages[^1] == UIController.instance.mainMenuHandler)
        //    BottomNavigator.instance.ActivateMainMenu();

        //UIController.instance.settingsPanel.ActivateExitBtn();
    }
    bool IsRun1;
    bool IsRun2;
    bool IsRun3;
    bool IsRun4;

    private IEnumerator AnimateInfoTxt()
    {
        int count = 1;
        while (ConnectingToServer.gameObject.activeSelf)
        {
            ConnectingToServer.text = "Connecting to server" + (count == 1 ? "<color=#ffdc2c>." : count == 2 ? "<color=#ffdc2c>.." : "<color=#ffdc2c>...");
            count = count >= 3 ? 1 : count + 1;
            yield return new WaitForSeconds(1f);
        }
    }
    public void Temp()
    {

        //UIController.instance.confirmationPopUpHandler.HideMe();
        //GameController.Instance.isPlayAgainGameLudo = false;
        ///GameController.Instance.isForceExit = false;
        //GameHUD.instance.ExitGame();
        HideMe();
        if (APIController.instance.userDetails.bootAmount != 0)
        {
            //UIController.instance.BuyInPage.ShowMe();
        }else
        {
            //UIController.instance.ExitWebGL();
        }
    }
    IEnumerator StartGame()
    {
        ConnectingToServer.gameObject.SetActive(true);
        StopCoroutine(nameof(AnimateInfoTxt));
        StartCoroutine(nameof(AnimateInfoTxt));
        yield return new WaitForSeconds(1f);
        //if (GameController.Instance.CurrentGameMode == GameMode.LUDOFOURPLAYERNINJA || GameController.Instance.CurrentGameMode == GameMode.LUDOFOURPLAYERSUPREME || GameController.Instance.CurrentGameMode == GameMode.LUDOFOURPLAYERTURBO)
        //{
        //    while (true)
        //    {

        //        if (Application.internetReachability == NetworkReachability.NotReachable)
        //        {
        //            if (GameManager.localInstance != null && GameManager.localInstance.gameState.currentState == 0)
        //            {
        //                   UIController.instance.alertWindow.ShowMessageWithAction("Check your Internet", (() => Temp()) , "Internet Issue", "Internet Issue");
        //            }
        //            else if (GameManager.localInstance != null && GameManager.localInstance.gameState.currentState == 1)
        //            {
        //                UIController.instance.alertWindow.ShowMessageWithAction("Click here for reconnection to server", GameController.Instance.Reconnect, "Reconnection", "ALERT");
        //            }

        //        }


        //        if (GameHUD.instance.playerUIDetails[0].isInit)
        //        {

        //            if (!IsRun1)
        //            {

        //                playerBG[0].GetComponent<Image>().sprite = GameController.Instance.playerProfileImages[GameController.Instance.my_PlayerData.avatarIndex];
        //                ConnectingToServer.gameObject.SetActive(false);
        //                ConnectingToServer.text = "Connecting to server";
        //                //  MyName.text = GameHUD.instance.playerUIDetails[0].myPlayerState.playerData.playerName;
        //                IsRun1 = true;
        //            }

        //        }
        //        else if (GameHUD.instance.playerUIDetails[1].isInit)
        //        {
        //            if (!IsRun2)
        //            {
        //                //playerBG[1].transform.GetChild(0).GetComponent<Image>().sprite = APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex(1);
        //                IsRun2 = true;
        //            }

        //        }
        //        else if (GameHUD.instance.playerUIDetails[2].isInit)
        //        {
        //            if (!IsRun3)
        //            {
        //                //playerBG[2].transform.GetChild(0).GetComponent<Image>().sprite = APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex(2);
        //                IsRun3 = true;
        //            }

        //        }
        //        else if (GameHUD.instance.playerUIDetails[3].isInit)
        //        {
        //            if (!IsRun4)
        //            {
        //               // playerBG[3].transform.GetChild(0).GetComponent<Image>().sprite = APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex(3);
        //                IsRun4 = true;
        //            }

        //        }


        //        yield return new WaitForSeconds(1f);
        //    }



        //}
        //else
        //{

        //    while (true)
        //    {

        //        if (Application.internetReachability == NetworkReachability.NotReachable)
        //        {
        //            if (GameManager.localInstance != null && GameManager.localInstance.gameState.currentState == 0)
        //            {
        //                 UIController.instance.alertWindow.ShowMessageWithAction("Check your Internet", (() => Temp()), "Internet Issue", "Internet Issue");
        //            }
        //            else if (GameManager.localInstance != null && GameManager.localInstance.gameState.currentState == 1)
        //            {
        //                   UIController.instance.alertWindow.ShowMessageWithAction("Click here for reconnection to server", GameController.Instance.Reconnect, "Reconnection", "ALERT");
        //            }

        //        }


        //        for (int i = 0; i < GameHUD.instance.playerUIDetails.Length; i++)
        //        {
        //            if (GameHUD.instance.playerUIDetails[i].isInit)
        //            {

        //                if (!IsRun1)
        //                {
        //                    ConnectingToServer.gameObject.SetActive(false);
        //                    ConnectingToServer.text = "Connecting to server";
        //                    if (i == 0)
        //                    {
        //                        playerBG[i].GetComponent<Image>().sprite = GameController.Instance.playerProfileImages[GameController.Instance.my_PlayerData.avatarIndex];
        //                    }
        //                    else
        //                    {
        //                        //playerBG[i].transform.GetChild(0).GetComponent<Image>().sprite = APIController.instance.CurrentPlayerData.GetAvatarSprite();
        //                    }

        //                    MyName.text = GameHUD.instance.playerUIDetails[i].myPlayerState.playerData.playerName;
        //                    IsRun1 = true;
        //                }

        //            }
        //            else
        //            {
        //                if (!IsRun2)
        //                {
        //                    //playerBG[1].transform.GetChild(0).GetComponent<Image>().sprite = APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex(2);
        //                    IsRun2 = true;
        //                }

        //            }

        //        }
        //        yield return new WaitForSeconds(1f);
        //    }
        //}

    }


    public void OnClickLeaveGame()
    {
        //UIController.instance.ExitWebGL();

        //UIController.instance.confirmationPopUpHandler.ShowMessageWithAction("Are you sure you want to exit the game?", () =>
        //{
        //    Debug.Log("ExitGame");

        //    UIController.instance.confirmationPopUpHandler.HideMe();
        //    GameController.Instance.isPlayAgainGameLudo = false;
        //    GameController.Instance.isForceExit = false;
        //    GameHUD.instance.ExitGame();
        //    HideMe();
        //    UIController.instance.BuyInPage.ShowMe();

        //}, UIController.instance.confirmationPopUpHandler.HideMe, "Exit Game", positiveBtnMsg: "Yes, Exit", negativeBtnMsg: "Cancel", AlertType.Exit);
    }

    private IEnumerator ScrollOpponentImg()
    {
        Debug.Log("Called 0");
        PlayerScroll[0].transform.localPosition = Vector2.zero;
        PlayerScroll[0].material.SetVector("_Offset", Vector2.zero);
        while (true)
        {
            PlayerScroll[0].material.SetVector("_Offset", (Vector2)PlayerScroll[0].material.GetVector("_Offset") + 2 * Time.deltaTime * Vector2.down);
            yield return null;
        }



    }
    //private IEnumerator ScrollOpponentImg1()
    //{

    //    Debug.Log("Called 1");
    //    PlayerScroll[1].transform.localPosition = Vector2.zero;
    //    PlayerScroll[1].material.SetVector("_Offset", Vector2.zero);

    //    while (true)
    //    {
    //        PlayerScroll[1].material.SetVector("_Offset", (Vector2)PlayerScroll[1].material.GetVector("_Offset") + 2 * Time.deltaTime * Vector2.down);
    //        yield return null;
    //    }


    //}

    //private IEnumerator ScrollOpponentImg2()
    //{
    //    Debug.Log("Called 2");
    //    PlayerScroll[2].transform.localPosition = Vector2.zero;
    //    PlayerScroll[2].material.SetVector("_Offset", Vector2.zero);

    //    while (true)
    //    {
    //        PlayerScroll[2].material.SetVector("_Offset", (Vector2)PlayerScroll[2].material.GetVector("_Offset") + 2 * Time.deltaTime * Vector2.down);
    //        yield return null;
    //    }



    //}

    public void StopScrolling()
    {
        Debug.Log("Callled Scroll 1");
        StopCoroutine(ScrollRoutine);
        //UIController.instance.settingsPanel.DeActivateExitBtn();
        //BackBtn.gameObject.SetActive(false);
        //   UIController.instance.confirmationPopUpHandler.HideMe();
        float count = 2;
        DOTween.To(() => count, x => count = x, 1f, 1f).OnUpdate(() =>
        {

            PlayerScroll[0].material.SetVector("_Offset", (Vector2)PlayerScroll[0].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
            playerBG[1].transform.GetChild(0).GetComponent<Image>().sprite = null;// GameController.Instance.GetAvatarSprite(1);// APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameManager.localInstance.myPlayerID).playerData.profilePicURL));
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            playerBG[1].GetComponent<RectTransform>().DOAnchorPosY(0, 1).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                OpponentName.gameObject.SetActive(true);
                ConnectingToServer.gameObject.SetActive(false);
               // CountdownTxt.SetActive(true);
                //    UIController.instance.confirmationPopUpHandler.HideMe();
                OpponentName.text = null;//GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameManager.localInstance.myPlayerID).playerData.playerName;
                MyName.text = "";//GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID == GameManager.localInstance.myPlayerID).playerData.playerName;
                playerBG[1].transform.GetChild(0).GetComponent<Image>().sprite = null; //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameManager.localInstance.myPlayerID).playerData.profilePicURL));
                playerBG[0].GetComponent<Image>().sprite = null; //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID == GameManager.localInstance.myPlayerID).playerData.profilePicURL));
            });
            DOTween.To(() => count, x => count = x, 0f, 1f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                PlayerScroll[0].material.SetVector("_Offset", (Vector2)PlayerScroll[0].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
            });
        });
    }

    public void StopScrolling0()
    {
        if (!CallOne)
        {
            Debug.Log("Callled Scroll 1");
            StopCoroutine(ScrollRoutine);
            //UIController.instance.settingsPanel.DeActivateExitBtn();
            //BackBtn.gameObject.SetActive(false);
            //      UIController.instance.confirmationPopUpHandler.HideMe();
            float count = 2;

            DOTween.To(() => count, x => count = x, 1f, 1f).OnUpdate(() =>
            {
                PlayerScroll[0].material.SetVector("_Offset", (Vector2)PlayerScroll[0].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
                playerBG[1].transform.GetChild(0).GetComponent<Image>().sprite = null;//GameController.Instance.GetAvatarSprite(1); //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.index == 1).playerData.profilePicURL));
            }).SetEase(Ease.Linear).OnComplete(() =>
            {
                playerBG[1].GetComponent<RectTransform>().DOAnchorPosY(0, 1f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    ConnectingToServer.gameObject.SetActive(false);
                    OpponentName.gameObject.SetActive(true);

                    //         UIController.instance.confirmationPopUpHandler.HideMe();
                    OpponentName.text = null;//GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameManager.localInstance.myPlayerID).playerData.playerName;
                    MyName.text = "";//GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID == GameManager.localInstance.myPlayerID).playerData.playerName;
                    playerBG[1].transform.GetChild(0).GetComponent<Image>().sprite = null;//APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.index == 1).playerData.profilePicURL));
                    playerBG[0].GetComponent<Image>().sprite = null; //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID == GameManager.localInstance.myPlayerID).playerData.profilePicURL));
                });
                DOTween.To(() => count, x => count = x, 0f, 1f).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    PlayerScroll[0].material.SetVector("_Offset", (Vector2)PlayerScroll[0].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
                });
            });

            CallOne = true;
        }

    }


    //public void StopScrolling1()
    //{
    //    if (!CallTwo)
    //    {
    //        Debug.Log("Callled Scroll 2");
    //        StopCoroutine(ScrollRoutine1);
    //        float count = 2;
    //        playerBG[2].transform.GetChild(0).GetComponent<Image>().sprite = GameController.Instance.GetAvatarSprite(1); //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex(GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameManager.localInstance.myPlayerID).index);
    //        DOTween.To(() => count, x => count = x, 1f, 1f).OnUpdate(() =>
    //        {
    //            PlayerScroll[1].material.SetVector("_Offset", (Vector2)PlayerScroll[1].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
    //            playerBG[2].transform.GetChild(0).GetComponent<Image>().sprite = GameController.Instance.GetAvatarSprite(1); //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.index == 2).playerData.profilePicURL));
    //        }).SetEase(Ease.Linear).OnComplete(() =>
    //        {
    //            playerBG[2].GetComponent<RectTransform>().DOAnchorPosY(0, 1f).SetEase(Ease.OutExpo).OnComplete(() =>
    //            {
    //                OpponentName1.gameObject.SetActive(true);
    //                UIController.instance.settingsPanel.DeActivateExitBtn();
    //                //BackBtn.gameObject.SetActive(false);
    //                OpponentName1.text = GameManager.localInstance.gameState.players[2].playerData.playerName;
    //                playerBG[2].transform.GetChild(0).GetComponent<Image>().sprite = GameController.Instance.GetAvatarSprite(1); //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.index == 2).playerData.profilePicURL));
    //            });
    //            DOTween.To(() => count, x => count = x, 0f, 1f).SetEase(Ease.Linear).OnUpdate(() =>
    //            {
    //                PlayerScroll[1].material.SetVector("_Offset", (Vector2)PlayerScroll[1].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
    //            });
    //        });
    //        CallTwo = true;
    //    }

    //}
    //public void StopScrolling2()
    //{
    //    if (!CallThree)
    //    {
    //        Debug.Log("Callled Scroll 3");
    //        StopCoroutine(ScrollRoutine2);
    //        float count = 2;
    //        playerBG[3].transform.GetChild(0).GetComponent<Image>().sprite = GameController.Instance.GetAvatarSprite(1); //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex(GameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameManager.localInstance.myPlayerID).index);
    //        DOTween.To(() => count, x => count = x, 1f, 1f).OnUpdate(() =>
    //        {
    //            PlayerScroll[2].material.SetVector("_Offset", (Vector2)PlayerScroll[2].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
    //            playerBG[3].transform.GetChild(0).GetComponent<Image>().sprite = GameController.Instance.GetAvatarSprite(1); //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.index == 3).playerData.profilePicURL));
    //        }).SetEase(Ease.Linear).OnComplete(() =>
    //        {
    //            playerBG[3].GetComponent<RectTransform>().DOAnchorPosY(0, 1).SetEase(Ease.OutExpo).OnComplete(() =>
    //            {
    //                OpponentName2.gameObject.SetActive(true);
    //                CountdownTxt.SetActive(true);
    //                //BackBtn.gameObject.SetActive(false);
    //                UIController.instance.settingsPanel.DeActivateExitBtn();
    //                OpponentName2.text = GameManager.localInstance.gameState.players[3].playerData.playerName;
    //                playerBG[3].transform.GetChild(0).GetComponent<Image>().sprite = GameController.Instance.GetAvatarSprite(1); //APIController.instance.CurrentPlayerData.GetAvatarSpriteByIndex((int)double.Parse(GameManager.localInstance.gameState.players.Find(x => x.index == 3).playerData.profilePicURL));
    //            });
    //            DOTween.To(() => count, x => count = x, 0f, 1f).SetEase(Ease.Linear).OnUpdate(() =>
    //            {
    //                PlayerScroll[2].material.SetVector("_Offset", (Vector2)PlayerScroll[2].material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
    //            });
    //        });
    //        CallThree = true;
    //    }

    //}
}

using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using static WebApiManager;

[Serializable]
public class BetRequest
{
    public string BetId;
    public string PlayerId;
    public string MatchToken;
    public int betId;
}

//[Serializable]
//public class APIResponse
//{
//    public string url;
//    public string code;
//    public string message;
//}

[Serializable]
public class APIRequestList
{
    public string url;
    public ReqCallback callback;
}

public class APIController : MonoBehaviour
{

    #region REST_API_VARIABLES
    public static APIController instance;
    [Header("==============================================")]

    #endregion
    public Action OnUserDetailsUpdate;
    public Action OnUserBalanceUpdate;
    public Action OnUserDeposit;
    public Action<string> OnInternetStatusChange;
    public Action<bool> OnSwitchingTab;
    public bool isWin = false;
    public bool IsBotInGame = true;
    public GameWinningStatus winningStatus;
    public UserGameData userDetails;
    public List<BetDetails> betDetails = new List<BetDetails>();
    public List<BetRequest> betRequest = new List<BetRequest>();
    public bool isPlayByDummyData;
    public double maxWinAmount;
    public bool isClickDeopsit = false;
    public string testJson;
    public string defaultGameName;
    public int defaultBootAmount = 25;
    public List<APIRequestList> apiRequestList;
    //public List<ApiResponse> apiResponse;
    //    public List<ApiRequest> apis = new List<ApiRequest>();

    public List<string> PlayerIDs = new() { "f0255647-61d5-4807-b700-352ce052c791", "f24c7429-3946-4567-87be-e258571f704f", "72b183bc-fcbb-4ed4-9061-775d3e908731", "72d43cf3-7bbb-4b88-b443-0669b1390d5e", "cb282b1f-d52f-4958-a28c-9c4a31610877", "bd004aed-2912-46ed-b30b-216e861376e4", "4b2921b6-275b-4454-b61a-d120b57933f3" };

#if UNITY_WEBGL
    #region WebGl Events

    [DllImport("__Internal")]
    public static extern void GetLoginData();
    [DllImport("__Internal")]
    public static extern void DisconnectGame(string message);
    [DllImport("__Internal")]
    public static extern void GetUpdatedBalance();
    [DllImport("__Internal")]
    public static extern void FullScreen();
    [DllImport("__Internal")]
    private static extern void ShowDeposit();

    [DllImport("__Internal")]
    public static extern void CloseWindow();

    [DllImport("__Internal")]
    public static extern void CheckOnlineStatus();

    private Action<BotDetails> GetABotAction;
    [DllImport("__Internal")]
    private static extern void GetABot();

    [DllImport("__Internal")]
    private static extern void InitPlayerBet(string type, int index, string game_user_Id, string game_Id, string metaData, string isAbleToCancel, double bet_amount, int isBot);

    [DllImport("__Internal")]
    private static extern void AddPlayerBet(string type, int index, string id, string metaData, string game_user_Id, string game_Id, double bet_amount, int isBot);
    [DllImport("__Internal")]
    private static extern void CancelPlayerBet(string type, string id, string metaData, string game_user_Id, string game_Id, double amount, int isBot);
    [DllImport("__Internal")]
    private static extern void FinilizePlayerBet(string type, string id, string metadata, string game_user_Id, string game_Id, int isBot);
    [DllImport("__Internal")]
    private static extern void WinningsPlayerBet(string type, string id, string metadata, string game_user_Id, string game_Id, double win_amount, double spend_amount, int isBot);
    [DllImport("__Internal")]
    private static extern void MultiplayerWinningsPlayerBet(string type, string id, string metadata, string game_user_Id, string game_Id, double win_amount, double spend_amount, double pot_amount, int isBot, int isWinner);
    [DllImport("__Internal")]
    private static extern void GetRandomPrediction(string type, int rowCount, int columnCount, int predectedCount);
    private Action<string, bool> GetPredictionAction;

    [DllImport("__Internal")]
    public static extern void ExecuteExternalUrl(string url, int timout);

    #endregion

    #region WebGl Response
    [ContextMenu("check json")]
    public void CheckJson()
    {
        InitPlayerBetResponse(testJson);
    }
    public void GetABotResponse(string data)
    {
        Debug.Log("get bot response :::::::----::: " + data);

        BotDetails bot = new BotDetails();
        bot = JsonUtility.FromJson<BotDetails>(data);
        GetABotAction?.Invoke(bot);
        GetABotAction = null;
        Debug.Log("get bot response :::::::----::: after response " + data);
    }
    public void UpdateBalanceResponse(string data)
    {
        Debug.Log("Balance Updated response  :::::::----::: " + data);
        userDetails.balance = double.Parse(data);
        OnUserBalanceUpdate?.Invoke();
        if (isClickDeopsit)
        {
            OnUserDeposit?.Invoke();
        }
    }
    [HideInInspector] public bool isOnline = true;
    bool isInFocus = true;
    public bool isNeedToPauseWhileSwitchingTab = false;
    public void GetNetworkStatus(string data)
    {
        Time.timeScale = 1;
        isOnline = data == "true" ? true : false;
        Debug.Log($"Calleeedddd check internet {data}   -   {isOnline}   -   {isInFocus}");
        if (isNeedToPauseWhileSwitchingTab)
        {
            if (isInFocus && isOnline)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;

            }
        }
        OnInternetStatusChange?.Invoke(data);

    }

    public void OnSwitchingTabs(string data)
    {
        Time.timeScale = 1;
        isInFocus = data == "true" ? true : false;
        Debug.Log($"Calleeedddd switching tab {data}   -   {isOnline}   -   {isInFocus}");
        if (isNeedToPauseWhileSwitchingTab)
        {
            if (isInFocus && isOnline)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;

            }
        }
        OnSwitchingTab?.Invoke(data.ToLower() == "true");
    }

    public void InitPlayerBetResponse(string data)
    {
        Debug.Log("init bet response :::::::----::: " + data);
        InitBetDetails response = JsonUtility.FromJson<InitBetDetails>(data);
        BetDetails bet = betDetails.Find(x => x.index == response.index);
        if (response.status)
        {
            winningStatus = response.message;
            Debug.Log("init bet response :::::::----::: " + response.message);
            Debug.Log("init bet response :::::::----::: " + winningStatus.Id);
            bet.betID = winningStatus.Id;
            bet.Status = BetProcess.Success;
            bet.betIdAction?.Invoke(winningStatus.Id);
            bet.action?.Invoke(true);
        }
        else
        {
            bet.action?.Invoke(false);
            betDetails.RemoveAll(x => x.index == response.index);
        }
        bet.action = null;
    }

    public void CancelPlayerBetResponse(string data)
    {
        Debug.Log("cancel bet response :::::::----::: " + data);
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);
            }
            bet.action = null;
        }
    }

    public void CheckInternet()
    {
#if !UNITY_EDITOR
        CheckOnlineStatus();
#endif
    }

    public void AddPlayerBetResponse(string data)
    {
        Debug.Log("add bet response :::::::----::: " + data);
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);
            }
            bet.action = null;
        }
    }

    public void FinilizePlayerBetResponse(string data)
    {
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);
            }
            bet.action = null;
        }
    }

    public void WinningsPlayerBetResponse(string data)
    {
        Debug.Log("winning bet response :::::::----::: " + data);
        BetResponse response = JsonUtility.FromJson<BetResponse>(data);
        if (betDetails.Exists(x => x.index == response.index))
        {
            BetDetails bet = betDetails.Find(x => x.index == response.index);
            if (response.status)
            {
                bet.Status = response.status ? BetProcess.Success : BetProcess.Failed;
                bet.action?.Invoke(true);
            }
            else
            {
                bet.action?.Invoke(false);
            }
            bet.action = null;
        }
    }
    #endregion


    public void OnClickDepositBtn()
    {
        isClickDeopsit = true;
        ShowDeposit();
    }
#endif

    private async void ClearBetResponse(string betID)
    {
        await UniTask.Delay(5000);
        try
        {
            if (betRequest != null && betRequest.Count > 0 && betRequest.Exists(x => x.BetId.Equals(betID)))
            {
                BetRequest request = betRequest.Find(x => x.BetId.Equals(betID));
                Debug.Log($"BetID is {request.BetId}\n Bet Index is {request.betId}\nPlayer Id is {request.PlayerId}\nMatch Token is {request.MatchToken}");
                betRequest.RemoveAll(x => x.BetId.Equals(betID));
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error: {e}");
        }
    }

    public void SendApiRequest(string url, ReqCallback callback)
    {

        byte[] bytesToEncode = Encoding.UTF8.GetBytes(url);

        string base64EncodedString = Convert.ToBase64String(bytesToEncode);
        apiRequestList.Add(new APIRequestList() { url = base64EncodedString, callback = callback });
        //Debug.Log($"Sending Api Request URl :-" + base64EncodedString);
#if UNITY_WEBGL
        ExecuteExternalUrl(base64EncodedString, 2);
#endif

        CheckAPICallBack(base64EncodedString);

    }
    public void SetUserData(string data)
    {
        Debug.Log("Response from webgl ::::: " + data);
        if (data.Length < 30)
        {
            userDetails = new UserGameData();
            userDetails.balance = 5000;
            userDetails.currency_type = "USD";
            userDetails.Id = PlayerIDs[Random.Range(0, PlayerIDs.Count)];
            userDetails.token = UnityEngine.Random.Range(5000, 500000) + SystemInfo.deviceUniqueIdentifier.ToGuid().ToString();
            //userDetails.name = SystemInfo.deviceName + SystemInfo.deviceModel;
            userDetails.name = "User_" + UnityEngine.Random.Range(100, 999);
            isPlayByDummyData = true;
            userDetails.hasBot = true;
            userDetails.game_Id = "demo_" + defaultGameName;
            userDetails.isBlockApiConnection = true;
            //userDetails.commission = 0.2f;
        }
        else
        {
            userDetails = JsonUtility.FromJson<UserGameData>(data);
            isPlayByDummyData = userDetails.isBlockApiConnection;
            isWin = userDetails.isWin;
            maxWinAmount = userDetails.maxWin;
        }
        IsBotInGame = userDetails.hasBot;
        if (userDetails.bootAmount == 0)
            userDetails.bootAmount = defaultBootAmount;
        if (string.IsNullOrWhiteSpace(userDetails.gameId))
            userDetails.gameId = "ecd5c5ce-e0a1-4732-82a0-099ec7d180be";
        Debug.Log(JsonUtility.ToJson(userDetails));
        OnUserDetailsUpdate?.Invoke();
        OnUserBalanceUpdate?.Invoke();
    }

    public void GetLoginDataResponseFromWebGL(string data)
    {
    }
    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetLoginData();
        //CheckInternetStatus();
#elif UNITY_EDITOR
        SetUserData("");
        //CheckInternetStatus();
#endif
    }
    private void Awake()
    {
        instance = this;
    }
    public PlayerData GeneratePlayerDataForBot(string botAccountData)
    {
        BotData account = JsonUtility.FromJson<BotData>(botAccountData);
        PlayerWalletData walletData = JsonUtility.FromJson<PlayerWalletData>(account.wallet);
        PlayerData player = new();
        player.playerID = account.user.id;
        player.playerName = account.user.display_name;
        player.isBot = true;
        ProfilePicture profile = JsonUtility.FromJson<ProfilePicture>(account.user.avatar_url);
        player.profilePicURL = profile.ProfileUrl;
        player.avatarIndex = (int)profile.ProfileType;
        player.gold = (walletData.CashDepositVal / 100) + (walletData.CashDepositVal / 1000);
        player.silver = (walletData.SilverVal / 100);
        player.money = player.gold;
        player.totalWinnings = walletData.NetWinning;
        return player;
    }
    public void RandomPrediction_Response(string data)
    {
        Debug.Log("get Prediction response :::::::----::: " + data);
#if UNITY_WEBGL
        GetPredictionAction?.Invoke(data, true);
        GetPredictionAction = null;
#endif
    }
    public void GetRandomPredictionIndex(int rowCount, int columnCount, int predectedCount, Action<string, bool> OnScucces = null)
    {
#if UNITY_WEBGL
        GetPredictionAction = OnScucces;
        GetRandomPrediction("RandomPrediction", rowCount, columnCount, predectedCount);
#endif
    }


    #region API
    int id = 0;

    public void GetBot(Action<BotDetails> action)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetABotAction = action;
        GetABot();
#endif
    }

    public int InitlizeBet(double amount, TransactionMetaData metadata, bool isAbleToCancel = false, Action<bool> action = null, string playerId = "", bool isBot = false, Action<string> betIdAction = null)
    {
        if (isPlayByDummyData)
        {
            Debug.Log("" + amount);
            if (string.IsNullOrWhiteSpace(playerId) || playerId == userDetails.Id)
            {
                Debug.Log("Dummy Data" + amount);
                userDetails.balance -= amount;
                OnUserBalanceUpdate.Invoke();
            }
            else
            {
                Debug.Log(playerId + " __ " + userDetails.Id + "__ Dummy Data ---1" + amount);
            }
            action?.Invoke(true);
            return 0;
        }
        Debug.Log("Dummy Data ---2" + amount);

        id += 1;
        if (id > 1000)
            id = 1;
        //var data = new
        //{
        //    type = "InitlizeBet",
        //    Index = id,
        //    Game_user_Id = userDetails.Id,
        //    Game_Id = userDetails.game_Id,
        //    MetaData = metadata,
        //    IsAbleToCancel = isAbleToCancel,
        //    Bet_amount = amount
        //};
        BetDetails bet = new BetDetails();
        bet.index = id;
        bet.betID = id.ToString();
        bet.Status = BetProcess.Processing;
        bet.IsAbleToCancel = isAbleToCancel ? "true" : "false";
        betDetails.Add(bet);
#if UNITY_WEBGL
        Debug.Log("Init Bet Data" + amount);
        bet.action = action;
        bet.betIdAction = betIdAction;
        InitPlayerBet("InitlizeBet", id, userDetails.Id, playerId == "" ? userDetails.Id : playerId, JsonUtility.ToJson(metadata), bet.IsAbleToCancel, amount, isBot ? 1 : 0);
#endif
        return id;
    }

    public void AddBet(int index, string BetId, TransactionMetaData metadata, double amount, Action<bool> action = null, string playerId = "", bool isBot = false)
    {
        if (isPlayByDummyData)
        {
            if (playerId == "" || playerId == userDetails.Id)
            {
                userDetails.balance -= amount;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return;
        }

        foreach (var item in betDetails)
        {
            Debug.Log(item.betID);
        }

        if (betDetails.Exists(x => x.betID == BetId))
        {
            BetDetails bet = betDetails.Find(x => x.betID == BetId);

#if UNITY_WEBGL
            Debug.Log("Add Bet Data");
            bet.action = action;
            AddPlayerBet("AddBet", index, BetId, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, amount, isBot ? 1 : 0);
#endif
        }
        else
        {
#if UNITY_WEBGL
            BetDetails bet = new BetDetails();
            id += 1;
            bet.index = id;
            bet.betID = BetId;
            bet.Status = BetProcess.Processing;
            betDetails.Add(bet);
            Debug.Log("Winning Bet Data");
            AddPlayerBet("AddBet", index, BetId, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, amount, isBot ? 1 : 0);
#endif

        }
    }

    public void CancelBet(int index, string metadata, double amount, Action<bool> action = null, string playerId = "", bool isBot = false)
    {
        if (isPlayByDummyData)
        {
            if (playerId == "" || playerId == userDetails.Id)
            {
                userDetails.balance += amount;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return;
        }

        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            bet.action = action;

            if (bet.IsAbleToCancel == "false")
            {
                bet.Status = BetProcess.Failed;
                return;
            }
            bet.Status = BetProcess.Processing;
#if UNITY_WEBGL
            Debug.Log("Cancel Bet Data");
            CancelPlayerBet("CancelBet", bet.betID, metadata, playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, amount, isBot ? 1 : 0);
#endif
        }
    }
    public void FinilizeBet(int index, TransactionMetaData metadata, Action<bool> action = null, string playerId = "", bool isBot = false)
    {
        if (isPlayByDummyData)
        {
            action?.Invoke(true);
            return;
        }

        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            bet.action = action;
            if (bet.IsAbleToCancel == "false")
            {
                bet.Status = BetProcess.Failed;
                return;
            }
            bet.Status = BetProcess.Processing;
            //var data = new
            //{
            //    type = "FinilizeBet",
            //    Id = bet.betID,
            //    MetaData = metadata,
            //    Game_user_Id = userDetails.Id,
            //    Game_Id = userDetails.game_Id,
            //};
#if UNITY_WEBGL
            Debug.Log("Finalize Bet Data");
            FinilizePlayerBet("FinilizeBet", bet.betID, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, isBot ? 1 : 0);
#endif
        }

    }

    public void CreateMatch(string lobbyName, Action<CreateMatchResponse> action, string gamename, string operatorname, string playerId, bool isBlockAPI)
    {
        CreateMatchResponse matchResponse = new CreateMatchResponse();
        if (isBlockAPI)
        {
            matchResponse.status = true;
            matchResponse.MatchToken = DateTime.UtcNow.ToString().ToGuid().ToString();
            action.Invoke(matchResponse);
            return;
        }
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "Created_By", value = playerId == "" ? userDetails.Id : playerId });
        param.Add(new KeyValuePojo { keyId = "Game_Name", value = gamename == "" ? userDetails.game_Id.Split()[1] : gamename });
        param.Add(new KeyValuePojo { keyId = "Operator", value = operatorname == "" ? userDetails.game_Id.Split()[0] : operatorname });
        param.Add(new KeyValuePojo { keyId = "requestType", value = "CreateMatch" });
        param.Add(new KeyValuePojo { keyId = "Lobby_Name", value = lobbyName });
        param.Add(new KeyValuePojo { keyId = "GameName", value = gamename == "" ? userDetails.game_Id.Split()[1] : gamename });


        WebApiManager.Instance.GetNetWorkCall(NetworkCallType.GET_METHOD, LootrixMatchAPIPath, param, (success, error, body) =>
        {
            if (success)
            {
                JObject jsonObject = JObject.Parse(body);
                if ((int)(jsonObject["code"]) == 200)
                {
                    JObject jsonObject1 = JObject.Parse(jsonObject["data"].ToString());
                    matchResponse = JsonUtility.FromJson<CreateMatchResponse>(jsonObject["data"].ToString());
                    matchResponse.status = true;
                    action.Invoke(matchResponse);

                }
                else
                {
                    matchResponse.status = false;
                    action.Invoke(matchResponse);

                }

            }
            else
            {
                matchResponse.status = false;
                action.Invoke(matchResponse);
            }
        });
    }
    [ContextMenu("RandomPrediction")]
    public void GetRandomPrediction()
    {
        GetRandomPredictionIndexApi(9, 5, 1, (data, status) => { Debug.Log(data); }, "tower");
    }



    public void ApiCallBackDebugger(string data)
    {
        //Debug.Log("API Call Back Debug :- " + data);

        //byte[] bytesToEncode = Encoding.UTF8.GetBytes(url);
        byte[] bytesToEncode = Convert.FromBase64String(data);

        string base64EncodedString = Encoding.UTF8.GetString(bytesToEncode);

        Debug.Log(base64EncodedString + "inpuT");

        JObject OBJ = JObject.Parse(base64EncodedString);
        string url = OBJ["url"].ToString();
        int code = int.Parse(OBJ["status"].ToString());
        string body = OBJ["body"].ToString();
        string error = OBJ["error"].ToString();
        //Debug.Log("===========================");
        //Debug.Log(url);
        //Debug.Log(body);
        //Debug.Log(code);
        //Debug.Log(error);
        //Debug.Log("===========================");
        APICallBack(url, code, body, error);
    }

    public void APICallBack(string url, int code, string body, string error)
    {
        // Debug.Log($"API_ Response :-  URL{url} -- Code {code} -- Body {body} -- Error {error}");

        foreach (var item in apiRequestList)
        {
            if (item.url == url)
            {
                if (code == 200)
                {
                    item.callback(true, error, body);
#if UNITY_WEBGL
                    GetUpdatedBalance();
#endif
                }
                else
                {
                    item.callback(false, error, body);
                }
            }
        }

        apiRequestList.RemoveAll(x => x.url == url);
    }

    public async void CheckAPICallBack(string url)
    {
        // Debug.Log($"API_ Response :-  URL{url} -- Code {code} -- Body {body} -- Error {error}");
        await UniTask.Delay(3000);

        foreach (var item in apiRequestList)
        {
            if (item.url == url)
            {
                item.callback(false, "timeout", "timeout");
            }
        }

        apiRequestList.RemoveAll(x => x.url == url);
    }

    public void GetRandomPredictionIndexApi(int rowCount, int columnCount, int predectedCount, Action<string, bool> OnScucces = null, string gamename = "")
    {
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "rowCount", value = rowCount.ToString() });
        param.Add(new KeyValuePojo { keyId = "columnCount", value = columnCount.ToString() });
        param.Add(new KeyValuePojo { keyId = "predictionCount", value = predectedCount.ToString() });
        param.Add(new KeyValuePojo { keyId = "requestType", value = "RandomPrediction" });
        param.Add(new KeyValuePojo { keyId = "gameName", value = gamename == "" ? userDetails.game_Id.Split()[1] : gamename });

        WebApiManager.Instance.GetNetWorkCall(NetworkCallType.GET_METHOD, LootrixAPIPath, param, (success, error, body) =>
        {
            if (success)
            {
                JObject jsonObject = JObject.Parse(body);

                ApiResponse response = JsonUtility.FromJson<ApiResponse>(body);
                if (response.code == 200)
                {
                    OnScucces?.Invoke(response.message, true);

                }
                else
                {
                    OnScucces?.Invoke(body, false);

                }

            }
            else
            {
                OnScucces?.Invoke("error", false);
            }
        });
    }

    public void CheckInternetStatus(int tryCount = 0)
    {
        return;
#if !UNITY_SERVER
        WebApiManager.Instance.GetNetWorkCall(NetworkCallType.GET_METHOD, "https://google.com/", new List<KeyValuePojo>(), async (success, error, body) =>
        {
            if (success)
            {
                OnInternetStatusChange?.Invoke(success.ToString());
                await UniTask.Delay(2500);
                CheckInternetStatus();
            }
            else
            {
                tryCount += 1;
                if (tryCount > 4)
                {
                    OnInternetStatusChange?.Invoke(success.ToString());
                    await UniTask.Delay(500);
                    CheckInternetStatus(tryCount);
                }
                else
                {
                    CheckInternetStatus(tryCount);
                }

            }
        }, 2);
#endif
    }

    public void AddMatchLog(string matchToken, string action, string metadata, string PlayerId = "")
    {
        if (userDetails.isBlockApiConnection)
        {
            return;
        }
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "Token", value = matchToken });
        param.Add(new KeyValuePojo { keyId = "Action", value = action });
        param.Add(new KeyValuePojo { keyId = "Metadata", value = metadata });
        param.Add(new KeyValuePojo { keyId = "PlayerId", value = PlayerId == "" ? userDetails.Id : PlayerId });
        param.Add(new KeyValuePojo { keyId = "requestType", value = "AddMatchLog" });

        WebApiManager.Instance.GetNetWorkCall(NetworkCallType.GET_METHOD, LootrixMatchAPIPath, param, (success, error, body) =>
        {
            if (success)
            {
                JObject jsonObject = JObject.Parse(body);
                if ((int)(jsonObject["code"]) == 200)
                {

                }
                else
                {

                }

            }
        });
    }

    public void AddUnclaimAmount(string matchToken, double amount)
    {
        if (APIController.instance.userDetails.isBlockApiConnection)
        {
            return;
        }
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "Id", value = matchToken });
        param.Add(new KeyValuePojo { keyId = "unclaim_amount", value = amount.ToString() });
        param.Add(new KeyValuePojo { keyId = "requestType", value = "AddUnclaimAmount" });

        WebApiManager.Instance.GetNetWorkCall(NetworkCallType.GET_METHOD, LootrixMatchAPIPath, param, (success, error, body) =>
        {
            if (success)
            {
                JObject jsonObject = JObject.Parse(body);
                if ((int)(jsonObject["code"]) == 200)
                {

                }
                else
                {
                }
            }
        });
    }

    public void AddPlayers(string matchToken, List<string> players)
    {
        if (APIController.instance.userDetails.isBlockApiConnection)
        {
            return;
        }
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "Id", value = matchToken });
        param.Add(new KeyValuePojo { keyId = "Players", value = JsonConvert.SerializeObject(players) });
        param.Add(new KeyValuePojo { keyId = "requestType", value = "AddPlayers" });

        WebApiManager.Instance.GetNetWorkCall(NetworkCallType.GET_METHOD, LootrixMatchAPIPath, param, (success, error, body) =>
        {
            if (success)
            {
                JObject jsonObject = JObject.Parse(body);
                if ((int)(jsonObject["code"]) == 200)
                {

                }
                else
                {
                }
            }
        });
    }

    public void ExecuteAPI(ApiRequest api, int timeout = 0)
    {
        WebApiManager.Instance.GetNetWorkCall(api.callType, api.url, api.param, (success, error, body) =>
        {
            Debug.Log($"<color=orange>Success is set to {success}, error is set to {error} and body is set to {body}\nURL is : {api.url}</color>");
            if (success)
            {
                Debug.Log($"<color=orange>API sent to success</color>");
                api.action?.Invoke(success, error, body);
            }
            else
            {
                if (timeout >= 3)
                {
                    api.action?.Invoke(success, error, body);
                    Debug.Log($"<color=orange>API run failed with timeout {timeout}</color>");
                }
                else
                {
                    Debug.Log($"<color=orange>API recalled with timeout set to {timeout}</color>");
                    ExecuteAPI(api, ++timeout);
                }
            }
        }, 15);
    }
    string LootrixMatchAPIPath = "https://56ebdif5s4aqltb74xhkvnnrai0sxaey.lambda-url.ap-south-1.on.aws/";
    string RumbleBetsAPIPath = "https://rumblebets.utwebapps.com:7350/v2/rpc/";
    string LootrixAPIPath = "https://xpxpmhpjldqvjulexwx34z7jca0kdxzf.lambda-url.ap-south-1.on.aws/";
    bool isRunApi = false;

    public void GetABotAPI(List<string> botId, Action<BotDetails> action)
    {
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "player", value = JsonConvert.SerializeObject(botId) });
        string url = RumbleBetsAPIPath + "rpc_GetABot?http_key=defaulthttpkey&unwrap=";
        ApiRequest apiRequest = new ApiRequest();
        apiRequest.action = (success, error, body) =>
        {
            if (success)
            {
                NakamaApiResponse nakamaApi = JsonUtility.FromJson<NakamaApiResponse>(body);
                BotDetails bot = new BotDetails();
                bot = JsonUtility.FromJson<BotDetails>(body);
                action?.Invoke(bot);
            }

        };
        apiRequest.url = url;
        apiRequest.param = param;
        apiRequest.callType = NetworkCallType.POST_METHOD_USING_JSONDATA;
        ExecuteAPI(apiRequest);
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="betId"></param>
    /// <param name="win_amount_with_comission"></param>
    /// <param name="spend_amount"></param>
    /// <param name="pot_amount"></param>
    /// <param name="metadata"></param>
    /// <param name="action"></param>
    /// <param name="playerId"></param>
    /// <param name="isBot"></param>
    /// <param name="isWinner"></param>
    /// <param name="gameName"></param> <param name="Operator"></param> game name must be APIController.instance.userDetails.game_Id. Get that from client side and stored that into server side. (or) manualy give that in serverside. ex : APIController.instance.userDetails.game_Id = rumbblebets_aviator
    /// <param name="gameId"></param> Get that from client side and stored that into server side. APIController.instance.userDetails.gameId


    public async void WinningsBetMultiplayerAPI(int betIndex, string betId, double win_amount_with_comission, double spend_amount, double pot_amount, TransactionMetaData metadata, Action<bool> action, string playerId, bool isBot, bool isWinner, string gameName, string operatorName, string gameId, float commission, string matchToken)
    {
        Debug.Log($"BetIndex: {betIndex}, playerId: {playerId}, matchToken: {matchToken}");
        BetRequest request = betRequest.Find(x => x.betId == betIndex && x.PlayerId == playerId && x.MatchToken.Equals(matchToken));
        Debug.Log($"Request data is {JsonUtility.ToJson(request)}");
        while (request.BetId != betId)
        {
            await UniTask.Delay(200);
        }

        Debug.Log($"<color=orange>WinningsBetMultiplayerAPI called with commission set to {commission}</color>");
        if (commission == 0 || !isWinner)
        {
            List<KeyValuePojo> param = new List<KeyValuePojo>();

            param.Add(new KeyValuePojo { keyId = "playerID", value = string.IsNullOrEmpty(playerId) ? userDetails.Id : playerId });
            param.Add(new KeyValuePojo { keyId = "amount", value = win_amount_with_comission.ToString() });
            param.Add(new KeyValuePojo { keyId = "cashType", value = 1.ToString() });
            param.Add(new KeyValuePojo { keyId = "metaData", value = JsonUtility.ToJson(metadata) });
            param.Add(new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" });
            param.Add(new KeyValuePojo { keyId = "matchToken", value = matchToken });

            string url = RumbleBetsAPIPath + "RPC_AddAmounttoUser?http_key=defaulthttpkey&unwrap=";
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.action = (success, error, body) =>
            {
                if (success)
                {
                    NakamaApiResponse nakamaApi = JsonUtility.FromJson<NakamaApiResponse>(body);
                    if (nakamaApi.Code == 200)
                    {
                        List<KeyValuePojo> param1 = new List<KeyValuePojo>
                    {
                        new KeyValuePojo { keyId = "Id", value = betId },
                        new KeyValuePojo { keyId = "Game_user_Id", value = string.IsNullOrEmpty(playerId) ? userDetails.Id : playerId },
                        new KeyValuePojo { keyId = "GameName", value = gameName },
                        new KeyValuePojo { keyId = "Operator", value = operatorName },
                        new KeyValuePojo { keyId = "Game_Id", value = gameId },
                        new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" },
                        new KeyValuePojo { keyId = "Win_amount", value = win_amount_with_comission.ToString() },
                        new KeyValuePojo { keyId = "amountSpend", value = spend_amount.ToString() },
                        new KeyValuePojo { keyId = "potamount", value = pot_amount.ToString() },
                        new KeyValuePojo { keyId = "Comission", value = commission.ToString() },
                        new KeyValuePojo { keyId = "isWin", value = isWinner ? "1" : "0" },
                        new KeyValuePojo { keyId = "requestType", value = "winningBet" }
                    };

                        string url1 = LootrixAPIPath;
                        ApiRequest apiRequest1 = new ApiRequest();
                        apiRequest1.action = (success, error, body) =>
                        {
                            if (success)
                            {

                                ApiResponse response = JsonUtility.FromJson<ApiResponse>(body);
                                action?.Invoke(response != null && response.code == 200);
                                ClearBetResponse(request.BetId);
                            }
                            else
                            {
                                action?.Invoke(false);
                            }
                        };
                        apiRequest1.url = url1;
                        apiRequest1.param = param1;
                        apiRequest1.callType = NetworkCallType.GET_METHOD;
                        ExecuteAPI(apiRequest1);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                else
                {
                    action?.Invoke(false);
                }
            };
            apiRequest.url = url;
            apiRequest.param = param;
            apiRequest.callType = NetworkCallType.POST_METHOD_USING_JSONDATA;
            ExecuteAPI(apiRequest);
        }
        else
        {
            List<KeyValuePojo> param = new List<KeyValuePojo>();

            param.Add(new KeyValuePojo { keyId = "playerID", value = playerId });
            param.Add(new KeyValuePojo { keyId = "amount", value = win_amount_with_comission.ToString() });
            param.Add(new KeyValuePojo { keyId = "amountSpend", value = spend_amount.ToString() });
            param.Add(new KeyValuePojo { keyId = "cashType", value = 1.ToString() });
            param.Add(new KeyValuePojo { keyId = "game_name", value = gameName });
            param.Add(new KeyValuePojo { keyId = "metaData", value = JsonUtility.ToJson(metadata) });
            param.Add(new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" });
            param.Add(new KeyValuePojo { keyId = "matchToken", value = playerId });

            string url = RumbleBetsAPIPath + "RPC_AddWinningAmount?http_key=defaulthttpkey&unwrap=";
            int timeout = 0;
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.action = (success, error, body) =>
            {
                if (success)
                {
                    NakamaApiResponse nakamaApi = JsonUtility.FromJson<NakamaApiResponse>(body);
                    if (nakamaApi.Code == 200)
                    {

                        List<KeyValuePojo> param1 = new List<KeyValuePojo>
                    {
                        new KeyValuePojo { keyId = "Id", value = betId },
                        new KeyValuePojo { keyId = "GameName", value = gameName },
                        new KeyValuePojo { keyId = "Operator", value = operatorName },
                        new KeyValuePojo { keyId = "Game_Id", value = gameId },
                        new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" },
                        new KeyValuePojo { keyId = "Win_amount", value = win_amount_with_comission.ToString() },
                        new KeyValuePojo { keyId = "amountSpend", value = spend_amount.ToString() },
                        new KeyValuePojo { keyId = "potamount", value = pot_amount.ToString() },
                        new KeyValuePojo { keyId = "Comission", value = commission.ToString() },
                        new KeyValuePojo { keyId = "isWin", value = isWinner ? "1" : "0" },
                        new KeyValuePojo { keyId = "requestType", value = "winningBet" }
                    };

                        string url1 = LootrixAPIPath;
                        ApiRequest apiRequest1 = new ApiRequest();
                        apiRequest1.action = (success, error, body) =>
                        {
                            if (success)
                            {
                                ApiResponse response = JsonUtility.FromJson<ApiResponse>(body);
                                action?.Invoke(response != null && response.code == 200);
                                ClearBetResponse(request.BetId);
                            }
                            else
                            {
                                action?.Invoke(false);
                            }
                        };
                        apiRequest1.url = url1;
                        apiRequest1.param = param1;
                        apiRequest1.callType = NetworkCallType.GET_METHOD;
                        ExecuteAPI(apiRequest1);
                    }
                    else
                    {
                        action?.Invoke(false);
                    }
                }
                else
                {
                    action?.Invoke(false);
                }
            };
            apiRequest.url = url;
            apiRequest.param = param;
            apiRequest.callType = NetworkCallType.POST_METHOD_USING_JSONDATA;
            ExecuteAPI(apiRequest);
        }

    }
    public async void CancelBetMultiplayerAPI(int betIndex, string betId, double amount, TransactionMetaData metadata, Action<bool> action, string playerId, bool isBot, bool isWinner, string gameName, string operatorName, string gameId, string matchToken)
    {
        BetRequest request = betRequest.Find(x => x.betId == betIndex && x.PlayerId == playerId && x.MatchToken.Equals(matchToken));
        while (request.BetId != betId)
        {
            await UniTask.Delay(200);
        }
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "playerID", value = string.IsNullOrEmpty(playerId) ? userDetails.Id : playerId });
        param.Add(new KeyValuePojo { keyId = "amount", value = amount.ToString() });
        param.Add(new KeyValuePojo { keyId = "cashType", value = 1.ToString() });
        param.Add(new KeyValuePojo { keyId = "metaData", value = JsonUtility.ToJson(metadata) });
        param.Add(new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" });
        param.Add(new KeyValuePojo { keyId = "matchToken", value = string.IsNullOrEmpty(playerId) ? userDetails.Id : playerId });
        string url = RumbleBetsAPIPath + "RPC_AddAmounttoUser?http_key=defaulthttpkey&unwrap=";
        ApiRequest apiRequest = new ApiRequest();
        apiRequest.action = (success, error, body) =>
        {
            if (success)
            {
                NakamaApiResponse nakamaApi = JsonUtility.FromJson<NakamaApiResponse>(body);
                if (nakamaApi.Code == 200)
                {
                    List<KeyValuePojo> param1 = new List<KeyValuePojo>
                {
                    new KeyValuePojo { keyId = "Id", value = betId },
                    new KeyValuePojo { keyId = "GameName", value = gameName },
                    new KeyValuePojo { keyId = "Operator", value = operatorName },
                    new KeyValuePojo { keyId = "Game_Id", value = gameId },
                    new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" },
                    new KeyValuePojo { keyId = "requestType", value = "cancelBet" }
                };
                    string url1 = LootrixAPIPath;
                    ApiRequest apiRequest1 = new ApiRequest();
                    apiRequest1.action = (success, error, body) =>
                    {
                        if (success)
                        {
                            ApiResponse response = JsonUtility.FromJson<ApiResponse>(body);
                            action?.Invoke(response != null && response.code == 200);
                        }
                        else
                        {
                            action?.Invoke(false);
                        }
                    };
                    apiRequest1.url = url1;
                    apiRequest1.param = param1;
                    apiRequest1.callType = NetworkCallType.GET_METHOD;
                    ExecuteAPI(apiRequest1);

                }
                else
                {
                    action?.Invoke(false);
                }
            }
            else
            {
                action?.Invoke(false);
            }
        };
        apiRequest.url = url;
        apiRequest.param = param;
        apiRequest.callType = NetworkCallType.POST_METHOD_USING_JSONDATA;
        ExecuteAPI(apiRequest);
    }

    public async void AddBetMultiplayerAPI(int index, string BetId, TransactionMetaData metadata, double amount, Action<bool> action, string playerId, bool isBot, string gameName, string operatorName, string gameId, string matchToken)
    {
        BetRequest request = betRequest.Find(x => x.betId == index && x.PlayerId == playerId && x.MatchToken.Equals(matchToken));
        while (request.BetId != BetId)
        {
            await UniTask.Delay(200);
        }
        List<KeyValuePojo> param = new List<KeyValuePojo>();
        param.Add(new KeyValuePojo { keyId = "playerID", value = string.IsNullOrEmpty(playerId) ? userDetails.Id : playerId });
        param.Add(new KeyValuePojo { keyId = "amount", value = amount.ToString() });
        param.Add(new KeyValuePojo { keyId = "cashType", value = 1.ToString() });
        param.Add(new KeyValuePojo { keyId = "metaData", value = JsonUtility.ToJson(metadata) });
        param.Add(new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" });
        param.Add(new KeyValuePojo { keyId = "matchToken", value = string.IsNullOrEmpty(playerId) ? userDetails.Id : playerId });

        string url = RumbleBetsAPIPath + "RPC_SubractAmountFromUser?http_key=defaulthttpkey&unwrap=";
        ApiRequest apiRequest = new ApiRequest();
        apiRequest.action = (success, error, body) =>
        {
            if (success)
            {
                NakamaApiResponse nakamaApi = JsonUtility.FromJson<NakamaApiResponse>(body);
                if (nakamaApi.Code == 200)
                {
                    id += 1;
                    List<KeyValuePojo> param1 = new List<KeyValuePojo>();

                    param1.Add(new KeyValuePojo { keyId = "Game_Id", value = gameId });
                    param1.Add(new KeyValuePojo { keyId = "GameName", value = gameName });
                    param1.Add(new KeyValuePojo { keyId = "Operator", value = operatorName });
                    param1.Add(new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" });
                    param1.Add(new KeyValuePojo { keyId = "Id", value = BetId });
                    param1.Add(new KeyValuePojo { keyId = "Bet_amount", value = amount.ToString() });
                    param1.Add(new KeyValuePojo { keyId = "requestType", value = "addBet" });
                    param1.Add(new KeyValuePojo { keyId = "MetaData", value = JsonUtility.ToJson(metadata) });

                    string url1 = LootrixAPIPath;
                    ApiRequest apiRequest1 = new ApiRequest();
                    apiRequest1.action = (success, error, body) =>
                    {
                        if (success)
                        {
                            ApiResponse response = JsonUtility.FromJson<ApiResponse>(body);
                            action?.Invoke(response != null && response.code == 200);
                        }
                        else
                        {
                            action?.Invoke(false);
                        }
                    };
                    apiRequest1.url = url1;
                    apiRequest1.param = param1;
                    apiRequest1.callType = NetworkCallType.GET_METHOD;
                    ExecuteAPI(apiRequest1);
                }
                else
                {
                    action?.Invoke(false);
                }
            }
            else
            {
                Debug.Log("AddBetCAlled   Failure" + amount + " *********************************** " + isBot + " *********************************************################################################################################### " + playerId);
                action?.Invoke(false);
            }
        };
        apiRequest.url = url;
        apiRequest.param = param;
        apiRequest.callType = NetworkCallType.POST_METHOD_USING_JSONDATA;
        ExecuteAPI(apiRequest);

    }

    public int InitBetMultiplayerAPI(int index, double amount, TransactionMetaData metadata, bool isAbleToCancel, Action<bool> action, string playerId, bool isBot, Action<string> betIdAction, string gameName, string operatorName, string gameID, string matchToken)
    {
        Debug.Log($"<color=orange>Initializing bet for player {playerId}, index is {index}</color>");

        List<KeyValuePojo> param = new List<KeyValuePojo>();
        BetRequest bet = new BetRequest();
        bet.MatchToken = matchToken;
        bet.PlayerId = playerId;
        bet.betId = index;
        betRequest.Add(bet);
        param.Add(new KeyValuePojo { keyId = "playerID", value = string.IsNullOrEmpty(playerId) ? userDetails.Id : playerId });
        param.Add(new KeyValuePojo { keyId = "amount", value = amount.ToString() });
        param.Add(new KeyValuePojo { keyId = "cashType", value = 1.ToString() });
        param.Add(new KeyValuePojo { keyId = "metaData", value = JsonUtility.ToJson(metadata) });
        param.Add(new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" });
        param.Add(new KeyValuePojo { keyId = "matchToken", value = matchToken });

        string url = RumbleBetsAPIPath + "rpc_subractamountfromuser?http_key=defaulthttpkey&unwrap=";
        ApiRequest apiRequest = new ApiRequest();
        apiRequest.action = (success, error, body) =>
        {
            if (success)
            {
                NakamaApiResponse nakamaApi = JsonUtility.FromJson<NakamaApiResponse>(body);
                if (nakamaApi.Code == 200)
                {
                    List<KeyValuePojo> param1 = new List<KeyValuePojo>
                {
                    new KeyValuePojo { keyId = "Game_Id", value = gameID},
                    new KeyValuePojo { keyId = "GameName", value = gameName },
                    new KeyValuePojo { keyId = "Operator", value = operatorName },
                    new KeyValuePojo { keyId = "Game_user_Id", value = playerId },
                    new KeyValuePojo { keyId = "Status", value = isAbleToCancel.ToString() },
                    new KeyValuePojo { keyId = "IsAbleToCancel", value = isAbleToCancel.ToString() },
                    new KeyValuePojo { keyId = "isBot", value = isBot ? "1" : "0" },
                    new KeyValuePojo { keyId = "Index", value = index.ToString() },
                    new KeyValuePojo { keyId = "Bet_amount", value = amount.ToString() },
                    new KeyValuePojo { keyId = "requestType", value = "initBet" },
                    new KeyValuePojo { keyId = "MatchToken", value = matchToken },
                    new KeyValuePojo { keyId = "MetaData", value = JsonUtility.ToJson(metadata) }
                };
                    GameWinningStatus _winningStatus;
                    string url1 = LootrixAPIPath;
                    ApiRequest apiRequest1 = new ApiRequest();
                    apiRequest1.action = (success, error, body) =>
                    {
                        if (success)
                        {
                            ApiResponse response = JsonUtility.FromJson<ApiResponse>(body);
                            action?.Invoke(response != null && response.code == 200);
                            if (response.code == 200)
                            {
                                Debug.Log($"<color=aqua>Response message is : {response.data}</color>");
                                _winningStatus = JsonUtility.FromJson<GameWinningStatus>(response.data);
                                bet.BetId = _winningStatus.Id;
                                betIdAction.Invoke(_winningStatus.Id);
                            }
                        }
                        else
                        {
                            action?.Invoke(false);
                        }
                    };
                    apiRequest1.url = url1;
                    apiRequest1.param = param1;
                    apiRequest1.callType = NetworkCallType.GET_METHOD;
                    ExecuteAPI(apiRequest1);
                }
                else
                {
                    action?.Invoke(false);
                }
            }
            else
            {
                action?.Invoke(false);
            }
        };
        apiRequest.url = url;
        apiRequest.param = param;
        apiRequest.callType = NetworkCallType.POST_METHOD_USING_JSONDATA;
        ExecuteAPI(apiRequest);
        return index;
    }

    public void WinningsBetMultiplayer(int betIndex, string betId, double win_amount_with_comission, double spend_amount, double pot_amount, TransactionMetaData metadata, Action<bool> action = null, string playerId = "", bool isBot = false, bool isWinner = true)
    {

        if (isPlayByDummyData)
        {
            if (playerId == "" || playerId == userDetails.Id)
            {
                Debug.Log("Winning Bet Data **********");
                userDetails.balance += win_amount_with_comission;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return;
        }

        if (betDetails.Exists(x => x.betID == betId))
        {
            BetDetails bet = betDetails.Find(x => x.betID == betId);
            bet.Status = BetProcess.Processing;
            bet.action = action;
            //var data = new
            //{
            //    type = "WinningsBet",
            //    Id = bet.betID,
            //    MetaData = metadata,
            //    Game_user_Id = userDetails.Id,
            //    Game_Id = userDetails.game_Id,
            //    Win_amount = amount,
            //    Spend_amount = amount,
            //};

#if UNITY_WEBGL
            Debug.Log("Winning Bet Data");
            MultiplayerWinningsPlayerBet("WinningsBet", betId, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, win_amount_with_comission, spend_amount, pot_amount, isBot ? 1 : 0, isWinner ? 1 : 0);
#endif
        }
        else
        {
#if UNITY_WEBGL
            BetDetails bet = new BetDetails();
            id += 1;
            bet.index = id;
            bet.betID = betId;
            bet.Status = BetProcess.Processing;
            betDetails.Add(bet);
            Debug.Log("Winning Bet Data");
            MultiplayerWinningsPlayerBet("WinningsBet", betId, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, win_amount_with_comission, spend_amount, pot_amount, isBot ? 1 : 0, isWinner ? 1 : 0);
#endif

        }
    }

    public void WinningsBet(int index, double amount, double spend_amount, TransactionMetaData metadata, Action<bool> action = null, string playerId = "", bool isBot = false)
    {


        if (isPlayByDummyData)
        {
            if (playerId == "" || playerId == userDetails.Id)
            {
                Debug.Log("Winning Bet Data **********");
                userDetails.balance += amount;
                OnUserBalanceUpdate.Invoke();
            }
            action?.Invoke(true);
            return;
        }

        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            bet.Status = BetProcess.Processing;
            bet.action = action;
            //var data = new
            //{
            //    type = "WinningsBet",
            //    Id = bet.betID,
            //    MetaData = metadata,
            //    Game_user_Id = userDetails.Id,
            //    Game_Id = userDetails.game_Id,
            //    Win_amount = amount,
            //    Spend_amount = amount,
            //};
#if UNITY_WEBGL
            Debug.Log("Winning Bet Data");
            WinningsPlayerBet("WinningsBet", bet.betID, JsonUtility.ToJson(metadata), playerId == "" ? userDetails.Id : playerId, userDetails.game_Id, amount, spend_amount, isBot ? 1 : 0);
#endif
        }
    }

    public BetProcess CheckBetStatus(int index)
    {
        if (isPlayByDummyData)
            return BetProcess.Success;
        if (betDetails.Exists(x => x.index == index))
        {
            BetDetails bet = betDetails.Find(x => x.index == index);
            return bet.Status;
        }
        return BetProcess.Failed;
    }
    #endregion
}

[System.Serializable]
public class ApiRequest
{
    public NetworkCallType callType;
    public string url;
    public List<KeyValuePojo> param;
    public Action<bool, string, string> action;
}

public class InitBetDetails
{
    public bool status;
    public GameWinningStatus message;
    public int index;
}

[System.Serializable]
public class MinMaxOffest
{
    public float min;
    public float max;
}

[System.Serializable]
public class GameWinningStatus
{
    public string Id;
    public double Amount;
    public MinMaxOffest WinCutOff;
    public float WinProbablity;
    public string Game_Id;
    public string Operator;
    public DateTime create_at;
}

[System.Serializable]
public class UserGameData
{
    public string Id;
    public string name;
    public string token;
    public double balance;
    public string currency_type;
    public string game_Id;
    public string gameId;
    public bool isBlockApiConnection;
    public double bootAmount;
    public bool isWin;
    public bool hasBot;
    public float commission;
    public float maxWin;
}

[System.Serializable]
public class TransactionMetaData
{
    public double Amount;
    public string Info;
}
[System.Serializable]
public class BetDetails
{
    public string betID;
    public int index;
    public BetProcess Status;
    public string IsAbleToCancel;
    public Action<bool> action;
    public Action<string> betIdAction;
}

public enum BetProcess
{
    Processing = 0,
    Success = 1,
    Failed = 2,
    None = 3
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ApiResponse
{
    public int code;
    public string message;
    public string data;
    public object output;
}

public class NakamaApiResponse
{
    public int Code;
    public string Message;
}

public class BetResponse
{
    public bool status;
    public string message;
    public int index;
}

[System.Serializable]
public class BotDetails
{
    public string userId;
    public string name;
    public double balance;
}

public static class MatchExtensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }
}
public class CreateMatchResponse
{
    public bool status;
    public string MatchToken;
    public int MatchCount;
    public double WinChance;
}
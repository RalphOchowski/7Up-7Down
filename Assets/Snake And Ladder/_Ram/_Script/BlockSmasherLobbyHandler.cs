using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockSmasherLobbyHandler : MonoBehaviour
{
    public List<Sprite> profiles = new();
    //public TMP_Text MyPlayerName;
    public Image OpponentScrollImg;
    public Image OpponentAvatarImg;
    public Image playerImage;
    public TMP_Text OpponentName;
    public TMP_Text CountdownTxt;
    public Button BackBtn;
    
    private Coroutine ScrollRoutine;
    private float OpponentProfileYPos;

    private void Awake()
    {
        OpponentProfileYPos = OpponentAvatarImg.GetComponent<RectTransform>().anchoredPosition.y;
    }
    private void Start()
    {
        Invoke(nameof(StopScrolling), 5f);
    }

    private void OnEnable()
    {
        if (ScrollRoutine != null) StopCoroutine(ScrollRoutine);
        ScrollRoutine = StartCoroutine(ScrollOpponentImg());


        OpponentAvatarImg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, OpponentProfileYPos);
        OpponentName.gameObject.SetActive(false);
        //CountdownTxt.gameObject.SetActive(false);
        BackBtn.gameObject.SetActive(true);


    }



    public void UpdateWaitingLobby(int count)
    {
        Debug.Log("Here one");
        //OpponentName.text = CarromGameManager.localInstance.gameState.players.Find(x => x.playerData.playerID != GameController.Instance.my_PlayerData.playerID).playerData.playerName;
        CountdownTxt.text = "Game will start in " ;
    }

    private IEnumerator ShowServerConnect()
    {
        Debug.Log("Displaying Show server connect");

        int count = 1;

        while (count <= 3)
        {
            CountdownTxt.text = $"Connencting to server" + (count == 1 ? "<color=#FFDC2C>." : count == 2 ? "<color=#FFDC2C>.." : "<color=#FFDC2C>...");
            count++;
            yield return new WaitForSeconds(1f);
        }

        CountdownTxt.text = " ";
    }

    private IEnumerator ScrollOpponentImg()
    {

        CountdownTxt.gameObject.SetActive(true);
        CountdownTxt.enabled = true;

        StartCoroutine(ShowServerConnect());

        OpponentScrollImg.transform.localPosition = Vector2.zero;
        OpponentScrollImg.material.SetVector("_Offset", Vector2.zero);

        while (true)
        {
            OpponentScrollImg.material.SetVector("_Offset", (Vector2)OpponentScrollImg.material.GetVector("_Offset") + 1 * Time.deltaTime * Vector2.down);
            yield return null;
        }



        StopScrolling();
    }

    public void StopScrolling()
    {
        Debug.Log("Here stop scrolling....");

            OpponentAvatarImg.sprite = profiles[Random.Range(0, profiles.Count)];
        StopCoroutine(ScrollRoutine);
        BackBtn.gameObject.SetActive(false);
        int maxValues = Random.Range(0, profiles.Count);

        float count = 1;
        DOTween.To(() => count, x => count = x, 1, 1).OnUpdate(() =>
        {
            OpponentScrollImg.material.SetVector("_Offset", (Vector2)OpponentScrollImg.material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
        }).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log("Finded opponenet player");
            StartCoroutine(StartGameCount());

            OpponentAvatarImg.GetComponent<RectTransform>().DOAnchorPosY(0, 1).SetEase(Ease.OutExpo).OnComplete(() => { OpponentName.gameObject.SetActive(true); CountdownTxt.gameObject.SetActive(true); });
            DOTween.To(() => count, x => count = x, 0f, 1f).SetEase(Ease.Linear).OnUpdate(() =>
            {
                OpponentScrollImg.material.SetVector("_Offset", (Vector2)OpponentScrollImg.material.GetVector("_Offset") + count * Time.deltaTime * Vector2.down);
            }).OnComplete(() =>
            {

            });
        });

    }

    private IEnumerator StartGameCount()
    {
        yield return new WaitForEndOfFrame();

        //gameObject.GetComponent<GameStarter>().StartGame();
       // GameManagerBlockSmasher.instance.GameTimerStart();


        //startPanel.GetComponent<GameStarter>().StartGame();
    }
}
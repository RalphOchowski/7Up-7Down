using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InsufficientPanelHandler : UIHandler
{
    //public Image BGImg;
    public static InsufficientPanelHandler instance;
    public RectTransform PanelBG;
    //public  SpeedTambolaGameManager ResultPanel;
    //public float BGYPos = 895f;
    public TMP_Text HeaderTxt, AlertMessageTxt, PositiveTxt, NegativeTxt;
    private UnityAction OnPositiveClickAction, OnNegativeClickAction;
    public Image AlertIcon;
    public List<AlertData> IconsData = new();
    public Button _backBtn;
    public Button _postiveBtn, _negtiveBtn;

    private void Awake()
    {
        instance = this;
        _backBtn.onClick.AddListener(() =>HideErrorPopUp());
        _postiveBtn.onClick.AddListener(() => OnPositiveBtnClick());
        _negtiveBtn.onClick.AddListener(() => OnNegativeBtnClick());
    }
    public void HideErrorPopUp()
    {

        HideMe();
    }
        

    public override void ShowMe()
    {
        base.ShowMe();
        //BGImg.DOKill();
        //float blurVal = 0;
        /*BGImg.material.DOKill();
        BGImg.material.DOColor(new Color32(100, 100, 100, 255), 0.25f).From(Color.white);
        DOTween.To(() => blurVal, x => blurVal = x, 3, 0.25f).From(0).OnUpdate(() => BGImg.material.SetFloat("_Size", blurVal));*/
        //BGImg.DOFade(0.5f, 0.25f).From(0);
        //PanelBG.DOKill();
        //PanelBG.DOAnchorPosY(0, 0.25f);

        this.gameObject.SetActive(true);

    }

    public override void HideMe()
    {
        //BGImg.DOKill();
        //float blurVal = BGImg.material.GetFloat("_Size");
        /*BGImg.material.DOKill();
        BGImg.material.DOColor(Color.white, 0.25f);
        DOTween.To(() => blurVal, x => blurVal = x, 0, 0.25f).OnUpdate(() => BGImg.material.SetFloat("_Size", blurVal));*/
        //BGImg.DOFade(0, 0.25f);
        //PanelBG.DOKill();
        //PanelBG.DOAnchorPosY(BGYPos, 0.25f).OnComplete(() =>
        //{
        //    AlertMessageTxt.text = string.Empty;
        //    HeaderTxt.text = string.Empty;
        //    base.HideMe();
        //});

        this.gameObject.SetActive(false);

    }
    public override void OnBack()
    {
        //if (PanelBG.anchoredPosition.y >= BGYPos - 50)
        //{
        //}
        CloseAlert();

    }

    public void ShowMessageWithAction(string message, UnityAction positiveAction, UnityAction negativeAction, string headerTxt = "Alert", string positiveTxt = "Yes", string negativeTxt = "No", AlertType type = AlertType.Alert)
    {
        Debug.Log(message);
        AlertMessageTxt.text = message;
        HeaderTxt.text = headerTxt;
        PositiveTxt.text = positiveTxt;
        NegativeTxt.text = negativeTxt;
        ShowMe();
        OnPositiveClickAction = null;
        OnNegativeClickAction = null;
        OnPositiveClickAction = positiveAction;
        OnNegativeClickAction = negativeAction;
        AlertIcon.sprite = IconsData.Find(x => x.Type.Equals(type)).Icon;
    }

    public void OnPositiveBtnClick()
    {
        OnPositiveClickAction?.Invoke();
 
    }

    public void OnNegativeBtnClick()
    {
        CloseAlert();
    }

    public void CloseAlert()
    {
        OnNegativeClickAction?.Invoke();
        OnNegativeClickAction = null;
        Debug.Log("Calling close alert");
        HideMe();
    }
}

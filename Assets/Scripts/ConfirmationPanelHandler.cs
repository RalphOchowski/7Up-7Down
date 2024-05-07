using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationPanelHandler : UIHandler
{
    public Image BGImg;
    public RectTransform PanelBG;
    public float BGYPos = 895f;
    public TMP_Text HeaderTxt, AlertMessageTxt, PositiveTxt, NegativeTxt;
    private UnityAction OnPositiveClickAction, OnNegativeClickAction;
    public Image AlertIcon;
    public List<AlertData> IconsData = new();

    public override void ShowMe()
    {
        base.ShowMe();
        BGImg.DOKill();
        float blurVal = 0;
        BGImg.material.DOKill();
        BGImg.material.DOColor(new Color32(100, 100, 100, 255), 0.25f).From(Color.white);
        DOTween.To(() => blurVal, x => blurVal = x, 3, 0.25f).From(0).OnUpdate(() => BGImg.material.SetFloat("_Size", blurVal));
        PanelBG.DOKill();
        PanelBG.DOAnchorPosY(BGYPos, 0.25f).From(new Vector2(0, -100));
    }

    public override void HideMe()
    {
        BGImg.DOKill();
        float blurVal = BGImg.material.GetFloat("_Size");
        BGImg.material.DOKill();
        BGImg.material.DOColor(Color.white, 0.25f);
        DOTween.To(() => blurVal, x => blurVal = x, 0, 0.25f).OnUpdate(() => BGImg.material.SetFloat("_Size", blurVal));
        PanelBG.DOKill();
        PanelBG.DOAnchorPosY(-100, 0.25f).OnComplete(() =>
        {
            AlertMessageTxt.text = string.Empty;
            HeaderTxt.text = string.Empty;
            PositiveTxt.text = string.Empty;
            NegativeTxt.text = string.Empty;
            base.HideMe();
        });
    }

    public override void OnBack()
    {
        if (PanelBG.anchoredPosition.y >= BGYPos - 50) CloseAlert();
    }

    public void ShowMessageWithAction(string message, UnityAction positiveAction, UnityAction negativeAction, string headerTxt = "Alert", string positiveTxt = "Yes", string negativeTxt = "No", AlertType type = AlertType.Alert)
    {
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
        OnPositiveClickAction = null;
        CloseAlert();
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
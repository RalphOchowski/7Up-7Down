using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlertPanelHandler : UIHandler
{
    public Image BGImg;
    public RectTransform PanelBG;
    public float BGYPos = 895f;
    public TMP_Text HeaderTxt, AlertMessageTxt, ButtonTxt;
    private UnityAction OnClickAction;
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
        PanelBG.DOAnchorPosY(BGYPos, 0.25f).From(Vector2.zero);
    }

    public override void HideMe()
    {
        BGImg.DOKill();
        float blurVal = BGImg.material.GetFloat("_Size");
        BGImg.material.DOKill();
        BGImg.material.DOColor(Color.white, 0.25f);
        DOTween.To(() => blurVal, x => blurVal = x, 0, 0.25f).OnUpdate(() => BGImg.material.SetFloat("_Size", blurVal));
        PanelBG.DOKill();
        PanelBG.DOAnchorPosY(0, 0.25f).OnComplete(() =>
        {
            AlertMessageTxt.text = string.Empty;
            HeaderTxt.text = string.Empty;
            base.HideMe();
        });
    }

    public override void OnBack()
    {
        if (PanelBG.anchoredPosition.y >= BGYPos - 50)
        {
            CloseAlert();
        }
    }

    public void ShowMessage(string message, string headerTxt = "Alert", AlertType type = AlertType.Alert)
    {
        Debug.Log(message);
        ButtonTxt.text = "Okay";
        HeaderTxt.text = headerTxt;
        AlertMessageTxt.text = message;
        AlertIcon.sprite = IconsData.Find(x => x.Type == type).Icon;
        AlertIcon.SetNativeSize();
        ShowMe();
    }

    public void ShowMessageWithAction(string message, UnityAction onClickAction, string headerTxt = "Alert", string buttonTxt = "Okay", AlertType type = AlertType.Alert)
    {
        ShowMessage(message, headerTxt);
        OnClickAction = null;
        OnClickAction = onClickAction;
        ButtonTxt.text = buttonTxt;
    }

    public void CloseAlert()
    {
        HideMe();
        OnClickAction?.Invoke();
        OnClickAction = null;
    }
}

public enum AlertType
{
    Alert,
    Location,
    Update,
    Logout,
    Exit,
    Low
}

[System.Serializable]
public class AlertData
{
    public AlertType Type;
    public Sprite Icon;
}
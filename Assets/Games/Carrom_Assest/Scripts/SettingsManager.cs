using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : UIHandler
{
    public RectTransform PanelBG;
    public Image BGImg;
    public float BGOffYPos = -1000f;
    public Button exitButton;
    public Toggle soundToggle, vibrationToggle;
    bool soundActive, vibrationactive;
    public TMP_Text versionDisplay;


    private void Awake()
    {
        CheckPlayerprefs();
    }

    void Start()
    {

    }

    public void VersionDisplay(string value)
    {
        //        versionDisplay.text = value;
    }

    public void ToggleSound(bool value)
    {
        /*FindObjectOfType<AudioManager>().Play("ClickSound");*/
        soundActive = value;
        soundToggle.isOn = value;
        PlayerPrefs.SetInt("SoundActive", soundActive ? 1 : 0);
        // AudioListener.pause = !value;
        if (value)
        {
            AudioListener.volume = 1;
          //  FindObjectOfType<AudioManager>().Play("BGMusic");
        }
        else
            AudioListener.volume = 0;
    }

    public void ToggleVibration(bool value)
    {
        /*FindObjectOfType<AudioManager>().Play("ClickSound");*/
        vibrationactive = value;
        vibrationToggle.isOn = value;
        PlayerPrefs.SetInt("mute_vibration", vibrationactive ? 0 : 1);
        //Vibration.MuteVibration = !vibrationactive;
    }

    public override void ShowMe()
    {
        /*FindObjectOfType<AudioManager>().Play("ClickSound");*/
        Debug.Log("Prefs" + PlayerPrefs.GetInt("SoundActive", 1) + "   " + PlayerPrefs.GetInt("mute_vibration", 1));
        //bg.SetActive(true);
        base.ShowMe();
        this.gameObject.SetActive(true);
        float blurVal = 0;
        BGImg.material.DOKill();
        BGImg.material.DOColor(new Color32(100, 100, 100, 255), 0.25f).From(Color.white);
        DOTween.To(() => blurVal, x => blurVal = x, 3, 0.25f).From(0).OnUpdate(() => BGImg.material.SetFloat("_Size", blurVal));
        PanelBG.DOKill();
        CheckPlayerprefs();
    }

    void CheckPlayerprefs()
    {
        //FindObjectOfType<AudioManager>().Play("ClickSound");
        ToggleSound(PlayerPrefs.GetInt("SoundActive", 1) == 1);
        ToggleVibration(PlayerPrefs.GetInt("mute_vibration", 0) == 0);
    }

    public override void HideMe()
    {
        /*FindObjectOfType<AudioManager>().Play("ClickSound");
        UIController.Instance.RemoveFromOpenPages(this);*/
        exitButton.onClick.RemoveAllListeners();
        float blurVal = BGImg.material.GetFloat("_Size");
        BGImg.material.DOKill();
        BGImg.material.DOColor(Color.white, 0.25f);
        DOTween.To(() => blurVal, x => blurVal = x, 0, 0.25f).OnUpdate(() => BGImg.material.SetFloat("_Size", blurVal));
        PanelBG.DOKill();
        PanelBG.DOAnchorPosY(BGOffYPos, 0.25f).SetEase(Ease.InSine).OnComplete(() => base.HideMe());
    }

    public void EnableSettingsPannel(Action OnExitButton = null)
    {
        ShowMe();
        PanelBG.DOKill();
        //PanelBG.anchoredPosition = new Vector2(PanelBG.anchoredPosition.x, PanelBG.anchoredPosition.y);
        PanelBG.DOAnchorPosY(755, 0.25f).SetEase(Ease.OutSine).From(Vector2.zero);
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() => { HideMe(); OnExitButton(); });
    }
}

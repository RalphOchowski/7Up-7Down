using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject SettingsPanel;
    public static GameObject HowToPlay;
    [SerializeField] GameObject HowToPlayObject;
    [SerializeField] Scrollbar scrollbar;

    private void Start()
    {
        HowToPlay = HowToPlayObject;
    }

    public void ShowHowToPlay(bool flag)
    {
        if (scrollbar.value  < 1 && scrollbar.value >= 0) scrollbar.value = 1;
        HowToPlay.SetActive(flag);
        SettingsPanel.GetComponent<SettingsPanelHandler>().HideSettings();
    }

    public void HidePanel(GameObject Panel)
    {
        Panel.SetActive(false);
    }
    public void ExitGame()
    {
        APIController.CloseWindow();
    }
}

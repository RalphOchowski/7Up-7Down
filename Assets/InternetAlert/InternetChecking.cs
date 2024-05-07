using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InternetChecking : MonoBehaviour
{
    public GameObject connectionPanel;
    public GameObject settingsPanel;
    public GameObject BlurredBG;
    public static bool isOnline;
    public bool isPaused;
    public void Start()
    {
        APIController.instance.OnInternetStatusChange += GetNetworkStatus;
    }

    public void GetNetworkStatus(string data)
    {       
        connectionPanel.SetActive(data != "true");
        BlurredBG.SetActive(data != "true");
        Debug.Log($"Blurred Background Activation: {BlurredBG.activeSelf} Connection Panel Activation: {connectionPanel.activeSelf}");
        isOnline = data != "false";
        //if(connectionPanel.activeSelf && data == "false") GameController.HowToPlay.SetActive(false);
        //if (settingsPanel.activeSelf && data == "false") settingsPanel.GetComponent<SettingsPanelHandler>().HideSettings();
        DiceRolla.DiceRoll.OnTabSwitch(data != "false");
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    public bool ExtraPay;
    public int ScoreMultiplier;
    [SerializeField] GameObject ExtraPayGameObject;
     public string name;

    public void TriggerExtraPay(bool flag)
    {
        ExtraPay = flag;
        ExtraPayGameObject.SetActive(flag);
        ScoreMultiplier = (Random.Range(6, 11) - 1) * 10;
        if (flag) ExtraPayGameObject.GetComponentInChildren<TMP_Text>().text = $"{ScoreMultiplier}";
        else ExtraPayGameObject.GetComponentInChildren<TMP_Text>().text = "";
    }
    public void CalculateExtraPay()
    {
        Debug.Log($"Pay Multiplier: {ScoreMultiplier}");
    }
}

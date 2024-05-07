using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public virtual void ShowMe()
    {
        gameObject.SetActive(true);
        //UIController.Instance.AddToOpenPages(this);
    }

    public virtual void HideMe()
    {
        //UIController.Instance.RemoveFromOpenPages(this);
        gameObject.SetActive(false);
    }

    public virtual void OnBack()
    {
        HideMe();
    }
}

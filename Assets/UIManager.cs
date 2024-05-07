using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public void OpenURL(string newURL)
    {
        Application.ExternalEval("window.open('" + newURL + "','_self')");
    }
}

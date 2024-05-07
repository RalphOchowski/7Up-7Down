using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolContainer : MonoBehaviour
{
    public static PoolContainer PoolInstance;
    public List<GameObject> PoolList = new List<GameObject>();
    public GameObject PoolParent;
    public GameObject History;
    [SerializeField] int RenderedHistoryCount = 15;

    void Awake()
    {
        PoolInstance = this;
    }

    public void CreateContainer()
    {
        if (PoolList.Count > 0) PoolList.Clear();
        GameObject buffer;
        for (int i = 0; i < RenderedHistoryCount; i++)
        {
            buffer = Instantiate(History, PoolParent.transform);
            //tmp.transform.localScale = Vector3.one;
            PoolList.Add(buffer);
        }
    }

    public GameObject GetProjectile()
    {
        GameObject buffer;
        buffer = PoolParent.transform.GetChild(0).gameObject;
        buffer.SetActive(true);
        return buffer;
    }

    public void ReturnProjectile(GameObject dud)
    {
        dud.SetActive(false);
        dud.transform.SetParent(PoolParent.transform, false);
        dud.transform.localPosition = Vector2.zero;
        //dud.transform.localRotation = Quaternion.identity;
    }
}

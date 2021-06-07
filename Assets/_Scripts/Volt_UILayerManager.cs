using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Volt_UILayerManager : MonoBehaviour
{
    public static Volt_UILayerManager instance;

    private List<GameObject> uiQueue = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Enqueue(GameObject newUI)
    {
        if (uiQueue.Contains(newUI))
            return;

        int depth = newUI.GetComponent<UIPanel>().depth;

        int index = 0;
        for (index = 0; index < uiQueue.Count; index++)
        {
            if(depth > uiQueue[index].GetComponent<UIPanel>().depth)
            {
                uiQueue.Insert(index, newUI);
                break;
            }
        }
        if (index == uiQueue.Count)
            uiQueue.Add(newUI);
    }

    public GameObject Dequeue()
    {
        if (uiQueue.Count == 0)
            return null;

        GameObject go = uiQueue[0];
        uiQueue.RemoveAt(0);
        return go;
    }

    public void RemoveUI(GameObject item)
    {
        uiQueue.Remove(item);
    }

    public bool Empty()
    {
        if (uiQueue.Count == 0)
            return true;
        return false;
    }
}

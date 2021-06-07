using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAccountPopup : MonoBehaviour
{
    private void OnEnable()
    {
        Volt_UILayerManager.instance.Enqueue(gameObject);
    }

    public void OnPressdownNoButton()
    {
        Volt_UILayerManager.instance.RemoveUI(gameObject);
        gameObject.SetActive(false);
    }
}

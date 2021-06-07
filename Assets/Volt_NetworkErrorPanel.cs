using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NetworkErrorType
{
    InternetNonReachable, InternetInstable, Resolved
}

public class Volt_NetworkErrorPanel : MonoBehaviour
{
    public static Volt_NetworkErrorPanel S;
    public UIButton confirmButton;
    public UILabel msgLabel;

    private void Awake()
    {
        S = this;
        gameObject.SetActive(false);
    }
    public void ShowErrorMsg(NetworkErrorType errorType)
    {
        switch (errorType)
        {
            case NetworkErrorType.InternetNonReachable:
                InternetNonReachable();
                break;
            case NetworkErrorType.InternetInstable:
                InternetInstable();
                break;
            case NetworkErrorType.Resolved:
                ErrorResolved();
                break;
            default:
                break;
        }
    }
    public void InternetNonReachable()
    {
        msgLabel.text = "인터넷 없음";
        confirmButton.onClick.Clear();
        confirmButton.onClick.Add(new EventDelegate(Volt_DontDestroyPanel.S, "OnPressDownDisconnectConfirmBtn"));
    }
    public void ErrorResolved()
    {
        gameObject.SetActive(false);
    }
    public void InternetInstable()
    {
        msgLabel.text = "인터넷 재접속 시도중...";
        confirmButton.onClick.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_DontDestroyPanel : MonoBehaviour
{
    public static Volt_DontDestroyPanel S;
    public Volt_NetworkErrorPanel ErrorPanel;
    private void Awake()
    {
        if (S == null)
        {
            DontDestroyOnLoad(this.gameObject);
            S = this;
        }
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NetworkErrorHandle(NetworkErrorType errorType)
    {
        ErrorPanel.gameObject.SetActive(true);
        ErrorPanel.ShowErrorMsg(errorType);
    }

    public void OnDisconnected()
    {
        Volt_NetworkErrorPanel.S.ShowErrorMsg(NetworkErrorType.InternetNonReachable);
    }
    public EventDelegate OnPressDownDisconnectConfirmBtn()
    {
        Application.Quit();
        return null;
    }
}

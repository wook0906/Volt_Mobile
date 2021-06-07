using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankUI : MonoBehaviour
{
    public GameObject playerNickName;
    public GameObject playCount;
    public GameObject winPercent;
    public GameObject kilCount;
    public GameObject coinCount;
    public GameObject DeathCount;
    public GameObject attackSuccessPercent;

    void Start()
    {
        playerNickName.GetComponent<UILabel>().text = Volt_PlayerData.instance.NickName;
        playCount.GetComponent<UILabel>().text = Volt_PlayerData.instance.PlayCount.ToString();
        kilCount.GetComponent<UILabel>().text = Volt_PlayerData.instance.KillCount.ToString();
        coinCount.GetComponent<UILabel>().text = Volt_PlayerData.instance.CoinCount.ToString();
        DeathCount.GetComponent<UILabel>().text = Volt_PlayerData.instance.DeathCount.ToString();
        if (Volt_PlayerData.instance.VictoryCount == 0 || Volt_PlayerData.instance.PlayCount == 0)
        {
            Debug.Log(Volt_PlayerData.instance.VictoryCount + " VictoryCount");
            Debug.Log(Volt_PlayerData.instance.PlayCount + " PlayCount");
            winPercent.GetComponent<UILabel>().text = 0 + "%";
        }
        else
        {
            winPercent.GetComponent<UILabel>().text = (((float)Volt_PlayerData.instance.VictoryCount / Volt_PlayerData.instance.PlayCount) * 100.0f).ToString("F01")+"%";
        }
        if (Volt_PlayerData.instance.AttackSuccessCount == 0 || Volt_PlayerData.instance.AttackTryCount == 0)
        {
            Debug.Log(Volt_PlayerData.instance.AttackSuccessCount + " AttackSuccessCount");
            Debug.Log(Volt_PlayerData.instance.AttackTryCount + " AttackTryCount");
            attackSuccessPercent.GetComponent<UILabel>().text = 0 + "%";
        }
        else
        {
            attackSuccessPercent.GetComponent<UILabel>().text = (((float)Volt_PlayerData.instance.AttackSuccessCount / Volt_PlayerData.instance.AttackTryCount) * 100.0f).ToString("F01") + "%";
        }
        Debug.Log("RankUI Init");
    }
}

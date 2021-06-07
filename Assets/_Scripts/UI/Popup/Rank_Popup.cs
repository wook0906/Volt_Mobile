using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank_Popup : UI_Popup
{
    enum Sprites
    {
        Block,
        Rank_BG,
        Record_BG,
        PlayerID_BG,
        TotalGame_BG,
        WinRate_BG,
        TotalKill_BG,
        TotalPoint_BG,
        TotalAttackRate_BG,
        TotalDeath_BG,
        Exit_Icon
    }

    enum Labels
    {
        Record_Label,
        PlayerID_Label,
        TotalGame_Label,
        TotalGameCount_Label,
        WinRate_Label,
        WinRateValue_Label,
        TotalKill_Label,
        TotalKillCount_Label,
        TotalPoint_Label,
        TotalPointCount_Label,
        TotalAttackRate_Label,
        TotalAttackRateValue_Label,
        TotalDeath_Label,
        TotalDeathCount_Label,
    }

    enum Buttons
    {
        Exit_Btn
    }

    public override void Init()
    {
        base.Init();
        Bind<UILabel>(typeof(Labels));
        Bind<UISprite>(typeof(Sprites));
        Bind<UIButton>(typeof(Buttons));

        //BindSprite<UISprite>(typeof(Sprites));
        //BindSprite<UIButton>(typeof(Buttons));

        //SetButtonSwap(typeof(Buttons));

        GetButton((int)Buttons.Exit_Btn).onClick.Add(new EventDelegate(() =>
        {
            ClosePopupUI();
        }));

        GetLabel((int)Labels.TotalGameCount_Label).text = Volt_PlayerData.instance.PlayCount.ToString();
        GetLabel((int)Labels.WinRateValue_Label).text = (Volt_PlayerData.instance.WinRate * 100.0f).ToString("F01");
        GetLabel((int)Labels.TotalKillCount_Label).text = Volt_PlayerData.instance.KillCount.ToString();
        GetLabel((int)Labels.TotalPointCount_Label).text = Volt_PlayerData.instance.CoinCount.ToString();
        GetLabel((int)Labels.TotalAttackRateValue_Label).text = (Volt_PlayerData.instance.AttackSuccessRate * 100.0f).ToString("F01");
        GetLabel((int)Labels.TotalDeathCount_Label).text = Volt_PlayerData.instance.DeathCount.ToString();
        GetLabel((int)Labels.PlayerID_Label).text = Volt_PlayerData.instance.NickName;
    }
}

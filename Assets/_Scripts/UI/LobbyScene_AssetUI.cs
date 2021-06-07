using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene_AssetUI : UIBase
{
    enum Labels
    {
        Money_Label,
        Diamond_Label,
        Battery_Label,
        BatteryTime_Label
    }

    enum Buttons
    {
        AddDiamond_Btn,
        AddBattery_Btn,
        ShowBatteryChargeTime_Btn,
        Option_Btn
    }

    enum GameObjects
    {
        BatteryDecrease_Label,
        BatteryTime_BG,
        InputBlock
    }

    private LobbyScene_UI lobbyScene_UI;

    public override void Init()
    {
        Debug.Log("Init Asset UI");
        Bind<UILabel>(typeof(Labels));
        Bind<UIButton>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        lobbyScene_UI = GameObject.Find("LobbyScene_UI").GetComponent<LobbyScene_UI>();//FindObjectOfType<LobbyScene_UI>();

        GetComponent<UIPanel>().depth = 100;

        GetButton((int)Buttons.Option_Btn).onClick.Add(new EventDelegate(() =>
        {
            Managers.UI.ShowPopupUIAsync<SystemOption_Popup>();
        }));
        GetButton((int)Buttons.AddBattery_Btn).onClick.Add(new EventDelegate(() =>
        {
            PlayerPrefs.SetString("Volt_ShopEnterKey", "Battery");
            Managers.Scene.LoadSceneAsync(Define.Scene.Shop);
        }));
        GetButton((int)Buttons.AddDiamond_Btn).onClick.Add(new EventDelegate(() =>
        {
            PlayerPrefs.SetString("Volt_ShopEnterKey", "Diamond");
            Managers.Scene.LoadSceneAsync(Define.Scene.Shop);
        }));

        GetButton((int)Buttons.ShowBatteryChargeTime_Btn).onClick.Add(new EventDelegate(ShowBatteryChargeTime));

        SetDiamondCountLabel(Volt_PlayerData.instance.DiamondCount);
        SetGoldCountLabel(Volt_PlayerData.instance.GoldCount);
        SetBatteryCountLabel(Volt_PlayerData.instance.BatteryCount);

        GetGameObject((int)GameObjects.BatteryTime_BG).SetActive(false);
        GetGameObject((int)GameObjects.BatteryDecrease_Label).SetActive(false);
        GetGameObject((int)GameObjects.InputBlock).SetActive(false);

        LobbyScene scene = Managers.Scene.CurrentScene as LobbyScene;
        scene.OnLoadedAssetUI();
    }

    public void SetOnInputBlock()
    {
        GetGameObject((int)GameObjects.InputBlock).SetActive(true);
    }

    public void SetDiamondCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000) / 100000;
        GetLabel((int)Labels.Diamond_Label).text = count.ToString() + Volt_Utils.GetUnitString(unit);
    }

    public void SetGoldCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000) / 100000;
        GetLabel((int)Labels.Money_Label).text = count.ToString() + Volt_Utils.GetUnitString(unit);
    }

    public void SetBatteryCountLabel(float count)
    {
        GetLabel((int)Labels.Battery_Label).text = $"{count}/{Volt_PlayerData.instance.MaxBetteryCount}";
        if (count < 0)
            lobbyScene_UI.HideStartGameBtnOutline();
        else
            lobbyScene_UI.ShowStartGameBtnOutline();
    }

    private void ShowBatteryChargeTime()
    {
        if (!GetGameObject((int)GameObjects.BatteryTime_BG).activeSelf)
        {
            GetGameObject((int)GameObjects.BatteryTime_BG).SetActive(true);
        }
        else
        {
            GetGameObject((int)GameObjects.BatteryTime_BG).SetActive(false);
        }
    }

    private void Update()
    {
        if (BatteryCharge.instance == null)
            return;

        if (BatteryCharge.instance.Timer.IsStartTimer)
        {
            GetLabel((int)Labels.BatteryTime_Label).text = BatteryCharge.instance.Timer.ToString();
        }
        else
        {
            GetLabel((int)Labels.BatteryTime_Label).text = "30:00";
        }
    }
}

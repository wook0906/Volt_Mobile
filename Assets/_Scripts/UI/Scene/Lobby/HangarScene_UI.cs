using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarScene_UI : UI_Scene
{
    enum Textures
    {
        BG
    }

    enum Sprites
    {
        SkinName_BG,
        Skin_Sprite,
        EquipSkinUI_BG
    }

    enum Buttons
    {
        ChangeRobot_Btn_Prev,
        ChangeRobot_Btn_Next,
        ChangeSkin_Btn_Prev,
        ChangeSkin_Btn_Next,
        Equipped_Btn,
        Equip_Btn,
        Buy_Btn,
        Back_Btn
    }

    enum Labels
    {
        SkinName_Label,
        Equipped_Label,
        Equip_Label,
        Buy_Label
    }

    private Color activeEquipButtonColor = Color.white;
    private Color inActiveEquipButtonColor = Color.gray;

    private Volt_LobbyRobotViewSection robotViewSection;
    private List<RobotSkin> currentSkins = new List<RobotSkin>();
    private int currentSelectedRobotSkinIndex = 0;
    private RobotSkin currentSkin;
    private LobbyScene lobbyScene;
    public override void Init()
    {
        base.Init();

        Bind<UITexture>(typeof(Textures));
        Bind<UISprite>(typeof(Sprites));
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));

        lobbyScene = Managers.Scene.CurrentScene as LobbyScene;

        GetButton((int)Buttons.Equip_Btn).onClick.Add(new EventDelegate(() =>
        {
            Managers.UI.ShowPopupUIAsync<Equip_Popup>();
        }));

        GetButton((int)Buttons.Back_Btn).onClick.Add(new EventDelegate(() =>
        {
            lobbyScene.ChangeToLobbyCamera();
            Managers.UI.CloseSceneUI();
        }));

        GetButton((int)Buttons.ChangeRobot_Btn_Next).onClick.Add(new EventDelegate(OnClickNextRobot));
        GetButton((int)Buttons.ChangeRobot_Btn_Prev).onClick.Add(new EventDelegate(OnClickPrevRobot));
        GetButton((int)Buttons.ChangeSkin_Btn_Next).onClick.Add(new EventDelegate(OnClickNextSkin));
        GetButton((int)Buttons.ChangeSkin_Btn_Prev).onClick.Add(new EventDelegate(OnClickPrevSkin));
        GetButton((int)Buttons.Equipped_Btn).enabled = false;
        GetButton((int)Buttons.Buy_Btn).gameObject.SetActive(false);

        robotViewSection = FindObjectOfType<Volt_LobbyRobotViewSection>();

        LobbyScene scene = Managers.Scene.CurrentScene as LobbyScene;
        scene.OnLoadedHangarSceneUI();
    }

    public override void OnActive()
    {
        lobbyScene.ChangeToHangarCamera();
        currentSkins = Managers.Data.SkinDatas[robotViewSection.SelectRobotType].RobotSkins;
        OnChangeRobot();

    }

    public void OnClickNextRobot()
    {
        robotViewSection.OnClickNextModelBtn();

        RobotType type = robotViewSection.SelectRobotType;
        currentSkins = Managers.Data.SkinDatas[type].RobotSkins;


        OnChangeRobot();
    }

    public void OnClickPrevRobot()
    {
        robotViewSection.OnClickPrevModelBtn();

        RobotType type = robotViewSection.SelectRobotType;
        currentSkins = Managers.Data.SkinDatas[type].RobotSkins;

        OnChangeRobot();
    }

    public void OnClickNextSkin()
    {
        currentSelectedRobotSkinIndex++;
        currentSelectedRobotSkinIndex %= currentSkins.Count;

        RobotType currRobotType = currentSkins[currentSelectedRobotSkinIndex].RobotType;
        SkinType currSkinType = currentSkins[currentSelectedRobotSkinIndex].SkinType;

        GetSprite((int)Sprites.Skin_Sprite).spriteName = $"{currRobotType}_{currSkinType}";

        OnChangeSkinType();
    }

    public void OnClickPrevSkin()
    {
        currentSelectedRobotSkinIndex--;
        if (currentSelectedRobotSkinIndex < 0)
        {
            currentSelectedRobotSkinIndex = currentSkins.Count - 1;
        }

        RobotType currRobotType = currentSkins[currentSelectedRobotSkinIndex].RobotType;
        SkinType currSkinType = currentSkins[currentSelectedRobotSkinIndex].SkinType;

        GetSprite((int)Sprites.Skin_Sprite).spriteName = $"{currRobotType}_{currSkinType}";

        OnChangeSkinType();
    }

    public void ConfirmChangeRobotSkin()
    {
        RobotType robotType = robotViewSection.SelectRobotType;
        currentSkin = currentSkins[currentSelectedRobotSkinIndex];
        Volt_PlayerData.instance.selectdRobotSkins[robotType] = currentSkin;
        PlayerPrefs.SetInt($"{robotType}_skin", currentSelectedRobotSkinIndex);

        robotViewSection.CreateRobot(robotType, currentSkin.SkinType);

        GetButton((int)Buttons.Equip_Btn).gameObject.SetActive(false);
        GetButton((int)Buttons.Equipped_Btn).gameObject.SetActive(true);

        RobotSkin userCurrentSkin = Volt_PlayerData.instance.selectdRobotSkins[robotType];
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Korean:
                GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_KR;
                break;
            case SystemLanguage.German:
                GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_GER;
                break;
            case SystemLanguage.French:
                GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_Fren;
                break;
            default:
                GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_EN;
                break;
        }
    }

    private void OnChangeRobot()
    {
        RobotSkin userCurrentSkin;
        if (Volt_PlayerData.instance.selectdRobotSkins.TryGetValue(robotViewSection.SelectRobotType, out userCurrentSkin))
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Korean:
                    GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_KR;
                    break;
                case SystemLanguage.German:
                    GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_GER;
                    break;
                case SystemLanguage.French:
                    GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_Fren;
                    break;
                default:
                    GetLabel((int)Labels.SkinName_Label).text = userCurrentSkin.skinName_EN;
                    break;
            }

            int index = 0;
            for (index = 0; index < currentSkins.Count; ++index)
            {
                if (userCurrentSkin.skinID == currentSkins[index].skinID)
                    break;
            }

            currentSelectedRobotSkinIndex = index;
            currentSkin = currentSkins[currentSelectedRobotSkinIndex];
            GetSprite((int)Sprites.Skin_Sprite).spriteName = $"{currentSkin.RobotType}_{currentSkin.SkinType}";
        }
        OnChangeSkinType();
    }

    private void OnChangeSkinType()
    {
        RobotSkin userSkin = Volt_PlayerData.instance.selectdRobotSkins[robotViewSection.SelectRobotType];
        if (userSkin == currentSkins[currentSelectedRobotSkinIndex])
        {
            GetButton((int)Buttons.Equip_Btn).gameObject.SetActive(false);
            GetButton((int)Buttons.Equipped_Btn).gameObject.SetActive(true);
        }
        else
        {
            GetButton((int)Buttons.Equip_Btn).gameObject.SetActive(true);
            GetButton((int)Buttons.Equipped_Btn).gameObject.SetActive(false);
            GetButton((int)Buttons.Equip_Btn).enabled = true;
            bool hasSkin = Volt_PlayerData.instance.IsHaveSkin(currentSkins[currentSelectedRobotSkinIndex].skinID);
            if (hasSkin)
            {
                // Active Equip
                UIButton equipButton = GetButton((int)Buttons.Equip_Btn);
                equipButton.GetComponent<UISprite>().color = activeEquipButtonColor;
                equipButton.defaultColor = activeEquipButtonColor;
                equipButton.hover = activeEquipButtonColor;
                equipButton.disabledColor = activeEquipButtonColor;
                equipButton.pressed = activeEquipButtonColor;
                equipButton.enabled = true;
                equipButton.GetComponentInChildren<UILabel>().alpha = 1f;
            }
            else
            {
                // Inactive Equip
                UIButton equipButton = GetButton((int)Buttons.Equip_Btn);
                equipButton.GetComponent<UISprite>().color = inActiveEquipButtonColor;
                equipButton.defaultColor = inActiveEquipButtonColor;
                equipButton.hover = inActiveEquipButtonColor;
                equipButton.disabledColor = inActiveEquipButtonColor;
                equipButton.pressed = inActiveEquipButtonColor;
                equipButton.enabled = false;
                equipButton.GetComponentInChildren<UILabel>().alpha = 0.5f;
            }
        }
    }
}

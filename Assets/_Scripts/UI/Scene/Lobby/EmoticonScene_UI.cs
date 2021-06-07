using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmoticonSelectType
{
    Mercury,
    Volt,
    Hound,
    Reaper,
    Common
}

public class EmoticonScene_UI : UI_Scene
{
    enum Sprites
    {
        slot0Sprite,
        slot1Sprite,
        slot2Sprite,
        slot3Sprite,
        slot4Sprite,
        slot5Sprite
    }
    enum Buttons
    {
        slot0,
        slot1,
        slot2,
        slot3,
        slot4,
        slot5,
        Back_Btn,
        EmoMercuryTap,
        EmoReaperTap,
        EmoHoundTap,
        EmoVoltTap,
        CommonTap

    }
    enum ScrollViews
    {
        SelectRobotType_ScrollView,
        EmoticonInventory_ScrollView
    }
    private LobbyScene lobbyScene;
    
    EmoticonSelectType selectedEmoType;
    public override void Init()
    {
        base.Init();

        Bind<UISprite>(typeof(Sprites));
        Bind<UIScrollView>(typeof(ScrollViews));
        Bind<UIButton>(typeof(Buttons));

        lobbyScene = Managers.Scene.CurrentScene as LobbyScene;

        GetButton((int)Buttons.EmoHoundTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoHoundTap();
        }));

        GetButton((int)Buttons.EmoMercuryTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoMercuryTap();
        }));

        GetButton((int)Buttons.EmoReaperTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoReaperTap();
        }));

        GetButton((int)Buttons.EmoVoltTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoVoltTap();
        }));
        GetButton((int)Buttons.CommonTap).onClick.Add(new EventDelegate(() =>
        {
            OnClickEmoCommonTap();
        }));

        GetButton((int)Buttons.slot0).onClick.Add(new EventDelegate(() =>
        {
            OnClickSlot(GetButton((int)Buttons.slot0).gameObject);
        }));
        GetButton((int)Buttons.slot1).onClick.Add(new EventDelegate(() =>
        {
            OnClickSlot(GetButton((int)Buttons.slot1).gameObject);
        }));
        GetButton((int)Buttons.slot2).onClick.Add(new EventDelegate(() =>
        {
            OnClickSlot(GetButton((int)Buttons.slot2).gameObject);
        }));
        GetButton((int)Buttons.slot3).onClick.Add(new EventDelegate(() =>
        {
            OnClickSlot(GetButton((int)Buttons.slot3).gameObject);
        }));
        GetButton((int)Buttons.slot4).onClick.Add(new EventDelegate(() =>
        {
            OnClickSlot(GetButton((int)Buttons.slot4).gameObject);
        }));
        GetButton((int)Buttons.slot5).onClick.Add(new EventDelegate(() =>
        {
            OnClickSlot(GetButton((int)Buttons.slot5).gameObject);
        }));


        GetButton((int)Buttons.Back_Btn).onClick.Add(new EventDelegate(() =>
        {
            lobbyScene.ChangeToLobbyCamera();
            Managers.UI.CloseSceneUI();
        }));
        GetScrollView((int)ScrollViews.EmoticonInventory_ScrollView).GetComponent<EmoticonInventory_ScrollView>().onCompletedInit += () =>
        {
            
        };
        GetScrollView((int)ScrollViews.EmoticonInventory_ScrollView).GetComponent<EmoticonInventory_ScrollView>().Init();

        for (int i = 0; i < 6; i++)
        {
            GetSprite(i).spriteName = Volt_PlayerData.instance.GetEmoticonSet()[i];
        }

        lobbyScene.OnLoadedEmoticonSceneUI();
    }
    public override void OnActive()
    {
        OnClickEmoMercuryTap();
    }
    private void OnClickEmoVoltTap()
    {
        GetScrollView((int)ScrollViews.EmoticonInventory_ScrollView).
            GetComponent<EmoticonInventory_ScrollView>().
            OnClickEmoticonTapButton(EmoticonSelectType.Volt);

        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.CommonTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoVoltTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickEmoHoundTap()
    {
        GetScrollView((int)ScrollViews.EmoticonInventory_ScrollView).
            GetComponent<EmoticonInventory_ScrollView>().
            OnClickEmoticonTapButton(EmoticonSelectType.Hound);

        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.CommonTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoHoundTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickEmoReaperTap()
    {
        GetScrollView((int)ScrollViews.EmoticonInventory_ScrollView).
            GetComponent<EmoticonInventory_ScrollView>().
            OnClickEmoticonTapButton(EmoticonSelectType.Reaper);
        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.CommonTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoReaperTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickEmoMercuryTap()
    {
        GetScrollView((int)ScrollViews.EmoticonInventory_ScrollView).
            GetComponent<EmoticonInventory_ScrollView>().
            OnClickEmoticonTapButton(EmoticonSelectType.Mercury);
        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.CommonTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.EmoMercuryTap).normalSprite = "Btn_button03_p";
    }

    private void OnClickEmoCommonTap()
    {
        GetScrollView((int)ScrollViews.EmoticonInventory_ScrollView).
            GetComponent<EmoticonInventory_ScrollView>().
            OnClickEmoticonTapButton(EmoticonSelectType.Common);
        for (int i = (int)Buttons.EmoMercuryTap; i <= (int)Buttons.CommonTap; ++i)
        {
            GetButton(i).normalSprite = "Btn_button03_n";
        }
        GetButton((int)Buttons.CommonTap).normalSprite = "Btn_button03_p";
    }

    public void OnClickSlot(GameObject slot)
    {
        int slotNumber = int.Parse(slot.name.Substring(4));
        Volt_PlayerData.instance.SetEmoticon(slotNumber, "EmoticonNone");
        slot.transform.parent.GetComponent<UISprite>().spriteName = "EmoticonNone";
    }
    public int GetEmptySlotNumber()
    {
        foreach (var item in Volt_PlayerData.instance.GetEmoticonSet())
        {
            if (item.Value.Equals("EmoticonNone"))
            {
                return item.Key;
            }
        }
        return -1;
    }

}

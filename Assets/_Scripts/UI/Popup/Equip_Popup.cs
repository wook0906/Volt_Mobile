using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip_Popup : UI_Popup
{
    enum Labels
    {
        ConfirmMsg_Label,
        Yes_Label,
        No_Label
    }

    enum Buttons
    {
        Yes_Btn,
        No_Btn,
    }

    enum Sprites
    {
        BG,
        Block
    }

    
    public override void Init()
    {
        base.Init();
        Bind<UILabel>(typeof(Labels));
        Bind<UIButton>(typeof(Buttons));
        Bind<UISprite>(typeof(Sprites));

        //BindSprite<UIButton>(typeof(Buttons));
        //BindSprite<UISprite>(typeof(Sprites));

        //SetButtonSwap(typeof(Buttons));

        GetButton((int)Buttons.No_Btn).onClick.Add(new EventDelegate(() =>
        {;
            ClosePopupUI();
        }));

        GetButton((int)Buttons.Yes_Btn).onClick.Add(new EventDelegate(() =>
        {
            HangarScene_UI hangarUI = FindObjectOfType<HangarScene_UI>();
            hangarUI.ConfirmChangeRobotSkin();
            ClosePopupUI();
        }));
    }
}

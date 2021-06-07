using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_Popup : UI_Popup
{
    enum Buttons
    {
        Yes_Btn,
        No_Btn
    }

    enum Labels
    {
        Msg_Label,
        Yes_Label,
        No_Label
    }

    enum Sprites
    {
        Background,
        Block,

    }

    public override void Init()
    {
        base.Init();

        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));
        Bind<UISprite>(typeof(Sprites));

        //BindSprite<UIButton>(typeof(Buttons));
        //BindSprite<UISprite>(typeof(Sprites));

        //SetButtonSwap(typeof(Buttons));

        Get<UIButton>((int)Buttons.No_Btn).onClick.Add(new EventDelegate(() =>
        {
            ClosePopupUI();
        }));

        Get<UIButton>((int)Buttons.Yes_Btn).onClick.Add(new EventDelegate(() =>
        {
            Application.Quit();
        }));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameLimitLengthExceeded_Popup : UI_Popup
{
    enum Labels
    {
        Msg_Label,
        Ok_Label
    }

    enum Buttons
    {
        Ok_Btn
    }

    enum Sprites
    {
        Background,
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

        Get<UIButton>((int)Buttons.Ok_Btn).onClick.Add(new EventDelegate(() =>
        {
            ClosePopupUI();
        }));
    }
}

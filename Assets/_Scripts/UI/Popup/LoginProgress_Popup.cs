using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginProgress_Popup : UI_Popup
{
    enum Lables
    {
        Access_Label
    }

    enum Sprites
    {
        Background,
        Block,
        Internet_Sprite
    }

    public override void Init()
    {
        base.Init();

        Bind<UILabel>(typeof(Lables));
        Bind<UISprite>(typeof(Sprites));

        //BindSprite<UISprite>(typeof(Sprites));
    }
}

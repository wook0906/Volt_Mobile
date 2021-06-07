using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkError_Popup : UI_Popup
{
    enum Labels
    {
        Error_Label,
        ErrorMsg_Label
    }

    enum Sprites
    {
        Background,
        Title_Sprite,
        WarningIcon_Sprite,
        Block
    }



    public override void Init()
    {
        
        base.Init();

        Bind<UILabel>(typeof(Labels));
        Bind<UISprite>(typeof(Sprites));

        //BindSprite<UISprite>(typeof(Sprites));
    }

}

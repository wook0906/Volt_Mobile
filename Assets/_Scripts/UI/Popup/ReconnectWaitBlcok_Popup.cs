using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconnectWaitBlcok_Popup : UI_Popup 
{
    enum Buttons
    {	
        
    }

    enum Labels
    {
        
        Msg_Label,
        NoticeLabel,
    }

    enum ScrollViews
    {
        
    }

    enum Sliders
    {
        
    }

    enum Progressbars
    {
        
    }

    enum Toggles
    {
        
    }

    enum Sprites
    {
        
        AlarmBG,
        AlarmLabelBG,
        NetworkSprite,
        Block,
    }

    enum GameObjects
    {
        
    }

    public override void Init()
    {
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));
        Bind<UIScrollView>(typeof(ScrollViews));
        Bind<UISlider>(typeof(Sliders));
        Bind<UIProgressBar>(typeof(Progressbars));
        Bind<UISprite>(typeof(Sprites));
        Bind<GameObject>(typeof(GameObjects));
     }
}

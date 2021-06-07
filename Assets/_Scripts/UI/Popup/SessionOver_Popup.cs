using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionOver_Popup : UI_Popup 
{
    enum Buttons
    {	
        
        OK_Btn,
    }

    enum Labels
    {
        
        Msg_Label,
        Ok_Label,
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
        
        BG,
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

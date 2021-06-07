using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAccountCompleted_Popup : UI_Popup
{
    enum Labels
    {
        Notice_Label
    }

    enum Buttons
    {
        Ok_Btn
    }

    public override void Init()
    {
        base.Init();
        Bind<UILabel>(typeof(Labels));
        Bind<UIButton>(typeof(Buttons));

        GetButton((int)Buttons.Ok_Btn).onClick.Add(new EventDelegate(() => { Application.Quit(); }));
    }
}

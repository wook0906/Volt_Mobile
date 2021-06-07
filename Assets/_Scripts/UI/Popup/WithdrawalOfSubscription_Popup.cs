using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithdrawalOfSubscription_Popup : UI_Popup
{
    enum Buttons
    {
        Confirm_Btn
    }

    public override void Init()
    {
        base.Init();
        Bind<UIButton>(typeof(Buttons));

        GetButton((int)Buttons.Confirm_Btn).onClick.Add(new EventDelegate(() =>
        {
            ClosePopupUI();
        }));


    }
}

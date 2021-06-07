using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseFail_Popup : UI_Popup
{
    enum Buttons
    {
        Ok_Btn
    }

    public override void Init()
    {
        base.Init();

        Bind<UIButton>(typeof(Buttons));

        GetButton((int)Buttons.Ok_Btn).onClick.Add(new EventDelegate(() =>
        {
            ClosePopupUI();
        }));
    }
}

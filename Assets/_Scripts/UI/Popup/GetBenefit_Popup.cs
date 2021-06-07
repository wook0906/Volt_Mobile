using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBenefit_Popup : UI_Popup
{
    enum Buttons
    {
        Confirm_Btn,
    }

    enum Labels
    {
        Notice_Label
    }


    public override void Init()
    {
        base.Init();
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));

        GetButton((int)Buttons.Confirm_Btn).onClick.Add(new EventDelegate(() =>
        {
            ClosePopupUI();
        }));

        switch (Managers.Data.CurrentGetBenefitType)
        {
            case EBenefitType.Pack1Battery:
                if (Managers.Data.GetBenefitResult == 0)//성공
                    GetLabel((int)Labels.Notice_Label).text = "배터리를 획득했습니다!";
                else
                    GetLabel((int)Labels.Notice_Label).text = "배터리를 획득하지 못했습니다.\n 다시 시도해주세요.";
                break;
            default:
                break;
        }
    }
}

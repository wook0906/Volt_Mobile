using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenefitAlarm_Popup : UI_Popup
{
    enum Buttons
    {
        Get_Btn,
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

        GetLabel((int)Labels.Notice_Label).text = "광고사로부터 배터리 5개가 도착했습니다.";

        GetButton((int)Buttons.Get_Btn).onClick.Add(new EventDelegate(() =>
        {
            PacketTransmission.SendRequestBenefit(EBenefitType.Pack1Battery);
            ClosePopupUI();
        }));

        
    }
}

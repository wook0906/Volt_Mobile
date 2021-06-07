using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireReward_Popup : UI_Popup
{
    enum Sprites
    {
        RewardAsset_Icon
    }

    enum Buttons
    {
        Ok_Btn
    }

    enum Labels
    {
        RewardCount_Label
    }

    public override void Init()
    {
        base.Init();

        Bind<UISprite>(typeof(Sprites));
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));

        //TODO: 구매버튼 누를 때 구매정보를 잠시 기록해뒀다가
        //그걸 꺼내와서 UI를 구성하자!!
    }
}

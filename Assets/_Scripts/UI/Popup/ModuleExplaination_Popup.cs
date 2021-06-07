using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleExplaination_Popup : UI_Popup
{
    enum Sprites
    {
        Block,
        CardImage
    }

    enum Buttons
    {
        Card_Btn
    }

    enum Labels
    {
        Descript_Label,
        CardName_Label
    }

    public override void Init()
    {
        base.Init();

        Bind<UISprite>(typeof(Sprites));
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));

        //BindSprite<UISprite>(typeof(Sprites));
        //BindSprite<UIButton>(typeof(Buttons));

        //SetButtonSwap(typeof(Buttons));

        GetButton((int)Buttons.Card_Btn).onClick.Add(new EventDelegate(() =>
        {
            gameObject.SetActive(false);
        }));

        ModuleScene_UI scene_UI = FindObjectOfType<ModuleScene_UI>();
        scene_UI.ExplainationPopup = this;
        gameObject.SetActive(false);
    }

    public void ShowPopup(string cardName)
    {
        GetSprite((int)Sprites.CardImage).spriteName = cardName;
        ModuleDescriptionInfo moduleDescriptionInfo = Volt_ModuleDescriptionInfos.GetModuleDescriptionInfo(cardName, Application.systemLanguage);
        GetLabel((int)Labels.CardName_Label).text = moduleDescriptionInfo.title;
        GetLabel((int)Labels.Descript_Label).text = moduleDescriptionInfo.description;
    }
}

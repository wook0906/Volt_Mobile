using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemOption_Popup : UI_Popup
{
    enum Sprites
    {
        Block,
        BG,
        Option_BG,
        Volume_Slider_FG,
        Volume_Icon,
        BGM_Icon,
        BGM_Slider_FG,
        Exit_Icon
    }

    enum Buttons
    {
        Exit_Btn,
        RestAccount_Btn,
        Volume_Slider_Arrow,
        BGM_Slider_Arrow
    }

    enum Labels
    {
        Option_Label,
        ResetAccount_Label,
        Volume_Label,
        BGM_Label
    }

    enum Sliders
    {
        Volume_Slider_BG,
        BGM_Slider_BG
    }

    public override void Init()
    {
        base.Init();
        Bind<UISprite>(typeof(Sprites));
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));
        Bind<UISlider>(typeof(Sliders));

        //BindSprite<UISprite>(typeof(Sprites));
        //BindSprite<UIButton>(typeof(Buttons));
        //BindSprite<UISlider>(typeof(Sliders));

        //SetButtonSwap(typeof(Buttons));

        GetButton((int)Buttons.Exit_Btn).onClick.Add(new EventDelegate(() =>
        {
            ClosePopupUI();
        }));

        GetButton((int)Buttons.RestAccount_Btn).onClick.Add(new EventDelegate(() =>
        {
            Managers.UI.ShowPopupUIAsync<ResetAccount_Popup>();
        }));

        if (!PlayerPrefs.HasKey("Volt_MusicVolume"))
            PlayerPrefs.SetFloat("Volt_MusicVolume", 1f);
        if (!PlayerPrefs.HasKey("Volt_SoundVolume"))
            PlayerPrefs.SetFloat("Volt_SoundVolume", 1f);

        GetSlider((int)Sliders.BGM_Slider_BG).value = PlayerPrefs.GetFloat("Volt_MusicVolume");
        GetSlider((int)Sliders.Volume_Slider_BG).value = PlayerPrefs.GetFloat("Volt_SoundVolume");

        GetSlider((int)Sliders.BGM_Slider_BG).onChange.Add(new EventDelegate(() =>
        {
            PlayerPrefs.SetFloat("Volt_MusicVolume", GetSlider((int)Sliders.BGM_Slider_BG).value);
            Volt_SoundManager.S.OnChangedMusicVolume(GetSlider((int)Sliders.BGM_Slider_BG).value);
        }));
        GetSlider((int)Sliders.Volume_Slider_BG).onChange.Add(new EventDelegate(() =>
        {
            PlayerPrefs.SetFloat("Volt_MusicVolume", GetSlider((int)Sliders.Volume_Slider_BG).value);
            Volt_SoundManager.S.OnChangedSoundVolume(GetSlider((int)Sliders.Volume_Slider_BG).value);
        }));
    }
}

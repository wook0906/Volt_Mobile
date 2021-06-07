using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMap_Popup : UI_Popup
{
    enum Sprites
    {
        Block,
        Background,
        Training_BG,
        Exit_Icon
    }

    enum GameObjects
    {
        MapSelectHighlight_Sprite
    }

    enum Buttons
    {
        Exit_Btn,
        TwinCity_Btn,
        Rome_Btn,
        Ruhrgebiet_Btn,
        Tokyo_Btn
    }

    enum Labels
    {
        Training_Label
    }

    private bool isSelectTwincity;
    private bool isSelectRome;
    private bool isSelectRuhrgebiet;
    private bool isSelectTokyo;

    public override void Init()
    {
        base.Init();
        Bind<UISprite>(typeof(Sprites));
        Bind<GameObject>(typeof(GameObjects));
        Bind<UIButton>(typeof(Buttons));
        Bind<UILabel>(typeof(Labels));

        //BindSprite<UISprite>(typeof(Sprites));
        //BindSprite(typeof(GameObjects));
        //BindSprite<UIButton>(typeof(Buttons));

        //SetButtonSwap(typeof(Buttons));

        GetGameObject((int)GameObjects.MapSelectHighlight_Sprite).SetActive(false);
        GetButton((int)Buttons.Exit_Btn).onClick.Add(new EventDelegate(() => { ClosePopupUI(); }));

        for (int i = (int)Buttons.TwinCity_Btn; i <= (int)Buttons.Tokyo_Btn; ++i)
        {
            EventDelegate onClickMapButton = new EventDelegate(this, "OnClickMapButton");
            onClickMapButton.parameters[0] = Util.MakeParameter(GetButton(i), typeof(UIButton));
            onClickMapButton.parameters[1] = Util.MakeParameter(GetButton(i).transform, typeof(Transform));
            GetButton(i).onClick.Add(onClickMapButton);
        }
    }

    private void OnClickMapButton(UIButton button, Transform mapButton)
    {
        GetGameObject((int)GameObjects.MapSelectHighlight_Sprite).SetActive(true);
        GetGameObject((int)GameObjects.MapSelectHighlight_Sprite).transform.position = mapButton.transform.position;

        bool isStartGame = false;
        switch (button.name)
        {
            case "TwinCity_Btn":
                if(isSelectTwincity)
                {
                    PlayerPrefs.SetInt("SELECTED_MAP", (int)MapType.TwinCity);
                    PlayerPrefs.SetInt("Volt_TrainingMode", 1);
                    isStartGame = true;
                    
                }
                else
                {
                    isSelectTwincity = true;
                    isSelectRome = false;
                    isSelectRuhrgebiet = false;
                    isSelectTokyo = false;
                }
                break;
            case "Rome_Btn":
                if (isSelectRome)
                {
                    PlayerPrefs.SetInt("SELECTED_MAP", (int)MapType.Rome);
                    PlayerPrefs.SetInt("Volt_TrainingMode", 1);
                    isStartGame = true;
                }
                else
                {
                    isSelectRome = true;
                    isSelectTwincity = false;
                    isSelectRuhrgebiet = false;
                    isSelectTokyo = false;
                }
                break;
            case "Ruhrgebiet_Btn":
                if (isSelectRuhrgebiet)
                {
                    PlayerPrefs.SetInt("SELECTED_MAP", (int)MapType.Ruhrgebiet);
                    PlayerPrefs.SetInt("Volt_TrainingMode", 1);
                    isStartGame = true;
                }
                else
                {
                    isSelectRuhrgebiet = true;
                    isSelectRome = false;
                    isSelectTwincity = false;
                    isSelectTokyo = false;
                }
                break;
            case "Tokyo_Btn":
                if (isSelectTokyo)
                {
                    PlayerPrefs.SetInt("SELECTED_MAP", (int)MapType.Tokyo);
                    PlayerPrefs.SetInt("Volt_TrainingMode", 1);
                    isStartGame = true;
                }
                else
                {
                    isSelectTokyo = true;
                    isSelectRome = false;
                    isSelectTwincity = false;
                    isSelectRuhrgebiet = false;
                }
                break;
            default:
                break;
        }

        if (!isStartGame)
            return;

        ClosePopupUI();
        LobbyScene_UI lobbyScene_UI = FindObjectOfType<LobbyScene_UI>();
        lobbyScene_UI.OnClickStartGame();
    }
}

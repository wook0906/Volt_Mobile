using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchScene_UI : UI_Scene
{
    enum Textures
    {
        BG
    }

    enum Sprites
    {
        Black_BG1,
    }

    enum Buttons
    {
        DailyTap_Btn,
        NormalTap_Btn,
        DailtyScrollView_VerticalScrollBar,
        DS_Foreground,
        NormalScrollView_VerticalScrollBar,
        NS_Foreground,
        Back_Btn,

    }

    enum GameObjects
    {
        DailyScrollViewRoot,
        DailyACHItemRoot,
        NormalScrollViewRoot,
        NormalACHItemRoot
    }

    enum Labels
    {
        Daily_Label,
        Normal_Label,

    }

    enum ScrollViews
    {
        DailyACHView,
        NormalACHView
    }

    private byte loadFlag = 0;
    private LobbyScene lobbyScene;
    public override void Init()
    {
        base.Init();
        Bind<UITexture>(typeof(Textures));
        Bind<UISprite>(typeof(Sprites));
        Bind<UIButton>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<UIScrollView>(typeof(ScrollViews));

        lobbyScene = Managers.Scene.CurrentScene as LobbyScene;

        GetButton((int)Buttons.Back_Btn).onClick.Add(new EventDelegate(() =>
        {
            lobbyScene.ChangeToLobbyCamera();
            Managers.UI.CloseSceneUI(this);
        }));
        GetButton((int)Buttons.DailyTap_Btn).onClick.Add(new EventDelegate(OnClickDailyTapButton));

        GetButton((int)Buttons.NormalTap_Btn).onClick.Add(new EventDelegate(OnClickNormalTapButton));

        Get<UIScrollView>((int)ScrollViews.DailyACHView).GetComponent<ScrollViewItemCreator>().Init(() =>
        {
            loadFlag |= 1;
            if (loadFlag == 3)
            {
                LobbyScene lobbyScene = Managers.Scene.CurrentScene as LobbyScene;
                lobbyScene.OnLoadedAchSceneUI();
            }
        });
        Get<UIScrollView>((int)ScrollViews.NormalACHView).GetComponent<ScrollViewItemCreator>().Init(() =>
        {
            loadFlag |= 2;
            if (loadFlag == 3)
            {
                LobbyScene lobbyScene = Managers.Scene.CurrentScene as LobbyScene;
                lobbyScene.OnLoadedAchSceneUI();
            }
        });
    }

    public override void OnActive()
    {
        OnClickDailyTapButton();
    }

    public ScrollViewItemCreator GetNormalAchScrollViewItemCreator()
    {
        return Get<UIScrollView>((int)ScrollViews.NormalACHView).GetComponent<ScrollViewItemCreator>();
    }

    public ScrollViewItemCreator GetDailyAchScrollViewItemCreator()
    {
        return Get<UIScrollView>((int)ScrollViews.DailyACHView).GetComponent<ScrollViewItemCreator>();
    }

    private void OnClickDailyTapButton()
    {
        GetButton((int)Buttons.DailyTap_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.DailyTap_Btn).normalSprite = "button01_textureless_p";
        GetButton((int)Buttons.NormalTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.NormalTap_Btn).normalSprite = "button01_textureless_n";
        Get<UIScrollView>((int)ScrollViews.DailyACHView).GetComponent<ScrollViewItemCreator>().Show();
        Get<UIScrollView>((int)ScrollViews.NormalACHView).GetComponent<ScrollViewItemCreator>().Hide();
    }

    private void OnClickNormalTapButton()
    {
        GetButton((int)Buttons.NormalTap_Btn).GetComponent<UISprite>().depth = 3;
        GetButton((int)Buttons.NormalTap_Btn).normalSprite = "button01_textureless_p";
        GetButton((int)Buttons.DailyTap_Btn).GetComponent<UISprite>().depth = 1;
        GetButton((int)Buttons.DailyTap_Btn).normalSprite = "button01_textureless_n";
        Get<UIScrollView>((int)ScrollViews.NormalACHView).GetComponent<ScrollViewItemCreator>().Show();
        Get<UIScrollView>((int)ScrollViews.DailyACHView).GetComponent<ScrollViewItemCreator>().Hide();
    }
}

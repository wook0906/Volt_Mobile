using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Popup : UI_Popup
{
    enum Progressbar
    {
        ProgressBarBG,

    }

    enum Sprites
    {
        ProgressBarFG_Sprite,
        MsgBoxBG_Sprite
    }

    enum Labels
    {
        Loading_Label
    }

    enum Textures
    {
        BG_Texture
    }

    public bool IsInit { get; set; }
    public override void Init()
    {
        base.Init();

        Bind<UISprite>(typeof(Sprites));
        Bind<UIProgressBar>(typeof(Progressbar));
        Bind<UILabel>(typeof(Labels));
        Bind<UITexture>(typeof(Textures));

        //BindSprite<UISprite>(typeof(Sprites));
        //BindSprite<UIProgressBar>(typeof(Progressbar));

        Managers.Resource.LoadAsync<Texture2D>(Random.Range(0, 9).ToString(),
            (result) =>
            {
                Get<UITexture>((int)Textures.BG_Texture).mainTexture = result.Result;
                IsInit = true;
                StartCoroutine(CoRunLoadAnimation());
            });

        Get<UIProgressBar>((int)Progressbar.ProgressBarBG).value = 0f;
    }

    public override void OnClose()
    {
        Managers.Resource.Release<Texture>(Get<UITexture>((int)Textures.BG_Texture).mainTexture);
    }

    IEnumerator CoRunLoadAnimation()
    {
        UIProgressBar slider = Get<UIProgressBar>((int)Progressbar.ProgressBarBG);
        BaseScene currScene = Managers.Scene.CurrentScene;
        while (!currScene.IsDone)
        {
            slider.value = currScene.Progress;
            yield return null;
        }
        slider.value = 1f;
    }
}

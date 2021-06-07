using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_Popup : UI_Popup
{
    enum Sprites
    {
        Dark_Sprite
    }

    public bool IsDone { private set; get; }
    public bool IsStartRightAway { set; get; } = false;

    public override void Init()
    {
        base.Init();

        Bind<UISprite>(typeof(Sprites));

        //BindSprite<UISprite>(typeof(Sprites));
        IsDone = false;
    }

    public void FadeIn(float time, float delayTime = 0f)
    {
        IsDone = false;
        IsStartRightAway = false;
        StartCoroutine(CoFadeIn(time, delayTime));
    }

    private IEnumerator CoFadeIn(float time, float delayTime)
    {
        while (delayTime > 0 && !IsStartRightAway)
        {
            delayTime -= Time.deltaTime;
            yield return null;
        }
        UIPanel panel = GetComponent<UIPanel>();
        panel.alpha = 1.0f;
        float elaspedTime = 0f;
        while (time > elaspedTime)
        {
            panel.alpha = Mathf.Lerp(1f, 0f, elaspedTime / time);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        panel.alpha = 0f;
        IsDone = true;
    }

    public void FadeOut(float time, float delayTime = 0f)
    {
        IsDone = false;
        IsStartRightAway = false;
        StartCoroutine(CoFadeOut(time, delayTime));
    }

    private IEnumerator CoFadeOut(float time, float delayTime)
    {
        while (delayTime > 0 && !IsStartRightAway)
        {
            delayTime -= Time.deltaTime;
            yield return null;
        }

        UIPanel panel = GetComponent<UIPanel>();
        panel.alpha = 0f;
        float elaspedTime = 0f;
        while (time > elaspedTime)
        {
            panel.alpha = Mathf.Lerp(0f, 1f, elaspedTime / time);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        panel.alpha = 1f;
        IsDone = true;
    }
}

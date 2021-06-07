using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_SpriteAnimationMSG : MonoBehaviour
{
    public float fadeTime = 1f;
    public float exposureTime = 1.5f;
    public UI2DSprite ui2dSprite;
    public UI2DSpriteAnimation ui2dSpriteAnimation;

    public Sprite[] DefeatFrames;
    public Sprite[] playerControlFrames;
    public Sprite[] RoundOverFrames;
    public Sprite[] SuddenDeathFrames;
    public Sprite[] VictoryFrames;

    public void SetMSG(GuideMSGType msgType, bool isNeedFadeOut)
    {
        switch (msgType)
        {
            case GuideMSGType.Victory:
                ui2dSprite.SetRect((680f / 2f) * -1f, (180f / 2f) * -1f, 680, 180);
                ui2dSprite.sprite2D = VictoryFrames[0];
                ui2dSpriteAnimation.frames = VictoryFrames;
                ui2dSpriteAnimation.Play();
                break;
            case GuideMSGType.Defeat:
                ui2dSprite.SetRect((680f / 2f) * -1f, (180f / 2f) * -1f, 680, 180);
                ui2dSprite.sprite2D = DefeatFrames[0];
                ui2dSpriteAnimation.frames = DefeatFrames;
                ui2dSpriteAnimation.Play();
                break;
            case GuideMSGType.RoundEnd:
                ui2dSprite.SetRect((450f / 2f) * -1f, (76f / 2f) * -1f, 450, 76);
                ui2dSprite.sprite2D = RoundOverFrames[0];
                ui2dSpriteAnimation.frames = RoundOverFrames;
                ui2dSpriteAnimation.Play();
                break;
            case GuideMSGType.SuddenDeath:
                ui2dSprite.SetRect((680f / 2f) * -1f, (180f / 2f) * -1f, 680, 180);
                ui2dSprite.sprite2D = SuddenDeathFrames[0];
                ui2dSpriteAnimation.frames = SuddenDeathFrames;
                ui2dSpriteAnimation.Play();
                break;
            default:
                Debug.Log("sprite animation SetMSG Error");
                break;
        }
        if (isNeedFadeOut)
            StartCoroutine(fadeOutDestroy());
    }
    
    IEnumerator fadeOutDestroy()
    {
        yield return new WaitUntil(() => !ui2dSpriteAnimation.isPlaying);
        yield return new WaitForSecondsRealtime(exposureTime);
        float timer = 0;
        while (timer < fadeTime)
        {
            timer += Time.fixedDeltaTime;
            ui2dSprite.alpha -= Time.fixedDeltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}

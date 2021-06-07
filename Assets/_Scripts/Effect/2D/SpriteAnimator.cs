using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteFrames
{
    GoldFrames,
    DiamondFrames
}

public class SpriteAnimator : MonoBehaviour
{
    public UI2DSprite ui2dSprite;
    public UI2DSpriteAnimation animation;

    public Sprite[] GetGoldRewardFrames;
    public Sprite[] GetDiamondRewardFrames;

    public void SetSpriteFrames(SpriteFrames type)
    {
        switch (type)
        {
            case SpriteFrames.GoldFrames:
                ui2dSprite.sprite2D = GetGoldRewardFrames[0];
                animation.frames = GetGoldRewardFrames;
                break;
            case SpriteFrames.DiamondFrames:
                ui2dSprite.sprite2D = GetDiamondRewardFrames[0];
                animation.frames = GetDiamondRewardFrames;
                
                break;
            default:
                Debug.Log($"Wrong Type:{type}");
                return;
        }
        animation.Play();
        Invoke("Destroy", (float)animation.frames.Length / animation.framesPerSecond + 0.1f);
    }

    private void Destroy()
    {
        Managers.Resource.DestoryAndRelease(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Volt_RobotPanel : MonoBehaviour
{
   

    [Header("Set in Inspector")]

    public List<UISprite> hpSprites;
    public List<UISprite> shieldSprites;
    public UILabel IDLabel;
    public UILabel orderNumberLabel;
    public Transform msgTransform;
    public UI2DSpriteAnimation controlWaitSprite;
    public UISprite emoticonSprite;
    //public Animator emoticonAnimator;

    

    [Header("Set in Script")]
    public Volt_Robot owner;
    UISprite bg;

    Vector3 originBGScale;
    
    public void IDSet(int playerNumber) //tmp
    {
        IDLabel.text = Volt_PlayerManager.S.GetPlayerByPlayerNumber(playerNumber).NickName;
    }
    public void OrderNumberSet(int orderNumber)
    {
        if (orderNumber != -1)
            orderNumberLabel.text = orderNumber.ToString();
        else
            orderNumberLabel.text = "-";
    }

    // Start is called before the first frame update
    private void Awake()
    {
        owner = Util.FindParent<Volt_Robot>(gameObject, null, true);
    }
    void Start()
    {
        bg = transform.Find("BG").GetComponent<UISprite>();
        originBGScale = bg.transform.localScale;
        foreach (var item in shieldSprites)
        {
            item.color = Color.clear;
        }
    }

    private void FixedUpdate()
    {
        if (!Volt_PlayerManager.S.I) return;
        if (!Volt_PlayerManager.S.I.playerCam) return;

        float distance = Vector3.Distance(Volt_PlayerManager.S.I.playerCam.transform.position
            , Volt_ArenaSetter.S.GetCenterTransform().position);

        //Vector3 tmp = originBGScale;
        //tmp *= 0.065f * distance;
        //bg.transform.localScale = tmp; 
    }

    public void HpRenew(int hitCount)
    {
        if (!owner) return;
        if (owner.AddOnsMgr.ShieldPoints > 0)
        {
            foreach (var item in hpSprites)
            {
                item.color = Color.clear;
            }
            return;
        }

        for(int i = 0; i < hitCount; ++i)
        {
            hpSprites[i].color = Color.red;
        }

        for(int i = 0; i < hpSprites.Count - hitCount; ++i)
        {
            hpSprites[hitCount + i].color = Color.green;
        }

        //if (hitCount >= 3)
        //{
        //    foreach (var item in hpSprites)
        //    {
        //        item.color = Color.red;
        //    }
        //}
        //else if (hitCount == 2)
        //{
        //    foreach (var item in hpSprites)
        //    {
        //        item.color = Color.green;
        //    }
        //    hpSprites[0].color = Color.red;
        //    hpSprites[1].color = Color.red;

        //}
        //else if (hitCount == 1)
        //{
        //    foreach (var item in hpSprites)
        //    {
        //        item.color = Color.green;
        //    }
        //    hpSprites[0].color = Color.red;
        //}
        //else if (hitCount == 0)
        //{
        //    foreach (var item in hpSprites)
        //    {
        //        item.color = Color.green;
        //    }
        //}
    }
    public void RenewShield(int shieldPoint)
    {
        if (shieldPoint == 2)
        {
            shieldSprites[0].color = Color.white;
            shieldSprites[1].color = Color.white;
        }
        else if(shieldPoint == 1)
        {
            shieldSprites[0].color = Color.red;
            shieldSprites[1].color = Color.white;
        }
        else
        {
            foreach (var item in shieldSprites)
                item.color = Color.clear;
        }
        HpRenew(owner.HitCount);
    }
    public void IndicateControl(bool show)
    {
        if (show)
        {
            controlWaitSprite.gameObject.SetActive(true);
            controlWaitSprite.Play();
        }
        else
        {
            controlWaitSprite.gameObject.SetActive(false);
        }
    }

    public void EmoticonPlay(Define.EmoticonType emoticonType)
    {
        string emoticonSoundName = string.Empty;
        emoticonSprite.spriteName = emoticonType.ToString();

        if(emoticonType == Define.EmoticonType.Hound_Angry ||
            emoticonType == Define.EmoticonType.Volt_Angry ||
            emoticonType == Define.EmoticonType.Mercury_Angry||
            emoticonType == Define.EmoticonType.Reaper_Angry)
        {
            emoticonSoundName = "Angry.wav";
        }
        else if (emoticonType == Define.EmoticonType.Hound_Joy ||
            emoticonType == Define.EmoticonType.Volt_Joy ||
            emoticonType == Define.EmoticonType.Mercury_Joy ||
            emoticonType == Define.EmoticonType.Reaper_Joy)
        {
            emoticonSoundName = "Joy.wav";
        }
        else if (emoticonType == Define.EmoticonType.Hound_Laughter ||
            emoticonType == Define.EmoticonType.Volt_Laughter ||
            emoticonType == Define.EmoticonType.Mercury_Laughter ||
            emoticonType == Define.EmoticonType.Reaper_Laughter)
        {
            emoticonSoundName = "Laugh.wav";
        }
        else if (emoticonType == Define.EmoticonType.Hound_Boo ||
            emoticonType == Define.EmoticonType.Volt_Boo ||
            emoticonType == Define.EmoticonType.Mercury_Boo ||
            emoticonType == Define.EmoticonType.Reaper_Boo ||
            emoticonType == Define.EmoticonType.Hound_MakeFunOf ||
            emoticonType == Define.EmoticonType.Volt_MakeFunOf ||
            emoticonType == Define.EmoticonType.Mercury_MakeFunOf ||
            emoticonType == Define.EmoticonType.Reaper_MakeFunOf)
        {
            emoticonSoundName = "MakeFunOfBoo.wav";
        }
        else if (emoticonType == Define.EmoticonType.Hound_QuestionMark ||
            emoticonType == Define.EmoticonType.Volt_QuestionMark ||
            emoticonType == Define.EmoticonType.Mercury_QuestionMark ||
            emoticonType == Define.EmoticonType.Reaper_QuestionMark ||
            emoticonType == Define.EmoticonType.Hound_Hi ||
            emoticonType == Define.EmoticonType.Volt_Hi ||
            emoticonType == Define.EmoticonType.Mercury_Hi ||
            emoticonType == Define.EmoticonType.Reaper_Hi)
        {
            emoticonSoundName = "QuestionHi.wav";
        }
        else if (emoticonType == Define.EmoticonType.Common_Surrender||
            emoticonType == Define.EmoticonType.Hound_Sadness ||
            emoticonType == Define.EmoticonType.Volt_Sadness ||
            emoticonType == Define.EmoticonType.Mercury_Sadness ||
            emoticonType == Define.EmoticonType.Reaper_Sadness)
        {
            emoticonSoundName = "SadSurrender.wav";
        }
        else if (emoticonType == Define.EmoticonType.Hound_Surprise ||
            emoticonType == Define.EmoticonType.Volt_Surprise ||
            emoticonType == Define.EmoticonType.Mercury_Surprise ||
            emoticonType == Define.EmoticonType.Reaper_Surprise ||
            emoticonType == Define.EmoticonType.Hound_Happiness ||
            emoticonType == Define.EmoticonType.Volt_Happiness ||
            emoticonType == Define.EmoticonType.Mercury_Happiness ||
            emoticonType == Define.EmoticonType.Reaper_Happiness||
            emoticonType == Define.EmoticonType.Hound_ThumbsUp ||
            emoticonType == Define.EmoticonType.Volt_ThumbsUp ||
            emoticonType == Define.EmoticonType.Mercury_ThumbsUp ||
            emoticonType == Define.EmoticonType.Reaper_ThumbsUp)
        {
            emoticonSoundName = "SurpriseHappinessThumbsUp.wav";
        }

        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/EmoticonSound/"+emoticonSoundName,
                    (result) =>
                    {
                        Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                    });
        Animator emoticonAnim = emoticonSprite.GetComponent<Animator>();
        
        emoticonSprite.GetComponent<Animator>().PlayInFixedTime("Show",0, 0f);
    }

    public bool IsEmoticonShowing()
    {
        Animator anim = emoticonSprite.GetComponent<Animator>();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("New State"))
        {
            return false;
        }
        else
            return true;


    }
}

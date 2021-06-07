using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACHItem_Delete : MonoBehaviour
{
    public UILabel titleLabel;
    public UILabel descriptionLabel;
    public UILabel rewardCountLabel;
    public UISprite rewardICONSprite;
    public UILabel conditionCountLabel;
    public UIButton getRewardButton;
    public UISprite getRewardButtonSprite;
    public UILabel getRewardButtonLabel;
    public NGUIFont nguiFont;

    //시스템 값(코드내에서 수정하면 안되는 값!!)
    [SerializeField]
    private int ID;
    private string title_KR;
    private string title_EN;
    private string title_GER;
    private string title_Fren;
    private string description_KR;
    private string description_EN;
    private string description_GER;
    private string description_Fren;
    private EAssetsType rewardType;
    private int rewardCount;
    private int conditionType;
    private int conditionCount;
    private string rewardICON;
    private string iconAtlas;
    private string progressButtonName_KR;
    private string progressButtonName_EN;
    private string progressButtonName_GER;
    private string progressButtonName_Fren;
    private string getRewardButtonName_KR;
    private string getRewardButtonName_EN;
    private string getRewardButtonName_GER;
    private string getRewardButtonName_Fren;
    private string getRewardButtonActiveSprite;
    private string getRewardButtonUnActiveSprite;
    private string atlas;
    private string font;
    private int titleFontSize;
    private int descriptionFontSize;
    private int rewardCountFontSize;
    private int conditionCountFontSize;

    //tmp
    //private SystemLanguage sungjunsLanguage = SystemLanguage.German; 
    


    private void Start()
    {
        GetComponent<UIDragScrollView>().scrollView = transform.parent.parent.GetComponent<UIScrollView>();
    }

    public void Init(int id, string title_KR, string title_EN, string title_GER,
        string title_Fren, string description_KR, string description_EN, string description_GER,
        string description_Fren, int reward, int rewardCount, int conditionType,
        int conditionCount, string rewardICON, string iconAtlas, string progressButtonName_KR,
        string progressButtonName_EN, string progressButtonName_GER, string progressButtonName_Fren,
        string getRewardButtonName_KR, string getRewardButtonName_EN, string getRewardButtonName_GER,
        string getRewardButtonName_Fren, string getRewardButtonActiveSprite, string getRewardButtonUnActiveSprite,
        string atlas, string font, int titleFontSize, int descriptionFontSize,
        int rewardCountFontSize, int conditionCountFontSize)
    {
        
        this.ID = id;
        this.title_KR = title_KR;
        this.title_EN = title_EN;
        this.title_GER = title_GER;
        this.title_Fren = title_Fren;
        this.description_KR = description_KR;
        this.description_EN = description_EN;
        this.description_GER = description_GER;
        this.description_Fren = description_Fren;
        this.rewardType = (EAssetsType)reward;
        this.rewardCount = rewardCount;
        this.conditionType = conditionType;
        this.conditionCount = conditionCount;
        this.rewardICON = rewardICON;
        this.iconAtlas = iconAtlas;
        this.progressButtonName_KR = progressButtonName_KR;
        this.progressButtonName_EN = progressButtonName_EN;
        this.progressButtonName_GER = progressButtonName_GER;
        this.progressButtonName_Fren = progressButtonName_Fren;
        this.getRewardButtonName_KR = getRewardButtonName_KR;
        this.getRewardButtonName_EN = getRewardButtonName_EN;
        this.getRewardButtonName_GER = getRewardButtonName_GER;
        this.getRewardButtonName_Fren = getRewardButtonName_Fren;
        this.getRewardButtonActiveSprite = getRewardButtonActiveSprite;
        this.getRewardButtonUnActiveSprite = getRewardButtonUnActiveSprite;
        this.atlas = atlas;
        this.font = font;
        this.titleFontSize = titleFontSize;
        this.descriptionFontSize = descriptionFontSize;
        this.rewardCountFontSize = rewardCountFontSize;
        this.conditionCountFontSize = conditionCountFontSize;

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Korean:
                titleLabel.text = this.title_KR;
                descriptionLabel.text = this.description_KR;
                break;
            case SystemLanguage.German:
                titleLabel.text = this.title_GER;
                descriptionLabel.text = this.description_GER;
                break;
            case SystemLanguage.French:
                titleLabel.text = this.title_Fren;
                descriptionLabel.text = this.description_Fren;
                break;
            default:
                titleLabel.text = this.title_EN;
                descriptionLabel.text = this.description_EN;
                break;
        }

        rewardCountLabel.text = this.rewardCount.ToString();

        //Debug.Log("Atlas path: " + this.atlas);
        rewardICONSprite.atlas = AtlasManager.instance.GetAtlas(this.iconAtlas);
        rewardICONSprite.material = AtlasManager.instance.GetMaterial(this.iconAtlas);
        rewardICONSprite.spriteName = this.rewardICON;
        rewardICONSprite.color = Color.yellow;

        int userACHCount = Volt_PlayerData.instance.GetACHProgressCount(this.ID);
        userACHCount = Mathf.Min(userACHCount, this.conditionCount);

        float percentage = (float)userACHCount / this.conditionCount;
        conditionCountLabel.text = userACHCount + "/" + this.conditionCount.ToString() + "(" + 
            percentage.ToString("P0") + ")";

        SetRewardButton(userACHCount);

        nguiFont = Resources.Load<NGUIFont>("Font/" + this.font);

        conditionCountLabel.trueTypeFont = nguiFont.dynamicFont;
        conditionCountLabel.fontSize = this.conditionCountFontSize;

        descriptionLabel.trueTypeFont = nguiFont.dynamicFont;
        descriptionLabel.fontSize = this.descriptionFontSize;

        rewardCountLabel.trueTypeFont = nguiFont.dynamicFont;
        rewardCountLabel.fontSize = this.rewardCountFontSize;

        titleLabel.trueTypeFont = nguiFont.dynamicFont;
        titleLabel.fontSize = this.titleFontSize;
    }

    public void SetUserConditionCount(int count, bool isAccomplish)
    {
        if (count < 0)
        {
            //Debug.LogWarning("[SetUserConditionCount] count below 0!!");
            return;
        }
        count = Mathf.Min(count, conditionCount);

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(count.ToString());
        builder.Append("/");
        builder.Append(this.conditionCount.ToString());
        int percentage = Mathf.RoundToInt((float)count / this.conditionCount * 100f);
        builder.Append("(" + percentage + "%)");

        SetRewardButton(count);

        conditionCountLabel.text = builder.ToString();

        if (isAccomplish)
        {
            getRewardButton.enabled = false;
            
        }
    }

    public int GetID()
    {
        return ID;
    }

    public void OnClickGetRewardBtn()
    {
        PacketTransmission.SendAchivementCompletionPacket(ID);
    }

    public void ACHComplete()
    {
        Volt_PlayerData.instance.AchievementProgresses[ID].OnAccomplish();

        //switch (rewardType)
        //{
        //    case EAssetsType.Gold:
        //        Volt_PlayerData.instance.AddGold(rewardCount);
        //        break;
        //    case EAssetsType.Diamond:
        //        Volt_PlayerData.instance.AddDiamond(rewardCount);
        //        break;
        //    case EAssetsType.Battery:
        //        Volt_PlayerData.instance.AddBattery(rewardCount);
        //        break;
        //    default:
        //        Debug.Log("뭐야뭐야 이상이상!");
        //        break;
        //}

        getRewardButtonSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
        getRewardButtonSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
        getRewardButtonSprite.spriteName = this.getRewardButtonActiveSprite;
        getRewardButton.normalSprite = this.getRewardButtonUnActiveSprite;
        getRewardButton.pressedSprite = null;
        getRewardButton.enabled = false;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                getRewardButtonLabel.text = "Done";
                break;
            case SystemLanguage.German:
                getRewardButtonLabel.text = "Fertig";
                break;
            case SystemLanguage.Korean:
                getRewardButtonLabel.text = "완료";
                break;
            default:
                getRewardButtonLabel.text = "Done";
                break;

        }
        Color color = GetComponent<UISprite>().color;
        color.a = 0.5f;
        GetComponent<UISprite>().color = color;
    }
    
    private void SetRewardButton(int userACHCount)
    {
        if (userACHCount >= this.conditionCount)
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Korean:
                    getRewardButtonLabel.text = this.getRewardButtonName_KR;
                    break;
                case SystemLanguage.German:
                    getRewardButtonLabel.text = this.getRewardButtonName_GER;
                    break;
                case SystemLanguage.French:
                    getRewardButtonLabel.text = this.getRewardButtonName_Fren;
                    break;
                default:
                    getRewardButtonLabel.text = this.getRewardButtonName_EN;
                    break;
            }
            if (Volt_PlayerData.instance.IsAccomplishACH(this.ID)) // 보상 수령함!
            {
                //Debug.Log($"{this.ID}이미 보상 수령했네");
                getRewardButtonSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
                getRewardButtonSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
                getRewardButtonSprite.spriteName = this.getRewardButtonActiveSprite;
                getRewardButton.normalSprite = this.getRewardButtonUnActiveSprite;
                getRewardButton.pressedSprite = null;
                getRewardButton.enabled = false;
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.French:
                        getRewardButtonLabel.text = "Done";
                        break;
                    case SystemLanguage.German:
                        getRewardButtonLabel.text = "Fertig";
                        break;
                    case SystemLanguage.Korean:
                        getRewardButtonLabel.text = "완료";
                        break;
                    default:
                        getRewardButtonLabel.text = "Done";
                        break;

                }
                Color color = GetComponent<UISprite>().color;
                color.a = 0.5f;
                GetComponent<UISprite>().color = color;

            }
            else // 보상 수령 안함!
            {
                //Debug.Log($"{this.ID}보상 수령안했네");
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Korean:
                        getRewardButtonLabel.text = this.getRewardButtonName_KR;
                        break;
                    case SystemLanguage.German:
                        getRewardButtonLabel.text = this.getRewardButtonName_GER;
                        break;
                    case SystemLanguage.French:
                        getRewardButtonLabel.text = this.getRewardButtonName_Fren;
                        break;
                    default:
                        getRewardButtonLabel.text = this.getRewardButtonName_EN;
                        break;
                }
                getRewardButtonSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
                getRewardButtonSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
                getRewardButtonSprite.spriteName = this.getRewardButtonActiveSprite;
                getRewardButton.normalSprite = this.getRewardButtonActiveSprite;
                getRewardButton.pressedSprite = null;
                getRewardButton.enabled = true;
            }
        }
        else
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Korean:
                    getRewardButtonLabel.text = this.progressButtonName_KR;
                    break;
                case SystemLanguage.German:
                    getRewardButtonLabel.text = this.progressButtonName_GER;
                    break;
                case SystemLanguage.French:
                    getRewardButtonLabel.text = this.progressButtonName_Fren;
                    break;
                default:
                    getRewardButtonLabel.text = this.progressButtonName_EN;
                    break;
            }
            getRewardButtonSprite.atlas = AtlasManager.instance.GetAtlas(this.atlas);
            getRewardButtonSprite.material = AtlasManager.instance.GetMaterial(this.atlas);
            getRewardButtonSprite.spriteName = this.getRewardButtonUnActiveSprite;
            getRewardButton.normalSprite = this.getRewardButtonUnActiveSprite;
            getRewardButton.pressedSprite = null;
            getRewardButton.enabled = false;
        }
    }
}

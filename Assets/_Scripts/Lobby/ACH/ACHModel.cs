using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public enum RewardType
{
    None = -1,
    Gold = 100,
    Jewelry,
    End
}

public enum ConditionType
{
    None = -1,
    Login = 1000,
    Kill,
    Victory,
    Play,
    BuySkin,
    VCoin,
    AllEnemyKillOneGame,
    PeaceWin,
    NoDeathWin,
    AllEnemyKillWin,
    TaticModule,
    AttackModule,
    MoveModule,
    AllModule,
    AllEnemyAttack,
    AllEnemyKill,
    RapidAttack,
    Bombing,
    Saw,
    Hologram,
    Dodge,
    Teleport,
    Shield,
    Bomb,
    Hacking,
    Timebomb,
    Anchor,
    Emp,
    End
}

public class ACHModel
{
    public int              ID;
    public string           title_KR;
    public string           title_EN;
    public string           title_GER;
    public string           title_Fren;
    public string           description_KR;
    public string           description_EN;
    public string           description_GER;
    public string           description_Fren;
    public EAssetsType      rewardType;
    public int              rewardCount;
    public int              conditionType;
    public int              conditionCount;
    public string           rewardICON;
    public string           iconAtlas;
    public string           progressButtonName_KR;
    public string           progressButtonName_EN;
    public string           progressButtonName_GER;
    public string           progressButtonName_Fren;
    public string           getRewardButtonName_KR;
    public string           getRewardButtonName_EN;
    public string           getRewardButtonName_GER;
    public string           getRewardButtonName_Fren;
    public string           getRewardButtonActiveSprite;
    public string           getRewardButtonUnActiveSprite;
    public string           atlas;
    public string           font;
    public int              titleFontSize;
    public int              descriptionFontSize;
    public int              rewardCountFontSize;
    public int              conditionCountFontSize;

    public ACHModel(int id, string title_KR, string title_EN, string title_GER,
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
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("Param ID: " + this.ID);
        builder.Append("/Title_KR: " + this.title_KR);
        builder.Append("/Description_KR: " + this.description_KR);
        builder.Append("/Title_EN: " + this.title_EN);
        builder.Append("/Description_EN: " + this.description_EN);
        builder.Append("/Title_GER: " + this.title_GER);
        builder.Append("/Description_GER: " + this.description_GER);
        builder.Append("/Title_Fren: " + this.title_Fren);
        builder.Append("/Description_Fren: " + this.description_Fren);
        builder.Append("/RewardType: " + this.rewardType.ToString());
        builder.Append("/RewardCount: " + this.rewardCount.ToString());
        builder.Append("/ConditionType: " + this.conditionType);
        builder.Append("/ConditionCount: " + this.conditionCount.ToString());
        builder.Append("/RewardICON Path: " + this.rewardICON);
        builder.Append("/ICONAtlas: " + this.iconAtlas);
        builder.Append("/GetRewardButtonActiveSpriteName: " + this.getRewardButtonActiveSprite);
        builder.Append("/GetRewardButtonUnActiveSpriteName: " + this.getRewardButtonUnActiveSprite);
        builder.Append("/Atlas: " + this.atlas);
        builder.Append("/Font: " + this.font);
        builder.Append("/TitleFontSize: " + this.titleFontSize);
        builder.Append("/DescriptionFontSize: " + this.descriptionFontSize);
        builder.Append("/RewardCountFontSize: " + this.rewardCountFontSize);
        builder.Append("/ConditionCountFontSize: " + this.conditionCountFontSize);

        return builder.ToString();
    }
}

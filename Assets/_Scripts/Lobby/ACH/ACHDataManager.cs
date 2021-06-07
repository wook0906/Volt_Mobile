using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ACHDataManager : MonoBehaviour
{
    public static ACHDataManager instance;

    [Header("Set In Inspector")]
//    public ACHTable ACHData;

    private Dictionary<int, ACHModel> dailyAchievementTable = new Dictionary<int, ACHModel>();
    private Dictionary<int, ACHModel> permanentAchievementTable = new Dictionary<int, ACHModel>();

    private const string DAILY = "Daily";
    private const string PERMANENT = "Normal";

    private ACHTable ACHData;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        ACHTable_Decrypt decrypt = new ACHTable_Decrypt();
        this.ACHData = decrypt.obj.Clone() as ACHTable;
        decrypt = null;

        Dictionary<int, InfoACHCondition> achConditionInfos = SystemInfoManager.instance.achConditionInfos;
        foreach (ACHTable.Sheet sheet in this.ACHData.sheets)
        {
            Dictionary<int, ACHModel> tempAchievementTalbe = new Dictionary<int, ACHModel>();

            foreach (ACHTable.Param param in sheet.list)
            {
                InfoACHCondition tInfoACHCondition; // 서버의 DB로부터 전달되는 정보
                if (achConditionInfos.TryGetValue(param.ID, out tInfoACHCondition))
                {
                    tempAchievementTalbe.Add(param.ID, new ACHModel(param.ID,
                        param.title_KR, param.title_EN, param.title_GER, param.title_Fren,
                        param.description_KR, param.description_EN, param.description_GER, param.description_Fren, tInfoACHCondition.rewardType,
                        tInfoACHCondition.reward, tInfoACHCondition.conditionType, tInfoACHCondition.condition, param.rewardICON, param.iconAtlas,
                        param.progressButtonName_KR, param.progressButtonName_EN, param.progressButtonName_GER,
                        param.progressButtonName_Fren, param.getRewardButtonName_KR, param.getRewardButtonName_EN,
                        param.getRewardButtonName_GER, param.getRewardButtonName_Fren,
                        param.getRewardButtonActiveSprite, param.getRewardButtonUnActiveSprite,
                        param.atlas, param.font, param.titleFontSize, param.descriptionFontSize,
                        param.rewardCountFontSize, param.conditionCountFontSize));
                    //Debug.Log(tempAchievementTalbe.ToString());
                }
                else
                {
                    //Debug.Log($"Warning Failed to load ach info @ {param.ID}");
                }
            }

            if (sheet.name.CompareTo(DAILY) == 0)
            {
                dailyAchievementTable = tempAchievementTalbe;
            }
            else if (sheet.name.CompareTo(PERMANENT) == 0)
            {
                permanentAchievementTable = tempAchievementTalbe;
            }
        }
    }

    public Dictionary<int, ACHModel> GetACHTable(ACHType type)
    {
        //print("GetACHTable");
        switch (type)
        {
            case ACHType.Daily:
                return dailyAchievementTable;
            case ACHType.Permanent:
                return permanentAchievementTable;
            default:
                return null;
        }
    }
}

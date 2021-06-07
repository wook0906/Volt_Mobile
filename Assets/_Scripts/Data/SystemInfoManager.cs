using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfoManager : MonoBehaviour
{
    public static SystemInfoManager instance;
    public Dictionary<int, InfoShop> shopInfos = new Dictionary<int, InfoShop>();
    public Dictionary<int, InfoACHCondition> achConditionInfos = new Dictionary<int, InfoACHCondition>();

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    
    public void ClearSystemInfo()
    {
        shopInfos.Clear();
        achConditionInfos.Clear();
    }
    public bool InitSystemInfo(List<InfoShop> packageShop, List<InfoShop> batteyShop, List<InfoShop> diamondShop, List<InfoShop> goldShop, List<InfoShop> frameDecoShop, List<InfoShop> robotSkinShop, List<InfoShop> emoticonShop, List<InfoACHCondition> daliy,
        List<InfoACHCondition> normal)
    {
        try
        {
            foreach (var item in packageShop)
            {
                shopInfos.Add(item.ID, new InfoShop(item.ID, item.priceAssetType, item.price, item.count));
                //Debug.Log($"PackageShop Info {item.ToString()}");
            }
            foreach (var item in batteyShop)
            {
                shopInfos.Add(item.ID, new InfoShop(item.ID, item.priceAssetType, item.price, item.count));
                //Debug.Log($"BatteryShop Info {item.ToString()}");
            }
            foreach (var item in diamondShop)
            {
                shopInfos.Add(item.ID, new InfoShop(item.ID, item.priceAssetType, item.price, item.count));

                //Debug.Log($"DiamondShop Info {item.ToString()}");
            }
            foreach (var item in goldShop)
            {
                shopInfos.Add(item.ID, new InfoShop(item.ID, item.priceAssetType, item.price, item.count));

                //Debug.Log($"GoldShop Info {item.ToString()}");
            }
            foreach (var item in frameDecoShop)
            {
                shopInfos.Add(item.ID, new InfoShop(item.ID, item.priceAssetType, item.price,
                    item.count));
                //Debug.Log($"FrameDecoShop Info {item.ToString()}");
            }
            foreach (var item in robotSkinShop)
            {
                shopInfos.Add(item.ID, new InfoShop(item.ID, item.priceAssetType, item.price,
                    item.count));
                //Debug.Log($"RobotSkinShop Info {item.ToString()}");
            }
            foreach (var item in emoticonShop)
            {
                shopInfos.Add(item.ID, new InfoShop(item.ID, item.priceAssetType, item.price,
                    item.count));
                //Debug.Log($"EmoticonShop Info {item.ToString()}");
            }
            foreach (var item in daliy)
            {
                achConditionInfos.Add(item.ID, new InfoACHCondition(item.ID, item.conditionType, item.condition, item.rewardType, item.reward));
                //Debug.Log($"Daily ACH condtionInfo {item.ToString()}");
            }
            foreach (var item in normal)
            {
                achConditionInfos.Add(item.ID, new InfoACHCondition(item.ID, item.conditionType, item.condition, item.rewardType, item.reward));
                //Debug.Log($"Normal ACH ConditionInfo {item.ToString()}");
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogError("Error : " + ex.Message);
            return false;
        }
        return true;
    }
    public bool InitACHInfo(List<InfoACHCondition> daily, List<InfoACHCondition> normal)
    {
        try
        {
            achConditionInfos.Clear();
            foreach (var item in daily)
            {
                achConditionInfos.Add(item.ID, new InfoACHCondition(item.ID, item.conditionType, item.condition, item.rewardType, item.reward));
                //Debug.Log($"Daily ACH condtionInfo {item.ToString()}");
            }
            foreach (var item in normal)
            {
                achConditionInfos.Add(item.ID, new InfoACHCondition(item.ID, item.conditionType, item.condition, item.rewardType, item.reward));
                //Debug.Log($"Normal ACH ConditionInfo {item.ToString()}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error : " + ex.Message);
            return false;
        }
        return true;

    }
    
}

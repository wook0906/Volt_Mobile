using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoadDatas
{
    None = 0,
    InfoPackageShop = 1 << 0,
    InfoBatteryShop = 1 << 1,
    InfoDiamondShop = 1 << 2,
    InfoGoldShop = 1 << 3,
    InfoRobotSkinShop = 1 << 4,
    InfoEmoticonShop = 1 << 5,
    InfoDaliyACHCondition = 1 << 6,
    InfoNormalACHCondition = 1 << 7,
    UserData = 1 << 8,
    UserDaliyACHCondition = 1 << 9,
    UserDaliyACHSuccess = 1 << 10,
    UserNormalACHCondition = 1 << 11,
    UserNormalACHSuccess = 1 << 12,
    UserSkinCondition = 1 << 13,
    UserEmoticonCondition = 1 << 14,
    UserPackageCondition = 1<<15,
    UserBenefitCondition = 1<<16,
    All = InfoPackageShop | InfoBatteryShop | InfoDiamondShop | InfoGoldShop | InfoRobotSkinShop | InfoEmoticonShop | InfoDaliyACHCondition | InfoNormalACHCondition | UserData |
          UserDaliyACHCondition | UserDaliyACHSuccess | UserNormalACHCondition | UserNormalACHSuccess | UserSkinCondition | UserEmoticonCondition | UserPackageCondition | UserBenefitCondition
};

public class DBManager : MonoBehaviour
{
    public static DBManager instance;

    public List<InfoShop> packageShopInfos = new List<InfoShop>();
    public List<InfoShop> goldShopInfos = new List<InfoShop>();
    public List<InfoShop> emoticonShopInfos = new List<InfoShop>();
    public List<InfoShop> batteryShopInfos = new List<InfoShop>();
    public List<InfoShop> diamondShopInfos = new List<InfoShop>();
    public List<InfoShop> robotSkinShopInfos = new List<InfoShop>();
    public List<InfoShop> frameDecorationShopInfos = new List<InfoShop>();

    public List<InfoACHCondition> daliyACHConditionInfos = new List<InfoACHCondition>();
    public List<InfoACHCondition> normalACHConditionInfos = new List<InfoACHCondition>();

    public UserData userData;
    public UserExtraData userExtraData;
    public Dictionary<int, bool> userSkinCondition = new Dictionary<int, bool>();
    public Dictionary<int, bool> userEmoticonCondition = new Dictionary<int, bool>();
    public Dictionary<int, bool> userBenefitCondition = new Dictionary<int, bool>();
    public Dictionary<int, Define.PackageProductState> userPackageCondition = new Dictionary<int, Define.PackageProductState>();
    

    private LoadDatas loadDatas = LoadDatas.None;
    public LoadDatas pLoadDatas
    {
        get { return loadDatas; }
        set
        {
            loadDatas = value;
        }

    }

    public float Progress
    {
        get
        {
            if (loadDatas == LoadDatas.All)
                return 1.0f;

            string[] strs = typeof(LoadDatas).GetEnumNames();
            int max = strs.Length - 2; // None과 All은 제외

            int count = 0;
            for (int i = 1; i <= max; ++i)
            {
                if ((loadDatas & (LoadDatas)Enum.Parse(typeof(LoadDatas), strs[i])) == 0)
                    continue;

                count++;
            }

            return (float)count / max;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        //print("loadDatas : "+loadDatas.ToString());
    }
    public void ClearDB()
    {
        emoticonShopInfos.Clear();
        packageShopInfos.Clear();
        goldShopInfos.Clear();
        batteryShopInfos.Clear();
        diamondShopInfos.Clear();
        robotSkinShopInfos.Clear();
        frameDecorationShopInfos.Clear();
        daliyACHConditionInfos.Clear();
        normalACHConditionInfos.Clear();
        userSkinCondition.Clear();
        userEmoticonCondition.Clear();
        userPackageCondition.Clear();
        userBenefitCondition.Clear();
    }
    private IEnumerator Start()
    {
        //PacketTransmission.SendSignInPacket("aaa");
        yield return new WaitUntil(() => { return pLoadDatas == LoadDatas.All; });

        //Debug.Log("Loaded All Data");
        packageShopInfos.Sort(delegate(InfoShop x, InfoShop y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });

        batteryShopInfos.Sort(delegate (InfoShop x, InfoShop y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });

        foreach (var item in batteryShopInfos)
        {
            //Debug.Log($"Battery:{item.ToString()}");
        }

        diamondShopInfos.Sort(delegate (InfoShop x, InfoShop y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });

        foreach (var item in diamondShopInfos)
        {
            Debug.Log($"Diamond:{item.ToString()}");
        }

        goldShopInfos.Sort(delegate (InfoShop x, InfoShop y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });

        foreach (var item in goldShopInfos)
        {
            //Debug.Log($"Gold:{item.ToString()}");
        }

        frameDecorationShopInfos.Sort(delegate (InfoShop x, InfoShop y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });
        robotSkinShopInfos.Sort(delegate (InfoShop x, InfoShop y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });

        emoticonShopInfos.Sort(delegate (InfoShop x, InfoShop y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });

        daliyACHConditionInfos.Sort(delegate (InfoACHCondition x, InfoACHCondition y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });
        normalACHConditionInfos.Sort(delegate (InfoACHCondition x, InfoACHCondition y)
        {
            if (x == null && y == null) return 0;
            return x.ID.CompareTo(y.ID);
        });

        //Debug.LogError("됐냐?");

        Volt_PlayerData.instance.Init(userData.nickname, userData.battery, userData.diamond, userData.gold, userSkinCondition, userEmoticonCondition, userPackageCondition, userBenefitCondition);

        if(!SystemInfoManager.instance.InitSystemInfo(packageShopInfos, batteryShopInfos, diamondShopInfos, goldShopInfos, frameDecorationShopInfos, robotSkinShopInfos, emoticonShopInfos, daliyACHConditionInfos, normalACHConditionInfos))
        {
            Debug.LogError("Error Falied to System information");
        }

    }


    #region 데이터 로드 콜백 함수들

    public void OnLoadedPackageShopInfo()
    {
        pLoadDatas |= LoadDatas.InfoPackageShop;
    }

    public void OnLoadedGoldShopInfo()
    {
        pLoadDatas |= LoadDatas.InfoGoldShop;
    }

    public void OnLoadedEmoticonShopInfo()
    {
        pLoadDatas |= LoadDatas.InfoEmoticonShop;
    }

    public void OnLoadedBatteryShopInfo()
    {
        //Debug.Log("Loaded battery shop information");
        pLoadDatas |= LoadDatas.InfoBatteryShop;
    }

    public void OnLoadedDiamondShopInfo()
    {
        //Debug.Log("Loaded diamond shop information");
        pLoadDatas |= LoadDatas.InfoDiamondShop;
    }

    public void OnLoadedRobotSkinShopInfo()
    {
        //Debug.Log("Loaded robot skin shop info");
        pLoadDatas |= LoadDatas.InfoRobotSkinShop;
    }

    public void OnLoadedDaliyAchievementInfo()
    {
        //Debug.Log("Loaded daliy achievement information");
        pLoadDatas |= LoadDatas.InfoDaliyACHCondition;
    }

    public void OnLoadedNormalAchievementInfo()
    {
        //Debug.Log("Loaded normal achievement information");
        pLoadDatas |= LoadDatas.InfoNormalACHCondition;
    }
    public void OnLoadedUserData()
    {
        //Debug.Log("Loaded user data");
        pLoadDatas |= LoadDatas.UserData;
    }

    public void OnLoadedUserDaliyACHCondition()
    {
        //Debug.Log("Loaded user daliy ach condition");
        pLoadDatas |= LoadDatas.UserDaliyACHCondition;
    }

    public void OnLoadedUserDaliyACHSuccess()
    {
        //Debug.Log("Loaded user daliy ach success");
        pLoadDatas |= LoadDatas.UserDaliyACHSuccess;
    }

    public void OnLoadedUserNormalACHCondition()
    {
        //Debug.Log("Loaded user normal ach condtion");
        pLoadDatas |= LoadDatas.UserNormalACHCondition;
    }
    public void OnLoadedUserNormalACHSuccess()
    {
        //Debug.Log("Loaded user normal ach success");
        pLoadDatas |= LoadDatas.UserNormalACHSuccess;
    }
    public void OnLoadedUserSkinCondition()
    {
        //Debug.Log("Loaded user skin condition");
        pLoadDatas |= LoadDatas.UserSkinCondition;
    }
    public void OnLoadedUserEmoticonCondition()
    { 
        //Debug.Log("Loaded user Emoticon condition");
        pLoadDatas |= LoadDatas.UserEmoticonCondition;
    }
    public void OnLoadedUserPackageCondition()
    {
        pLoadDatas |= LoadDatas.UserPackageCondition;
    }
    public void OnLoadedUserBenefitCondition()
    {
        pLoadDatas |= LoadDatas.UserBenefitCondition;
    }
   
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACHProgress
{
    
    private bool isAccomplishACH;
    public  bool IsAccomplishACH { get { return isAccomplishACH; } }
    private int achievementProgressCount;
    public  int ACHProgressCount { get { return achievementProgressCount; } }

    public void OnAccomplish()
    {
        isAccomplishACH = true;
    }

    public void AddAccomplishmentProgress(int count)
    {
        if (count < 0)
        {
            //Debug.LogWarning("Warning!! progress count below 0!! : " + count);
            return;
        }
        this.achievementProgressCount += count;
    }

    public void SetAchievementProgress(int count)
    {
        if (count < 0)
        {
            //Debug.LogWarning("Warning!! progress count below 0!! : " + count);
            return;
        }
        this.achievementProgressCount = count;
    }
}

public class Volt_PlayerData : MonoBehaviour
{
    public static Volt_PlayerData instance;

    private LoginType loginType = LoginType.None;
    public LoginType LoginType
    {
        get { return loginType; }
        set
        {
            loginType = value;
            PlayerPrefs.SetInt("Volt_LoginType", (int)loginType);
        }
    }

    public System.DateTime lastDateTime;
    [SerializeField]
    private string userToken;
    public string UserToken
    {
        get { return userToken; }
        set { userToken = value; }
    }
    private string nickName = string.Empty;
    public string NickName { get { return nickName; } set { nickName = value; } }

    public bool NeedReConnection { get; set; }

    private static int maxBetteryCount = 5;
    public  int MaxBetteryCount { get { return maxBetteryCount; } }
    private static int maxAdViewCount = 5;
    public int MaxAdViewCount { get { return maxAdViewCount; } }

    [SerializeField]
    private int batteryCount = 0;
    public int BatteryCount
    {
        get { return Volt_SafeInt.UnpackValue(batteryCount); }
        set { batteryCount = Volt_SafeInt.SetValue(value); }
    }
    [SerializeField]
    private int goldCount = 0;
    public int GoldCount
    {
        get { return Volt_SafeInt.UnpackValue(goldCount); }
        set { goldCount = Volt_SafeInt.SetValue(value); }
    }
    [SerializeField]
    private int diamondCount = 0;
    public int DiamondCount
    {
        get { return Volt_SafeInt.UnpackValue(diamondCount); }
        set { diamondCount = Volt_SafeInt.SetValue(value); }
    }
    [SerializeField]
    private int playCount = 0;
    public int PlayCount
    {
        get { return playCount; }
        set { playCount = value; }
    }
    [SerializeField]
    private int victoryCount = 0;
    public int VictoryCount
    {
        get { return victoryCount; }
        set { victoryCount = value; }
    }

    public float WinRate
    {
        get
        {
            float rate = (float)VictoryCount / PlayCount;
            if (rate < 0.001)
                return 0f;
            if (rate > 1.0f)
                return 1.0f;

            return rate;
        }
    }
    [SerializeField]
    private int killCount = 0;
    public int KillCount
    {
        get { return killCount; }
        set { killCount = value; }
    }
    [SerializeField]
    private int coinCount = 0;
    public int CoinCount
    {
        get { return coinCount; }
        set
        {
            coinCount = value;
        }
    }
    [SerializeField]
    private int deathCount = 0;
    public int DeathCount
    {
        get { return deathCount; }
        set { deathCount = value; }
    }
    [SerializeField]
    private int attackTryCount = 0;
    public int AttackTryCount
    {
        get { return attackTryCount; }
        set { attackTryCount = value; }
    }
    [SerializeField]
    private int attackSuccessCount = 0;
    public int AttackSuccessCount
    {
        get { return attackSuccessCount; }
        set { attackSuccessCount = value; }
    }

    public float AttackSuccessRate
    {
        get
        {
            float rate = (float)AttackSuccessCount / AttackTryCount;
            if (rate < 0.001f)
                return 0f;
            if (rate > 1.0f)
                return 1.0f;

            return rate;
        }
    }

    public Dictionary<int, ACHProgress> AchievementProgresses = new Dictionary<int, ACHProgress>();
    private Dictionary<int, bool> robotSkins = new Dictionary<int, bool>();
    private Dictionary<int, Define.PackageProductState> packages = new Dictionary<int, Define.PackageProductState>();
    private Dictionary<int, bool> emoticons = new Dictionary<int, bool>();
    private Dictionary<int, bool> frameDecorations = new Dictionary<int, bool>();
    private Dictionary<int, string> emoticonSet = new Dictionary<int, string>();
    public Dictionary<RobotType, RobotSkin> selectdRobotSkins = new Dictionary<RobotType, RobotSkin>();
    public Dictionary<int, bool> Benefits = new Dictionary<int, bool>();
    private int remainAdCnt = 0;
    public int RemainAdCnt
    {
        get { return remainAdCnt; }
        set { remainAdCnt = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(this.gameObject);
    }

    public void Clear()
    {
        LoginType = LoginType.None;
        lastDateTime = new DateTime();
        UserToken = string.Empty;
        BatteryCount = 0;
        GoldCount = 0;
        DiamondCount = 0;
        PlayCount = 0;
        VictoryCount = 0;
        KillCount = 0;
        CoinCount = 0;
        DeathCount = 0;
        AttackTryCount = 0;
        AttackSuccessCount = 0;
        RemainAdCnt = 0;
        AchievementProgresses.Clear();
        robotSkins.Clear();
        packages.Clear();
        emoticons.Clear();
        frameDecorations.Clear();
        emoticonSet.Clear();
        selectdRobotSkins.Clear();
        Benefits.Clear();
    }
    public void LoadUserSkinData()
    {
        for (int i = 0; i < (int)RobotType.Max; ++i)
        {
            RobotType type = (RobotType)i;
            string key = $"{type}_skin";
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, 0);
            }
            int skinIndex = PlayerPrefs.GetInt(key);
            RobotSkinObject skinData = Managers.Data.SkinDatas[type];//Resources.Load<RobotSkinObject>($"SkinDatas/{skinDataName}");
            selectdRobotSkins.Add(type, skinData.RobotSkins[skinIndex]);
        }
    }
    public void EmoticonSetInit()
    {
        for (int i = 0; i < 6; i++)
        {
            if (PlayerPrefs.HasKey("Volt_EmoticonSetupSlot" + i.ToString()))
            {
                emoticonSet.Add(i, PlayerPrefs.GetString("Volt_EmoticonSetupSlot" + i.ToString()));
            }
            else
            {
                emoticonSet.Add(i, "EmoticonNone");
            }
        }
    }
    public void SkinDataInit()
    {
        selectdRobotSkins.Clear();
        for (int i = 0; i < (int)RobotType.Max; ++i)
        {
            RobotType type = (RobotType)i;
            string key = $"{type}_skin";
            PlayerPrefs.SetInt(key, 0);
            int skinIndex = PlayerPrefs.GetInt(key);
            string skinDataName = $"{type}_SkinData";
            RobotSkinObject skinData = Resources.Load<RobotSkinObject>($"SkinDatas/{skinDataName}");
            selectdRobotSkins.Add((RobotType)i, skinData.RobotSkins[skinIndex]);
        }
    }

    public void Init(string nickName, int betteryCount,
        int diamondCount, int goldCount, Dictionary<int, bool> userSkinCondition,
        Dictionary<int, bool> userEmoticonCondition, Dictionary<int, Define.PackageProductState> userPackageCondition,
        Dictionary<int,bool> userBenefitCondition)
    {
        this.nickName = nickName;
        this.BatteryCount = betteryCount;
        this.DiamondCount = diamondCount;
        this.GoldCount = goldCount;

        foreach (var item in AchievementProgresses)
        {
            //Debug.Log($"ACH ID:{item.Key}, ACH Count:{item.Value.ACHProgressCount}, ACH Accomplish:{item.Value.IsAccomplishACH}");
        }
        robotSkins = userSkinCondition;
        emoticons = userEmoticonCondition;
        packages = userPackageCondition;
        Benefits = userBenefitCondition;


        //Debug.Log("PlayerData Init Complete");
        //Debug.Log(this.ToString());


        //tmp
        EmoticonSetInit();
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("Nickname: " + nickName);
        builder.Append(" /BatteryCount: " + BatteryCount);
        builder.Append(" /DiamondCount: " + DiamondCount);
        builder.Append(" /GoldCount: " + GoldCount);
        foreach (var item in AchievementProgresses)
        {
            builder.Append(" /ID: " + item.Key + ", Count: " + item.Value.ACHProgressCount + ", isAccomplish: " + item.Value.IsAccomplishACH);
        }

        return builder.ToString();
    }

    //public void Init(string nickName, int betteryCount,
    //    int diamondCount, int goldCount, Dictionary<int, ACHProgress> achievementProgresses,
    //    Dictionary<int, bool> robotSkins, Dictionary<int, bool> frameDecorations)
    //{
    //    this.nickName = nickName;
    //    this.batteryCount = betteryCount;
    //    this.diamondCount = diamondCount;
    //    this.goldCount = goldCount;
    //    this.AchievementProgresses = achievementProgresses;
    //    this.robotSkins = robotSkins;
    //    this.frameDecorations = frameDecorations;
    //}

    //TODO: 모든 함수의 끝에 DB에 저장하는 코드들이 필요하다.
    public void OnPlayGame()
    {
        //Debug.Log("OnPlayeGame");
        --BatteryCount;
        PacketTransmission.SendBatteryPacket(-1);
        if (BatteryCount < BatteryCharge.maxBatteryCount)
        {
            BatteryCharge.RegisterTime();
            //DateTime curTime = Volt.Time.GetGoogleDateTime();
            //PacketTransmission.SendBatteryTimeRegisterPacket(curTime.Year, curTime.Month, curTime.Day,
            //curTime.Hour, curTime.Minute, curTime.Second);
            //TODO: 서버로 현재시간 패킷 보내기!!
        }
        //TODO: 게임 플레이 횟수와 배터리 개수를 DB에 저장
    }
   
    public void AddBattery(int count)
    {
        if (count < 0)
        {
            //Debug.LogWarning("Can not add bettery cause count below 0!! ");
            return;
        }
        
        if(BatteryCharge.instance != null)
        {
            BatteryCharge.instance.Timer.Reset();
        }
        //Debug.Log($"Add Battey count @ {count}");
        //TODO: 현재 배터리 개수 DB에 저장
        BatteryCount += count;
        PacketTransmission.SendBatteryPacket(count);
    }

    public void OnChargedBattery()
    {
        AddBattery(1);
    }
    public void OnChargedAd()
    {
        //광고버튼을 클릭가능한 상태로 변경;
    }
    public void OnDisabledAd()
    {
        //광고버튼을 클릭 불가능한 상태로 변경;
    }

    public int GetBatteryCount()
    {
        return BatteryCount;
    }


    public void AddDiamond(int count)
    {
        if(count < 0)
        {
            //Debug.LogWarning("Can not add diamond cause count below 0!! ");
            return;
        }
        DiamondCount += count;
        LobbyScene_AssetUI assetUI = FindObjectOfType<LobbyScene_AssetUI>();
        if (!assetUI)
        {
            Debug.LogError("Not Find asset UI");
            return;
        }
        assetUI.SetDiamondCountLabel(DiamondCount);
    }

    public void UsedDiamond(int count)
    {
        DiamondCount = Mathf.Max(0, DiamondCount - count);
        LobbyScene_AssetUI assetUI = FindObjectOfType<LobbyScene_AssetUI>();
        if (!assetUI)
        {
            Debug.LogError("Not Find asset UI");
            return;
        }
        assetUI.SetDiamondCountLabel(DiamondCount);
    }

    public int GetDiamondCount()
    {
        return DiamondCount;
        
    }

    public void AddGold(int count)
    {
        if(count < 0)
        {
            //Debug.LogWarning("Can not add gold cause count below 0!! ");
            return;
        }
        GoldCount += count;

        LobbyScene_AssetUI assetUI = FindObjectOfType<LobbyScene_AssetUI>();
        if (!assetUI)
        {
            Debug.LogError("Not Find asset UI");
            return;
        }
        assetUI.SetGoldCountLabel(GoldCount);
    }

    public void UsedGold(int count)
    {
        GoldCount = Mathf.Max(0, GoldCount - count);

        LobbyScene_AssetUI assetUI = FindObjectOfType<LobbyScene_AssetUI>();
        if (!assetUI)
        {
            Debug.LogError("Not Find asset UI");
            return;
        }
        assetUI.SetGoldCountLabel(GoldCount);
    }

    public void OnAccomplishACH(int achievementID)
    {
        if (AchievementProgresses.Count == 0 || !AchievementProgresses.ContainsKey(achievementID))
        {
            //Debug.LogWarning("Warning!! ACH dictionary doesn't have any items or doesn't contain key!! \ncount: " + AchievementProgresses.Count
                //+ ", key:" + achievementID);
            return;
        }
        AchievementProgresses[achievementID].OnAccomplish();
    }

    public bool IsAccomplishACH(int achievementID)
    {
        if (AchievementProgresses.Count == 0 || !AchievementProgresses.ContainsKey(achievementID))
        {
            //Debug.LogWarning("Warning!! ACH dictionary doesn't have any items or doesn't contain key!! \ncount: " + AchievementProgresses.Count
                //+ ", key:" + achievementID);
            return false;
        }
        return AchievementProgresses[achievementID].IsAccomplishACH;
    }

    public void AddACHProgressCount(int achievementID, int count)
    {
        if (!AchievementProgresses.ContainsKey(achievementID))
        {
            //Debug.LogWarning("Warning!! ACH dictionary doesn't contain key!! \nkey:" + achievementID);
            return;
        }
        if(count < 0)
        {
            //Debug.LogWarning("Warning!! count below 0!! : " + count);
            return;
        }
        AchievementProgresses[achievementID].AddAccomplishmentProgress(count);
    }

    public int GetACHProgressCount(int achievementID)
    {
        if(AchievementProgresses.Count == 0 || !AchievementProgresses.ContainsKey(achievementID))
        {
            //Debug.LogWarning("Warning!! ACH dictionary doesn't have any items or doesn't contain key!! \ncount: " + AchievementProgresses.Count
                //+ ", key:" + achievementID);
            return -1;
        }
        return (int)AchievementProgresses[achievementID].ACHProgressCount;
    }

    public bool IsHavePackage(int packageID)
    {
        if (packageID == -1)
            return true;

        if (!packages.ContainsKey(packageID))
        {
            //Debug.LogWarning("Warning!! package dictionary doesn't contain key!! : " + packageID);
            return false;
        }
        return packages[packageID].isPurchased;
    }

    public void OnPurchasedPackage(int packageID)
    {
        if (!packages.ContainsKey(packageID))
        {
            //Debug.LogWarning("Warning!! package dictionary doesn't contain key!! : " + packageID);
            return;
        }
        packages[packageID].isPurchased = true;
    }
    public bool IsGetBenefit(int packageID)
    {
        if (packageID == -1)
            return false;
        else
        {
            return Benefits[packageID];
        }
    }
    public void OnGetBenefit(int packageID)
    {
        if (!Benefits.ContainsKey(packageID))
            return;
        Benefits[packageID] = true;
    }

    public void OnPurchasedSkin(int skinID)
    {
        if(!robotSkins.ContainsKey(skinID))
        {
            //Debug.LogWarning("Warning!! skin dictionary doesn't contain key!! : " + skinID);
            return;
        }
        robotSkins[skinID] = true;
    }

    public bool IsHaveSkin(int skinID)
    {
        if (skinID == -1)
            return true;

        if (!robotSkins.ContainsKey(skinID))
        {
            //Debug.LogWarning("Warning!! skin dictionary doesn't contain key!! : " + skinID);
            return false;
        }
        return robotSkins[skinID];
    }

    public void OnPurchasedEmoticon(int emoticonID)
    {
        if (!emoticons.ContainsKey(emoticonID))
        {
            //Debug.LogWarning("Warning!! emoticon dictionary doesn't contain key!! : " + emoticonID);
            return;
        }
        emoticons[emoticonID] = true;
    }

    public bool IsHaveEmoticon(int emoticonID)
    {
        if (emoticonID == -1)
            return true;

        if (!emoticons.ContainsKey(emoticonID))
        {
            //Debug.LogWarning("Warning!! emoticon dictionary doesn't contain key!! : " + emoticonID);
            return false;
        }
        return emoticons[emoticonID];
    }

    public void OnObtainFrameDecoration(int frameDecoID)
    {
        if(!frameDecorations.ContainsKey(frameDecoID))
        {
            //Debug.LogWarning("Warning!! frame decoration dictionary doesn't contain key!! : " + frameDecoID);
            return;
        }
        frameDecorations[frameDecoID] = true;
    }

    public bool IsHaveFrameDeco(int frameDecoID)
    {
        if (!frameDecorations.ContainsKey(frameDecoID))
        {
            //Debug.LogWarning("Warning!! frame decoration dictionary doesn't contain key!! : " + frameDecoID);
            return false;
        }
        return frameDecorations[frameDecoID];
    }

    public void RenewGoldText()
    {
        RenewGoldText(this.GoldCount);
    }
    public void RenewGoldText(int count)
    {
        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Lobby)
        {
            LobbyScene_AssetUI assetUI = FindObjectOfType<LobbyScene_AssetUI>();
            if (!assetUI)
            {
                Debug.LogError("Not Find asset UI");
                return;
            }
            assetUI.SetGoldCountLabel(count);
            //LobbySceneUIManager.S.SetGoldCountLabel(count);
        }
        else if(Managers.Scene.CurrentScene.SceneType == Define.Scene.Shop)
        {
            ShopScene_UI scene_UI = Managers.UI.GetSceneUI<ShopScene_UI>();
            if (scene_UI == null)
            {
                Debug.LogError("Null Reference ShopScene_UI");
                return;
            }
            scene_UI.SetGoldCountLabel(count);
            //Volt_ShopUIManager.S.SetGoldCountLabel(count);
        }
    }
    public void RenewBatteryText(int count)
    {
        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Lobby)
        {
            LobbyScene_AssetUI assetUI = FindObjectOfType<LobbyScene_AssetUI>();
            if (!assetUI)
            {
                Debug.LogError("Not Find asset UI");
                return;
            }
            assetUI.SetBatteryCountLabel(count);
            //LobbySceneUIManager.S.SetGoldCountLabel(count);
        }
        else if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Shop)
        {
            ShopScene_UI scene_UI = Managers.UI.GetSceneUI<ShopScene_UI>();
            if (scene_UI == null)
            {
                Debug.LogError("Null Reference ShopScene_UI");
                return;
            }
            scene_UI.SetBatteryCountLabel(count);
            //Volt_ShopUIManager.S.SetGoldCountLabel(count);
        }
    }
    public void RenewDiamondText(int count)
    {
        if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Lobby)
        {
            LobbyScene_AssetUI assetUI = FindObjectOfType<LobbyScene_AssetUI>();
            if (!assetUI)
            {
                Debug.LogError("Not Find asset UI");
                return;
            }
            assetUI.SetDiamondCountLabel(count);
            //LobbySceneUIManager.S.SetGoldCountLabel(count);
        }
        else if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Shop)
        {
            ShopScene_UI scene_UI = Managers.UI.GetSceneUI<ShopScene_UI>();
            if (scene_UI == null)
            {
                Debug.LogError("Null Reference ShopScene_UI");
                return;
            }
            scene_UI.SetDiamondCountLabel(count);
            //Volt_ShopUIManager.S.SetGoldCountLabel(count);
        }
    }
    public void RenewPackageData(int itemID)
    {
        packages[itemID].isPurchased = true;
    }
    public void RenewRobotSkinData(int itemID)
    {
        PacketTransmission.SendAchievementProgressPacket(2000011, 0, false);
        robotSkins[itemID] = true;
    }
    public void RenewEmoticonData(int emoticonItemID)
    {
        emoticons[emoticonItemID] = true;
    }
    public bool SetEmoticon(int slotNumber, string newEmoticon)
    {
        foreach (var item in emoticonSet.Values)
        {
            if (newEmoticon.Equals("EmoticonNone")) continue;
            if (item.Equals(newEmoticon))
                return false;
        }

        if (emoticonSet.ContainsKey(slotNumber))
        {
            emoticonSet.Remove(slotNumber);
        }
        PlayerPrefs.SetString("Volt_EmoticonSetupSlot" + slotNumber.ToString(), newEmoticon);
        emoticonSet.Add(slotNumber, newEmoticon);
        return true;
    }
    public Dictionary<int,string> GetEmoticonSet()
    {
        return emoticonSet;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Attack,
    Throw,
    Push,
    EMP,
    Teleport,
    DoubleAttack
}

public enum EffectLaunchPoint
{
    LaunchPoint,
    LaunchPoint_L,
    LaunchPoint_R,
    Win_LaunchPoint,
    Reaper_SawEffectPoint,
    Volt_DoubleAttack_LaunchPoint,
    HoundMuzzle_0,
    HoundMuzzle_1,
    HoundMuzzle_2,
    HoundMuzzle_3,
    Run_Smoke
}

public enum WhatIsTarget { Bot, Coin, All }

public enum PushType
{
    PushedCandidate, // 밀릴 수 있는 후보군들
    Pushed, // Pusher 혹은 Pushed에게 밀린 candidate들은 pushed가 된다.
    Pusher,  // 자신의 턴에 이동을 선택한 플레이어는 Pusher가 된다.
    Immune // 더 이상 그 누구에게도 밀리지 않는 로봇 Pushed 상태에서 누군가를 밀거나 혹은 밀리거나 PushedCandidate 상태에서 앵커를 사용하면 이 상태로 전이된다. Pusher는 Immnue 상태의 로봇에게 밀릴경우 이 상태로 전이된다.
}

public class CollisionData
{
    public GameObject robot = null;
    public PushType pushType = PushType.PushedCandidate;
    public bool isHaveAnchor = false;
    public bool isHaveSawblade = false;
    public int behaviorPoints = 0;
    public Volt_Tile destTile = null;
}

public class Volt_Utils
{
    private const float offset = 10; // 각도 오차 범윈
    public static Dictionary<RobotType, string[]> killbotNameCandidates = new Dictionary<RobotType, string[]>()
    {
        {RobotType.Volt, new string[]{ "Kojima","Han","Blackbull","Masato","Oya","Tony","Minsu" } },
        {RobotType.Mercury, new string[]{ "KingPizza","BusMetroWork","Don","BigbosS", "Ratchet", "Dexter" } },
        {RobotType.Hound, new string[]{ "Max", "Cooper", "Buddy", "Jack", "Sindy", "Bella", "Coco" } },
        {RobotType.Reaper, new string[]{ "MasterClass", "Destroyer", "Bongoo", "Champ", "BrushCutter", "Hoon", "Sonny" } },
    };

    public static bool IsForward(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, Vector3.forward);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsBackward(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, Vector3.back);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsRight(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, Vector3.right);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsLeft(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, Vector3.left);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsForwardRight(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, (Vector3.forward + Vector3.right).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsForwardLeft(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, (Vector3.forward + Vector3.left).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsBackRight(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, (Vector3.back + Vector3.right).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsBackLeft(Transform transform)
    {
        float angle = Vector3.Angle(transform.forward, (Vector3.back + Vector3.left).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsForward(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, Vector3.forward);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsBackward(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, Vector3.back);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsRight(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, Vector3.right);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsLeft(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, Vector3.left);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsForwardRight(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, (Vector3.forward + Vector3.right).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsForwardLeft(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, (Vector3.forward + Vector3.left).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsBackRight(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, (Vector3.back + Vector3.right).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    public static bool IsBackLeft(Vector3 dir)
    {
        float angle = Vector3.Angle(dir, (Vector3.back + Vector3.left).normalized);

        if (angle >= 0 && angle < offset)
            return true;
        return false;
    }

    static string unitString = "abcdefghijklmnopqrstuvwxyz";
    public static string GetUnitString(int unit)
    {
        if (unit == -1)
            return string.Empty;

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(unitString[unit]);
        return builder.ToString();

    }
    public static string GetRandomKillbotName(RobotType robotType)
    {
        return killbotNameCandidates[robotType][Random.Range(0, killbotNameCandidates[robotType].Length)];
    }


    public static EShopPurchase GetEShopPurchaseWithId(int id)
    {
        if(id > 3000000 && id <= 4000000)
        {
            return EShopPurchase.Battery;
        }
        else if(id > 4000000 && id <= 5000000)
        {
            return EShopPurchase.Diamond;
        }
        else if(id > 5000000 && id <= 6000000)
        {
            return EShopPurchase.Skin;
        }
        else if (id > 7000000 && id <= 8000000)
        {
            return EShopPurchase.Gold;
        }
        else if(id > 8000000 && id <= 9000000)
        {
            return EShopPurchase.Package;
        }
        else if (id > 9000000)
        {
            return EShopPurchase.Emoticon;
        }

        Debug.Log("EShopPurchase Type Error!");
        return EShopPurchase.EShopPurchase;
    }
    public static string GetItemNameByLanguage(EShopPurchase purchaseItemType)
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                switch (purchaseItemType)
                {
                    case EShopPurchase.Emoticon:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Skin:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Package:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Gold:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Diamond:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Battery:
                        return purchaseItemType.ToString();
                    default:
                        return "";
                }
            case SystemLanguage.German:
                switch (purchaseItemType)
                {
                    case EShopPurchase.Emoticon:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Skin:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Package:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Gold:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Diamond:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Battery:
                        return purchaseItemType.ToString();

                    default:
                        return "";
                }
            case SystemLanguage.Korean:
                switch (purchaseItemType)
                {
                    case EShopPurchase.Emoticon:
                        return "감정 표현";
                    case EShopPurchase.Skin:
                        return "스킨";
                    case EShopPurchase.Package:
                        return "패키지";
                    case EShopPurchase.Gold:
                        return "골드";
                    case EShopPurchase.Diamond:
                        return "다이아몬드";
                    case EShopPurchase.Battery:
                        return "배터리";

                    default:
                        return "";
                }
            default:
                switch (purchaseItemType)
                {
                    case EShopPurchase.Emoticon:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Skin:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Package:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Gold:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Diamond:
                        return purchaseItemType.ToString();
                    case EShopPurchase.Battery:
                        return purchaseItemType.ToString();

                    default:
                        return "";
                }
        }
    }

    public static string GetGoldCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000) / 100000;
        return count.ToString() + Volt_Utils.GetUnitString(unit);
    }

    public static string GetDiamondCountLabel(float count)
    {
        int unit = -1;
        while (count >= 100000f)
        {
            count /= 100000f;
            unit++;
        }
        count = (float)System.Math.Truncate(count * 100000) / 100000;
        return count.ToString() + Volt_Utils.GetUnitString(unit);
    }
}
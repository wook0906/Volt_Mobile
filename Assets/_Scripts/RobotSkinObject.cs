using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RobotType
{
    Volt,
    Mercury,
    Hound,
    Reaper,
    Max
}
public enum SkinType
{
    Origin,
    Demon,
    Military,
    Neon,
    Old,
    OriginBlue,
    OriginRed,
    Christmas,
    Max
}

[System.Serializable]
public class RobotSkin
{
    public RobotType RobotType;
    public SkinType SkinType;
    public int skinID;
    public string skinName_KR;
    public string skinName_EN;
    public string skinName_GER;
    public string skinName_Fren;
    public GameObject skinPrefab;

    public static bool operator==(RobotSkin r1, RobotSkin r2)
    {
        if ((r1.skinID == r2.skinID) &&
            (r1.RobotType == r2.RobotType))
            return true;
        else return false;
    }

    public static bool operator!=(RobotSkin r1, RobotSkin r2)
    {
        if (r1 == r2)
            return false;
        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj is RobotSkin skin)
            return this == skin;
        return false;
    }
}

[CreateAssetMenu(menuName = "Skins/SkinData")]
public class RobotSkinObject : ScriptableObject
{
    public RobotType robotType;
    public List<RobotSkin> RobotSkins;

    public GameObject GetPrefab(SkinType skinType)
    {
        foreach (var item in RobotSkins)
        {
            if (item.SkinType == skinType)
                return item.skinPrefab;
        }
        //Debug.LogError($"Wrong skin type {skinType}");
        return null;
    }
}

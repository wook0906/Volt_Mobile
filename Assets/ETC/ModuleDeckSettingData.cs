using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSetting/ModuleDeckSetting")]
public class ModuleDeckSettingData : ScriptableObject
{
    public MapType mapType;

    [Header("해당 맵에서의 모듈 타입별 등장 빈도")]
    public int movementTypePercentage;
    public int attackTypePercentage;
    public int tacticTypePercentage;

    [Header("공격모듈")]
    public int CROSSFIRE;
    public int GRENADES;
    public int PERNERATE;
    public int POWERBEAM;
    public int SAWBLADE;
    public int SHOCKWAVE;
    public int TIMEBOMB;
    public int DOUBLEATTACK;

    [Header("이동모듈")]
    public int REPULSIONBLAST;
    public int STEERINGNOZZLE;
    public int TELEPORT;
    public int DODGE;

    [Header("전략모듈")]
    public int ANCHOR;
    public int BOMB;
    public int SHIELD;
    public int HACKING;
    public int DUMMYGEAR;
    public int AMARGEDDON;
    public int EMP;
}

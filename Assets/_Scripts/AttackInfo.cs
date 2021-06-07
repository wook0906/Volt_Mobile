using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo
{
    private int attackerNumber;
    public int AttackerNumber { get { return attackerNumber; } }
    private int damage;
    public int Damage { get { return damage; } }
    private CameraShakeType cameraShakeType;
    public CameraShakeType CameraShakeType { get { return cameraShakeType; } }
    private GameObject hitEffect;
    public Define.Effects effectType;
    private Card moduleInfo;
    public Card ModuleInfo { get { return moduleInfo; } }

    public AttackInfo(int attackerNumber, int damage, CameraShakeType cameraShakeType, Card moduleInfo = Card.NONE)
    {
        this.attackerNumber = attackerNumber;
        this.damage = damage;
        this.cameraShakeType = cameraShakeType;
        this.moduleInfo = moduleInfo;
    }
    public AttackInfo(int attackerNumber, int damage, CameraShakeType cameraShakeType, Define.Effects hitEffectType, Card moduleInfo = Card.NONE)
    {
        this.attackerNumber = attackerNumber;
        this.damage = damage;
        this.cameraShakeType = cameraShakeType;
        this.effectType = hitEffectType;
        this.moduleInfo = moduleInfo;
    }
}
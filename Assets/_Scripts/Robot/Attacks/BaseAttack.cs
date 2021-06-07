using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack
{
    protected StateMachine fsm;
    protected Volt_ModuleCardBase moduleCard;
    public AttackType AttackType { private set; get; }

    public virtual void Init(StateMachine fsm, Volt_ModuleCardBase moduleCard)
    {
        this.fsm = fsm;
        this.moduleCard = moduleCard;

        if (moduleCard == null)
            this.AttackType = AttackType.Attack;
        else
        {
            //Debug.Assert(moduleCard is IActiveModuleCard, "Active module가 아님!!");
            this.AttackType = (moduleCard as IActiveModuleCard).AttackType;
        }
    }

    protected virtual void PlayMuzzleEffect(Transform robot, AttackData attackData, Volt_RobotBehavior behavior = null)
    {
        for (int i = 0; i < attackData.effectData.Length; ++i)
        {
            Transform launchPoint = robot.Find(attackData.effectData[i].launchPointPath);
            //Debug.Assert(launchPoint != null, $"launchPoint가 왜 널이야 너 이 xx 경로 문제있어? \nPath:{attackData.effectData[i].launchPointPath}");

            if (attackData.cameraShakeType == CameraShakeType.EMP)
                Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(attackData.effectData[i].effectType),
                    launchPoint, true, behavior.BehaviorPoints * attackData.effectData[i].scaleMultValue);
            else
                Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(attackData.effectData[i].effectType),
                    launchPoint, true);
        }
    }

    protected virtual void BlinkTiles(Volt_Tile[] tiles)
    {
        foreach (Volt_Tile tile in tiles)
        {
            if (tile == null) continue;

            if (tile.pTileType == Volt_Tile.TileType.none)
                continue;

            tile.SetBlinkOption(BlinkType.Attack, 0.5f);
            tile.BlinkOn = true;
        }
    }

    protected virtual AttackInfo CreateAttackInfo(AttackData attackData)
    {
        //HitEffect도 전달해주기 위해서 AttackData를 받음.
        int damageMult = fsm.behavior.BehaviorPoints <= 3 ? 1 : 2;
        return new AttackInfo(fsm.Owner.playerInfo.playerNumber,
            (attackData.damage * damageMult), attackData.cameraShakeType, fsm.Owner.playerInfo.GetHitEffect());
    }

    public abstract void AttackHandler(StateMachine fsm, AttackData attackData);
}

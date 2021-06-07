using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BaseAttack
{
    private void PlayEffect(Volt_Tile standingTile)
    {
        GameObject from = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_Teleport);//Volt_PrefabFactory.S.GetInstance(Volt_PrefabFactory.S.effect_Teleport);
        Vector3 pos = standingTile.transform.position;
        pos.y += Volt_ArenaSetter.S.tileScale.y + (Volt_ArenaSetter.S.tileScale.y * 0.1f);
        from.transform.position = pos;
    }

    private Transform SearchOpponent(Volt_Tile targetTile)
    {
        List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;
        foreach (Volt_Robot robot in robots)
        {
            if (robot.transform == fsm.transform)
                continue;
            Volt_Tile robotStandingTile = Volt_ArenaSetter.S.GetTile(robot.transform.position);
            if (robotStandingTile == targetTile)
            {
                return robot.transform;
            }
        }

        return null;
    }

    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseTeleport, fsm.Owner.playerInfo.playerNumber);

        //Debug.Log("Teleport Activate");

        Volt_Tile curStandingTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position);
        
        PlayEffect(curStandingTile);
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_TELEPORT.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        //targetTile은 내가 찍었던 타일로.
        Volt_Tile targetTile = fsm.behavior.SelectTile;
        
        Vector3 targetPos = targetTile.transform.position;
        targetPos.y = fsm.transform.position.y;

        Volt_ParticleManager.Instance.PlayParticle(
            Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_Teleport),
            fsm.transform);

        Transform opponent = SearchOpponent(targetTile);

        if (opponent != null)
            opponent.position = fsm.transform.position;

        fsm.transform.position = targetPos;
    }
}

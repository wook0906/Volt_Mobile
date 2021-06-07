using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_RepulsionBlast : Volt_ExtraModuleCard
{
    public override void Activated()
    {
        OnUseCard();
        Debug.Log("Active RepulsionBlast module");

        //GameObject go = Volt_PrefabFactory.S.GetInstance(Volt_PrefabFactory.S.effect_RepulsionBlast);
        //Vector3 pos = Volt_ArenaSetter.S.GetTile(owner.transform.position).transform.position;
        //pos.y += (1.4f *Volt_ArenaSetter.S.tileScale.y);
        //go.transform.position = pos;

        Volt_ParticleManager.Instance.PlayParticle(
            Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_Repulsionblast),
            owner.transform, 0.1f);
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_REPULSIONBLAST.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        Volt_PlayerManager.S.I.playerCamRoot.SetShakeType(CameraShakeType.RepulsionBlast);
        Volt_PlayerManager.S.I.playerCamRoot.CameraShake();

        Volt_Tile curStandingTile = Volt_ArenaSetter.S.GetTile(owner.transform.position);
        List<Volt_Tile> adjecentTiles = new List<Volt_Tile>(curStandingTile.GetAdjecentTiles());
        //밀리는 중간 로봇의 경우 아직 나와 같은 타일에 있을 수 있으므로
        adjecentTiles.Add(curStandingTile);
        List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;

        foreach (var robot in robots)
        {
            if (robot.gameObject == owner.gameObject)
                continue;
            if (robot.fsm.isDead)
                continue;

            Volt_Tile tile = Volt_ArenaSetter.S.GetTile(robot.transform.position);
            if(adjecentTiles.Contains(tile))
            {
                Vector3 knockbackDir = (robot.transform.position - owner.transform.position).normalized;
                
                if (Volt_ArenaSetter.S.IsCanMoveNextTile(tile.transform.position,
                    knockbackDir))
                {
                    robot.PushType = PushType.PushedCandidate;
                    robot.fsm.isKnockback = true;
                    robot.fsm.knockbackInfor = new KnockbackInfor(owner.gameObject,
                        knockbackDir, tile);
                }
            }
        }
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseRepulsionBlast, owner.playerInfo.playerNumber);
        isCanUse = false;
        owner.moduleCardExcutor.DestroyCard(this);
    }
}

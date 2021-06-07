using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_ShockWave : Volt_ExtraModuleCard
{
    public override void Activated()

    {
        OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseShockWave, owner.playerInfo.playerNumber);
        Debug.Log("Active ShockWave~");

        //GameObject go = Volt_PrefabFactory.S.GetInstance(Volt_PrefabFactory.S.effect_ShockPulse);
        //Vector3 pos = Volt_ArenaSetter.S.GetTile(owner.transform.position).transform.position;
        //pos.y += (1.4f * Volt_ArenaSetter.S.tileScale.y);
        //go.transform.position = pos;
        Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_Shockwave),
            owner.transform,0.1f);
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_SHOCKWAVE.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        Volt_Tile standingTile = Volt_ArenaSetter.S.GetTile(owner.transform.position);
        Volt_Tile[] adjecentTiles = standingTile.GetAdjecentTiles();

        List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;

        Volt_PlayerManager.S.I.playerCamRoot.SetShakeType(CameraShakeType.ShockWave);
        Volt_PlayerManager.S.I.playerCamRoot.CameraShake();

        foreach (var robot in robots)
        {
            if (robot.gameObject == owner.gameObject)
                continue;
            if (robot.fsm.isDead)
                continue;

            Volt_Tile tile = Volt_ArenaSetter.S.GetTile(robot.transform.position);

            for (int i = 0; i < adjecentTiles.Length; i++)
            {
                if (!adjecentTiles[i]) continue;
                if(adjecentTiles[i] == tile)
                {
                    robot.GetDamage(new AttackInfo(
                    owner.playerInfo.playerNumber, 1, CameraShakeType.ShockWave, owner.playerInfo.GetHitEffect()));
                }
            }
            //if (adjecentTiles.Contains(tile))
            //{
                
            //    robot.GetDamage(new AttackInfo(
            //        owner.playerInfo.playerNumber, 1, CameraShakeType.ShockWave, owner.playerInfo.GetHitEffect()));
            //    //robot.PlayDamagedEffect(owner.transform);
            //}
        }

        isCanUse = false;
        owner.moduleCardExcutor.DestroyCard(this);

        Volt_Tile[] betweenTiles = Volt_ArenaSetter.S.GetTile(owner.transform.position).GetAdjecentTiles();
        foreach (var item in betweenTiles)
        {
            if (item == null) continue;

            item.SetBlinkOption(BlinkType.Attack, 0.5f);
            item.BlinkOn = true;
        }
    }
}

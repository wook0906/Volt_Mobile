using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Grenades : Volt_Projectile
{
    
    private void FixedUpdate()
    {
        if(isEndMove)
        {
            Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(transform.position);
            Explosion(targetTile);
        }
    }

    public void Explosion(Volt_Tile targetTile)
    {
        //Debug.Log("폭발!");
        Vector3 pos = targetTile.transform.position;
        pos.y += 2.5f;

        GameObject vfx = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_ExplosionGrenade);
        vfx.transform.position = pos;
        vfx.transform.rotation = Quaternion.identity;
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/Module_Sound/Grenade.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        List<Volt_Tile> attackPoints = new List<Volt_Tile>();
        attackPoints.Add(targetTile);
        attackPoints.AddRange(targetTile.GetAdjecentTiles());

        List<Volt_Robot> robots = Volt_ArenaSetter.S.robotsInArena;
        Volt_PlayerManager.S.I.playerCamRoot.SetShakeType(CameraShakeType.Grenade);
        Volt_PlayerManager.S.I.playerCamRoot.CameraShake();

        foreach (Volt_Robot robot in robots)
        {
            if (robot.fsm.isDead)
                continue;

            Volt_Tile robotStandingTile = Volt_ArenaSetter.S.GetTile(robot.transform.position);
            if(attackPoints.Contains(robotStandingTile))
            {
                if(robotStandingTile == targetTile)
                {
                    robot.GetDamage(new AttackInfo(owner.GetComponent<Volt_Robot>().playerInfo.playerNumber,
                        2, CameraShakeType.Grenade, owner.playerInfo.GetHitEffect()));
                }
                else
                {
                    robot.GetDamage(new AttackInfo(owner.GetComponent<Volt_Robot>().playerInfo.playerNumber,
                        1, CameraShakeType.Grenade, owner.playerInfo.GetHitEffect()));
                }
            }
        }

        isEndMove = false;
        owner.fsm.AniEventHandler.OnDoneAttackAnimationCallback();
        Volt_PrefabFactory.S.PushObject(GetComponent<Poolable>());
        //Destroy(gameObject);
    }
}

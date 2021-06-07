using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_TimeBomb : Volt_Projectile
{
    public Volt_Robot targetRobot;  // 시한폭탄이 달리게 된 로봇
    public bool isTriggered = false;
    public bool isPlaying = false;
    private void OnTriggerEnter(Collider other)
    {

        //if (other.gameObject.GetComponent<Volt_Robot>().playerInfo.playerNumber == ownerPlayerNumber)
        //    return;
        if (!other.GetComponent<Volt_Robot>()) return;

        targetRobot = other.GetComponent<Volt_Robot>();
        //Debug.Log("Timebomb TriggerEnter" + other.name);
        isTriggered = true;
        targetRobot.SetTimeBomb(this);
    }
    private void Start()
    {
        //StartCoroutine(WaitDoMove());
    }

    public void StartWaitMoveCoroutine()
    {
        StartCoroutine(WaitDoMove());
    }

    IEnumerator WaitDoMove()
    {
        yield return new WaitUntil(() => isEndMove);

        Volt_Tile bombSetTile = Volt_ArenaSetter.S.GetTile(transform.position);
        if (bombSetTile.pTileType == Volt_Tile.TileType.pits)
        {
            Explosion();
            yield break;
        }
        if (bombSetTile.GetRobotInTile())
            bombSetTile.GetRobotInTile().SetTimeBomb(this);
        else
            bombSetTile.SetTimeBomb(this);
        owner.fsm.AniEventHandler.OnDoneAttackAnimationCallback();
    }
    private void Update()
    {
        
    }
    public void CountDown()
    {
        if (isPlaying) return;
        isPlaying = true;
        StartCoroutine(CountDownWait());

    }
    IEnumerator CountDownWait()
    {
        --count;
        if (count == 0)
        {
            //Debug.Log("Explosion 1");
            //Explosion();
            StartCoroutine(ExplosionWait());
        }
        else
        {
            Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_TIMEBOMB.mp3",
                (result) =>
                {
                    Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
                });
            
            Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(transform.position);
            List<Volt_Tile> targetTiles = new List<Volt_Tile>(targetTile.GetAdjecentTiles());
            targetTiles.Add(targetTile);
            foreach (var item in targetTiles)
            {
                if (item == null) continue;
                item.SetBlinkOption(BlinkType.Tactic, 0.5f);
                item.BlinkOn = true;
            }
            yield return new WaitForSeconds(2f);
            isPlaying = false;
        }
        yield break;
        
    }

    public void Explosion()
    {
        if (isPlaying) return;
        //Debug.Log("Explosion timebomb");
        isPlaying = true;
        StartCoroutine(ExplosionWait());
    }

    IEnumerator ExplosionWait()
    {
        //Debug.Log("Coroutine Explosion");
        GetComponent<MeshRenderer>().enabled = false;
        List<Volt_Tile> adjecentTiles;

        GameObject go = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_ExplosionTimebomb);//Volt_PrefabFactory.S.GetInstance(Volt_PrefabFactory.S.effect_TimeBomb);

        Volt_Tile tile = Volt_ArenaSetter.S.GetTile(transform.position);
        Vector3 pos = tile.transform.position;
        pos.y += 2.5f;
        go.transform.position = pos;
        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Robot_Attack_Effects/Hound_ImpactSE.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });

        Volt_PlayerManager.S.I.playerCamRoot.SetShakeType(CameraShakeType.Timebomb);
        Volt_PlayerManager.S.I.playerCamRoot.CameraShake();


        adjecentTiles = new List<Volt_Tile>(Volt_ArenaSetter.S.GetTile(transform.position).GetAdjecentTiles());
        adjecentTiles.Add(Volt_ArenaSetter.S.GetTile(transform.position));

        foreach (var item in adjecentTiles)
        {
            if (item == null) continue;
            item.SetBlinkOption(BlinkType.Attack, 0.5f);
            item.BlinkOn = true;
        }

        foreach (var item in adjecentTiles)
        {
            if (item == null) continue;

            if (item.GetRobotInTile() != null)
            {
                Volt_Robot robot = item.GetRobotInTile();

                if (robot.fsm.isDead)
                    continue;

                if (Volt_ArenaSetter.S.GetTile(robot.transform.position) == Volt_ArenaSetter.S.GetTile(transform.position))
                {
                    robot.GetDamage(new AttackInfo(ownerPlayerNumber, 2,
                        CameraShakeType.Timebomb, Volt_PlayerManager.S.GetPlayerByPlayerNumber(ownerPlayerNumber).GetHitEffect()));
                    //robot.PlayDamagedEffect();
                    //Debug.Log("Time bomb Center Damage!");
                }
                else
                {
                    robot.GetDamage(new AttackInfo(ownerPlayerNumber, 1,
                        CameraShakeType.Timebomb, Volt_PlayerManager.S.GetPlayerByPlayerNumber(ownerPlayerNumber).GetHitEffect()));
                    //robot.PlayDamagedEffect();
                    //Debug.Log("Time bomb Damage!");
                }
                //Volt_RobotBehavior behavior = new Volt_RobotBehavior();
                //behavior.SetBehaivor(0, Vector3.zero, BehaviourType.None, robot.playerInfo.playerNumber);
                
            }
        }
        //yield return new WaitUntil(() => RobotBehaviourObserver.Instance.IsAllRobotBehaviourFlagOff());
        yield return new WaitForSeconds(2f);
        Destroy();
    }
    public void Destroy()
    {
        if (transform.parent != null)
        {
            Volt_Robot robot = transform.parent.GetComponent<Volt_Robot>();
            robot.TimeBombInstance = null;
            robot.IsTimeBombOn = false;
            Debug.Log($"{targetRobot.playerInfo.playerNumber} Timebomb Destroy");
        }
        else
        {
            Volt_Tile tile = Volt_ArenaSetter.S.GetTile(transform.position);
            tile.TimeBombInstance = null;
            tile.IsHaveTimeBomb = false;
            Debug.Log($"{tile.tileIndex} tile Timebomb Destroy");
        }
        isPlaying = false;
        targetRobot = null;
        isTriggered = false;
        isEndMove = false;
        ownerPlayerNumber = 0;
        count = 3;
        GetComponent<MeshRenderer>().enabled = true;
        Volt_PrefabFactory.S.PushObject(GetComponent<Poolable>());
        //Destroy(this.gameObject);
    }


}

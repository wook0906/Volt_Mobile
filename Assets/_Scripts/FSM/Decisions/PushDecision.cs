using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 충돌 상태에서 내가 상대편 로봇에게 밀릴 것인가를 판단한다.
/// </summary>
[CreateAssetMenu(menuName = "StateMachine/Decisions/PushDecision")]
public class PushDecision : DecisionBase
{
    private bool CheckPusherNotMoving(ref CollisionData collisionData)
    {
        return collisionData.destTile == null;
    }

    private bool IsStandingOnBetweenTiles(Collider[] colliders)
    {
        return colliders.Length > 1;
    }

    private void SetKnockbackInfo(StateMachine fsm, Vector3 pushDir)
    {
        Collider[] colliders = Physics.OverlapBox(fsm.collisionData.robot.transform.position,
                Vector3.one * 0.5f, Quaternion.identity, fsm.tileMaks);

        // 날 미는 로봇이 한 타일 위에 있을 때
        if (!IsStandingOnBetweenTiles(colliders))
        {
            Vector3 pos = colliders[0].transform.position;
            pos.y = fsm.transform.position.y;
            Vector3 dir = (pos - fsm.transform.position).normalized;
            if (Vector3.Angle(dir, pushDir) > 15f)
            {
                fsm.knockbackInfor = new KnockbackInfor(fsm.collisionData.robot,
                pushDir, Volt_ArenaSetter.S.GetTile(fsm.collisionData.robot.transform.position));
                //Debug.Log($"1 tile [{fsm.Owner.playerInfo.playerNumber}]standinTile:{Volt_ArenaSetter.S.GetTile(fsm.collisionData.robot.transform.position)}");
            }
            else
            {
                fsm.knockbackInfor = new KnockbackInfor(fsm.collisionData.robot,
                pushDir, Volt_ArenaSetter.S.GetTile(fsm.collisionData.robot.transform.position, -pushDir));
                //Debug.Log($"1 tile [{fsm.Owner.playerInfo.playerNumber}]standinTile:{Volt_ArenaSetter.S.GetTile(fsm.collisionData.robot.transform.position, -pushDir)}");
            }
        }
        else // 날 미는 로봇이 타일 사이에 서 있을 때
        {
            Transform pusher = fsm.collisionData.robot.transform;
            StateMachine pusherFSM = fsm.collisionData.robot.GetComponent<StateMachine>();
            for (int i = 0; i < colliders.Length; i++)
            {
                Vector3 pos = colliders[i].transform.position;
                pos.y = pusher.transform.position.y;
                Vector3 dir = (pos - pusher.position);
                float angle = Vector3.Angle(dir, pusherFSM.MoveDir);
                if(angle < 15f)
                {
                    fsm.knockbackInfor = new KnockbackInfor(pusher.gameObject,
                        pushDir, Volt_ArenaSetter.S.GetTile(colliders[i].transform.position));
                }
            }
        }
    }

    public override bool Decision(StateMachine fsm)
    {
        if (fsm.isDonePushDecision)
            return false;

        fsm.isDonePushDecision = true;
        if (!IsPushedByOpponent(fsm, ref fsm.collisionData))
            return false;

        
        Vector3 pushDir = (fsm.transform.position - fsm.collisionData.robot.transform.position).normalized;
        if(CheckPusherNotMoving(ref fsm.collisionData))
        {
            //Debug.Log($"{fsm.collisionData.robot.GetComponent<Volt_Robot>().playerInfo.playerNumber} is stop");
            SetKnockbackInfo(fsm, pushDir);
        }
        else // Pusher가 이동 중...
        {
            //Debug.Log($"{fsm.collisionData.robot.GetComponent<Volt_Robot>().playerInfo.playerNumber} is move");
            Vector3 opponentDestPos = fsm.collisionData.destTile.transform.position;
            opponentDestPos.y = fsm.transform.position.y;

            Vector3 direction = (opponentDestPos - fsm.transform.position).normalized;
            float angle = Vector3.Angle(direction, pushDir);
            //Debug.Log($"between toTile and pusDir Angle:{angle}");
            if (angle > 80f)
            {
                Volt_Tile[] tiles = Volt_ArenaSetter.S.GetTiles(fsm.collisionData.destTile, pushDir, 9);
                Volt_Tile tile = null;
                for (int i = 0; i < tiles.Length; ++i)
                {
                    // 도착해야할 타일 설정
                    Vector3 pos = tiles[i].transform.position;
                    pos.y = fsm.transform.position.y;
                    Vector3 toTile = (pos - fsm.transform.position).normalized;
                    if (Vector3.Angle(toTile, pushDir) < 15f)
                    {
                        //도착해야할 타일보다 앞의 타일을 넣어준다. 
                        if (i == 0)
                            tile = fsm.collisionData.destTile;
                        else 
                            tile = tiles[i - 1]; 
                        break;
                    }
                }

                fsm.knockbackInfor = new KnockbackInfor(fsm.collisionData.robot,
                    pushDir, tile);
            }
            else
            {
                fsm.knockbackInfor = new KnockbackInfor(fsm.collisionData.robot,
                        pushDir, fsm.collisionData.destTile);
            }
        }

        fsm.behavior = null;
        fsm.isKnockback = true;
        return true;
    }

    private bool IsPushedByOpponent(StateMachine fsm, ref CollisionData collisionData)
    {
        if (collisionData.robot.GetComponent<Volt_Robot>().fsm.isDead)
            return false;

        PushType myPushType = fsm.Owner.PushType;
        //Debug.Log($"{fsm.Owner.playerInfo.playerNumber} is {fsm.Owner.PushType}");

        if (myPushType == PushType.Pusher)
        {
            // TODO: 로봇이 다음 타일로 밀릴 수 있음에도 가장자리에 있다고 인식해
            // 밀리지 않는 현상이 종종 발생함!
            //Debug.Log($"{fsm.Owner.playerInfo.playerNumber} is pusher");
            Vector3 pushDir = (collisionData.robot.transform.position - fsm.transform.position).normalized;
            if(false == Volt_ArenaSetter.S.IsCanMoveNextTile(collisionData.robot.transform.position, pushDir))
            {
                
                //Debug.Log("[Can't move next tile]");
                return true;
            }
            else if(collisionData.pushType == PushType.PushedCandidate &&
                collisionData.isHaveAnchor)
            {
                //Debug.Log("[Is have anchor]");
                return true;
            }
            else if(collisionData.pushType == PushType.Pushed)
            {
                //Debug.Log("[Other Pushed]");
                return true;
            }
            else if(collisionData.pushType == PushType.Immune)
            {
                //Debug.Log("[Other Immune]");
                return true;
            }
            return false;
        }
        else if(myPushType == PushType.Pushed)
        {
            //Debug.Log($"{fsm.Owner.playerInfo.playerNumber} is pushed");
            Vector3 pushDir = (collisionData.robot.transform.position - fsm.transform.position).normalized;
            if (false == Volt_ArenaSetter.S.IsCanMoveNextTile(collisionData.robot.transform.position, pushDir))
            {
                //Debug.Log("[Can't move next tile]");
                return true;
            }
            else if (collisionData.pushType == PushType.PushedCandidate &&
                collisionData.isHaveAnchor)
            {
                //Debug.Log("[Is have anchor]");
                return true;
            }
            else if(collisionData.pushType == PushType.Immune)
            {
                //Debug.Log("[Other Immune]");
                return true;
            }
            return false;
        }
        else if(myPushType == PushType.PushedCandidate)
        {
            //Debug.Log($"{fsm.Owner.playerInfo.playerNumber} is pushed");
            Vector3 pushedDir = (fsm.transform.position - collisionData.robot.transform.position).normalized;
            if (false == Volt_ArenaSetter.S.IsCanMoveNextTile(fsm.transform.position, pushedDir))
            {
                return false;
            }
            else if (fsm.Owner.AddOnsMgr.IsHaveAnchor)
            {
                fsm.Owner.AddOnsMgr.UseAnchorModule();
                return false;
            }
            return true;
        }
        return false;
        
    }
}

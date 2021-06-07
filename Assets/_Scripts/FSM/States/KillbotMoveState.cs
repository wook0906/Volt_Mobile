using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/KillbotMoveState")]
public class KillbotMoveState : StateBase, ICoAction
{
    private Vector3 startPos;

    private Quaternion fromRot;
    private Quaternion toRot;

    private float rotationTime;
    private float moveTime;
    private float elapsedTime = 0f;
    private float u = 0f;

    private bool isDetectSomething = false;

    public override void OnEnterState(StateMachine fsm)
    {
        Debug.Assert(fsm.behavior != null, "Error! 행동 정보가 null입니다.");

        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);

        fsm.Animator.CrossFade("Movement", .1f);
        //fsm.Animator.Play("Movement", -1, 0);

        elapsedTime = 0f;
        u = 0f;

        fsm.StartCoroutine("CoAction");

    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.isDoneMove = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {
    }

    public IEnumerator CoAction(StateMachine fsm)
    {
        isDetectSomething = false;

        while (fsm.behavior.BehaviorPoints > 0)
        {
            fsm.behavior.BehaviorPoints--;

            Volt_Tile standingTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position);
            Volt_Tile subTargetTile = GetSubTargetTile(fsm);

            fsm.behavior.Direction = (subTargetTile.transform.position - standingTile.transform.position).normalized;

            if(IsCanMoveNextTile(fsm.transform.position, fsm.behavior.Direction) == false)
            {
                Volt_Tile nextTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position, fsm.behavior.Direction);
                ArenaSection section = Volt_ArenaSetter.S.GetSection(nextTile);
                float angle = Volt_Rotation.GetAngleUsingSectionAndDirection(section, fsm.behavior.Direction);
                fsm.behavior.Direction = Quaternion.AngleAxis(angle, Vector3.up) * fsm.behavior.Direction;

                Ray ray = new Ray(fsm.transform.position, fsm.behavior.Direction);
                RayController rayController = new RayController(ray, 1.41f, LayerMask.GetMask("Obstacle"), "Wall");
                if(rayController.Raycast())
                {
                    fsm.behavior.Direction *= -1f;
                }
                subTargetTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position, fsm.behavior.Direction);
            }

            // 이동 방향으로 회전
            fromRot = fsm.transform.rotation;
            toRot = Quaternion.LookRotation(fsm.behavior.Direction);

            rotationTime = Vector3.Angle(fsm.transform.forward, fsm.behavior.Direction) / fsm.angular;
            elapsedTime = 0f;
            u = 0f;
            while(u < 1f)
            {
                Quaternion rotation = Quaternion.Slerp(fromRot, toRot, u);
                fsm.transform.rotation = rotation;

                elapsedTime += Time.deltaTime;
                u = elapsedTime / rotationTime;
                yield return null;
            }
            fsm.transform.rotation = toRot;

            fsm.destPos = subTargetTile.transform.position;
            startPos = fsm.transform.position;
            moveTime = (fsm.destPos - startPos).magnitude;

            elapsedTime = 0f;
            u = 0f;
            while (u < 1f)
            {
                Vector3 pos = Vector3.Lerp(startPos, fsm.destPos, u);
                fsm.transform.position = pos;

                elapsedTime += Time.deltaTime;
                u = elapsedTime / moveTime;
                yield return null;
            }

            fsm.transform.position = fsm.destPos;
        }

        if (fsm.behavior.BehaviorPoints == 0)
        {
            fsm.isDoneMove = true;
            fsm.behavior = null;
            fsm.destPos = Vector3.positiveInfinity;
        }
    }

    private bool IsCanMoveNextTile(Vector3 pos, Vector3 direction)
    {
        if (Volt_ArenaSetter.S.IsCanMoveNextTile(pos, direction) == false)
            return false;

        Volt_Tile tile = Volt_ArenaSetter.S.GetTile(pos, direction);
        if (tile.pTileType == Volt_Tile.TileType.pits)
            return false;
        if (tile.IsOnVoltage == true)
            return false;

        Ray ray = new Ray(pos, direction);
        RayController rayController = new RayController(ray, 1.41f, LayerMask.GetMask("Obstacle"));
        if (rayController.Raycast() == true)
            return false;

        return true;
    }

    private Volt_Tile GetSubTargetTile(StateMachine fsm)
    {
        List<GameObject> detectedGOs = SearchSomething(fsm);

        if(detectedGOs == null)
        {
            return Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                fsm.behavior.Direction);
        }
        else
        {
            // 가장 가까운 물체를 찾는다.

            // 만약 감지된게 1개면 그걸 쫒는다.
            if(detectedGOs.Count == 1)
            {
                return Volt_ArenaSetter.S.GetTile(detectedGOs[0].transform.position);
            }
            else
            {
                GameObject closetGO = null;
                float closetDist = float.MinValue;
                for(int i = 0; i < detectedGOs.Count; ++i)
                {
                    Vector3 toTarget = (detectedGOs[i].transform.position - fsm.transform.position);
                    if(toTarget.magnitude < closetDist)
                    {
                        closetDist = toTarget.magnitude;
                        closetGO = detectedGOs[i];
                    }
                }

                return Volt_ArenaSetter.S.GetTile(closetGO.transform.position);
            }
        }
    }

    private List<GameObject> SearchSomething(StateMachine fsm)
    {
        if(!isDetectSomething)
        {
            Volt_Tile[] tiles = new Volt_Tile[3]
            {
                Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                fsm.behavior.Direction, 3),
                Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                Vector3.Cross(fsm.behavior.Direction, Vector3.up), 3),
                Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                -Vector3.Cross(fsm.behavior.Direction, Vector3.up), 3)
            };

            List<GameObject> detectedGOs = new List<GameObject>();
            Volt_Tile standingTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position);
            for(int i = 0; i < tiles.Length; ++i)
            {
                Vector3 rayDir = (tiles[i].transform.position - standingTile.transform.position).normalized;
                Ray ray = new Ray(fsm.transform.position, rayDir);
                RayController rayController = new RayController(ray,
                    (tiles[i].transform.position - standingTile.transform.position).magnitude,
                    LayerMask.GetMask("Items") | LayerMask.GetMask("Robot"),
                    "Killbot", "Robot", "VP");

                if(rayController.Raycast() == true)
                {
                    detectedGOs.Add(rayController.GetHitedGameObject());
                }
            }

            return detectedGOs;
        }
        else
        {
            return null;
        }
    }
}

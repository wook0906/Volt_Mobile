using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillbotMoveController : MoveController
{
    private Queue<Volt_Tile> qTiles = new Queue<Volt_Tile>(); // 킬봇이 이동할 타일들 (킬봇은 타일 단위로 이동한다.)
    private Vector3 destPos;

    public KillbotMoveController(StateMachine fsm, Transform robot, float speed, float angular)
        : base(fsm, robot, speed, angular)
    {
        qTiles = new Queue<Volt_Tile>();
        destPos = Vector3.zero;
        SetTargetTiles();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void UpdatePrev(float deltaTime)
    {
        CalculateMoveDirection();

        // 이동 방향으로 1타일 이동
        destPos = qTiles.Dequeue().transform.position;
        destPos.y = fsm.transform.position.y;
        moveTime = (destPos - robot.position).magnitude / speed;
        state = State.Rotation;

        //Debug.Log($"{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber} moveTime:{moveTime} direction:{destPos - robot.position}");
        rotationTime = Vector3.Angle(robot.forward, fsm.MoveDir) / angular;
    }

    public override void UpdateMove(float deltaTime)
    {
        Vector3 direction = (destPos - robot.position);
        robot.position += direction.normalized * speed * deltaTime;
        
        if ((Time.time - moveStartTime) >= moveTime)
        {
            robot.position = destPos;
            fsm.startTile = Volt_ArenaSetter.S.GetTile(robot.position);
            fsm.behavior.BehaviorPoints = qTiles.Count;

            if (qTiles.Count > 0)
                state = State.Prev;
            else
                state = State.Finish;
        }
    }

    private void SetTargetTiles()
    {
        qTiles.Clear();
        if (fsm.startTile == null)
            fsm.startTile = Volt_ArenaSetter.S.GetTile(robot.position);

        Volt_Tile to = Volt_ArenaSetter.S.GetTile(fsm.startTile.transform.position, fsm.behavior.Direction,
            fsm.behavior.BehaviorPoints);

        Volt_Tile[] tiles = Volt_ArenaSetter.S.GetTiles(fsm.startTile, to);

        for (int i = 0; i < tiles.Length; ++i)
        {
            qTiles.Enqueue(tiles[i]);
        }

        if (tiles.Length < fsm.behavior.BehaviorPoints)
        {
            for (int i = 0; i < fsm.behavior.BehaviorPoints - tiles.Length; ++i)
            {
                qTiles.Enqueue(tiles[tiles.Length - 1]);
            }
        }

        fsm.MoveDir = fsm.behavior.Direction;
    }

    private void CalculateMoveDirection()
    {
        Vector3 direction = fsm.behavior.Direction;
        GameObject target = GetTarget();
        // 뭔가 감지된게 있다.
        if (target != null)
        {
#if UNITY_EDITOR
            //Debug.Log($"Killbot detect {target.name}");
#endif
            Vector3 targetPos = target.transform.position;
            targetPos.y = fsm.transform.position.y;
            fsm.behavior.Direction = (targetPos - fsm.transform.position).normalized;
            int range = qTiles.Count;
            fsm.destPos = Volt_ArenaSetter.S.GetTile(fsm.startTile.transform.position,
                fsm.behavior.Direction, range).transform.position;
            fsm.destPos.y = fsm.transform.position.y;
        }

        // 이동 방향으로 이동이 불가능하다면
        Vector3 pos = fsm.startTile.transform.position;
        pos.y = fsm.transform.position.y;
        if (!CanMoveNextTile(pos, fsm.behavior.Direction))
        {
            ChangeMoveDirection();
        }

        // 이동 방향이 처음 셋팅한 것과 다르다면
        if (Vector3.Angle(fsm.behavior.Direction, direction) > 15f)
        {
            SetTargetTiles();
        }
    }

    private void ChangeMoveDirection()
    {
#if UNITY_EDITOR
        //Debug.Log($"[{fsm.Owner.playerInfo.playerNumber}] cant move next tile");
#endif
        Volt_Tile nextTile = Volt_ArenaSetter.S.GetTile(fsm.startTile.transform.position, fsm.behavior.Direction);
        ArenaSection section = ArenaSection.None;
        if (nextTile.pTileType == Volt_Tile.TileType.pits)
            section = Volt_ArenaSetter.S.GetSection(nextTile);
        else
        {
            section = Volt_ArenaSetter.S.GetSection(fsm.startTile);
            if(section == ArenaSection.Center || section == ArenaSection.CenterBack ||
                section == ArenaSection.CenterLeft || section == ArenaSection.CenterRight ||
                section == ArenaSection.CenterTop)
            {
                section = Volt_ArenaSetter.S.GetSection(nextTile);
            }
        }

        float angle = Volt_Rotation.GetAngleUsingSectionAndDirection(section, fsm.behavior.Direction);
        fsm.behavior.Direction = Quaternion.AngleAxis(angle, Vector3.up) * fsm.behavior.Direction;

        if (CheckWall(fsm.transform.position, fsm.behavior.Direction) ||
            CheckVoltage(fsm.transform.position, fsm.behavior.Direction))
        {
            fsm.behavior.Direction *= -1f;
        }
        int range = qTiles.Count;
        fsm.destPos = Volt_ArenaSetter.S.GetTile(fsm.startTile.transform.position,
            fsm.behavior.Direction, range).transform.position;
        fsm.destPos.y = fsm.transform.position.y;
        fsm.isDetectSomething = false;
    }

    private bool CheckWall(Vector3 rayPosition, Vector3 rayDirection)
    {
        Ray ray = new Ray(rayPosition, rayDirection);
        RayController rayController = new RayController(ray, 1.41f, LayerMask.GetMask("Wall"), "Wall");
        rayController.rayColor = Color.blue;

        return rayController.Raycast();
    }

    private bool CheckVoltage(Vector3 pos, Vector3 direction)
    {
        Volt_Tile tile = Volt_ArenaSetter.S.GetTile(pos, direction);
        if (tile.IsOnVoltage)
            return true;
        return false;
    }

    private bool CanMoveNextTile(Vector3 pos, Vector3 direction)
    {
        if (Volt_ArenaSetter.S.IsCanMoveNextTile(pos, direction) == false)
            return false;

        RayController rayController = new RayController(pos, direction, 1.41f, LayerMask.GetMask("Obstacle"), "ElectricTrap");
        if(rayController.Raycast())
        {
            //Debug.Log("#######################함정");
            return false;
        }

        Volt_Tile tile = Volt_ArenaSetter.S.GetTile(pos, direction);
        if (tile.pTileType == Volt_Tile.TileType.pits)
            return false;
        if (tile.IsOnVoltage == true)
            return false;

        return true;
    }

    private GameObject GetTarget()
    {
        List<GameObject> detectedGOs = SearchNear();

        if (detectedGOs.Count == 0)
        {
            return null;
        }
        else
        {
            fsm.isDetectSomething = true;
            // 가장 가까운 물체를 찾는다.

            // 만약 감지된게 1개면 그걸 쫒는다.
            if (detectedGOs.Count == 1)
            {
                return detectedGOs[0].gameObject;
            }

            Volt_Tile myTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position);
            GameObject closetGO = null;
            int closetDist = int.MaxValue;

            for (int i = 0; i < detectedGOs.Count; ++i)
            {
                Volt_Tile tile = Volt_ArenaSetter.S.GetTile(detectedGOs[i].transform.position);

                int distance = CalculateDistance(myTile, tile);
                if (distance < closetDist)
                {
                    closetDist = distance;
                    closetGO = detectedGOs[i];
                }
            }

            return closetGO;
        }
    }

    private int CalculateDistance(Volt_Tile myTile, Volt_Tile otherTile)
    {
        Vector2Int myTilePos = new Vector2Int(myTile.tilePosInArray.x, myTile.tilePosInArray.y);
        Vector2Int otherTilePos = new Vector2Int(otherTile.tilePosInArray.x, otherTile.tilePosInArray.y);

        Vector2Int toTarget = (otherTilePos - myTilePos);
        return (int)toTarget.magnitude;
    }

    private List<GameObject> SearchNear()
    {
        if (!fsm.isDetectSomething)
        {
            // 시야가 닿는 최대거리의 타일을 받아온다.(앞, 좌, 우)
            Volt_Tile[] tiles = new Volt_Tile[3]
            {
                Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                fsm.behavior.Direction, 3),
                Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                Vector3.Cross(fsm.behavior.Direction, fsm.transform.up), 3),
                Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                -Vector3.Cross(fsm.behavior.Direction, fsm.transform.up), 3)
            };

            List<GameObject> detectedGOs = new List<GameObject>();
            Volt_Tile standingTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position);
            fsm.GetComponent<Collider>().enabled = false;
            for (int i = 0; i < tiles.Length; ++i)
            {
                Vector3 rayDir = (tiles[i].transform.position - standingTile.transform.position).normalized;
                Vector3 rayPos = fsm.transform.position;
                rayPos.y += 1f;
                Ray ray = new Ray(rayPos, rayDir);
                RayController rayController = new RayController(ray,
                    (tiles[i].transform.position - standingTile.transform.position).magnitude,
                    fsm.itemMask | fsm.robotMask,
                    "Killbot", "Robot", "VP");

                if (rayController.Raycast() == true)
                {
                    detectedGOs.Add(rayController.GetHitedGameObject());
                }
            }
            fsm.GetComponent<Collider>().enabled = true;
            return detectedGOs;
        }
        else
        {
            return new List<GameObject>();
        }
    }
}

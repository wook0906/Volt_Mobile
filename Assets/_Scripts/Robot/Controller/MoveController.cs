using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : BaseController
{
    public enum State
    {
        Prev,
        Rotation,
        Move,
        Finish
    }
    protected float speed;
    protected float angular;
    protected State state;
    protected float moveTime;
    protected float rotationTime;
    protected float moveStartTime;

    public MoveController(StateMachine fsm, Transform robot,
        float speed, float angular) : base(fsm, robot)
    {
        this.speed = speed;
        this.angular = angular;
        state = State.Prev;

    }

    public override void Update(float deltaTime)
    {
        //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] Update Controller");
        switch (state)
        {
            case State.Prev:
                UpdatePrev(deltaTime);
                break;
            case State.Rotation:
                UpdateRotation(deltaTime);
                break;
            case State.Move:
                UpdateMove(deltaTime);
                break;
            case State.Finish:
                UpdateFinish();
                break;
            default:
                break;
        }
    }

    public virtual void UpdatePrev(float deltaTime)
    {
        if (IsDestinationSet())
        {
            Volt_Tile to = Volt_ArenaSetter.S.GetTile(fsm.transform.position,
                fsm.behavior.Direction, fsm.behavior.BehaviorPoints);
            fsm.destPos = to.transform.position;
            fsm.destPos.y = fsm.transform.position.y;

            fsm.MoveDir = fsm.behavior.Direction;
            fsm.startTile = Volt_ArenaSetter.S.GetTile(fsm.transform.position);
        }
        

        moveTime = (fsm.destPos - robot.position).magnitude / speed;
        //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] moveTime:{moveTime}, moveStartTime:{moveTime}, direction:{fsm.destPos - robot.position}");
        rotationTime = Vector3.Angle(robot.forward, fsm.MoveDir) / angular;
        state = State.Rotation;
        //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] to rotation");
    }

    public virtual void UpdateMove(float deltaTime)
    {
        Vector3 direction = (fsm.destPos - robot.position);
        robot.position += direction.normalized * speed * deltaTime;
        
        //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}player]" +
            //$" Current frame Time:{Time.time}, moveStartTime:{moveStartTime}, " +
            //$" moveTime:{moveTime}, elapsedTime:{Time.time - moveStartTime}");

        if ((Time.time - moveStartTime) >= moveTime)
        {
            robot.position = fsm.destPos;
            state = State.Finish;
            //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] to finish");
        }
    }

    public virtual void UpdateRotation(float deltaTime)
    {
        //if (robot.GetComponent<Volt_Robot>().playerInfo.playerNumber == 3)
        //    Debug.Log("Update Rotation");

        robot.rotation = Quaternion.Slerp(robot.rotation, Quaternion.LookRotation(fsm.MoveDir), angular * deltaTime);

        float angle = Vector3.Angle(robot.forward, fsm.MoveDir);
        //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] angle:{angle}");
        if (angle < 5f)
        {
            robot.rotation = Quaternion.LookRotation(fsm.MoveDir);
            state = State.Move;
            //Debug.Log($"[{robot.GetComponent<Volt_Robot>().playerInfo.playerNumber}] to move");
            moveStartTime = Time.time;
        }
    }

    public virtual void UpdateFinish()
    {
        fsm.isDoneMove = true;
    }

    protected bool IsDestinationSet()
    {
        return fsm.destPos.magnitude == Vector3.positiveInfinity.magnitude;
    }
}

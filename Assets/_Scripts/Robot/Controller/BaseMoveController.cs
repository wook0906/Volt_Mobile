using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMoveController
{
    public enum State
    {
        Prev,
        Rotation,
        Move,
        Finish
    }

    protected StateMachine fsm;
    protected Transform robot;
    protected float speed;
    protected float angular;
    protected State state;

    public virtual void Init(StateMachine fsm, Transform robot,
        float speed, float angular)
    {
        this.robot = robot;
        this.speed = speed;
        this.angular = angular;
        this.fsm = fsm;
        state = State.Prev;
    }

    public virtual void Update(float deltaTime)
    {
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
        state = State.Rotation;
    }

    public virtual void UpdateMove(float deltaTime)
    {
        Vector3 direction = (fsm.destPos - robot.position);
        robot.position += direction.normalized * speed * Time.fixedDeltaTime;

        if (direction.magnitude <= speed * Time.fixedDeltaTime)
        {
            robot.position = fsm.destPos;
            state = State.Finish;
        }
    }

    public virtual void UpdateRotation(float deltaTime)
    {
        robot.rotation = Quaternion.Slerp(robot.rotation, Quaternion.LookRotation(fsm.MoveDir), angular * deltaTime);

        float angle = Vector3.Angle(robot.forward, fsm.MoveDir);
        if(angle < 5f)
        {
            robot.rotation = Quaternion.LookRotation(fsm.MoveDir);
            state = State.Move;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : BaseController
{
    public enum State
    {
        None,
        Prev,
        Rotation,
        Attack,
        Finish
    }

    protected State state;
    protected Quaternion toRot;

    public AttackController(StateMachine fsm, Transform robot)
        : base(fsm, robot)
    {

    }

    protected virtual void PlayMuzzleEffect(Transform robot, AttackData attackData)
    {
        for (int i = 0; i < attackData.effectData.Length; ++i)
        {
            Transform launchPoint = robot.Find(attackData.effectData[i].launchPointPath);
            Debug.Assert(launchPoint != null, $"launchPoint가 왜 널이야 너 이 xx 경로 문제있어? \nPath:{attackData.effectData[i].launchPointPath}");

            Volt_ParticleManager.Instance.PlayParticle(Volt_PrefabFactory.S.PopEffect(attackData.effectData[i].effectType),
                launchPoint, true);
        }
    }

    protected bool IsAttackBlocking(GameObject hitGo)
    {
        return hitGo.CompareTag("Wall");
    }

    public override void Update(float deltaTime)
    {
        switch (state)
        {
            case State.Prev:
                break;
            case State.Rotation:
                break;
            case State.Attack:
                break;
            case State.Finish:
                break;
            default:
                break;
        }
    }

    protected virtual void UpdatePrev(float deltaTime)
    {
        toRot = Quaternion.LookRotation(fsm.behavior.Direction);
        fsm.AniEventHandler.isDoneAttack = false;
        state = State.Rotation;
    }

    protected virtual void UpdateRotation(float deltaTime)
    {
        fsm.transform.rotation = Quaternion.Slerp(fsm.transform.rotation,
            Quaternion.LookRotation(fsm.behavior.Direction), fsm.angular * deltaTime);
        float angle = Vector3.Angle(fsm.transform.forward, fsm.behavior.Direction);

        if (angle < 5f)
        {
            fsm.transform.rotation = Quaternion.LookRotation(fsm.behavior.Direction);
            fsm.Animator.Play(fsm.attackType.ToString());
            state = State.Attack;
        }
    }

    protected virtual void UpdateAttack(float deltaTime)
    {

    }

    protected virtual void UpdateFinish(float deltaTime)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private StateMachine fsm;
    private Volt_ModuleCardExcutor moduleCardExcutor;

    public delegate void AttackDelegate(StateMachine fsm, AttackData attackData);
    public AttackDelegate attackHandler;

    public bool isDoneDamaged = false;
    public bool isDoneFireWave = false;
    public bool isDoneAttack = false;
    public bool isDoneDodge = false;
    public bool isDoneInActivity = false;
    public bool isDoneSpawn = false;
    public bool isDoneStop = false;
    public bool isDoneCollideWithRobot = false;

    private void Awake()
    {
        fsm = GetComponent<StateMachine>();
        moduleCardExcutor = GetComponent<Volt_ModuleCardExcutor>();
    }

    public void AttackAnimationCallback(AttackData attackData)
    {
        //Debug.Assert(attackHandler != null,
            //"닝겐 뭔가 잘되었다냥 이거 아니다냥 공격 기회일 때만 이게 호출되어야 한다냥" +
            //"만약 공격 기회인데 이 오류가 뜬다면 attackHandler를 제대로 할당했는지 확인해라냥");

        if (attackHandler != null)
        {
            attackHandler.Invoke(fsm, attackData);
        }
    }
    public void WinAnimationEffectPlay(AttackData attackData)
    {
        //foreach (var item in attackData.effectData)
        //{
        //    Transform parent = transform.Find(item.launchPointPath);
        //    Volt_ParticleManager.Instance.PlayParticle(item.particlePrefab, parent, true);
        //}
    }

    public void OnDonePowerbeamAnimationCallback()
    {
        fsm.Owner.OnEndBeamCallback();
        isDoneAttack = true;
    }

    public void OnFireWaveCallback()
    {
        fsm.Owner.UseExtraCard();
        //Debug.Assert(moduleCardExcutor.ExcuteExtraCard(), "야 큰일 남 FireWave 사용이 제대로 안됨 확인해보셈 ㅇㅇ");
    }

    public void OnDoneAttackAnimationCallback()
    {
        isDoneAttack = true;
    }

    public void OnDoneDamagedAnimationCallback()
    {
        isDoneDamaged = true;
    }

    public void OnDoneFireWaveAnimationCallback()
    {
        isDoneFireWave = true;
    }

    public void OnDoneDodgeAnimationCallback()
    {
        isDoneDodge = true;
    }

    public void OnDoneInActivityAnimationCallback()
    {
        isDoneInActivity = true;
    }

    public void OnDoneSpawnAnimationCallback()
    {
        Debug.Log("Done Sapwn");
        isDoneSpawn = true;
    }

    public void OnDoneStopAnimationCallback()
    {
        isDoneStop = true;
    }

    public void OnDoneCollideWithRobotAnimationCallback()
    {
        isDoneCollideWithRobot = true;
    }
}

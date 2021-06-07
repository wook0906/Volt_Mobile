using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/States/DodgeState")]
public class DodgeState : StateBase
{
    private void ForwardToAttacker(Transform my, Volt_Robot attacker)
    {
        if (attacker == null)
            return;

        Vector3 toAttacker = (attacker.transform.position - my.position).normalized;
        my.rotation = Quaternion.LookRotation(toAttacker);
    }

    public override void OnEnterState(StateMachine fsm)
    {
        fsm.isDodgeAttack = false;
        RobotBehaviourObserver.Instance.OnBehaviorFlag(fsm.Owner.playerInfo.playerNumber);

        fsm.Owner.moduleCardExcutor.GetModuleCardByCardType(Card.DODGE).OnUseCard();
        Volt_GMUI.S.Create2DMsg(MSG2DEventType.UseDodge, fsm.Owner.playerInfo.playerNumber);

        if(fsm.attackInfo!=null)
            ForwardToAttacker(fsm.transform, Volt_PlayerManager.S.GetPlayerByPlayerNumber(fsm.attackInfo.AttackerNumber).GetRobot());

        Managers.Resource.LoadAsync<AudioClip>("Assets/_SFX/VOLT_Soundsource_20200623/Module/module_DODGE.wav",
            (result) =>
            {
                Volt_SoundManager.S.RequestSoundPlay(result.Result, false);
            });
        
        Volt_GMUI.S.Create3DMsg(MSG3DEventType.Miss, fsm.Owner.playerInfo);
        fsm.Animator.CrossFade("Dodge", .1f);
    }

    public override void OnExitState(StateMachine fsm)
    {
        fsm.Owner.moduleCardExcutor.DestroyCard(Card.DODGE);
        fsm.AniEventHandler.isDoneDodge = false;
        fsm.isDodgeAttack = false;
    }

    public override void Action(StateMachine fsm, float deltaTime)
    {

    }
}

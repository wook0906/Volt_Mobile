using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack : MeleeAttack
{
    public override void AttackHandler(StateMachine fsm, AttackData attackData)
    {
        moduleCard.OnUseCard();
        base.AttackHandler(fsm, attackData);
    }
}

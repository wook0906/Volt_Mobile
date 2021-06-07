using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackFactory
{
    public static BaseAttack CreateAttackInstance(StateMachine fsm,
        Volt_ModuleCardBase moduleCard)
    {
        if (moduleCard == null)
        {
            return new MeleeAttack();
        }

        switch (moduleCard.card)
        {
            case Card.DOUBLEATTACK:
                return new DoubleAttack();
            case Card.CROSSFIRE:
                return new CrossFireAttack();
            case Card.POWERBEAM:
                return new PowerbeamAttack();
            case Card.PERNERATE:
                return new PernerateAttack();
            case Card.EMP:
                return new EMPAttack();
            case Card.TELEPORT:
                return new Teleport();
            case Card.TIMEBOMB:
                return new TimeBombAttack();
            case Card.GRENADES:
                return new GrenadesAttack();
        }

        return new MeleeAttack();
    }
}

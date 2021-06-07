using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoAction
{
    IEnumerator CoAction(StateMachine fsm);
}

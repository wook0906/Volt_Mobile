using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController
{
    protected StateMachine fsm;
    protected Transform robot;

    public BaseController(StateMachine fsm, Transform robot)
    {
        this.fsm = fsm;
        this.robot = robot;
    }

    public abstract void Update(float deltaTime);
}

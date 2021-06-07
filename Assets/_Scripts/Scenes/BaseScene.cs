using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.UnKnown;

    protected virtual void Init()
    {
    }

    public abstract void Clear();

    public abstract float Progress { get; }
    public abstract bool IsDone { get; }
}

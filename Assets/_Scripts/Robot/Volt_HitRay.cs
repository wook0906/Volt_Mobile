using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitRayOption
{
    NonPernerate,
    Pernerate
}

public enum RayType
{
    HitRay,
    EffectRay,
    PushRay,
    EMP_Ray
}

public class Volt_HitRay
{
    private Ray         ray;
    public  Ray         Ray { get { return ray; } }
    private float       rayDistance;
    public  float       RayDistance { get { return rayDistance; } }
    private LayerMask   whatIsBot;
    public  LayerMask   WhatIsBot { get { return whatIsBot; } }
    private CameraShakeType camShakeType;
    public CameraShakeType CamShakeType { get { return camShakeType; } }
    /// <summary>
    /// hitCount 혹은 pushCount의 값이 들어간다.
    /// </summary>
    private int count = 0;
    public  int Count { get { return count; } }
    int behaviourPoints; //타겟이 없을때의 파워빔을 위해서...
    public int BehaviourPoints { get { return behaviourPoints; } }

    public Volt_HitRay(Ray ray, float rayDistance, LayerMask whatIsBot, int count = 1, CameraShakeType camShakeType = CameraShakeType.None)
    {
        this.ray = ray;
        this.rayDistance = rayDistance;
        this.whatIsBot = whatIsBot;
        this.count = count;
        this.camShakeType = camShakeType;
    }

    public Volt_HitRay(Vector3 origin, Vector3 direction, float rayDistance, LayerMask whatIsBot, int count = 1, CameraShakeType camShakeType = CameraShakeType.None)
    {
        Ray ray = new Ray(origin, direction);
        this.ray = ray;
        this.rayDistance = rayDistance;
        this.whatIsBot = whatIsBot;
        this.count = count;
        this.camShakeType = camShakeType;
    }
    public Volt_HitRay(Vector3 origin, Vector3 direction, float rayDistance, LayerMask whatIsBot, int behaviourPoints, int count = 1, CameraShakeType camShakeType = CameraShakeType.None)
    {
        Ray ray = new Ray(origin, direction);
        this.ray = ray;
        this.rayDistance = rayDistance;
        this.whatIsBot = whatIsBot;
        this.behaviourPoints = behaviourPoints;
        this.count = count;
        this.camShakeType = camShakeType;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackInfor
{
    public GameObject pusher;
    public Vector3 direction;
    public Volt_Tile startTile;
    public int range;
    public CameraShakeType cameraShakeType = CameraShakeType.None;

    public KnockbackInfor(GameObject who, Vector3 direction, Volt_Tile startTile, int range = 1, CameraShakeType cameraShakeType = CameraShakeType.None)
    {
        pusher = who;
        this.direction = direction;
        this.range = range;
        this.startTile = startTile;
        this.cameraShakeType = cameraShakeType;
    }
}

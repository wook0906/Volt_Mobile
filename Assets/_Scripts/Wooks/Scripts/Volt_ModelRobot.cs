using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_ModelRobot : MonoBehaviour
{
    public void Init(RobotType robotType)
    {
        transform.localPosition = new Vector3(0f, -0.65f, 0f);
        gameObject.GetOrAddComponent<Volt_WinMuzzleEffect>().Init(robotType);
        
        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        if (robotType == RobotType.Hound)
            transform.localRotation = Quaternion.Euler(0f, -165f, 0f);
    }
}

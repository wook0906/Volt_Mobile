using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Volt_WinMuzzleEffect : MonoBehaviour
{
    private GameObject winEffect;
    private Transform[] launchPoint;

    public void Init(RobotType robotType)
    {
        SetWinMuzzleEffectLauchPoint(robotType);
        LoadWinMuzzleEffect(robotType);
    }

    private void SetWinMuzzleEffectLauchPoint(RobotType robotType)
    {
        switch (robotType)
        {
            case RobotType.Volt:
                {
                    Transform rHand = Util.FindChild<Transform>(gameObject, "Dummy_R_Hand", true);
                    launchPoint = new Transform[1];
                    launchPoint[0] = Util.FindChild<Transform>(rHand.gameObject,
                        "LaunchPoint", true);
                }
                break;
            case RobotType.Mercury:
                {
                    Transform lHand = Util.FindChild<Transform>(gameObject, "Bone_R_ForeArm", true);
                    Transform rHand = Util.FindChild<Transform>(gameObject, "Bone_L_ForeArm", true);
                    launchPoint = new Transform[2];
                    launchPoint[0] = Util.FindChild<Transform>(lHand.gameObject, "WinMuzzleLaunchPoint", true);
                    launchPoint[1] = Util.FindChild<Transform>(rHand.gameObject, "WinMuzzleLaunchPoint", true);
                }
                break;
            case RobotType.Reaper:
                {
                    Transform gun = Util.FindChild<Transform>(gameObject, "D_Gun", true);
                    launchPoint = new Transform[1];
                    launchPoint[0] = Util.FindChild<Transform>(gun.gameObject, "Win_LaunchPoint", true);
                }
                break;
            case RobotType.Hound:
                {
                    Transform cannon = Util.FindChild<Transform>(gameObject, "Dummy_Canon", true);
                    launchPoint = new Transform[1];
                    launchPoint[0] = Util.FindChild<Transform>(cannon.gameObject, "WinMuzzleLaunchPoint", true);
                }
                break;
            default:
                break;
        }
    }

    private void LoadWinMuzzleEffect(RobotType type)
    {
        switch (type)
        {
            case RobotType.Volt:
                Managers.Resource.LoadAsync<GameObject>($"VFX_VoltMuzzle",
                    (result) => { winEffect = result.Result; });
                break;
            case RobotType.Mercury:
                Managers.Resource.LoadAsync<GameObject>($"VFX_MercuryMuzzle",
                    (result) => { winEffect = result.Result; });
                break;
            case RobotType.Hound:
                Managers.Resource.LoadAsync<GameObject>($"VFX_HoundMuzzle",
                    (result) => { winEffect = result.Result; });
                break;
            case RobotType.Reaper:
                Managers.Resource.LoadAsync<GameObject>($"VFX_ReaperMuzzle",
                    (result) => { winEffect = result.Result; });
                break;
            default:
                break;
        }
    }

    public void PlayWinEffect()
    {
        foreach (var item in launchPoint)
        {
            GameObject go = Instantiate(winEffect, item);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
        }

    }
}

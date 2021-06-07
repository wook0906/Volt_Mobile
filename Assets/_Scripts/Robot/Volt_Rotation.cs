using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Rotation
{
    public Transform _owner { get; set; }
    private float angular;

    /// <summary>
    /// 시계 방향으로 90도
    /// </summary>
    private const float cw = 90f;
    /// <summary>
    /// 반 시계 방향으로 90도
    /// </summary>
    private const float ccw = -90f;
    /// <summary>
    /// 시계 방향으로 180도
    /// </summary>
    private const float flip = 180f;
    private const float offset = 10f; // 각도 오차 범위

    public Volt_Rotation(Transform owner, float angular)
    {
        _owner = owner;
        this.angular = angular;
    }

    public IEnumerator RotateBy(Vector3 axis, float angle)
    {
        Vector3 from = _owner.rotation.eulerAngles;
        Vector3 to = _owner.rotation.eulerAngles + axis * angle;
        float rotationTime = Mathf.Abs(angle / angular);
        float elapsedTime = 0f;
        float u = 0f;

        while (u < 1f)
        {
            Vector3 eulerAnlges = Vector3.Lerp(from, to, u);
            _owner.rotation = Quaternion.Euler(eulerAnlges);
            u = elapsedTime / rotationTime;
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        _owner.rotation = Quaternion.Euler(to);
        //Communication.SendKillbotBehaivorDonePacket();
    }

    /// <summary>
    /// 로봇이 바라보는 방향과 타겟 타일의 좌표(월드좌표)를 이용해
    /// 로봇이 회전해야하는 방향과 각도 값을 계산하여 반환한다.
    /// </summary>
    /// <param name="botTrans"></param>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public static float GetAngle(Transform botTrans, Vector3 targetPos)
    {
        Vector3 target = (targetPos - botTrans.position).normalized;

        return GetAngle(botTrans.forward, target);
    }

    /// <summary>
    /// 로봇이 바라보는 방향과 회전 후 바라보게 될 방향을 이용해
    /// 로봇이 회전해야하는 방향과 각도 값을 계산하여 반환한다.
    /// </summary>
    /// <param name="botForward"></param>
    /// <param name="rotationDir"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 botForward, Vector3 rotationDir)
    {
        //Debug.Log("botFoward: " + botForward + ", rotationDir: " + rotationDir);
        float theta = Vector3.Dot(botForward, rotationDir);
        //Debug.Log("before Theta: " + theta);
        theta = Mathf.Clamp(theta, -1f, 1f);
        //Debug.Log("After theta: " + theta);
        float angle = Mathf.Acos(theta) * Mathf.Rad2Deg;
        if (angle <= float.Epsilon * Mathf.Pow(2, 5))
            return 0f;

        Vector3 normal = Vector3.Cross(botForward, rotationDir);
        if (normal.y > 0)
            return angle;
        return -angle;
    }

    public static float GetAngleUsingSectionAndDirection(ArenaSection section, Vector3 forward)
    {
        float angle = 0f;
        switch (section)
        {
            case ArenaSection.Center:
                angle = flip;
                break;
            case ArenaSection.CenterBack:
                angle = flip;
                break;
            case ArenaSection.CenterTop:
                angle = flip;
                break;
            case ArenaSection.CenterLeft:
                angle = flip;
                break;
            case ArenaSection.CenterRight:
                angle = flip;
                break;
            case ArenaSection.OneQudrant:
                if (Volt_Utils.IsBackward(forward) || Volt_Utils.IsRight(forward))
                    angle = cw;
                else angle = ccw;
                break;
            case ArenaSection.TwoQudrant:
                if (Volt_Utils.IsForward(forward) || Volt_Utils.IsRight(forward))
                    angle = cw;
                else angle = ccw;
                break;
            case ArenaSection.ThreeQudrant:
                //Debug.Log("ThreeQudrant");
                if (Volt_Utils.IsForward(forward) || Volt_Utils.IsLeft(forward))
                    angle = cw;
                else angle = ccw;
                break;
            case ArenaSection.FourQudrant:
                //Debug.Log("FourQudrant");
                if (Volt_Utils.IsBackward(forward) || Volt_Utils.IsLeft(forward))
                    angle = cw;
                else
                    angle = ccw;
                break;
            default:
                angle = 0f;
                break;
        }
        //Debug.Log("Angle: " + angle);
        return angle;
    }

    /// <summary>
    /// 현재 봇이 서있는 타일이 속한 구역과 현재 봇이 바라보는 방향을 기준으로
    /// 봇이 회전해야하는 방향과 각도 값을 계산하여 얻어온다.
    /// (각도 값은 90도 혹은 180도이다.)
    /// </summary>
    /// <param name="section"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static float GetAngleUsingSectionAndDirection(ArenaSection section, Transform transform)
    {
        return GetAngleUsingSectionAndDirection(section, transform.forward);
    }
}
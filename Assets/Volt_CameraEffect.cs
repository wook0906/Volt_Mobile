using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum CameraShakeType
{
    None, Attack, Damaged, RepulsionBlast, CrossFire, Grenade, Timebomb, PowerBeam, SawBlade,
    ShockWave, DoubleAttack, Pernerate, Bomb, EMP, Amargeddon, Satlite, Ballista
}


public class Volt_CameraEffect : MonoBehaviour
{
    public bool isShakePlaying = false;
    public bool isSaturating = false;

    float duration = 0f;
    float magnitude = 0f;
    Vignette vignette = null;
    ColorParameter color = new ColorParameter();
    FloatParameter intensity = new FloatParameter();
    FloatParameter smoothness = new FloatParameter();

    private void Start()
    {
        if (Volt_GameManager.S)
        {
            if (Volt_GameManager.S.postProcessVolume.profile.TryGetSettings<Vignette>(out vignette))
            {
                //Debug.Log("Vigenette 찾았당께");
            }
            else
            {
                //Debug.Log("그뭔씹");
            }
        }
    }
    private void FixedUpdate()
    {
        
    }
    public void SetVignette(Color newColor)
    {
        if (Volt_GameManager.S.pCurPhase == Phase.gameOver || isSaturating) return;

        if (vignette)
        {
            //Debug.Log("SetVignette");
            vignette.active = true;
            color.value = newColor;
            vignette.color.value = color;
            intensity.value = 0.5f;
            vignette.intensity.value = intensity;
            smoothness.value = 0.2f;
            vignette.smoothness.value = smoothness;
            StartCoroutine(DoVignette());
        }
    }
    public void SetShakeType(CameraShakeType shakeType)
    {
        if (Volt_GameManager.S.pCurPhase == Phase.gameOver) return;
        //Debug.Log("SetShakeType : " + shakeType.ToString());
        switch (shakeType)
        {
            case CameraShakeType.None:
                duration = 0f;
                magnitude = 0f;
                break;
            case CameraShakeType.Ballista:
                duration = 1.7f;
                magnitude = 1f;
                break;
            case CameraShakeType.Satlite:
                duration = 2f;
                magnitude = 2f;
                break;
            case CameraShakeType.Amargeddon:
                duration = 2.5f;
                magnitude = 3f;
                break;
            case CameraShakeType.EMP:
                duration = 1.7f;
                magnitude = 0.25f;
                break;
            case CameraShakeType.Pernerate:
                duration = 0.51f;
                magnitude = 0.6f;
                break;
            case CameraShakeType.DoubleAttack:
                duration = 1.7f;
                magnitude = 0.6f;
                break;
            case CameraShakeType.ShockWave:
                duration = 1.7f;
                magnitude = 1f;
                break;
            case CameraShakeType.SawBlade:
                duration = 0.34f;
                magnitude = 0.5f;
                break;
            case CameraShakeType.PowerBeam:
                duration = 0.85f;
                magnitude = 1f;
                break;
            case CameraShakeType.Timebomb:
                duration = 1.7f;
                magnitude = 0.25f;
                break;
            case CameraShakeType.Grenade:
                duration = 1.7f;
                magnitude = 1f;
                break;
            case CameraShakeType.CrossFire:
                duration = 1.7f;
                magnitude = 0.25f;
                break;
            case CameraShakeType.RepulsionBlast:
                duration = 1.7f;
                magnitude = 1f;
                break;
            case CameraShakeType.Bomb:
                duration = 1.7f;
                magnitude = 1f;
                break;
            case CameraShakeType.Attack:
                duration = 1.7f;
                magnitude = 0.25f;
                break;
            case CameraShakeType.Damaged:
                duration = 1.7f;
                magnitude = 0.5f;
                break;
            default:
                duration = 1.7f;
                magnitude = 0.5f;
                break;
        }
    }
    public void VignetteOff()
    {
        vignette.active = false;
    }
    public IEnumerator Shake()//CameraShakeType으로 값, 움직임 조절가능하도록 수정
    {
        if (Volt_GameManager.S.pCurPhase == Phase.gameOver ||
            Volt_GameManager.S.isRangeSelectCameraMoving) yield break;
        //Debug.Log("Is Shake Playing? : " + isShakePlaying);
        if (isShakePlaying) yield break;

        //Debug.Log("Shaking...");
        isShakePlaying = true;
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = originalPos.z + Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x / 4, y / 4, z);
            magnitude = Mathf.Lerp(magnitude, 0, 0.05f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
        isShakePlaying = false;

    }
    public IEnumerator DoVignette()
    {
        if (Volt_GameManager.S.pCurPhase == Phase.gameOver) yield break;

        //Debug.Log("saturating start");
        isSaturating = true;
        float elapsed = 0f;

        while (elapsed < 1.7f)
        {
            float tmp = intensity.value -= Time.deltaTime * 0.5f;
            if (tmp >= 0)
            {
                vignette.intensity.value = tmp;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        //Debug.Log("saturating End");
        isSaturating = false;
        VignetteOff();
    }

}

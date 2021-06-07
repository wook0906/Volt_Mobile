using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_ParticleManager : MonoBehaviour
{
    public static Volt_ParticleManager Instance;
    [HideInInspector]
    public List<FXV.FXVShield> shieldEffects;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else
            //Destroy(gameObject);
    }
    public void Init()
    {
        //for(int i = 0; i < 3; ++i)
        //{
        //    GameObject shieldEffect = Volt_PrefabFactory.S.Instantiate(Volt_PrefabFactory.S.shieldEffect, Vector3.one * 1000f, Quaternion.identity);
        //    shieldEffects.Add(shieldEffect.GetComponent<FXV.FXVShield>());
        //}

        //Debug.Log(shieldEffects.Count+" @@@@@@ shield");
    }
    public FXV.FXVShield GetShieldEffect()
    {
        GameObject go = Volt_PrefabFactory.S.PopEffect(Define.Effects.VFX_SHIELD);
        FXV.FXVShield shieldEffect = go.GetComponent<FXV.FXVShield>();
        //shieldEffects.RemoveAt(0);
        return shieldEffect;
    }
    public void ReturnShieldEffect(FXV.FXVShield effect)
    {
        Managers.Pool.Push(effect.GetComponent<Poolable>());
        //shieldEffects.Add(effect);
    }
    public void DelayedPlayParticleOwnPosition(Define.Effects type, Transform ownTransform, float delayTime)
    {
        StartCoroutine(DelayedPlayOwnPosition(type, ownTransform, delayTime));
    }
    IEnumerator DelayedPlayOwnPosition(Define.Effects type, Transform ownTransform, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Vector3 pos = Volt_ArenaSetter.S.GetTile(ownTransform.position).transform.position;
        pos.y += 0.65f * Volt_ArenaSetter.S.tileScale.x;
        PlayParticle(Volt_PrefabFactory.S.PopEffect(type), pos);
    }

    public void PlayParticle(GameObject particleGO, Vector3 pos)
    {
        particleGO.transform.position = pos;
    }

    public void PlayParticle(GameObject particleGO, Transform parent, float heightDetailAdjustmentValue = 0f)
    {
        //GameObject particleGO = Instantiate<GameObject>(prefab, parent);
        particleGO.transform.SetParent(parent);
        Vector3 pos = Vector3.zero;
        pos.y += heightDetailAdjustmentValue;
        particleGO.transform.localPosition = Vector3.zero;
        particleGO.transform.localPosition = pos;
        particleGO.transform.localRotation = Quaternion.identity;
        particleGO.transform.localScale = Vector3.one;
    }

    public void PlayParticle(GameObject particleGO, bool useOwnTransform, Transform parent)
    {
        if (useOwnTransform)
        {
            particleGO.transform.SetParent(parent);
            particleGO.transform.localPosition = Vector3.zero;
            particleGO.transform.localScale = Vector3.one;
        }
        else
        {
            particleGO.transform.SetParent(parent);
            particleGO.transform.localPosition = Vector3.zero;
            particleGO.transform.localRotation = Quaternion.identity;
            particleGO.transform.localScale = Vector3.one;
        }
    }

    public void PlayParticle(GameObject particleGO, Transform parent, bool isSetParent, float scaleMultValue = 1f)
    {
        if (isSetParent)
        {
            particleGO.transform.SetParent(parent);
            particleGO.transform.localPosition = Vector3.zero;
            particleGO.transform.localRotation = Quaternion.identity;
            particleGO.transform.localScale = Vector3.one;
        }
        else
        {
            particleGO.transform.position = parent.transform.position;
            particleGO.transform.rotation = Quaternion.identity;
        }
        foreach (Transform item in particleGO.GetComponentsInChildren<Transform>())
        {
            if (item.CompareTag("NeedScalingParticle"))
            {
                item.GetComponent<ParticleSystemRenderer>().lengthScale = scaleMultValue;
            }
        }
       
    }

    /// <summary>
    /// 부모오브젝트를 갖지 않음
    /// </summary>
    /// <param name="prefab"></param>
    public void PlayParticle(GameObject particleGO, Vector3 pos, Quaternion Rot)
    {
        particleGO.transform.position = pos;
        particleGO.transform.rotation = Rot;
    }
}

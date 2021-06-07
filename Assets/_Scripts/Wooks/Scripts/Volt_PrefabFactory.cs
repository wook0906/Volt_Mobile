using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Define;
using System;

public class Volt_PrefabFactory : MonoBehaviour
{
    public static Volt_PrefabFactory S;

    public GameObject robotPanel;
    // Start is called before the first frame update
    private void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        else
        {
            S = this;
        }
    }

    private Dictionary<Effects, GameObject> vfxPrefabs = new Dictionary<Effects, GameObject>();
    private Dictionary<Objects, GameObject> objectPrefabs = new Dictionary<Objects, GameObject>();

    private IEnumerator Start()
    {
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>("VFX", (origin) =>
        {
            Effects type = (Effects)Enum.Parse(typeof(Effects), origin.name);
            if(vfxPrefabs.ContainsKey(type))
            {
                Debug.LogError($"[VFX]Same key:{origin.name}");
                Debug.LogError($"[VFX]Origin:{vfxPrefabs[type].name}");
                return;
            }
            vfxPrefabs.Add(type, origin);
        });

        AsyncOperationHandle<IList<GameObject>> objectsHandle = Addressables.LoadAssetsAsync<GameObject>("Object", (origin) =>
        {
            Objects type = (Objects)Enum.Parse(typeof(Objects), origin.name);
            if (objectPrefabs.ContainsKey(type))
            {
                Debug.LogError($"[Object]Same key:{origin.name}");
                Debug.LogError($"[Object]Origin:{objectPrefabs[type].name}");
                return;
            }
            objectPrefabs.Add(type, origin);
        });

        yield return new WaitUntil(() => { return handle.IsDone && Managers.Data.VFXPoolData != null; ; });

        foreach (var item in vfxPrefabs)
        {
            int count = Managers.Data.VFXPoolData.creationInfos[item.Key];
            Managers.Pool.CreatePool(item.Value, count);
        }

        yield return new WaitUntil(() =>
        {
            return objectsHandle.IsDone && Managers.Data.ObjectData != null;
        });

        foreach (var item in objectPrefabs)
        {
            int count = Managers.Data.ObjectData.creationInfos[item.Key];
            Managers.Pool.CreatePool(item.Value, count);
        }
    }

    public GameObject PopEffect(Effects type)
    {
        return Managers.Pool.Pop(vfxPrefabs[type]).gameObject;
    }

    public void PushEffect(Poolable poolable)
    {
        Managers.Pool.Push(poolable);
    }

    public GameObject PopObject(Objects type)
    {
        if(!objectPrefabs.ContainsKey(type))
        {
            Debug.LogError($"[Ojbect]Not contain Key:{type}");
            return null;
        }
        Poolable poolable = Managers.Pool.Pop(objectPrefabs[type]);
        if(poolable == null)
        {
            Debug.LogError($"Pool empty:{type}");
            return null;
        }
        return poolable.gameObject;
    }

    public void PushObject(Poolable poolable)
    {
        Managers.Pool.Push(poolable);
    }

    public SkinType SkinTypeDecisionByRobotType(RobotType robotType, SkinType skinType)
    {
        if (skinType == SkinType.OriginBlue || skinType == SkinType.OriginRed)
        {
            if (robotType == RobotType.Volt || robotType == RobotType.Hound)
            {
                if (skinType == SkinType.OriginBlue)
                    return skinType;
                else
                {
                    //Debug.Log(skinType.ToString()+" Swap To " + SkinType.OriginBlue.ToString());
                    return SkinType.OriginBlue;
                }
            }
            else
            {
                if (skinType == SkinType.OriginRed)
                    return skinType;
                else
                {
                    //Debug.Log(skinType.ToString() + " Swap To " + SkinType.OriginRed.ToString());
                    return SkinType.OriginRed;
                }
            }
        }
        return skinType;
    }

    //public GameObject GetInstance(GameObject prefab)
    //{
    //    return GameObject.Instantiate<GameObject>(prefab);
    //}

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        return GameObject.Instantiate<GameObject>(prefab, parent);
    }

    public GameObject Instantiate(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        return GameObject.Instantiate<GameObject>(prefab, pos, rotation);
    }

    //public void DelayedPlayParticleOwnPosition(GameObject prefab, Vector3 pos, float delayTime)
    //{
    //    StartCoroutine(CoPlayParticleDelayed(prefab, delayTime, pos));
    //}

    //IEnumerator CoPlayParticleDelayed(GameObject prefab, float delayTime, Vector3 pos)
    //{
    //    yield return new WaitForSeconds(delayTime);

    //    Instantiate(prefab, pos, Quaternion.identity);
    //}
}

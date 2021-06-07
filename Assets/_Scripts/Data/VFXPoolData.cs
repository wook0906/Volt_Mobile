using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class VFXPool
{
    public Effects type;
    public int count;
}

[CreateAssetMenu(menuName = "PoolDatas/VFXPoolData")]
public class VFXPoolData : ScriptableObject
{
    [SerializeField]
    private List<VFXPool> data;

    public Dictionary<Effects, int> creationInfos = new Dictionary<Effects, int>();

    public void Init()
    {
        foreach (var item in data)
        {
            creationInfos.Add(item.type, item.count);
        }
    }
}

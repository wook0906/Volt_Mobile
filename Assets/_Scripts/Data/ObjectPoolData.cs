using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class ObjectPool
{
    public Objects type;
    public int count;
}

[CreateAssetMenu(menuName = "PoolDatas/ObjectPoolData")]
public class ObjectPoolData : ScriptableObject
{
    [SerializeField]
    private List<ObjectPool> data;

    public Dictionary<Objects, int> creationInfos = new Dictionary<Objects, int>();

    public void Init()
    {
        foreach (var item in data)
        {
            creationInfos.Add(item.type, item.count);
        }
    }
}

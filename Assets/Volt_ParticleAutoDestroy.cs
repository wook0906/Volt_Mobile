using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_ParticleAutoDestroy : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem[] pss;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (!ps)
        {
           pss = GetComponentsInChildren<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ps)
        {
            if (!ps.IsAlive(true))
            {
                Managers.Pool.Push(GetComponent<Poolable>());
                //Destroy(this.gameObject);
            }
        }
        else
        {
            if (IsAllParticlePlayDone())
            {
                Managers.Pool.Push(GetComponent<Poolable>());
                //Destroy(this.gameObject);
            }
        }

            
    }
    bool IsAllParticlePlayDone()
    {
        foreach (var item in pss)
        {
            if (item.IsAlive(true))
                return false;
        }
        return true;
    }
}

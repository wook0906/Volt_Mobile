using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Ballista : MonoBehaviour
{
    public float speed;
    public float destroyTime;
    Vector3 moveDir;
    List<Volt_Tile> targetTiles;
    bool isInitialized = false;

    private void OnEnable()
    {
        Invoke("DelayedDestroy", destroyTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (isInitialized)
        {
            transform.position += moveDir * Time.fixedDeltaTime * speed;
            transform.Rotate(0f, 0f, 15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Robot"))
        {
            other.gameObject.GetComponent<Volt_Robot>().GetDamage(new AttackInfo(0, 1, CameraShakeType.Ballista));
        }
    }
    public void Init(Vector3 dir, List<Volt_Tile> targetTiles)
    {
        moveDir = dir;
        transform.rotation = Quaternion.LookRotation(dir,Vector3.up);
        this.targetTiles = targetTiles;
        isInitialized = true;

    }
    void DelayedDestroy()
    {
        if (!gameObject.activeSelf)
            return;

        foreach (var item in targetTiles)
        {
            item.SetDefaultBlinkOption();
            item.BlinkOn = false;
        }

        isInitialized = false;
        transform.rotation = Quaternion.identity;
        Volt_PrefabFactory.S.PushObject(GetComponent<Poolable>());
        //Destroy(gameObject);
    }
}

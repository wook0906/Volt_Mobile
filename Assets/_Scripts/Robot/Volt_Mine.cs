using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Mine : MonoBehaviour
{/*
    public int damage;

    public bool __________________;

    [SerializeField]
    private GameObject owner;
    [SerializeField]
    private Volt_Module_Mine mineModule;

    public void Init(GameObject owner, Volt_Module_Mine mineModule)
    {
        this.owner = owner;
        this.mineModule = mineModule;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject != owner)
        {
            mineModule.AddMine();
            other.SendMessage("GetDamage", damage);
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        //TODO: Play explode effet
    }

    public void Move(int distance, Vector3 direction)
    {
        Volt_Tile targetTile = Volt_ArenaSetter.S.GetTile(transform.position, direction, distance);
        StartCoroutine(MoveTo(targetTile));
    }

    private IEnumerator MoveTo(Volt_Tile targetTile)
    {
        Vector3 start = transform.position;
        Vector3 end = targetTile.transform.position;
        end.y = start.y;

        float u = 0f;
        float elapsedTime = 0f;
        float moveTime = (end - start).magnitude / 6f;
        while (true)
        {
            if (u <= 0f)
            {

            }
            else if (u > 0 && u < 1)
            {
                Vector3 pos = Vector3.Lerp(start, end, u);
                transform.position = pos;
            }
            else
            {
                transform.position = end;
                break;
            }
            u = elapsedTime / moveTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    */
}
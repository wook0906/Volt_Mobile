using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Projectile : MonoBehaviour
{
    public float moveTime;
    public int count = 3;
    protected Transform launcherRoot;
    protected Volt_Robot owner;
    protected bool isEndMove = false;

    public int ownerPlayerNumber; //for sync;

    public void Initialize(Volt_Robot owner, Transform launcherRoot)
    {
        this.owner = owner;
        this.ownerPlayerNumber = owner.gameObject.GetComponent<Volt_Robot>().playerInfo.playerNumber;
        this.launcherRoot = launcherRoot;
    }

    public Volt_Robot GetOwner()
    {
        if (owner != null)
            return owner.GetComponent<Volt_Robot>();
        else
        {
            Debug.Log("GetOwner Error");
            return null;
        }
    }
    public void SetOwner(int ownerPlayerNumber)
    {
        this.ownerPlayerNumber = ownerPlayerNumber;
    }
    public void SetLauncher(Transform launcherTrans)
    {
        this.launcherRoot = launcherTrans;
    }

    public void DoMove(Volt_Tile targetTile)
    {
        StartCoroutine(MoveTo(targetTile));
    }

    protected virtual IEnumerator MoveTo(Volt_Tile targetTile)
    {
        Vector3 from = launcherRoot.position;
        from.y += 1f; // 삭제 예정
        Vector3 to = targetTile.GetComponent<Collider>().bounds.center;
        to.y += targetTile.GetComponent<Collider>().bounds.extents.y + GetComponent<Collider>().bounds.extents.y;

        Vector3 mid = Vector3.Lerp(from, to, .3f);
        mid.y = from.y + 4.5f;

        float elapsedTime = 0f;
        float u = 0f;
        while (u < 1f)
        {
            Vector3 vec1 = Vector3.Lerp(from, mid, u);
            Vector3 vec2 = Vector3.Lerp(mid, to, u);
            Vector3 pos = Vector3.Lerp(vec1, vec2, u);
            transform.position = pos;

            u = elapsedTime / moveTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
        isEndMove = true;
    }
}

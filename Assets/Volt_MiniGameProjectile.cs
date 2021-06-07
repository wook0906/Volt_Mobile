using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_MiniGameProjectile : MonoBehaviour
{
    GameObject aimPointGo;
    int itemType = -1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(Vector3 aimPointPos, GameObject aimPoint, float speed, float waitTimeBeforeMoveObj, int itemType)
    {
        this.itemType = itemType;
        aimPointGo = aimPoint;
        transform.LookAt(aimPointPos);
        StartCoroutine(MoveTo(aimPointPos, speed, waitTimeBeforeMoveObj));
    }
    IEnumerator MoveTo(Vector3 aimPointPos, float speed, float waitTimeBeforeMoveObj)
    {

        yield return new WaitForSeconds(waitTimeBeforeMoveObj);
        Vector3 start = transform.position;
        float elapsedTime = 0;
        float time = (aimPointPos - transform.position).magnitude / speed;
        float u = 0f;

        while (true)
        {
            if (u == 0f)
            {

            }
            else if (u > 0f && u < 1f)
            {
                Vector3 pos = Vector3.Lerp(start, aimPointPos, u);
                transform.position = pos;
            }
            else
            {
                Destroy(aimPointGo);
                Destroy(this.gameObject);
                break;
            }

            elapsedTime += Time.fixedDeltaTime;
            u = elapsedTime / time;
            yield return new WaitForFixedUpdate();
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            other.SendMessage("GetItem", itemType, SendMessageOptions.DontRequireReceiver);
            Destroy(aimPointGo);
            Destroy(this.gameObject);
        }
        
    }

}
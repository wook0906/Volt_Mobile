using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_2dUIMsgHandler : MonoBehaviour
{
    Queue<Volt_2dUIMsg> msgWaitingQueue;
    Queue<Volt_2dUIMsg> msgShowingQueue;
    float msgInterval = 0.5f;
    public bool isRenewing = false;
    // Start is called before the first frame update
    void Start()
    {
        msgWaitingQueue = new Queue<Volt_2dUIMsg>();
        msgShowingQueue = new Queue<Volt_2dUIMsg>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
            
    }
    public void RenewMsgStart()
    {
        StartCoroutine(RenewMsg());
    }
    IEnumerator RenewMsg()
    {
        isRenewing = true;
        //Debug.Log("Renew Start!");
        if (msgShowingQueue.Count != 0)
        {
            foreach (var item in msgShowingQueue)
            {
                item.MoveUpStart();
            }
            //Debug.Log(msgShowingQueue.Count + "개의 메시지가 출력중임");
            yield return new WaitUntil(() => IsAllMsgStopped());
        }
        Volt_2dUIMsg newMsg = msgWaitingQueue.Dequeue();
        msgShowingQueue.Enqueue(newMsg);
        newMsg.Show();

        yield return new WaitForSecondsRealtime(msgInterval);
        if (msgWaitingQueue.Count != 0)
        {
            RenewMsgStart();
            //Debug.Log("Waiting Queue count : " + msgWaitingQueue.Count);
        }
        else
            isRenewing = false;
    }
    bool IsAllMsgStopped()
    {
        foreach (var item in msgShowingQueue)
        {
            if (item.isMoving)
                return false;
        }
        return true;
    }
    public void EntryMsg(Volt_2dUIMsg msg)
    {
        msgWaitingQueue.Enqueue(msg);
        if(!isRenewing)
            RenewMsgStart();
    }
    public void RemoveMsg()
    {
        Volt_PrefabFactory.S.PushObject(msgShowingQueue.Dequeue().GetComponent<Poolable>());
    }
}

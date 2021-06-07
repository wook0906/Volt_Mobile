using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MSG3DEventType
{
    PointUp, PointDown, hpUp, hpDown, Miss, NoPoint
}
public class Volt_3dUIMsg : MonoBehaviour
{
    public float speed;
    public float moveRange;

    UISprite bg;

    Vector3 originBGScale;
    Vector3 InitPos;
    
    // Start is called before the first frame update
    void Start()
    {
        InitPos = transform.position;
        bg = transform.Find("BG").GetComponent<UISprite>();
        originBGScale = bg.transform.localScale;
        float randomX = Random.Range(-0.15f, 0.15f);
        float randomY = Random.Range(-0.15f, 0.15f);
        Vector3 pos = transform.position;
        pos.x = randomX;
        pos.y = randomY + InitPos.y;
        transform.position = pos;
    }
    public void StartMove()
    {
        GetComponentInChildren<UISprite>().alpha = 1f;
        StartCoroutine(MoveUp());
    }

    private void FixedUpdate()
    {
        if (!Volt_PlayerManager.S.I) return;
        if (!Volt_PlayerManager.S.I.playerCam) return;

        float distance = Vector3.Distance(Volt_PlayerManager.S.I.playerCam.transform.position
            , Volt_ArenaSetter.S.GetCenterTransform().position);

        Vector3 tmp = originBGScale;
        tmp *= 0.065f * distance;
        bg.transform.localScale = tmp;
    }
    IEnumerator MoveUp()
    {
        Vector3 originPos = transform.position;
        Vector3 tmpPos = transform.position;
        UISprite sprite = GetComponentInChildren<UISprite>();
        float t = 0;
        while (t < moveRange)
        {
            t += Time.fixedDeltaTime * speed;
            tmpPos.y = (t+ originPos.y);
            if (t > 0.4f)
                sprite.alpha -= Time.fixedDeltaTime;
            transform.position = tmpPos;
            yield return null;
        }
        Volt_PrefabFactory.S.PushObject(GetComponent<Poolable>());
        //Destroy(gameObject);
    }
    public void SetMsg(MSG3DEventType msgType, int optionValue = 1)
    {
        UISprite sprite = GetComponentInChildren<UISprite>();
        switch (msgType)
        {
            case MSG3DEventType.PointUp:
                sprite.spriteName = "Point+1";
                break;
            case MSG3DEventType.PointDown:
                sprite.spriteName = "Point-1";
                break;
            case MSG3DEventType.hpUp:
                sprite.spriteName = "HP+1";
                break;
            case MSG3DEventType.hpDown:
                if (optionValue != 1)
                    sprite.spriteName = "HP-" + optionValue.ToString();
                else
                    sprite.spriteName = "HP-1";
                break;
            case MSG3DEventType.Miss:
                sprite.spriteName = "Miss";
                break;
            case MSG3DEventType.NoPoint:
                sprite.spriteName = "NoPoint";
                break;
            default:
                break;
        }
        
    }
}

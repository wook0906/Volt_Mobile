using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum ACHType
{
    Daily,
    Permanent
}

public class ScrollViewItemCreator : MonoBehaviour
{
    public ACHType type;
    //public GameObject itemPrefab;
    public BoxCollider bgCollider;         // 백그라운 이미지의 콜라이더
    public BoxCollider pivotBoxCollider;   // 백그라운드 이미지의 중앙에 위치한 스크롤 뷰 위치 조정용 콜라이더
    public Transform gridTransform;
    public UIPanel scrollViewPanel;
    public UIScrollBar uiScrollBar;
    public UIDragScrollView dragableRect;

    private Dictionary<int, ACHModel> ACHTable = new Dictionary<int, ACHModel>();
    private List<GameObject> items = new List<GameObject>();

    private const float OFFSET = 0.08f; // 스크롤 뷰 위치 조정시 필요한 보정 값.

    private const int SCROLLVIEWHEIGHTPERITEM = 170;
    private const int SCROLLVIEWMAXHEIGHT = 720;
    private const int SCROLLVIEWWIDTH = 1200;

    private bool isInit = false;
    public  bool IsInit { get { return isInit; } }

    private GameObject root;

    public void Init(System.Action callback)
    {
        root = transform.parent.gameObject;
        StartCoroutine(CoInit(callback));
    }
    private IEnumerator CoInit(System.Action callback)
    {
        ACHTable = ACHDataManager.instance.GetACHTable(type);

        if (ACHTable == null || ACHTable.Count == 0)
        {
            //Debug.LogWarning(type.ToString() + " ACHTable null or empty");
            yield break;
        }

        //print("ACHTable Count: " + ACHTable.Count);
        uiScrollBar.value = 0f;
        int count = 0;
        foreach (var ACH in ACHTable)
        {
            ACHModel param = ACH.Value;
            AsyncOperationHandle<GameObject> handle = Managers.UI.MakeSubItemAsync<ACHItem>(gridTransform);
            yield return new WaitUntil(() => { return handle.IsDone; });

            GameObject itemGO = handle.Result;
            itemGO.GetComponent<ACHItem>().Init(param.ID, param.title_KR, param.title_EN, param.title_GER,
                param.title_Fren, param.description_KR, param.description_EN, param.description_GER,
                param.description_Fren, (int)param.rewardType, param.rewardCount,
                param.conditionType, param.conditionCount, param.rewardICON,
                param.progressButtonName_KR, param.progressButtonName_EN, param.progressButtonName_GER,
                param.progressButtonName_Fren, param.getRewardButtonName_KR, param.getRewardButtonName_EN,
                param.getRewardButtonName_GER, param.getRewardButtonName_Fren,
                param.getRewardButtonActiveSprite, param.getRewardButtonUnActiveSprite,
                param.titleFontSize, param.descriptionFontSize, param.rewardCountFontSize,
                param.conditionCountFontSize);
            itemGO.GetComponent<ACHItem>().SetUserConditionCount(Volt_PlayerData.instance.GetACHProgressCount(param.ID),
                Volt_PlayerData.instance.IsAccomplishACH(param.ID));

            itemGO.transform.localScale = Vector3.one;
            itemGO.transform.localPosition = Vector3.zero;
            Vector3 moveVector = Vector3.up * gridTransform.GetComponent<UIGrid>().cellHeight * count;
            handle.Result.transform.localPosition -= moveVector;
            items.Add(handle.Result);
            ++count;
        }
        float totalScrollViewHeight = SCROLLVIEWHEIGHTPERITEM * ACHTable.Count;
        float scrollViewHeight = Mathf.Min(SCROLLVIEWMAXHEIGHT, totalScrollViewHeight);
        scrollViewPanel.SetRect(0f, 0f, SCROLLVIEWWIDTH, scrollViewHeight);

        Vector3 newPivotColliderSize = pivotBoxCollider.size;
        newPivotColliderSize.y = scrollViewHeight;
        pivotBoxCollider.size = newPivotColliderSize;

        if (scrollViewHeight < SCROLLVIEWMAXHEIGHT)
        {
            float distance = Mathf.Abs(bgCollider.bounds.max.y - pivotBoxCollider.bounds.max.y);
            Vector3 newPos = transform.position;
            newPos.y += distance - OFFSET;
            transform.position = newPos;
        }
        pivotBoxCollider.enabled = false;

        // 현재 스크롤뷰의 높이가 최대 높이보다 작거나 같으면 아이템들을 스크롤 불가하게 만든다.
        if (totalScrollViewHeight <= SCROLLVIEWMAXHEIGHT)
        {
            foreach (var item in items)
            {
                item.GetComponent<UIDragScrollView>().enabled = false;
            }
        }

        isInit = true;

        root.SetActive(false);
        callback.Invoke();
    }

    public void Show()
    {
        root.SetActive(true);
        uiScrollBar.value = 0f;
        uiScrollBar.ForceUpdate();
        dragableRect.scrollView = GetComponent<UIScrollView>();

        foreach (var item in items)
        {
            item.GetComponent<ACHItem>().SetUserConditionCount(Volt_PlayerData.instance.GetACHProgressCount(item.GetComponent<ACHItem>().GetID()),
                Volt_PlayerData.instance.IsAccomplishACH(item.GetComponent<ACHItem>().GetID()));
        }
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (var item in items)
        {
            Managers.Resource.DestoryAndRelease(item);
        }
    }

    public ACHItem GetACHItemByID(int achId)
    {
        foreach (var item in items)
        {
            if (item.GetComponent<ACHItem>().GetID() == achId)
                return item.GetComponent<ACHItem>();
        }
        //Debug.Log("GetACHItemByID Error");
        return null;
    }

    public void Clear()
    {
        foreach (var item in items)
        {
            Managers.Resource.DestoryAndRelease(item);
        }
        items.Clear();
    }
}

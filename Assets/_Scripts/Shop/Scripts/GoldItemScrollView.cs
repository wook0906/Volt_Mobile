using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {

        public class GoldItemScrollView : MonoBehaviour
        {
            public GameObject itemPrefab;
            public BoxCollider scrollCollider;
            public UIScrollBar uiScrollBar;

            public Transform gridTransform;
            public UIPanel scrollViewPanel;
            public UIGrid grid;

            private Dictionary<int, GoldShopModel> goldItemTable = new Dictionary<int, GoldShopModel>();
            private List<GameObject> items = new List<GameObject>();

            // 한 페이지에는 (2,4)의 행렬로 구성된다. 그렇기 때문에
            // 한 페이지에 최대 아이템의 갯수는 8개가 된다.
            private const int PAGEMAXITEMCOUNT = 8;
            private const int DEFAULTMAXPERLINE = 4;

            private const int DEFAULTSCROLLVIEWWIDTH = 300;
            private const int SCROLLVIEWWIDTHPERITEM = 320;
            private const int SCROLLVIEWMAXWIDTH = 1260;
            private const int SCROLLVIEWHEIGHT = 770;

            private const int BOARDWIDTH = 1350;
            private const int BOARDHEIGHT = 830;

            // 스크롤 뷰의 아이템을 나타낼 때 왼쪽에 살짝 여유 공간을 두기 위함!
            private const float LEFTPADDING = (BOARDWIDTH - SCROLLVIEWMAXWIDTH) / 2.0f;

            public System.Action onCompletedInit;

            public void Init()
            {
                goldItemTable = ShopDataManager.instance.GetGoldShopItemTable();

                if (goldItemTable == null || goldItemTable.Count == 0)
                {
                    //Debug.LogWarning("Diamon shop item table is null or There are no items!!");
                    return;
                }

                foreach (var gold in goldItemTable)
                {
                    MakeSubItem();
                }

                int pageCount = goldItemTable.Count / PAGEMAXITEMCOUNT;
                int offset = goldItemTable.Count % PAGEMAXITEMCOUNT;
                if (offset > 4)
                    offset = 0;

                //Debug.LogFormat("PageCount: {0}, offset: {1} ", pageCount, offset);
                int columnLimit = DEFAULTMAXPERLINE * pageCount + offset;
                grid.maxPerLine = Mathf.Max(columnLimit, DEFAULTMAXPERLINE);
                grid.Reposition();

                // 스크롤뷰의 넓이를 계산한다.

                float totalScrollViewWidth = DEFAULTSCROLLVIEWWIDTH + SCROLLVIEWWIDTHPERITEM * (Mathf.Max(4, goldItemTable.Count) - 1);
                float scrollviewWidth = Mathf.Min(SCROLLVIEWMAXWIDTH, totalScrollViewWidth);
                Vector2 scrollCenter = Vector2.zero;
                scrollCenter.x = (float)BOARDWIDTH / 2 - scrollviewWidth / 2 - LEFTPADDING;
                scrollViewPanel.SetRect(0f, 0f, scrollviewWidth, SCROLLVIEWHEIGHT);

                Vector3 scrollRect = new Vector3(scrollviewWidth, SCROLLVIEWHEIGHT, 1);
                scrollCollider.size = scrollRect;
                scrollCollider.center = scrollCenter;

                DragScrollViewOnOff();
            }

            private void MakeSubItem()
            {
                Managers.UI.MakeSubItemAsync<GoldShopItem>(gridTransform, null,
                        (GameObject itemGO) =>
                        {
                            itemGO.transform.localScale = Vector3.one;
                            itemGO.transform.localPosition = Vector3.zero;

                            items.Add(itemGO);
                            if (items.Count < goldItemTable.Count)
                                return;

                            int idx = 0;
                            foreach (GoldShopModel param in goldItemTable.Values)
                            {
                                items[idx].GetComponent<GoldShopItem>().Init(param.ID, param.objectName, param.priceType, param.priceCount,
                                param.objectType, param.objectCount, param.objectICON, param.priceICON, param.iconAtlas, param.getBuyButtonNormalSprite,
                                param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize,
                                param.objectFontSize);

                                items[idx++].name = param.objectName;
                            }
                            gridTransform.GetComponent<UIGrid>().Reposition();
                            GetComponent<UIScrollView>().ResetPosition();
                            GetComponent<UIPanel>().Refresh();
                            onCompletedInit.Invoke();
                            gameObject.SetActive(false);
                        });
            }

            private void OnEnable()
            {
                DragScrollViewOnOff();
                uiScrollBar.value = 0;
            }

            private void DragScrollViewOnOff()
            {
                if (goldItemTable.Count > PAGEMAXITEMCOUNT)
                {
                    //print("Scroll drag on");
                    scrollCollider.GetComponent<UIDragScrollView>().scrollView = GetComponent<UIScrollView>();
                    scrollCollider.GetComponent<UIDragScrollView>().enabled = true;
                }
                else
                {
                    //print("Scroll drag on");
                    scrollCollider.GetComponent<UIDragScrollView>().enabled = false;
                }
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class FrameDecorationItemScrollView : MonoBehaviour
        {
            public GameObject itemPrefab;
            public BoxCollider scrollCollider;
            public UIScrollBar uiScrollBar;

            public Transform gridTransform;
            public UIPanel scrollViewPanel;
            public UIGrid grid;

            private Dictionary<int, FrameDecorationShopModel> frameDecorationItemTable = new Dictionary<int, FrameDecorationShopModel>();
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

            private void Start()
            {
                frameDecorationItemTable = ShopDataManager.instance.GetFrameDecorationShopItemTable();

                if (frameDecorationItemTable == null || frameDecorationItemTable.Count == 0)
                {
                    //Debug.LogWarning("Diamon shop item table is null or There are no items!!");
                    return;
                }

                //print("FrameDecoTable Count: " + frameDecorationItemTable.Count);
                int count = 0;
                foreach (var frameDecoration in frameDecorationItemTable)
                {
                    FrameDecorationShopModel param = frameDecoration.Value;
                    GameObject itemGO = Instantiate(itemPrefab, gridTransform) as GameObject;
                    itemGO.GetComponent<FrameDecorationShopItem>().Init(param.ID, param.edgeName, param.priceType,
                        param.priceCount, param.objectType, param.objectCount, param.priceICON, param.iconAtlas, param.edgeSprite,
                        param.getBuyButtonNormalSprite, param.getBuyButtonPushedSprite, param.atlas, param.font, param.priceFontSize);

                    itemGO.name = param.edgeName;
                    itemGO.transform.localScale = Vector3.one;
                    itemGO.transform.localPosition = Vector3.zero;

                    items.Add(itemGO);
                    ++count;
                }

                int pageCount = frameDecorationItemTable.Count / PAGEMAXITEMCOUNT;
                int offset = frameDecorationItemTable.Count % PAGEMAXITEMCOUNT;
                if (offset > 4)
                    offset = 0;

                //Debug.LogFormat("PageCount: {0}, offset: {1} ", pageCount, offset);
                int columnLimit = DEFAULTMAXPERLINE * pageCount + offset;
                grid.maxPerLine = Mathf.Max(columnLimit, DEFAULTMAXPERLINE);
                grid.Reposition();

                // 스크롤뷰의 넓이를 계산한다.
                float totalScrollViewWidth = DEFAULTSCROLLVIEWWIDTH + SCROLLVIEWWIDTHPERITEM * (frameDecorationItemTable.Count - 1);
                float scrollviewWidth = Mathf.Min(SCROLLVIEWMAXWIDTH, totalScrollViewWidth);
                Vector2 scrollCenter = Vector2.zero;
                scrollCenter.x = (float)BOARDWIDTH / 2 - scrollviewWidth / 2 - LEFTPADDING;
                scrollViewPanel.SetRect(0f, 0f, scrollviewWidth, SCROLLVIEWHEIGHT);

                Vector3 scrollRect = new Vector3(scrollviewWidth, SCROLLVIEWHEIGHT, 1);
                scrollCollider.size = scrollRect;
                scrollCollider.center = scrollCenter;

                DragScrollViewOnOff();
            }

            private void OnEnable()
            {
                DragScrollViewOnOff();
                uiScrollBar.value = 0;
            }

            private void DragScrollViewOnOff()
            {
                if (frameDecorationItemTable.Count > PAGEMAXITEMCOUNT)
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
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class EmoticonItemScrollView : MonoBehaviour
        {
            public GameObject itemPrefab;
            public BoxCollider scrollCollider;
            public UIScrollBar uiScrollBar;

            public Transform gridTransform;
            public UIPanel scrollViewPanel;

            private List<GameObject> items = new List<GameObject>();

            private const int MAXITEMS = 20;
            //화면에 보이는 최대 아이템 개수
            private const int MAXITEMCOUNTONSCREEN = 8;
            private const int DEFAULTSCROLLVIEWWIDTH = 300;
            private const int SCROLLVIEWWIDTHPERITEM = 300;
            private const int SCROLLVIEWMAXWIDTH = 1240;
            private const int SCROLLVIEWHEIGHT = 670;
            private const float SCROLLVIEWYPOSOFFSET = -50f;

            private const int BOARDWIDTH = 1350;
            private const int BOARDHEIGHT = 1350;

            // 스크롤 뷰의 아이템을 나타낼 때 왼쪽에 살짝 여유 공간을 두기 위함!
            private const float LEFTPADDING = (BOARDWIDTH - SCROLLVIEWMAXWIDTH) / 2.0f;
            public System.Action onCompletedInit;

            public void Init()
            {
                for (int i = 0; i < MAXITEMS; ++i)
                {
                    MakeSubItem();
                }
            }

            public void OnClickEmoticonTapButton(ShopRobotSelectType selectRobotType)
            {
                transform.localPosition = Vector3.zero;
                EmoticonShopModel[] robotEmoticonShopModels;

                if (!ShopDataManager.instance.GetEmoticonShopItemTable().TryGetValue(selectRobotType, out robotEmoticonShopModels))
                {
                    Debug.LogError("Emotion테이블에 없는 값");
                    return;
                }


                for (int i = 0; i < robotEmoticonShopModels.Length; ++i)
                {
                    EmoticonShopModel param = robotEmoticonShopModels[i];
                    items[i].GetComponent<EmoticonShopItem>().Init(param.ID, param.emoticonName_KOR, param.emoticonName_EN, param.emoticonName_GER,
                        param.emoticonName_Fren, param.priceType, param.priceCount, param.shopRobotSelectType, param.objectType, param.emoticonModel, param.objectCount,
                        param.priceICON, param.emoticonSprite, param.BuyButtonName_KR, param.BuyButtonName_EN, param.BuyButtonName_GER, param.BuyButtonName_Fren,
                        param.getBuyButtonNormalSprite, param.getBuyButtonPushedSprite);

                    items[i].name = param.emoticonName_EN;
                    items[i].gameObject.SetActive(true);
                }
                RefreshScrollView(robotEmoticonShopModels.Length);
            }

            private void RefreshScrollView(int itemCount)
            {
                gridTransform.GetComponent<UIGrid>().Reposition();

                float totalScrollViewWidth = DEFAULTSCROLLVIEWWIDTH + SCROLLVIEWWIDTHPERITEM * (itemCount - 1);
                float scrollviewWidth = Mathf.Min(SCROLLVIEWMAXWIDTH, totalScrollViewWidth);
                Vector2 scrollCenter = Vector2.zero;
                scrollCenter.x = (float)BOARDWIDTH / 2 - scrollviewWidth / 2 - LEFTPADDING;
                scrollViewPanel.SetRect(0f, 0f, scrollviewWidth, SCROLLVIEWHEIGHT);

                Vector3 scrollRect = new Vector3(scrollviewWidth, SCROLLVIEWHEIGHT, 1);
                scrollCollider.size = scrollRect;

                scrollCollider.center = scrollCenter;
                scrollViewPanel.clipOffset = new Vector2(0f, -50f);

                DragScrollViewOnOff(itemCount);
            }

            private void MakeSubItem()
            {
                Managers.UI.MakeSubItemAsync<EmoticonShopItem>(gridTransform, null,
                    (GameObject itemGO) =>
                    {
                        itemGO.transform.localScale = Vector3.one;
                        itemGO.transform.localPosition = Vector3.zero;

                        items.Add(itemGO);
                        if (items.Count < MAXITEMS)
                            return;
                        foreach (var item in items)
                        {
                            item.SetActive(false);
                        }
                        onCompletedInit.Invoke();
                        gameObject.transform.parent.gameObject.SetActive(false);
                    });
                gridTransform.GetComponent<UIGrid>().Reposition();
                RefreshScrollView(12);
            }

            private void OnEnable()
            {
                //DragScrollViewOnOff();
                uiScrollBar.value = 0;
            }

            private void OnDisable()
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    items[i].SetActive(false);
                }
            }

            private void DragScrollViewOnOff(int itemCount)
            {
                if (itemCount > MAXITEMCOUNTONSCREEN)
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
            public void SetDisabledItemByID(int itemID)
            {
                EmoticonShopItem item = GetEmoticonShopItemByID(itemID);
                item.OnPurchased();
            }
            EmoticonShopItem GetEmoticonShopItemByID(int itemID)
            {
                foreach (var item in gridTransform.GetComponentsInChildren<EmoticonShopItem>())
                {
                    if (item.GetID() == itemID)
                        return item;
                }
                Debug.LogError("GetEmoticonShopItemByID Error");
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
    }
}

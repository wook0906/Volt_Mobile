using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class PackageItemScrollView : MonoBehaviour
        {
            public GameObject itemPrefab;
            public BoxCollider scrollCollider;
            public UIScrollBar uiScrollBar;

            public Transform gridTransform;
            public UIPanel scrollViewPanel;

            private List<GameObject> items = new List<GameObject>();

            private Dictionary<int, PackageShopModel> packageItemTable = new Dictionary<int, PackageShopModel>();

            //화면에 보이는 최대 아이템 개수
            private const int MAXITEMCOUNTONSCREEN = 8;
            private const int DEFAULTMAXPERLINE = 4;

            private const int DEFAULTSCROLLVIEWWIDTH = 1260;
            private const int SCROLLVIEWWIDTHPERITEM = 320;
            private const int SCROLLVIEWMAXWIDTH = 1260;
            private const int SCROLLVIEWHEIGHT = 770;
            private const float SCROLLVIEWYPOSOFFSET = -50f;

            private const int BOARDWIDTH = 1350;
            private const int BOARDHEIGHT = 830;

            // 스크롤 뷰의 아이템을 나타낼 때 왼쪽에 살짝 여유 공간을 두기 위함!
            private const float LEFTPADDING = (BOARDWIDTH - SCROLLVIEWMAXWIDTH) / 2.0f;
            public System.Action onCompletedInit;

            public void Init()
            {
                packageItemTable = ShopDataManager.instance.GetPackageShopItemTable();

                if (packageItemTable == null || packageItemTable.Count == 0)
                {
                    Debug.LogWarning("Package shop item table is null or There are no items!!");
                    return;
                }


                foreach (var package in packageItemTable)
                {
                    MakeSubItem();
                }

                int pageCount = packageItemTable.Count / MAXITEMCOUNTONSCREEN;
                int offset = packageItemTable.Count % MAXITEMCOUNTONSCREEN;
                if (offset > 4)
                    offset = 0;

                //Debug.LogFormat("PageCount: {0}, offset: {1} ", pageCount, offset);
                int columnLimit = DEFAULTMAXPERLINE * pageCount + offset;
                gridTransform.GetComponent<UIGrid>().maxPerLine = Mathf.Max(columnLimit, DEFAULTMAXPERLINE);
                gridTransform.GetComponent<UIGrid>().Reposition();

                // 스크롤뷰의 넓이를 계산한다.
                float totalScrollViewWidth = DEFAULTSCROLLVIEWWIDTH + SCROLLVIEWWIDTHPERITEM * (packageItemTable.Count - 1);
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
                Managers.UI.MakeSubItemAsync<PackageShopItem>(gridTransform, null,
                        (GameObject itemGO) =>
                        {
                            itemGO.transform.localScale = Vector3.one;
                            itemGO.transform.localPosition = Vector3.zero;

                            items.Add(itemGO);
                            if (items.Count < packageItemTable.Count)
                                return;
                            int idx = 0;
                            foreach (PackageShopModel param in packageItemTable.Values)
                            {
                                items[idx].GetComponent<PackageShopItem>().Init(param.ID, param.objectName, param.priceType, param.priceCount,
                                param.objectType, param.objectCount, param.objectICON, param.priceICON, param.iconAtlas,
                                param.getBuyButtonNormalSprite, param.getBuyButtonPushedSprite, param.atlas, param.font,
                                param.priceFontSize, param.objectFontSize);


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

            private void OnDisable()
            {
                //for (int i = 0; i < items.Count; ++i)
                //{
                //    items[i].SetActive(false);
                //}
            }

            private void DragScrollViewOnOff()
            {
                if (packageItemTable.Count > MAXITEMCOUNTONSCREEN)
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

            //private void DragScrollViewOnOff(int itemCount)
            //{

            //    if (itemCount > MAXITEMCOUNTONSCREEN)
            //    {
            //        //print("Scroll drag on");
            //        scrollCollider.GetComponent<UIDragScrollView>().scrollView = GetComponent<UIScrollView>();
            //        scrollCollider.GetComponent<UIDragScrollView>().enabled = true;
            //    }
            //    else
            //    {
            //        //print("Scroll drag on");
            //        scrollCollider.GetComponent<UIDragScrollView>().enabled = false;
            //    }
            //}
            public void SetDisabledItemByID(int itemID)
            {
                PackageShopItem item = GetPackageShopItemByID(itemID);
                item.OnPurchased();
            }
            PackageShopItem GetPackageShopItemByID(int itemID)
            {
                foreach (var item in gridTransform.GetComponentsInChildren<PackageShopItem>())
                {
                    if (item.GetID() == itemID)
                        return item;
                }
                //Debug.LogError("GetPackageShopItemByID Error");
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

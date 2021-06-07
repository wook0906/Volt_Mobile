using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volt
{
    namespace Shop
    {
        public class RobotSkinItemScrollView : MonoBehaviour
        {
            public GameObject itemPrefab;
            public BoxCollider scrollCollider;
            public UIScrollBar uiScrollBar;

            public Transform gridTransform;
            public UIPanel scrollViewPanel;

            private List<GameObject> items = new List<GameObject>();

            // 각 로봇별 스킨 최대 수
            private const int MAXITEMS = 10;

            //화면에 보이는 최대 아이템 개수
            private const int MAXITEMCOUNTONSCREEN = 2;
            private const int DEFAULTSCROLLVIEWWIDTH = 480;
            private const int SCROLLVIEWWIDTHPERITEM = 480;
            private const int SCROLLVIEWMAXWIDTH = 1240;
            private const int SCROLLVIEWHEIGHT = 670;
            private const float SCROLLVIEWYPOSOFFSET = -50f;

            private const int BOARDWIDTH = 1350;
            private const int BOARDHEIGHT = 830;
            
            // 스크롤 뷰의 아이템을 나타낼 때 왼쪽에 살짝 여유 공간을 두기 위함!
            private const float LEFTPADDING = (BOARDWIDTH - SCROLLVIEWMAXWIDTH) / 2.0f;

            public System.Action onCompletedInit;
            public void Init()
            {
                for(int i = 0; i < MAXITEMS; ++i)
                {
                    MakeSubItem();
                }
            }

            public void OnClickSkinTapButton(ShopRobotSelectType skinType)
            {
                transform.localPosition = Vector3.zero;
                RobotSkinShopModel[] robotSkinShopModels;
                if(!ShopDataManager.instance.GetRobotSkinShopItemTable().TryGetValue(skinType, out robotSkinShopModels))
                {
                    Debug.LogError("이거 스킨정보가 테이블에 없음");
                    return;   
                }

                for (int i = 0; i < robotSkinShopModels.Length; ++i)
                {
                    RobotSkinShopModel param = robotSkinShopModels[i];
                    items[i].GetComponent<RobotSkinShopItem>().Init(param.ID, param.skinName_KOR, param.skinName_EN, param.skinName_GER,
                        param.skinName_Fren, param.priceType, param.priceCount, param.shopRobotSelectType, param.objectType,
                        param.skinModel, param.objectCount, param.priceICON, param.skinSprite,
                        param.BuyButtonName_KR, param.BuyButtonName_EN, param.BuyButtonName_GER, param.BuyButtonName_Fren,
                        param.getBuyButtonNormalSprite, param.getBuyButtonPushedSprite);

                    items[i].name = param.skinName_EN;
                    items[i].gameObject.SetActive(true);
                }

                RefreshScrollView(robotSkinShopModels.Length);
            }

            private void MakeSubItem()
            {
                Managers.UI.MakeSubItemAsync<RobotSkinShopItem>(gridTransform, null,
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
                scrollViewPanel.clipOffset = new Vector2(-27f, -50f);

                DragScrollViewOnOff(itemCount);
            }

            private void OnEnable()
            {
                //DragScrollViewOnOff();
                uiScrollBar.value = 0;
            }

            private void OnDisable()
            {
                for(int i = 0; i < items.Count; ++i)
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
                RobotSkinShopItem item = GetSkinShopItemByID(itemID);
                item.OnPurchased();
            }
            RobotSkinShopItem GetSkinShopItemByID(int itemID)
            {
                foreach (var item in gridTransform.GetComponentsInChildren<RobotSkinShopItem>())
                {
                    if (item.GetID() == itemID)
                        return item;
                }
                //Debug.LogError("GetSkinShopItemByID Error");
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
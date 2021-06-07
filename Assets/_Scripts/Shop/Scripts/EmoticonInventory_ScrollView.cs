using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Volt.Shop;

public class EmoticonInventory_ScrollView : MonoBehaviour
{
    public GameObject itemPrefab;
    public BoxCollider scrollCollider;
    public UIScrollBar uiScrollBar;

    public Transform gridTransform;
    public UIPanel scrollViewPanel;
    public UIGrid grid;

    private Dictionary<ShopRobotSelectType, EmoticonShopModel[]> emoticonShopItemTable = new Dictionary<ShopRobotSelectType, Volt.Shop.EmoticonShopModel[]>();
    //private Dictionary<EmoticonSelectType, EmoticonInventoryModel> inventoryEmoticonItemTable = new Dictionary<EmoticonSelectType, EmoticonInventoryModel>();
    private List<GameObject> items = new List<GameObject>();

    private const int MAXITEMS = 14;

    private const int MAXITEMCOUNTONSCREEN = 8;
    private const int DEFAULTSCROLLVIEWWIDTH = 140;
    private const int SCROLLVIEWWIDTHPERITEM = 140;
    private const int SCROLLVIEWMAXWIDTH = 1000;
    private const int SCROLLVIEWHEIGHT = 500;
    private const float SCROLLVIEWYPOSOFFSET = -64f;

    private const int BOARDWIDTH = 1100;
    private const int BOARDHEIGHT = 700;

    public System.Action onCompletedInit;
    // 스크롤 뷰의 아이템을 나타낼 때 왼쪽에 살짝 여유 공간을 두기 위함!
    private const float LEFTPADDING = (BOARDWIDTH - SCROLLVIEWMAXWIDTH) / 2.0f;

    public void Init()
    {
        for (int i = 0; i < MAXITEMS; ++i)
        {
            MakeSubItem();
        }
    }

    private void MakeSubItem()
    {
        Managers.UI.MakeSubItemAsync<EmoticonInventoryItem>(gridTransform, null,
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
                //gameObject.transform.parent.gameObject.SetActive(false);
            });
        gridTransform.GetComponent<UIGrid>().Reposition();
        RefreshScrollView(items.Count);
    }
    private void RefreshScrollView(int itemCount)
    {
        gridTransform.GetComponent<UIGrid>().Reposition();

        //float totalScrollViewWidth = DEFAULTSCROLLVIEWWIDTH + SCROLLVIEWWIDTHPERITEM * (itemCount - 1);
        //float scrollviewWidth = Mathf.Min(SCROLLVIEWMAXWIDTH, totalScrollViewWidth);
        Vector2 scrollCenter = Vector2.zero;
        scrollCenter.x = (float)BOARDWIDTH / 2 - 1000 / 2 - LEFTPADDING;
        scrollViewPanel.SetRect(0f, 0f, 1000, SCROLLVIEWHEIGHT);

        Vector3 scrollRect = new Vector3(1000, SCROLLVIEWHEIGHT, 1);
        scrollCollider.size = scrollRect;

        scrollCollider.center = scrollCenter;
        scrollViewPanel.clipOffset = new Vector2(0f, SCROLLVIEWYPOSOFFSET);

        DragScrollViewOnOff(itemCount);
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

    public void OnClickEmoticonTapButton(EmoticonSelectType selectEmoticonType)
    {
        transform.localPosition = Vector3.zero;
        EmoticonShopModel[] robotEmoticonShopModels;
        int emoticonType = (int)selectEmoticonType;

        if (selectEmoticonType == EmoticonSelectType.Common)
        {
            Dictionary<ShopRobotSelectType, EmoticonShopModel[]> tmpShopItemTable = 
                new Dictionary<ShopRobotSelectType, EmoticonShopModel[]>(ShopDataManager.instance.GetEmoticonShopItemTable());
            EmoticonShopModel[] tmpModels = new EmoticonShopModel[11];
            EmoticonShopModel surrnderModel = new EmoticonShopModel (9000401, "항복", "Surrender", "Surrender",
                "Surrender", 1, 0, "SkinType_End", "Emoticon",
                "None", 0, "Icon_Money","Volt_iconatlas",
                "Common_Surrender", "EmoticonAtlas",
                "구매하기", "Purchase", "Kaufen", "None",
                "Btn_button03_n", "Btn_button03_p", "Volt_UI",
                "Spoqa Han Sans Bold", 25, 30 );
            tmpModels[0] = surrnderModel;
            if(!tmpShopItemTable.ContainsKey((ShopRobotSelectType)4))
                tmpShopItemTable.Add((ShopRobotSelectType)4, tmpModels);

            if (!tmpShopItemTable.TryGetValue((ShopRobotSelectType)emoticonType, out robotEmoticonShopModels))
            {
                Debug.LogError("Emotion테이블에 없는 값");
                return;
            }
            
            EmoticonShopModel param = robotEmoticonShopModels[0];
            items[0].GetComponent<EmoticonInventoryItem>().Init(param.ID, (int)param.shopRobotSelectType, param.emoticonSprite, param.emoticonSprite);
            items[0].gameObject.SetActive(true);
            for (int i = 1; i < tmpModels.Length; i++)
            {
                items[i].gameObject.SetActive(false);
            }
            RefreshScrollView(tmpModels.Length);
        }
        else
        {
            if (!ShopDataManager.instance.GetEmoticonShopItemTable().TryGetValue((ShopRobotSelectType)emoticonType, out robotEmoticonShopModels))
            {
                Debug.LogError("Emotion테이블에 없는 값");
                return;
            }


            for (int i = 0; i < robotEmoticonShopModels.Length; ++i)
            {
                EmoticonShopModel param = robotEmoticonShopModels[i];
                items[i].GetComponent<EmoticonInventoryItem>().Init(param.ID, (int)param.shopRobotSelectType, param.emoticonSprite, param.emoticonSprite);
                items[i].gameObject.SetActive(true);
            }
            RefreshScrollView(robotEmoticonShopModels.Length);
        }
        


    }
}

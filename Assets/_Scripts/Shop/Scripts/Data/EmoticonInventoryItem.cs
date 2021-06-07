using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoticonInventoryItem : UIBase
{
    private int ID;
    private EmoticonSelectType emoticonSelectType;
    private UIButton button;


    public UISprite emoticonSprite;


    public void Init(int ID, int emoticonType, string emoticonSprite, string objectName)
    {
        button = this.emoticonSprite.GetComponentInChildren<UIButton>();
        
        this.ID = ID;
        this.emoticonSprite.spriteName = emoticonSprite;
        this.emoticonSelectType = (EmoticonSelectType)emoticonType;
        this.name = objectName;

        if (Volt_PlayerData.instance.IsHaveEmoticon(this.ID))
        {
            this.emoticonSprite.color = Color.white;
            button.enabled = true;
            button.SetState(UIButtonColor.State.Normal, true);
        }
        else
        {
            this.emoticonSprite.color = Color.gray;
            button.SetState(UIButtonColor.State.Disabled, true);
            button.enabled = false;
        }
    }

    public override void Init()
    {

    }
    public void OnClickBtn()
    {
        int emptySlotNumber = Managers.UI.GetSceneUI<EmoticonScene_UI>().GetEmptySlotNumber();
        if (emptySlotNumber == -1)
        {
            Debug.Log("Slot is Full!");
        }
        else
        {
            if(Volt_PlayerData.instance.SetEmoticon(emptySlotNumber, emoticonSprite.spriteName))
                GameObject.Find("slot" + emptySlotNumber.ToString() + "Sprite").GetComponent<UISprite>().spriteName = emoticonSprite.spriteName;
        }
    }
}

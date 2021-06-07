using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UIBase
{

    public override void Init()
    {
        isInit = true;
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }

    public virtual void OnClose()
    {
        
    }
}

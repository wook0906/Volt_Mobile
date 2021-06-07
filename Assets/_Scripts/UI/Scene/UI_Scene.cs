using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UIBase
{
    public override void Init()
    {
        isInit = true;
        Managers.UI.SetCanvas(gameObject, false);
    }

    public virtual void OnActive()
    {

    }
}

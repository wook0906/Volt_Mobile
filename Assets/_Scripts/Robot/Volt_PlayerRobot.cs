using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 로봇은 행동할 시기가 되면 등록됀 행동정보대로 행동한다.
//행동정보는 PlayerPanel에서 등록한다.
public class Volt_PlayerRobot : Volt_Robot
{
    public bool moduleInputDone = true;
    int idx = 0;
    public int _idx
    {
        set
        {
            idx = value;
        }
        get { return idx; }
    }
    // Start is called before the first frame update
    
    
 
    public void ModuleUseCallback()
    {
        //조작이 끝났음을 서버에 알려야함.
        moduleInputDone = true;
       
    }
}

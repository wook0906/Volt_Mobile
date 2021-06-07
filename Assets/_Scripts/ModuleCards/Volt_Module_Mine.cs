using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Module_Mine : Volt_ModuleCardBase
{
    /*[Header("Set In Inspector")]
    public GameObject minePrefab;
    [SerializeField]
    private int numOfMines = 3;

    [Header("Set In Script")]
    [SerializeField]
    private List<GameObject> activeMines;
    public  bool isCanUse { get; set; }

    public override void OnEuipped()
    {
        isCanUse = true;
    }

    public override void OnUnEuipped()
    {
        isCanUse = false;
    }

    public void ActiveModuleCard()
    {
        numOfMines--;
        GameObject mineGo = Instantiate(minePrefab) as GameObject;
        mineGo.transform.position = owner.transform.position;
        mineGo.GetComponent<Volt_Mine>().Init(owner.gameObject, this);
        activeMines.Add(mineGo);

        if(numOfMines == 3)
        {
            owner.moduleCardExcutor.DestroyCard(this);
        }
    }

    public void AddMine()
    {
        if (numOfMines >= 3)
            return;
        numOfMines++;
    }*/
}

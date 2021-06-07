using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_RandomBox : MonoBehaviour
{
    public List<Material> boxMaterials;
    public Volt_ModuleCardBase moduleInBox;
    [SerializeField]
    private ModuleType moduleType;
    public ModuleType ModuleType { get { return moduleType; } }
    MeshRenderer r;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<MeshRenderer>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        r.material.SetColor("_EmissionColor", new Color((Mathf.PingPong(Time.time*0.6f, 1f - 0.2f) + 0.2f)*2f,
                                                        (Mathf.PingPong(Time.time * 0.6f, 1f - 0.2f) + 0.2f)*2f,
                                                        (Mathf.PingPong(Time.time * 0.6f, 1f - 0.2f) + 0.2f)*2f));
    }
    //public void Initialize()
    //{
    //    moduleType = Volt_ModuleDeck.S.GetRandomModuleType();
    //    switch (moduleType)
    //    {
    //        case ModuleType.Attack:
    //            r.material = boxMaterials[0];
    //            break;
    //        case ModuleType.Movement:
    //            r.material = boxMaterials[1];
    //            break;
    //        case ModuleType.Tactic:
    //            r.material = boxMaterials[2];
    //            break;
    //        default:
    //            break;
    //    }
    //    moduleInBox = Volt_ModuleDeck.S.DrawRandomModuleCard(this);
    //}
    public void SpecificInit(Card card)
    {
        moduleInBox = Volt_ModuleDeck.S.GetModuleFromDeck(card);
        if (moduleInBox == null)
            return;
        switch (moduleInBox.moduleType)
        {
            case ModuleType.Movement:
                GetComponent<MeshRenderer>().material = boxMaterials[1];
                break;
            case ModuleType.Attack:
                GetComponent<MeshRenderer>().material = boxMaterials[0];
                break;
            case ModuleType.Tactic:
                GetComponent<MeshRenderer>().material = boxMaterials[2];
                break;
            default:
                break;
        }
    }
    
    public IEnumerator Positioning()
    {
        //애니메이션...
        yield return null;
    }
    public void DestroyModule(bool isNeedReturnToModuleDeck)
    {
        Volt_Tile placedTile = Volt_ArenaSetter.S.GetTile(transform.position);
        if(isNeedReturnToModuleDeck)
            Volt_ModuleDeck.S.ReturnModuleCard(moduleInBox);
        placedTile.ModuleInstance = null;
        placedTile.tileInModuleType = Card.NONE;
        Volt_ArenaSetter.S.numOfModule--;

        Volt_PrefabFactory.S.PushObject(GetComponent<Poolable>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Volt_ScreenRaycaster : MonoBehaviour
{

    [SerializeField]
    public Camera cam;
    private int interactableLayer;

    Ray ray;
    public RaycastHit curHit;

    void Start()
    {
        interactableLayer = 1 << 8 | 1 << 9;

    }
    
    private void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject(-1))
        //{
        //    return;
        //}
        if (Volt_GameManager.S == null)
            return;

        if(Volt_GameManager.S.pCurPhase!=Phase.waitSync)
            MouseInputRaycaster();
    }

    void MouseInputRaycaster()
    {
        if (cam == null)return;
        if (Input.GetMouseButtonDown(0))
        {
            switch (Volt_GameManager.S.pCurPhase)
            {
                case Phase.none:
                    break;
                case Phase.matching:
                    break;
                case Phase.fieldSetup:
                    break;
                case Phase.playerSetup:
                    break;
                case Phase.ItemSetup:
                    break;

                case Phase.robotSetup:
                    interactableLayer = 1 << 8 | 1 << 5;
                    break;
                case Phase.behavoiurSelect:
                    break;
                case Phase.rangeSelect:
                    interactableLayer = 1 << 8 | 1 << 5;
                    break;
                case Phase.simulation:
                    break;
                case Phase.resolution:
                    break;
                case Phase.synchronization:
                    break;
                default:
                    break;
            }
            ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out curHit, Mathf.Infinity, interactableLayer))
            {
                curHit.transform.SendMessage("OnTouchBegin", SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            switch (Volt_GameManager.S.pCurPhase)
            {
                case Phase.none:
                    break;
                case Phase.matching:
                    break;
                case Phase.fieldSetup:
                    break;
                case Phase.playerSetup:
                    break;
                case Phase.ItemSetup:
                    break;

                case Phase.robotSetup:
                    interactableLayer = 1 << 8 | 1 << 5; break;
                case Phase.behavoiurSelect:
                    break;
                case Phase.rangeSelect:
                    interactableLayer = 1 << 8 | 1 << 5; break;
                case Phase.simulation:
                    break;
                case Phase.resolution:
                    break;
                case Phase.synchronization:
                    break;
                default:
                    break;
            }
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out curHit, Mathf.Infinity, interactableLayer))
            {
         
                curHit.transform.SendMessage("OnTouch", SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            switch (Volt_GameManager.S.pCurPhase)
            {
                case Phase.none:
                    break;
                case Phase.matching:
                    break;
                case Phase.fieldSetup:
                    break;
                case Phase.playerSetup:
                    break;
                case Phase.ItemSetup:
                    break;

                case Phase.robotSetup:
                    interactableLayer = 1 << 8 | 1<<5;
                    break;
                case Phase.behavoiurSelect:
                    break;
                case Phase.rangeSelect:
                    interactableLayer = 1 << 8 | 1 << 5;
                    break;
                case Phase.simulation:
                    break;
                case Phase.resolution:
                    break;
                case Phase.synchronization:
                    break;
                default:
                    break;
            }
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out curHit, Mathf.Infinity, interactableLayer))
            {
                curHit.transform.SendMessage("OnTouchEnd", SendMessageOptions.DontRequireReceiver);
            }
        }
        
        interactableLayer = 1 << 8 | 1 << 9;
    }
}
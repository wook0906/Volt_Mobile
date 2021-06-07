using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_LookAtTarget : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Volt_PlayerManager.S.I == null) return;
        if (Volt_PlayerManager.S.I.playerCam == null) // <--NullRefer Error 뜸!!
            return;

        transform.LookAt(Volt_PlayerManager.S.I.playerCam.transform);
        if (!Volt_PlayerManager.S.I.playerCamRoot.isMoving)
        {
            switch (Volt_PlayerManager.S.I.playerNumber)
            {
                case 1:
                    this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180f, 0f);
                    break;
                case 2:
                    this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -90f, 0f);
                    break;
                case 3:
                    this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, 0f);
                    break;
                case 4:
                    this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90f, 0f);
                    break;
                default:
                    //print("LookAtTartget err");
                    break;
            }
        }

        //Vector3 targetDirection = transform.position - Volt_PlayerManager.S.I.playerCam.transform.position;

        //Quaternion rot = Quaternion.LookRotation(targetDirection);
        //Vector3 eulerAngle = rot.eulerAngles;
        //eulerAngle.y -= 180f;
        //eulerAngle.x = Mathf.Clamp(eulerAngle.x, -90f, 0f);
        //transform.rotation = Quaternion.Euler(eulerAngle);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_CharacterController : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    Animator anim;
    bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per .0frame
    void Update()
    {
        if (isDead) return;

        Vector3 direction = Vector3.back * variableJoystick.Vertical + Vector3.left * variableJoystick.Horizontal;

        

        if(direction.magnitude > 0f)
        {
            anim.SetBool("isMoving", true);
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position += direction * Time.deltaTime * speed;
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
    public void GetItem(int itemType)
    {
        if (itemType == 0)
            GetVP();
        else
            Death();
    }
    void Death()
    {
        isDead = true;
        anim.SetBool("isDead", isDead);
        Volt_AvoidMiniGameManager.S.GameOver();
    }
    void GetVP()
    {
        Volt_AvoidMiniGameManager.S.Score += 100;
    }
}

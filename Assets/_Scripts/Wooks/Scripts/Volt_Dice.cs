using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_Dice : MonoBehaviour
{
    Animator animator; 
    void Start()
    {
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
    }
    void PlayAnimation(int diceNumber)
    {
        animator.Play("Dice" + diceNumber);
    }
    public void OnTouchBegin()
    {
        //얘가 어떤타입의 주사위인지...?
    }
    public void OnTouch()
    {
        //뭔가 거리측정해야함...
    }
    public void OnTouchEnd()
    {
        //이떄 그냥 숫자가 정해지고 정해진 애니메이션 롤링하자!
    }
    public void DiceAnimationDone()
    {
        
    }
}

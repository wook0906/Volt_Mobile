using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_LoadingTip : MonoBehaviour
{
    readonly string[] tips_Kor = {"공격거리가 4~6 일 때는 두 배의 피해를 입힙니다!",
                        "모듈 박스는 최대 6개까지 배치됩니다.",
                        "코인은 5라운드 간격으로 배치됩니다.",
                        "[시한폭탄]은 자신에게도 장착됩니다.",
                        "[킬 봇]은 장착하지 못하는 모듈이 있습니다.",
                        "모듈을 적재적소에 활용하면 아주 유리해집니다.",
                        "[함정]은 위험하지만, 전략적 활용도 가능합니다.",
                        "[서든 데스]에 사망 시 점수를 잃습니다.",
                        "새해 복 많이 받으세요~!",
                        "상대의 움직임을 예측하는 플레이가 중요합니다.",
                        "스킨을 구매하고 배틀봇을 꾸며 보세요.",
                        "점수를 갖고있지 않은 킬봇은 죽여도 점수를 얻지 못합니다."};
    readonly string[] tips_Eng = { "hi", "bye" };
    readonly string[] tips_Ger = { "ger", "germany" };
    readonly string[] tips_Fren = { "F", "French" };
    UILabel tipText;
    float changeTimer = 0;
    int prevIdx;
    int currentIdx;
    
    private void Awake()
    {
        
    }
    void Start()
    {
        tipText = GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - changeTimer > 5f)
        {
            ChangeTip();
        }
    }
    void ChangeTip()
    {
        changeTimer = Time.time;
        do
        {
            currentIdx = Random.Range(0, tips_Kor.Length);
        }
        while (prevIdx == currentIdx);
        tipText.text = "Tip : " + tips_Kor[currentIdx];
        prevIdx = currentIdx;
    }

}

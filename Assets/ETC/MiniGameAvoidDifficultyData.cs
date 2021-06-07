using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniGame/difficulty")]
public class MiniGameAvoidDifficultyData : ScriptableObject
{
    [Tooltip("해당 난이도 데이터를 나타내는 Key 값(순서대로넣으소...)")]
    public int difficulty;
    [Tooltip("하운드가 한번 공격할 때, 떨어질 수 있는 오브젝트의 최대 갯수")]
    public int numOfMaxDropObjAtOnce;
    [Tooltip("하운드가 한번 공격할 때, 떨어질 수 있는 오브젝트의 최소 갯수")]
    public int numOfMinDropObjAtOnce;
    [Tooltip("하운드 공격과 공격사이의 시간")]
    public float termOfDrop;
    [Tooltip("해당 난이도에서 하운드의 공격횟수")]
    public int opportunityOfDrop;
    [Tooltip("낙하 오브젝트의 최대 낙하거리")]
    public float maxDistanceOfDrop;
    [Tooltip("낙하 오브젝트의 최소 낙하거리")]
    public float minDistanceOfDrop;
    [Tooltip("낙하 오브젝트의 속도")]
    public float objSpeed;
    [Tooltip("어떤 오브젝트가 떨어질 것인가에 대한 확률\n 0 == 점수오브젝트\n1 == 데미지오브젝트 \n 수식 = (n / List.Count) * 100")]
    public List<int> probabilties;
    [Tooltip("오브젝트가 생성된 후, 낙하를 시작하기 전 대기하는 시간")]
    public float waitTimeBeforeMoveObj;
}

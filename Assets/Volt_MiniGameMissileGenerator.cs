using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volt_MiniGameMissileGenerator : MonoBehaviour
{
    public Volt_MiniGameProjectile missilePrefab;
    public Volt_MiniGameProjectile coinPrefab;
    public GameObject aimPointPrefab;
    public MiniGameAvoidDifficultyData curDifficultyData;

    int remainDropCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void GenerateStart()
    {
        //난이도 설정을 한 뒤에 호출됨에도 불구하고 remainCount가 갱신되지 않았다면, 남은 난이도가 없다고 간주하고 GameOver시킴.
        if (remainDropCount == 0) 
        {
            Volt_AvoidMiniGameManager.S.GameOver();
            return;
        }
        StartCoroutine(RepeatGenerate());
    }
    IEnumerator RepeatGenerate()
    {
        while (remainDropCount != 0)
        {
            if (!Volt_AvoidMiniGameManager.S.isGamePlaying) yield break;
            for (int i = 0; i < Random.Range(curDifficultyData.numOfMinDropObjAtOnce, curDifficultyData.numOfMaxDropObjAtOnce+1); i++)
            {
                Shoot();
            }
            remainDropCount--;
            yield return new WaitForSeconds(curDifficultyData.termOfDrop);
        }
        Volt_AvoidMiniGameManager.S.DropDoneCallback();
        
    }
    void Shoot()
    {
        int randomIdx = Random.Range(0, curDifficultyData.probabilties.Count);
        Volt_MiniGameProjectile dropObj;
        if (curDifficultyData.probabilties[randomIdx] == 0)
        {
            dropObj = Instantiate<Volt_MiniGameProjectile>(coinPrefab);
        }
        else
        {
            dropObj = Instantiate<Volt_MiniGameProjectile>(missilePrefab);
        }


        dropObj.transform.position = new Vector3(Random.Range(12f, -11f), Random.Range(curDifficultyData.minDistanceOfDrop, curDifficultyData.maxDistanceOfDrop), Random.Range(-11f, 6f));
        Vector3 aimPointPos = dropObj.transform.position;
        aimPointPos.y = 0.6f;
        GameObject aimPoint = Instantiate(aimPointPrefab, aimPointPos,Quaternion.identity);
        dropObj.Init(aimPointPos,aimPoint, curDifficultyData.objSpeed, curDifficultyData.waitTimeBeforeMoveObj,curDifficultyData.probabilties[randomIdx]);
    }
    public void SetDifficulty(MiniGameAvoidDifficultyData difficulty)
    {
        this.curDifficultyData = difficulty;
        remainDropCount = difficulty.opportunityOfDrop;
    }
}

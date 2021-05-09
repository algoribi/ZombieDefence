using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public void GenerateEnemy()
    {
        StartCoroutine(stageGenerateEnemy());
    }
    //좀비를 16번 + 일수 만큼 생성
    IEnumerator stageGenerateEnemy()
    {
        WaitForSeconds spownDelay = new WaitForSeconds(0.75f);
        for (int dayIdx = 0; dayIdx < 16 + StateManager.dayCount; dayIdx++)
        {
            //사용가능한 좀비를 찾아 위치 조정후 활성화해줌
            for (int zomIdx = 0; zomIdx < GameManager.instance.Enemies.Length; zomIdx++)
            {
                if (!GameManager.instance.Enemies[zomIdx].activeSelf)
                {
                    GameManager.instance.Enemies[zomIdx].transform.position = getSpownPoint();
                    GameManager.instance.Enemies[zomIdx].SetActive(true);
                    EnemyChecker.enemyCount++;
                    yield return spownDelay;
                    break;
                }
            }
        }
    }
    //맵의 -22 부터 22까지 사각형의 형태로 랜덤 위치 반환
    Vector3 getSpownPoint()
    {
        int PosType = Random.Range(0, 4);
        int ranX = Random.Range(-15, 16);
        int ranY = Random.Range(-15, 16);
        if (PosType == 0)
            return new Vector3(15, ranY, 0);
        else if (PosType == 1)
            return new Vector3(-15, ranY, 0);
        else if (PosType == 2)
            return new Vector3(ranX, 15, 0);
        else if (PosType == 3)
            return new Vector3(ranX, -15, 0);
        else
            return new Vector3(15, 15, 0);
    }
}

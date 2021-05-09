using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    GameStarter gameStarter;
    public static int enemyCount = 0; //적의 수
    void Start()
    {
        gameStarter = GameObject.Find("GameStarter").GetComponent<GameStarter>();
        enemyCount = 0;
    }
    //클리어시 실행
    public void clearNight()
    {
        gameStarter.NextDay();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] PistolBullets;
    public GameObject[] RifleBullets;
    public GameObject[] BowArrows;
    //--Turret
    int TurretBulletLength = 500;
    [SerializeField] GameObject TurretBulletPrefab;
    [SerializeField] public Transform TurretBulletParent;
    public GameObject[] TurretBullets;
    //--Turret
    //-Enemy
    int EnemyLength = 100;
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] public Transform EnemyParent;
    public GameObject[] Enemies;
    //-Enemy

    public EnemyChecker enemyChecker;
    public StateManager stateManager;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        GenerateTurretBullet();
        GenerateEnemy();
    }
    //DontDestroyOnLoad(gameObject);
    void GenerateTurretBullet()
    {
        TurretBullets = new GameObject[TurretBulletLength];
        for (int idx = 0; idx < TurretBulletLength; idx++)
        {
            TurretBullets[idx] = Instantiate(TurretBulletPrefab, TurretBulletParent);
        }
    }
    void GenerateEnemy()
    {
        Enemies = new GameObject[EnemyLength];
        for (int idx = 0; idx < EnemyLength; idx++)
        {
            Enemies[idx] = Instantiate(EnemyPrefab, EnemyParent);
        }
    }
}

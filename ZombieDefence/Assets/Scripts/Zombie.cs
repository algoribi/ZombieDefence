using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] SpriteRenderer s_renderer;
    [SerializeField] Animator animator;

    GameObject currentTarget;
    LayerMask TargetLayer;
    float findDelay = 0; //적 찾기 딜레이
    float closeDistance = 0;
    float attackDistance = 0;
    bool canAttack = true;

    int currentDamage = 0;
    int currentHP = 0;
    float currentSpeed = 0;
    float currentAttackRate = 0;

    int currentScorePlusValue = 0;
    int currentMoneytPlusValue = 0;

    void Start()
    {
        TargetLayer = 1 << LayerMask.NameToLayer("Team");
        findDelay = 1f;
        closeDistance = 1.2f;
        attackDistance = 1.5f;
        findTarget();
        currentHP = GameData.ENEMY_HP;
        SetZombieData();
        currentSpeed = 1f;
    }
    void Update()
    {
        MoveToTarget();
        RotateSprite();
    }
    //활성화시 달리기 애니메이션 실행
    //적 이동속도, 공격력, 체력등 정보를
    //곱하기율을 곱한 값으로 갱신해주고 체력값 갱신
    private void OnEnable()
    {
        //GetComponent<Animator>().SetTrigger("Run");
        MoveToTarget();
    }
    //비활성화시 정보 초기화
    private void OnDisable()
    {
        SetZombieData();
    }
    //좀비인지 보스인지 구별 후
    //현재 정보에 곱하기율을 곱해 대입
    void SetZombieData()
    {
        currentDamage = (int)(GameData.ENEMY_DAMAGE * StateManager.enemyDamageMult);
        currentHP = (int)(GameData.ENEMY_HP * StateManager.enemyHPMult);
        currentSpeed = GameData.ENEMY_SPEED * StateManager.enemySpeedMult;
        currentAttackRate = GameData.ENEMY_ATTACK_RATE;
        //점수는 100점부터 시작해 1일마다 100점의 10%씩 더해줌
        currentScorePlusValue = (int)(100 * (1 + (StateManager.dayCount - 1) * 0.1f));
        //돈은 200원부터 시작해 1일마다 200원의 10%씩 더해줌
        currentMoneytPlusValue = (int)(200 * (1 + (StateManager.dayCount - 1) * 0.1f));
    }
    //타겟을 향해 이동함
    void MoveToTarget()
    {
        if (currentTarget == null)
            return;
        //거리구하기
        float dis = Vector3.Distance(transform.position, currentTarget.transform.position);
        //방향구하기
        Vector3 dir = currentTarget.transform.position - transform.position;
        dir = dir.normalized;
        //타겟과 일정 거리 밖에있으면 이동함 
        if (dis > closeDistance)
        {
            transform.Translate(new Vector2(dir.x, dir.y) * currentSpeed * Time.deltaTime);
        }
        //타겟과 일정 거리 안에 있으면 공격
        else
        {
            //공격
            if (canAttack)
            {
                Attack();
                canAttack = false;
                StartAttackDamageAni();
                Invoke("SetTrueAttack", currentAttackRate);
            }
        }
    }
    //타겟이 있는 방향을 바라봄 만약 잘못되면 여기 수정
    void RotateSprite()
    {
        if (currentTarget == null)
            return;
        if(currentTarget.transform.position.x > transform.position.x)
        {
            s_renderer.flipX = true;
        }
        else
        {
            s_renderer.flipX = false;
        }
    }
    //어택 가능
    void SetTrueAttack()
    {
        canAttack = true;
    }
    //공격 애니메이션 실행 함수
    void StartAttackDamageAni()
    {
        animator.SetTrigger("Attack");
    }
    //가장 가까운적을 찾아 현재 타겟에 넣어줌
    void findTarget()
    {
        Collider2D[] targetCols = Physics2D.OverlapCircleAll(transform.position, 100, TargetLayer);
        float dis = 999;
        foreach (Collider2D col in targetCols)
        {
            float targetDis = Vector3.Distance(transform.position, col.transform.position); //거리가 안 맞으면 수정 here
            if (dis > targetDis)
            {
                dis = targetDis;
                currentTarget = col.gameObject;
            }
        }
        //쓸데없는 실행을 방지하기 위해 함수 실행에 딜레이를 줌
        Invoke("findTarget", findDelay);
    }
    //공격 기능 레이캐스트를 활용해 레이를 맞은 적 피격해줌
    void Attack()
    {
        if (currentTarget == null)
            return;

        //빌딩일 때
        if (currentTarget.CompareTag("Building"))
        {
            currentTarget.GetComponent<Building>().Hit(currentDamage);
        }
        //플레이어일 때
        else if (currentTarget.CompareTag("Player"))
        {
            currentTarget.GetComponent<Player>().Hit(currentDamage);
        }
    }
    //사망 함수
    void DieEntity()
    {
        if (currentHP <= 0)
        {
            //점수와 돈을 증가값 만큼 증가 시키고 텍스트 갱신
            StateManager.score += currentScorePlusValue;
            StateManager.money += currentMoneytPlusValue;
            GameManager.instance.stateManager.SetScoreText();
            GameManager.instance.stateManager.SetMoneyText();
            //적 개수 감소
            EnemyChecker.enemyCount--;
            //적의 수가 0이하면 클리어 실행
            if (EnemyChecker.enemyCount <= 0)
            {
                GameManager.instance.enemyChecker.clearNight();
            }
            gameObject.SetActive(false);
        }
    }
    //피격
    public void Hit(int dmg_size)
    {
        currentHP -= dmg_size;
        DieEntity();
    }
}

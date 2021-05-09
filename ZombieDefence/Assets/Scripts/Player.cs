using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;
    Animator animator;

    [SerializeField] Transform Weapon;

    float speed = 3f;
    int HP = 100;

    float recoverHPDelay = 3; //체력회복 대기시간

    Vector3 mousePos;

    [SerializeField] Slider HPslider; //플레이어 체력 슬라이더

    [SerializeField] GameObject GameEndBG; //BG = 백그라운드
    [SerializeField] Text EndScoreText;

    static public bool isPlayerDead = false;

    void Start()
    {
        isPlayerDead = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        HPslider.maxValue = GameData.PLAYER_HP;
        HPslider.value = GameData.PLAYER_HP;

        StartCoroutine(recoverHP());
    }
    void Update()
    {
        if (!isPlayerDead)
        {
            SetDir();
            SetWeaponDir();
        }
    }
    void FixedUpdate()
    {
        if (!isPlayerDead)
            Move();
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float currentSpeed = speed;
        if (h != 0 && v != 0)
            currentSpeed *= 0.7f;
        if (h != 0 || v != 0)
        {
            transform.Translate(new Vector2(h, v) * currentSpeed * Time.deltaTime);
            PlayAni(1);
        }
        else
        {
            PlayAni(0);
        }
    }
    //스케일로 방향을 조정하기 때문에 각도에 180도 혹은 0도를 더해줄 변수
    float angleSettingValue = 0;
    void SetDir()
    {
        //낮 밤 전부 플레이어 보도록 해줌
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x > transform.position.x)
        {
            //0.8은 현재 플레이어의 스케일 값
            transform.localScale = new Vector2(-0.8f, 0.8f);
            angleSettingValue = 0;
        }
        else
        {
            transform.localScale = new Vector2(0.8f, 0.8f);
            angleSettingValue = 180;
        }
    }
    //무기 방향 구하기
    void SetWeaponDir()
    {
        float angle = Mathf.Atan2(mousePos.y - Weapon.position.y, mousePos.x - Weapon.position.x) * Mathf.Rad2Deg;
        Weapon.transform.rotation = Quaternion.AngleAxis(angle + angleSettingValue, Vector3.forward);
    }
    void PlayAni(int state)
    {
        animator.SetInteger("State", state);
    }

    public void Hit(int size)
    {
        HP -= size;
        HPslider.value = HP;
        Die();
    }
    //플레이어의 이동속도, 체력, 회복속도와 같은 정보를 업데이트
    //각 곱하기율과 뺄셈율을 적용해줌
    public void SetPlayerInfo()
    {
        speed = GameData.PLAYER_SPEED * StateManager.playerSpeedMult;
        HP = (int)(GameData.PLAYER_HP * StateManager.playerHPMult);
        recoverHPDelay = 3 - StateManager.playerRecoverMinus;
        HPslider.maxValue = HP;
        HPslider.value = HP;
    }
    void Die()
    {
        if(HP <= 0)
        {
            //실행
            EnemyChecker.enemyCount = 9999;
            GameEndBG.SetActive(true);
            EndScoreText.text = "최종점수 : " + StateManager.score;
            isPlayerDead = true;
        }
    }
    //체력회복 기능 코루틴
    IEnumerator recoverHP()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (!StateManager.isDay)
            {
                yield return new WaitForSeconds(recoverHPDelay);
                if (((int)(GameData.PLAYER_HP * StateManager.playerHPMult)) > HP)
                {
                    HP += 1; //1씩 회복
                    HPslider.value = HP;
                }
            }
            yield return new WaitForSeconds(recoverHPDelay);
        }
    }
}

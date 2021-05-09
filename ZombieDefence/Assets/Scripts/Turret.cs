using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] AudioSource FireSound;
    public float fireRate;
    GameObject currentTarget;
    LayerMask targetLayer;

    float getTargetDelay = 1;

    void Start()
    {
        FireSound.volume = Manager.instance.volume_val * 0.6f;
        targetLayer = 1 << LayerMask.NameToLayer("Enemy");
        GetTarget();
        StartCoroutine(Attack_Routine());
    }
    //타겟 공격
    void Attack()
    {
        if (currentTarget == null || StateManager.isDay)
            return;
        if (!currentTarget.activeSelf)
        {
            currentTarget = null;
            return;
        }
        //사용가능 총알 찾기
        for(int idx = 0; idx < GameManager.instance.TurretBullets.Length; idx++)
        {
            if (!GameManager.instance.TurretBullets[idx].activeSelf)
            {
                //총알의 위치와 방향을 조정
                GameManager.instance.TurretBullets[idx].transform.position = transform.position;
                float angle = Mathf.Atan2(currentTarget.transform.position.y - transform.position.y, currentTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                GameManager.instance.TurretBullets[idx].transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                GameManager.instance.TurretBullets[idx].SetActive(true);
                FireSound.Play();
                return;
            }
        }
    }
    //가장 가까운 타겟의 오브젝트 정보를 현재 타겟에 넣어줌
    void GetTarget()
    {
        if(StateManager.isDay)
        {
            Invoke("GetTarget", getTargetDelay);
            return;
        }
        Collider2D[] targetCols = Physics2D.OverlapCircleAll(transform.position, 10, targetLayer);
        float dis = 999;
        foreach (Collider2D col in targetCols)
        {
            float targetDis = Vector2.Distance(transform.position, col.transform.position);
            if (dis > targetDis)
            {
                dis = targetDis;
                currentTarget = col.gameObject;
            }
        }
        //딜레이를 주어 반복 실행하여 낭비를 줄임
        Invoke("GetTarget", getTargetDelay);
    }
    //Attack 함수를 fireRate만큼 딜레이를 주어 무한반복하게함
    IEnumerator Attack_Routine()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(fireRate);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameObject player;
    WeaponManager weaponManager;
    StoreManager storeManager;
    BuildManager buildManager;
    StateManager stateManager;
    StageManager stageManager;
    [SerializeField] GameObject EnemyInfoBg;

    [SerializeField]
    UnityEngine.Experimental.Rendering.Universal.Light2D light;

    [SerializeField] GameObject DayUI;
    [SerializeField] GameObject NightUI;

    [SerializeField] Text NoticePointText; //포인트 알림 텍스트

    void Start()
    {
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        storeManager = GameObject.Find("StoreManager").GetComponent<StoreManager>();
        buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    //아침으로 바꾸는 기능 함수
    public void NextDay()
    {
        StateManager.isDay = true;

        DayUI.SetActive(true);
        NightUI.SetActive(false);
        player.layer = 8; //dayPlayer Layer

        storeManager.SetDayStoreState();
        weaponManager.SetDayWeaponState();
        StartCoroutine(SetDay());
        buildManager.SetDayBuildState();
        player.GetComponent<Player>().SetPlayerInfo();
        StateManager.point++;
        NoticePointText.gameObject.SetActive(true);
        GameObject.Find("NightBGMSound").GetComponent<AudioSource>().Stop();
    }
    //밤으로 바꿔 게임을 시작하는 함수
    public void NextNight()
    {
        DayUI.SetActive(false);
        NightUI.SetActive(true);

        StateManager.isDay = false;
        StateManager.dayCount++;
        stateManager.SetEnemyMult();
        stateManager.SetNightInfoText();
        player.layer = 10; //Team Layer

        storeManager.SetNightStoreState();
        weaponManager.SetNightWeaponState();
        buildManager.SetNightBuildState();
        stageManager.GenerateEnemy();
        StartCoroutine(SetNight());
        StartCoroutine(EffectEnemyInfo());
        GameObject.Find("ZombieSound").GetComponent<AudioSource>().Play();
        GameObject.Find("NightBGMSound").GetComponent<AudioSource>().Play();
    }
    //라이트 빛의 세기를 증가시켜 자연스러운 빛 구현
    IEnumerator SetDay()
    {
        WaitForSeconds Delay = new WaitForSeconds(0.02f);

        while (light.pointLightOuterRadius <= 500)
        {
            light.pointLightOuterRadius += 5f;
            yield return Delay;
        }
        light.pointLightOuterRadius = 500f;
    }
    //라이트 빛의 세기를 감소시켜 자연스러운 빛 구현
    IEnumerator SetNight()
    {
        WaitForSeconds Delay = new WaitForSeconds(0.02f);

        while (40 <= light.pointLightOuterRadius)
        {
            light.pointLightOuterRadius -= 5f;
            yield return Delay;
        }
        light.pointLightOuterRadius = 40f;
    }
    //적 정보 텍스트의 크기를 서서히 증가시키고 동시에 투명도를 조절해 자연스럽게 텍스트가 사라지도록 함
    IEnumerator EffectEnemyInfo()
    {

        EnemyInfoBg.SetActive(true);
        WaitForSeconds effectDelay = new WaitForSeconds(0.03f);
        for (float i = 1f; i <= 1.3f; i += 0.005f)
        {
            EnemyInfoBg.transform.localScale = new Vector3(i, i, 1);
            yield return effectDelay;
        }
        //텍스트 초기값으로 바꿔줌
        EnemyInfoBg.SetActive(false);
        EnemyInfoBg.transform.localScale = Vector3.one;
    }
}

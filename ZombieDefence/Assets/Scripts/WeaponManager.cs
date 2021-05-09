using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    WeaponController weaponController;
    ItemManager itemManager;
    InputManager inputManager;

    [SerializeField] Pistol pistol;
    [SerializeField] Rifle rifle;
    [SerializeField] Bow bow;

    public Slider RemaningBulletSlider;
    public Text RemaningAmmoText;

    int weaponKey = 2000;

    float attackRate;
    float reloadSpeed;

    bool canAttack = true;
    bool isReloading = false;

    private void Start()
    {
        weaponController = GameObject.Find("WeaponController").GetComponent<WeaponController>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        /*--무기 초기화--*/
        SetActiveWeapon(pistol.gameObject);
        weaponController.ChangeWeapon(pistol);
        attackRate = GameData.PISTOL_FIRE_RATE;
        reloadSpeed = GameData.PISTOL_RELOAD_SPEED;
        /*--무기 초기화--*/
    }
    private void Update()
    {
        if(canAttack && !isReloading && !StateManager.isDay && !Player.isPlayerDead)
        {
            SetWeapon();
            Attack();
            reloadManual();
        }
    }
    public void SetWeapon()
    {
        if (inputManager.WeaponChangeKey(1))
        {
            //Pistol
            SetWeaponData(2000, pistol.gameObject, pistol, GameData.PISTOL_FIRE_RATE, GameData.PISTOL_RELOAD_SPEED);
        }
        else if (inputManager.WeaponChangeKey(2))
        {
            //Rifle
            if (itemManager.items[2001].itemCount > 0)
            {
                SetWeaponData(2001, rifle.gameObject, rifle, GameData.RIFLE_FIRE_RATE, GameData.RIFLE_RELOAD_SPEED);
            }
        }
        else if (inputManager.WeaponChangeKey(3))
        {
            //Bow
            if (itemManager.items[2002].itemCount > 0)
            {
                SetWeaponData(2002, bow.gameObject, bow, GameData.BOW_FIRE_RATE, GameData.BOW_RELOAD_SPEED);
            }
        }
    }
    //무기 데이터 업데이트
    void SetWeaponData(int key, GameObject weaponObj, IWeapon weapon_, float attack_rate, float reload_speed)
    {
        weaponKey = key;
        SetActiveWeapon(weaponObj);
        weaponController.ChangeWeapon(weapon_);
        attackRate = attack_rate;
        reloadSpeed = reload_speed;

        /*UI설정*/
        RemaningBulletSlider.maxValue = itemManager.items[weaponKey].maxBullet;
        RemaningBulletSlider.value = itemManager.items[weaponKey].bulletCount;

        if (weaponKey != 2000) //권총이 아닐 때
            RemaningAmmoText.text = "탄창 : " + itemManager.items[weaponKey].ammoCount;
        else
            RemaningAmmoText.text = "탄창 : 무제한";
        /*UI설정*/
    }
    //무기 추가시 무기오브젝트 추가
    void SetActiveWeapon(GameObject weaponObj)
    {
        pistol.gameObject.SetActive(false);
        rifle.gameObject.SetActive(false);
        bow.gameObject.SetActive(false);
        weaponObj.SetActive(true);
    }
    public void Attack()
    {
        if (!canAttack)
            return;
        if (inputManager.Fire())
        {
            if (itemManager.items[weaponKey].bulletCount > 0)
            {
                weaponController.Attack();
                weaponController.weapon.OnFlash();
                canAttack = false;
                Invoke("WaitOffFlash", 0.08f);
                Invoke("WaitAttackRate", attackRate);
                itemManager.items[weaponKey].bulletCount--;
                SetBulletSlide(itemManager.items[weaponKey].bulletCount, itemManager.items[weaponKey].maxBullet);
                if (itemManager.items[weaponKey].bulletCount <= 0)
                {
                    reload(reloadSpeed);
                }
            }
            else
            {
                reload(reloadSpeed);
            }
        }
    }
    //사용중인 함수
    void WaitAttackRate()
    {
        canAttack = true;
    }
    //사용중인 함수
    //총구섬광 딜레이
    void WaitOffFlash()
    {
        weaponController.weapon.OffFlash();
    }
    //총알 정보 슬라이드UI 갱신
    void SetBulletSlide(int current_bullet_count, int max_bullet)
    {
        RemaningBulletSlider.maxValue = max_bullet;
        RemaningBulletSlider.value = current_bullet_count;
    }
    //수동 재장전 함수
    void reloadManual()
    {
        if (StateManager.isDay)
            return;
        if (inputManager.Reload())
        {
            reload(reloadSpeed);
        }
    }
    //재장전 함수 탄창이 0이상일시 실행 가능
    void reload(float reloadSpanTime)
    {
        if (itemManager.items[weaponKey].ammoCount > 0)
        {

            GameObject.Find("ReloadSound").GetComponent<AudioSource>().Play();
            StartCoroutine(reloadEffect(reloadSpanTime));
            itemManager.items[weaponKey].ammoCount--; //탄창 감소
            SetAmmoText(weaponKey); //탄창 텍스트 갱신
        }
    }
    IEnumerator reloadEffect(float reloadSpanTime)
    {
        //장전중 무기 변경 불가능
        isReloading = true;
        for (float idx = 0; idx < reloadSpanTime; idx += 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            //최대 총알 개수 / 장전속도 * 100을 해서 반복되는 동안 일정하게 증가시켜줌
            RemaningBulletSlider.value += itemManager.items[weaponKey].maxBullet / (reloadSpanTime * 100);
        }
        //최대값으로 초기화
        RemaningBulletSlider.value = itemManager.items[weaponKey].maxBullet;
        itemManager.items[weaponKey].bulletCount = itemManager.items[weaponKey].maxBullet;
        isReloading = false;
    }
    //남은탄창 텍스트 갱신
    void SetAmmoText(int key)
    {
        if (key == 2000)
        {
            RemaningAmmoText.text = "탄창 : 무제한";
            return;
        }
        RemaningAmmoText.text = "탄창 : " + itemManager.items[key].ammoCount;
    }
    //아침이 될시 권총으로 바꿔주고 권총 정보로 슬라이더와
    //무기 정보 텍스트 갱신
    public void SetDayWeaponState()
    {
        SetWeaponData(2000, pistol.gameObject, pistol, GameData.PISTOL_FIRE_RATE, GameData.PISTOL_RELOAD_SPEED);
        //--무기 탄약 초기화
        itemManager.items[2000].bulletCount = itemManager.items[2000].maxBullet;
        itemManager.items[2001].bulletCount = itemManager.items[2001].maxBullet;
        itemManager.items[2002].bulletCount = itemManager.items[2002].maxBullet;
        //--무기 탄약 초기화
        RemaningBulletSlider.maxValue = GameData.PISTOL_MAX_BULLET;
        RemaningBulletSlider.value = GameData.PISTOL_MAX_BULLET;
    }
    //구조 변경으로 쓰이지 않음
    public void SetNightWeaponState()
    {
        //WeaponInfoBg.SetActive(true);
    }
}

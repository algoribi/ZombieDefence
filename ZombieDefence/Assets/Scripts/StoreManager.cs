using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    InputManager inputManager;
    ItemManager itemManager;
    StateManager stateManager;
    BuildManager buildManager;


    [SerializeField] Text[] listText;
    [SerializeField] RectTransform catalog;

    [SerializeField] Text ItemPriceText;
    [SerializeField] Text AmmoPriceText;
    [SerializeField] Text RemaningItemText;
    [SerializeField] Text RemaningAmmoText;

    [SerializeField] GameObject BuildButton;
    [SerializeField] GameObject RemoveButton;

    [SerializeField] GameObject BuyAmmoButton;

    [SerializeField] Text MoneyText;

    int selectedList = 1; //선택된 아이템 0번부터
    float targetXPos = 0; //상점 UI가 이동할 X포지션
    int currentKey = 1000;

    public int getSelectedList { get => selectedList; private set { } }

    int item_price = 0;
    int ammo_price = 0;
    int remaning_item = 0;
    int remaning_ammo = 0;

    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();

        selectedList = 0;
        targetXPos = 0;
        currentKey = 1000;
        SetItemInfo();
    }
    void Update()
    {
        moveItemList();
        catalog.anchoredPosition = Vector2.Lerp(new Vector2(catalog.anchoredPosition.x, 0), new Vector2(targetXPos, 0), 0.095f);
    }
    void moveItemList()
    {
        if (inputManager.CatalogUp())
        {
            if (selectedList < listText.Length - 1)
            {
                SetOutFocusText(selectedList);
                selectedList++;
                SetFocusText(selectedList);
                targetXPos -= 350;
                SetSelectedItem(selectedList);
                //현재 선택된 아이템이 건물이면 건물 데이터를 줌
                buildManager.getCurrentBuilding(true);
                GameObject.Find("ScrollSound").GetComponent<AudioSource>().Play();
            }
        }
        else if (inputManager.CatalogDown())
        {
            if (selectedList > 0)
            {
                SetOutFocusText(selectedList);
                selectedList--;
                SetFocusText(selectedList);
                targetXPos += 350;
                SetSelectedItem(selectedList);
                //현재 선택된 아이템이 건물이면 건물 데이터를 줌
                buildManager.getCurrentBuilding(true);
                GameObject.Find("ScrollSound").GetComponent<AudioSource>().Play();
            }
        }
    }
    void SetFocusText(int idx)
    {
        listText[idx].color = new Color32(255, 255, 255, 255);
        listText[idx].fontSize = 160;
    }
    //이전 선택 아이템 텍스트 아웃포커스 상태로 조정
    void SetOutFocusText(int idx)
    {
        listText[idx].color = new Color32(170, 170, 170, 255);
        listText[idx].fontSize = 110;
    }

    //매개변수로 받은 키를 현재 키에 대입
    //아이템 정보 갱신
    //무기인지 건물인지 검사
    void SetSelectedItem(int itemKey)
    {
        currentKey = itemManager.ItemCode[itemKey];
        SetItemInfo();

        //아이템 구매 정보 텍스트 현재 아이템 정보로 갱신
        SetStoreInfos(item_price, ammo_price, remaning_item, remaning_ammo);
        //무기일시 탄창관련 버튼 활성화
        if (itemManager.items[currentKey].IT == Item.EItemType.weapon)
        {
            SetOnOffAmmo(true);

            //- 건물이 선택된 것이 아니므로 건설 버튼 비활성화 -//
            BuildButton.SetActive(false);
            BuildChecker.isSelectedBuilding = false;
        }
        //건물일시 탄창관련 버튼 비활성화
        else
        {
            SetOnOffAmmo(false);
            //- 해당 자리에 건물이 없으면 건설 버튼 활성화 -//
            if (BuildChecker.isBuildState)
                BuildButton.SetActive(true);
            //- 이미 건물이 있으면 제거 버튼 활성화 -//
            else
                RemoveButton.SetActive(true);
            BuildChecker.isSelectedBuilding = true;
        }
    }    
    //아이템 정보 현재 키에 해당하는 아이템 정보로 변경함
    void SetItemInfo()
    {
        item_price = itemManager.itemInfos[currentKey].itemCost;
        ammo_price = itemManager.itemInfos[currentKey].ammoCost;
        remaning_item = itemManager.items[currentKey].itemCount;
        remaning_ammo = itemManager.items[currentKey].ammoCount;
    }
    //받은 매개변수에 따라 탄창관련 버튼, 텍스트 활성화 또는 비활성화
    void SetOnOffAmmo(bool isOn)
    {
        if (isOn)
        {
            BuyAmmoButton.SetActive(true);
            AmmoPriceText.gameObject.SetActive(true);
            RemaningAmmoText.gameObject.SetActive(true);
        }
        else
        {
            BuyAmmoButton.SetActive(false);
            AmmoPriceText.gameObject.SetActive(false);
            RemaningAmmoText.gameObject.SetActive(false);
        }
    }
    //아이템 구매 정보 텍스트 현재 아이템 정보로 갱신
    void SetStoreInfos(int item_price, int ammo_price, int remaning_item, int remaning_ammo)
    {
        ItemPriceText.text = "가격 : " + string.Format("{0:#,##0}", item_price) + "원";
        AmmoPriceText.text = "탄약 : " + string.Format("{0:#,##0}", ammo_price) + "원";
        RemaningItemText.text = "남은개수 : " + string.Format("{0:#,##0}", remaning_item) + "개";
        RemaningAmmoText.text = "남은탄약 : " + string.Format("{0:#,##0}", remaning_ammo) + "개";
    }
    //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신
    void SetStoreInfos_Remaning(int remaning_item, int remaning_ammo)
    {
        RemaningItemText.text = "남은개수 : " + string.Format("{0:#,##0}", remaning_item) + "개";
        RemaningAmmoText.text = "남은탄약 : " + string.Format("{0:#,##0}", remaning_ammo) + "개";
    }
    //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신 (*다른 스크립트에서 사용)
    public void SetItemCountText()
    {
        SetStoreInfos_Remaning(itemManager.items[currentKey].itemCount, remaning_ammo);
    }
    //구매 버튼으로 실행 매개변수로 탄창인지 여부 받아옴
    public void buyItem(bool isAmmo)
    {
        //탄창구매이면
        if (isAmmo)
        {
            //가진 돈이 가격보다 많으면
            //해당 키의 탄창을 증가시키고 돈을 가격만큼 줄임
            //현재 아이템 정보 갱신
            if (StateManager.money >= ammo_price)
            {
                itemManager.items[currentKey].ammoCount++;
                reduceMoney(ammo_price);
                SetItemInfo();
                //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신
                SetStoreInfos_Remaning(remaning_item, itemManager.items[currentKey].ammoCount);
                GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            //가진 돈이 가격보다 많으면
            //해당 키의 아이템을 증가시키고 돈을 가격만큼 줄임
            //현재 아이템 정보 갱신
            if (StateManager.money >= item_price)
            {
                itemManager.items[currentKey].itemCount++;
                reduceMoney(item_price);
                //아이템 구매 탄창 정보 텍스트 현재 아이템 정보로 갱신
                SetStoreInfos_Remaning(itemManager.items[currentKey].itemCount, remaning_ammo);
                GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
            }
        }
    }
    //받은 매개변수 만큼 돈을 줄이고 돈 텍스트 갱신
    void reduceMoney(int how_much)
    {
        StateManager.money -= how_much;
        MoneyText.text = "가진 돈 : " + string.Format("{0:#,##0}", StateManager.money) + "원";
    }

    //아침이 될시 모든 정보 초기화
    public void SetDayStoreState()
    {
        SetOutFocusText(selectedList);
        selectedList = 0;
        SetFocusText(selectedList);
        currentKey = itemManager.ItemCode[selectedList];
        SetItemInfo();
        SetStoreInfos(item_price, ammo_price, remaning_item, remaning_ammo);
        targetXPos = 0;
        SetOnOffAmmo(false);
        BuildButton.SetActive(true);
    }
    //구조 변경으로 쓰이지 않음
    public void SetNightStoreState()
    {
        //StoreBg.SetActive(false);
    }
}

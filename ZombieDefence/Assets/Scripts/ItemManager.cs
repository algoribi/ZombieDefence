using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    int dicLength = 0;
    public Dictionary<int, Item> items = new Dictionary<int, Item>();
    public Dictionary<int, ItemInfo> itemInfos = new Dictionary<int, ItemInfo>();
    public Dictionary<int, int> ItemCode = new Dictionary<int, int>();
    //1000번대-건물 2000번대-무기
    

    void Awake()
    {
        //상점서 ItemCode 딕셔너리 키 1000번부터 0번은 방어벽
        //권총은 해당 없음
        items[2000] = new Item(Item.EItemType.weapon, 2000, 1, 99999, GameData.PISTOL_MAX_BULLET, GameData.PISTOL_MAX_BULLET);
        itemInfos[2000] = new ItemInfo("권총", 999999, 999999);


        AddItem(Item.EItemType.building, 1000, 0, "방어벽", GameData.NORMAL_BARRICADE_PRICE);
        AddItem(Item.EItemType.building, 1001, 0, "강화방어벽", GameData.ENHANCED_BARRICADE_PRICE);
        AddItem(Item.EItemType.building, 1002, 0, "일반포탑", GameData.FIRST_TURRET_PRICE);
        AddItem(Item.EItemType.building, 1003, 0, "중급포탑", GameData.SECOND_TURRET_PRICE);
        AddItem(Item.EItemType.building, 1004, 0, "고급포탑", GameData.THIRD_TURRET_PRICE);

        AddItem(Item.EItemType.weapon, 2001, 0, 0, GameData.RIFLE_MAX_BULLET, GameData.RIFLE_MAX_BULLET, "자동소총", GameData.RIFLE_PRICE, GameData.RIFLE_AMMO_PRICE);
        AddItem(Item.EItemType.weapon, 2002, 0, 0, GameData.BOW_MAX_BULLET, GameData.BOW_MAX_BULLET, "활", GameData.BOW_PRICE, GameData.BOW_AMMO_PRICE);
    }

    void AddItem(Item.EItemType item_type, int item_code, int item_count, string item_name, int item_cost)
    {
        ItemCode[dicLength++] = item_code;
        items[item_code] = new Item(item_type, item_code, item_count);
        itemInfos[item_code] = new ItemInfo(item_name, item_cost);
    }
    void AddItem(Item.EItemType item_type,int item_code, int item_count, int ammo_count, int max_bullet, int bullet_count, string item_name, int item_cost, int ammo_cost)
    {
        ItemCode[dicLength++] = item_code;
        items[item_code] = new Item(item_type, item_code, item_count, ammo_count, max_bullet, bullet_count);
        itemInfos[item_code] = new ItemInfo(item_name, item_cost, ammo_cost);
    }
}

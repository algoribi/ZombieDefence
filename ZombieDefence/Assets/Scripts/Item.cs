using UnityEngine;
public class Item// : MonoBehaviour
{
    public enum EItemType { weapon, building }
    public EItemType IT;
    public int itemCode;
    public int itemCount;
    public int ammoCount;
    public int maxBullet;
    public int bulletCount;

    public Item(EItemType item_type ,int item_code, int item_count)
    {
        IT = item_type;
        itemCode = item_code;
        itemCount = item_count;
    }
    public Item(EItemType item_type, int item_code, int item_count, int ammo_count, int max_bullet, int bullet_count)
    {
        IT = item_type;
        itemCode = item_code;
        itemCount = item_count;
        ammoCount = ammo_count;
        maxBullet = max_bullet;
        bulletCount = bullet_count;
    }
}
public class ItemInfo
{
    public string itemName;
    public int itemCost;
    public int ammoCost;
    public ItemInfo(string item_name, int item_cost, int ammo_cost)
    {
        itemName = item_name;
        itemCost = item_cost;
        ammoCost = ammo_cost;
    }
    public ItemInfo(string item_name, int item_cost)
    {
        itemName = item_name;
        itemCost = item_cost;
    }
}
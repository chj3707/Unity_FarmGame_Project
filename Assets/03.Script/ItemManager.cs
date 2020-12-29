using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum E_ITEMTYPE
{
    SEED = 0,
    CROPS
}

[System.Serializable]
public class Item
{
    public int ItemCode;
    public string ItemName;
    public int ItemPrice;
    public E_ITEMTYPE ItemType;
    public Sprite ItemSprite;

    public Item(int p_code, string p_name, int p_price, E_ITEMTYPE p_type, Sprite p_sprite)
    {
        ItemCode = p_code;
        ItemName = p_name;
        ItemPrice = p_price;
        ItemType = p_type;
        ItemSprite = p_sprite;
    }
    public Item() { }
}

[System.Serializable]
public class ItemList
{
    public List<Item> ItemListData = new List<Item>();
}


public class ItemManager : MonoBehaviour
{
    public static ItemList CropsItemDataBase = new ItemList();
    public static ItemList StoreItemDataBase = new ItemList();
    void Awake()
    {
        SaveData();
        LoadData();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SaveData()
    {
        CropsItemDataBase.ItemListData.Clear();
        CropsItemDataBase.ItemListData.Add(new Item(0, "Carrot", 5, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Carrot")));
        CropsItemDataBase.ItemListData.Add(new Item(1, "Pumpkin", 7, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Pumpkin")));
        CropsItemDataBase.ItemListData.Add(new Item(2, "Watermelon", 10, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Watermelon")));
        CropsItemDataBase.ItemListData.Add(new Item(3, "Tomato", 3, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Tomato")));
        CropsItemDataBase.ItemListData.Add(new Item(4, "Onion", 3, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Onion")));

        File.WriteAllText(Application.dataPath + "/Resources/ItemData.json", JsonUtility.ToJson(CropsItemDataBase));

        StoreItemDataBase.ItemListData.Clear();
        StoreItemDataBase.ItemListData.Add(new Item(0, "Seed", 5, E_ITEMTYPE.SEED, Resources.Load<Sprite>("UI Texture/Seed")));

        File.WriteAllText(Application.dataPath + "/Resources/StoreItemData.json", JsonUtility.ToJson(StoreItemDataBase));
    }

    void LoadData()
    {
        string str1 = File.ReadAllText(Application.dataPath + "/Resources/ItemData.json");
        CropsItemDataBase = JsonUtility.FromJson<ItemList>(str1);

        string str2 = File.ReadAllText(Application.dataPath + "/Resources/StoreItemData.json");
        StoreItemDataBase = JsonUtility.FromJson<ItemList>(str2);
    }
}


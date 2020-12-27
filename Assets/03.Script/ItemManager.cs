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

public class ItemList
{
    public List<Item> ItemListData;
}


public class ItemManager : MonoBehaviour
{
    private List<Item> ItemSaveData = new List<Item>();

    public static ItemList DataBase = new ItemList();

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
        ItemSaveData.Add(new Item(0, "Carrot", 5, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Carrot")));
        ItemSaveData.Add(new Item(1, "Pumpkin", 7, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Pumpkin")));
        ItemSaveData.Add(new Item(2, "Watermelon", 10, E_ITEMTYPE.CROPS, Resources.Load<Sprite>("UI Texture/Watermelon")));

        DataBase.ItemListData = ItemSaveData;

        File.WriteAllText(Application.dataPath + "/Resources/ItemData.json", JsonUtility.ToJson(DataBase));
    }

    void LoadData()
    {
        string str = File.ReadAllText(Application.dataPath + "/Resources/ItemData.json");
        DataBase = JsonUtility.FromJson<ItemList>(str);

    }
}


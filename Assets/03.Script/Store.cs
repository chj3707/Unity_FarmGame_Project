using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Transform StoreSlotPivot = null;
    public Slot StoreSlot = null;

    void Start()
    {
        /* Init */
        for (int i = 0; i < ItemManager.StoreItemDataBase.ItemListData.Count; i++)
        {
            Slot copyslot = GameObject.Instantiate(StoreSlot);
            copyslot.name = string.Format("{0}", ItemManager.StoreItemDataBase.ItemListData[i].ItemName);
            
            copyslot.ItemInfo = ItemManager.StoreItemDataBase.ItemListData[i];
            copyslot.SlotImage.sprite = copyslot.ItemInfo.ItemSprite;
            copyslot.Count = 1;
            copyslot.SlotState = E_SLOTSTATE.FULL;
            copyslot.SlotType = E_SLOTTYPE.STORE;

            copyslot.transform.SetParent(StoreSlotPivot);
            copyslot.transform.localScale = Vector3.one;

            copyslot.gameObject.SetActive(true);
        }

        this.gameObject.SetActive(false);
    }


    void Update()
    {

    }


}

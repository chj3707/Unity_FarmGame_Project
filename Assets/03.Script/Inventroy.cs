using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventroy : MonoBehaviour
{
    public Transform SlotPivot = null;
    public Slot Slot = null;

    public List<Slot> SlotList = new List<Slot>();
    public int SlotCount = 10;

    public BlockManager BlockManager = null;

    public Text Gold = null;
    public int MyGold = 50;

    public Text Seed = null;
    public int MySeedCount = 5;

    IEnumerator Start()
    {
        yield return null;

        this.gameObject.SetActive(false);
        Slot.gameObject.SetActive(false);

        /* Init */
        for (int i = 0; i < SlotCount; i++)
        {
            Slot copyobj = GameObject.Instantiate(Slot);
            copyobj.name = string.Format("SLOT {0}", i + 1);
            copyobj.SlotType = E_SLOTTYPE.INVENTORY;
            copyobj.transform.SetParent(SlotPivot);
            copyobj.transform.localScale = Vector3.one;

            SlotList.Add(copyobj);
        }

        foreach(var item in SlotList)
        {
            item.gameObject.SetActive(true);
        }

        UpdateInvenUI();
    }

    public void UpdateInvenUI()
    {
        Gold.text = string.Format("Gold : {0}", MyGold);
        Seed.text = string.Format(": {0}", MySeedCount);
    }

    void Update()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_SLOTSTATE
{ 
    EMPTY = 0,
    FULL
}

public class Slot : MonoBehaviour
{
    [Header("아이템 정보")] public Item ItemInfo = null;
    public Image SlotImage = null; // 하위 스프라이트에 넣을 이미지
    
    public Text CountText = null;
    public int CropsCount = 0; // 수확한 작물 개수


    public E_SLOTSTATE SlotState = E_SLOTSTATE.EMPTY;


    void Start()
    {
        SlotImage = transform.GetChild(0).GetComponent<Image>();
        DragImage.gameObject.SetActive(false);
    }

    void Update()
    {
        // 슬롯이 비어있으면 비활성화
        SlotImage.enabled = SlotState == E_SLOTSTATE.EMPTY ? false : true;
        CountText.enabled = SlotState == E_SLOTSTATE.EMPTY ? false : true;
          
        CountText.text = string.Format("{0}", CropsCount);
    }

    public Image DragImage = null;
    // 드래그 시작
    public void _On_BeginDrag(BaseEventData p_data)
    {
        PointerEventData eventdata = p_data as PointerEventData;
        // 빈칸이면 리턴
        if (eventdata == null || SlotState == E_SLOTSTATE.EMPTY)
        {
            return;
        }

        DragImage.sprite = SlotImage.sprite;
        DragImage.gameObject.SetActive(true);
        p_data.selectedObject = this.gameObject;
    }

    // 드래그 중
    public void _On_Drag(BaseEventData p_data)
    {
        PointerEventData eventdata = p_data as PointerEventData;
        DragImage.transform.position = eventdata.position;
    }

    // 드래그 종료
    public void _On_EndDrag(BaseEventData p_data)
    {
        DragImage.gameObject.SetActive(false);
    }

    // 드롭
    public void _On_Drop(BaseEventData p_data)
    {
        PointerEventData eventdata = p_data as PointerEventData;

        Slot slot = eventdata.selectedObject.GetComponent<Slot>();


        if (this.SlotState == E_SLOTSTATE.EMPTY)
        {
            Swap<E_SLOTSTATE>(ref this.SlotState, ref slot.SlotState);
            Swap<Item>(ref this.ItemInfo, ref slot.ItemInfo);
            Swap<int>(ref this.CropsCount, ref slot.CropsCount);

            this.SlotImage.sprite = slot.SlotImage.sprite;
            slot.SlotImage.sprite = null;
        }

        else
        {
            Swap<Item>(ref this.ItemInfo, ref slot.ItemInfo);
            Swap<int>(ref this.CropsCount, ref slot.CropsCount);

            Sprite tempsprite = this.SlotImage.sprite;
            this.SlotImage.sprite = slot.SlotImage.sprite;
            slot.SlotImage.sprite = tempsprite;
        }
    }

    // 데이터 스왑
    public void Swap<T>(ref T p_data1, ref T p_data2)
    {
        T tempdata = p_data1;
        p_data1 = p_data2;
        p_data2 = tempdata;
    }
}


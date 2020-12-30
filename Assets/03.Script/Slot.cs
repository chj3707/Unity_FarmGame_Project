using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_SLOTSTATE
{ 
    EMPTY = 0,
    FULL,
}

public enum E_SLOTTYPE
{
    NONE = 0,
    STORE,
    INVENTORY
}

// https://docs.unity3d.com/kr/2019.4/Manual/SupportedEvents.html 이벤트 트리거 호출 정리

public class Slot : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
    , IPointerClickHandler
{
    [Header("아이템 정보")] public Item ItemInfo = null;
    public Image SlotImage = null; // 하위 스프라이트에 넣을 이미지
    
    public Text CountText = null;
    public int Count = 0;

    public E_SLOTSTATE SlotState = E_SLOTSTATE.EMPTY;
    public E_SLOTTYPE SlotType = E_SLOTTYPE.NONE;

    Rect m_ToolTipSize;
    CanvasScaler m_CanvasScaler = null;

    private void Awake()
    {
        SlotImage = transform.GetChild(0).GetComponent<Image>();
    }
    void Start()
    {
        DragImage.gameObject.SetActive(false);
        SlotFocus.gameObject.SetActive(false);

        m_ToolTipSize = ToolTip.transform.GetChild(0).GetComponent<RectTransform>().rect; // 툴팁 판넬 사이즈
        m_CanvasScaler = transform.GetComponentInParent<CanvasScaler>();

        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        // 슬롯이 비어있으면 비활성화
        SlotImage.enabled = SlotState == E_SLOTSTATE.EMPTY ? false : true;
        CountText.enabled = SlotState == E_SLOTSTATE.EMPTY ? false : true;

        CountText.text = string.Format("{0}", Count);
    }

    void Update()
    {

    }

    public SlotToolTip ToolTip = null;
    public Transform SlotFocus = null;

    // 포인터가 오브젝트에 들어갈때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        //https://docs.unity3d.com/kr/530/ScriptReference/EventSystems.PointerEventData.html PointerEventData
        Slot slot = eventData.pointerEnter.GetComponent<Slot>();

        if (slot.SlotState == E_SLOTSTATE.EMPTY)
        {
            return;
        }

        Vector3 tempvec = new Vector3(m_ToolTipSize.width * 0.5f, m_ToolTipSize.height * 0.5f, 0f);
        Vector3 posvec = eventData.pointerEnter.transform.position + tempvec;

        ToolTip.transform.position = SetToolTipPosition(posvec); // 툴팁 위치 설정
        SlotFocus.position = eventData.pointerEnter.transform.position;

        // 툴팁 텍스트 설정
        ToolTip.ItemsName.text = string.Format("{0}", slot.ItemInfo.ItemName);
        ToolTip.ItemsInfo.text = string.Format("개수 : {0}\n판매가 : {1} Gold", slot.Count, slot.ItemInfo.ItemPrice);
        
        switch(slot.SlotType)
        {
            case E_SLOTTYPE.STORE:
                ToolTip.ItemsTip.text = "구매 - 우 클릭";
                break;
            case E_SLOTTYPE.INVENTORY:
                ToolTip.ItemsTip.text = "판매 - 우 클릭";
                break;
        }
        

        ToolTip.gameObject.SetActive(true);
        SlotFocus.gameObject.SetActive(true);
    }

    Vector3 SetToolTipPosition(Vector3 p_posvec)
    {
        Vector3 retvec = new Vector3();
        float width = p_posvec.x + (m_ToolTipSize.width * 0.5f);
        float height = p_posvec.y + (m_ToolTipSize.height * 0.5f);

        // 툴팁 잘림 방지 (슬롯 중앙을 기준으로 오른쪽 위로 툴팁 위치를 잡아주었음)
        if (width > m_CanvasScaler.referenceResolution.x) // 오른쪽
        {
            
            if (height > m_CanvasScaler.referenceResolution.y) // 오른쪽, 위쪽
            {
                retvec = new Vector3(p_posvec.x - m_ToolTipSize.width, p_posvec.y - m_ToolTipSize.height, 0f);
                return retvec;
            }
            retvec = new Vector3(p_posvec.x - m_ToolTipSize.width, p_posvec.y, 0f);
            return retvec;
        }
        
        if (height > m_CanvasScaler.referenceResolution.y) // 위쪽
        {
            retvec = new Vector3(p_posvec.x, p_posvec.y - m_ToolTipSize.height, 0f);
            return retvec;
        }

        retvec = p_posvec;
        return retvec;
    }

    // 포인터가 오브젝트에서 나올때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.gameObject.SetActive(false);
        SlotFocus.gameObject.SetActive(false);
    }

    public void SlotReset(Slot p_slot)
    {
        p_slot.ItemInfo = null;
        p_slot.SlotImage.sprite = null;
        p_slot.SlotState = E_SLOTSTATE.EMPTY;
        p_slot.SlotType = E_SLOTTYPE.INVENTORY;
    }

    public Inventroy Inven = null;
    public Store Store = null;
    public EventManager EventMgr = null;

    // 포인터를 누르고 뗄 때 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!Store.gameObject.activeSelf)
        {
            return;
        }

        Slot slot = eventData.pointerPress.GetComponent<Slot>();
        if (eventData.pointerId != -2 || slot.SlotState == E_SLOTSTATE.EMPTY)
        {
            return;
        }

        switch (slot.SlotType)
        {
            case E_SLOTTYPE.INVENTORY: // 인벤토리 슬롯 우클릭
                // 작물 판매
                --slot.Count;
                Inven.MyGold += slot.ItemInfo.ItemPrice;
                if (slot.Count <= 0)
                {
                    SlotReset(slot); // 작물이 0개가 되면 슬롯 초기화
                }

                slot.UpdateSlotUI();
                Inven.UpdateInvenUI();
                break;

            case E_SLOTTYPE.STORE: // 상점 슬롯 우클릭
                if (Inven.MyGold < slot.ItemInfo.ItemPrice) // 골드 부족
                {
                    Text temptext = GameObject.Instantiate(EventMgr.ErrorMsg);
                    temptext.transform.SetParent(EventMgr.transform);
                    temptext.transform.localPosition = Vector3.up * 300f;
                    temptext.transform.localScale = Vector3.one;
                    StartCoroutine(ErrorMsg.ErrorMsgCoroutine(temptext, EventMgr.GoldErrorMsg));
                    return;
                }
                // 씨앗 구입
                --slot.Count;
                ++Inven.MySeedCount;
                if (slot.Count <= 0)
                {
                    slot.Count = 99;
                }
                Inven.MyGold -= slot.ItemInfo.ItemPrice;

                slot.UpdateSlotUI();
                Inven.UpdateInvenUI();
                break;
        }
    }

    public Image DragImage = null;
    // 드래그 시작
    public void _On_BeginDrag(BaseEventData p_data)
    {
        PointerEventData eventdata = p_data as PointerEventData;
        // PointerEventData -> .pointerId // -1 : 좌클릭 -2 : 우클릭 -3 : 휠
        // Debug.Log(eventdata.pointerId);
        // 마우스 좌클릭만 입력 받기, 빈칸, 상점 슬롯 드래그 방지
        if (eventdata.pointerId != -1 || SlotState == E_SLOTSTATE.EMPTY || SlotType == E_SLOTTYPE.STORE)
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

        if (eventdata.selectedObject == null)
        {
            return;
        }

        Slot slot = eventdata.selectedObject.GetComponent<Slot>();
        // 상점 슬롯과 스왑 방지
        if (this.SlotType == E_SLOTTYPE.STORE)
        {
            return;
        }

        if (this.SlotState == E_SLOTSTATE.EMPTY)
        {
            Swap<E_SLOTSTATE>(ref this.SlotState, ref slot.SlotState);
            Swap<Item>(ref this.ItemInfo, ref slot.ItemInfo);
            Swap<int>(ref this.Count, ref slot.Count);

            this.SlotImage.sprite = slot.SlotImage.sprite;
            slot.SlotImage.sprite = null;       
        }

        else
        {
            Swap<Item>(ref this.ItemInfo, ref slot.ItemInfo);
            Swap<int>(ref this.Count, ref slot.Count);

            Sprite tempsprite = this.SlotImage.sprite;
            this.SlotImage.sprite = slot.SlotImage.sprite;
            slot.SlotImage.sprite = tempsprite;
        }

        this.UpdateSlotUI();
        slot.UpdateSlotUI();
    }

    // 데이터 스왑
    public void Swap<T>(ref T p_data1, ref T p_data2)
    {
        T tempdata = p_data1;
        p_data1 = p_data2;
        p_data2 = tempdata;
    }
}


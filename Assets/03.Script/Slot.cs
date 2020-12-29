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

// https://docs.unity3d.com/kr/2019.4/Manual/SupportedEvents.html 이벤트 트리거 호출 정리

public class Slot : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    [Header("아이템 정보")] public Item ItemInfo = null;
    public Image SlotImage = null; // 하위 스프라이트에 넣을 이미지
    
    public Text CountText = null;
    public int CropsCount = 0; // 수확한 작물 개수

    public E_SLOTSTATE SlotState = E_SLOTSTATE.EMPTY;

    Rect m_ToolTipSize;
    CanvasScaler m_CanvasScaler = null;

    private void Awake()
    {
        SlotImage = transform.GetChild(0).GetComponent<Image>();

        m_ToolTipSize = ToolTip.transform.GetChild(0).GetComponent<RectTransform>().rect; // 툴팁 판넬 사이즈
        m_CanvasScaler = transform.GetComponentInParent<CanvasScaler>();
    }
    void Start()
    {
        DragImage.gameObject.SetActive(false);
        UpdateSlotUI();
    }
    public void UpdateSlotUI()
    {
        // 슬롯이 비어있으면 비활성화
        SlotImage.enabled = SlotState == E_SLOTSTATE.EMPTY ? false : true;
        CountText.enabled = SlotState == E_SLOTSTATE.EMPTY ? false : true;

        CountText.text = string.Format("{0}", CropsCount);
    }

    void Update()
    {

    }

    public SlotToolTip ToolTip = null;

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

        // 툴팁 텍스트 설정
        ToolTip.ItemsName.text = string.Format("{0}", slot.ItemInfo.ItemName);
        ToolTip.ItemsInfo.text = string.Format("개수 : {0}", slot.CropsCount);
        ToolTip.ItemsPrice.text = string.Format("{0} Gold", slot.ItemInfo.ItemPrice);

        ToolTip.gameObject.SetActive(true);
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
    }



    public Image DragImage = null;
    // 드래그 시작
    public void _On_BeginDrag(BaseEventData p_data)
    {
        PointerEventData eventdata = p_data as PointerEventData;
        // 빈칸이면 리턴
        if (SlotState == E_SLOTSTATE.EMPTY)
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GAMESTATE
{
    NONE = 0,
    STOP_WORK,
    WEEDING, // 풀 제거
    PLOWING, // 밭 갈기
    PLANTING, // 씨앗 심기
    FARMING // 수확
}

public class GameManager : MonoBehaviour
{
    public static GAMESTATE GameState;

    public BlockManager m_BlockManager = null;

    void Start()
    {
        GameState = GAMESTATE.NONE;

        DirtBlock.SetActive(false);
        FarmLand.SetActive(false);
        Seed.SetActive(false);
    }

    void Update()
    {
        switch(GameState)
        {
            case GAMESTATE.WEEDING:
                WeedingProcess();
                break;
            case GAMESTATE.PLOWING:
                PlowingProcess();
                break;
            case GAMESTATE.PLANTING:
                PlantingProcess();
                break;
            case GAMESTATE.FARMING:
                FarmingProcess();
                break;
        }
    }

    public GameObject DirtBlock = null;
    /* 잡초 제거 */
    void WeedingProcess()
    {
        if (!m_BlockManager.IS_Focus)
        {
            return;
        }

        // 블록 포커스 상태에서 마우스 클릭
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_BlockManager.Hit_Info.transform.tag != "Grass")
                {
                    return;
                }

                ObjectManagement(DirtBlock);
            }
        }
    }

    public GameObject FarmLand = null;

    /* 밭 갈기 */
    void PlowingProcess()
    {
        if(!m_BlockManager.IS_Focus)
        {
            return;
        }

        // 블록 포커스 상태에서 마우스 클릭
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (m_BlockManager.Hit_Info.transform.tag != "Dirt")
                {
                    return;
                }

                ObjectManagement(FarmLand);
            }
        }
    }

    public GameObject Seed = null;
    public EventManager EventMgr = null;

    /* 씨앗 심기 */
    void PlantingProcess()
    {
        if (!m_BlockManager.IS_Focus)
        {
            return;
        }

        // 블록 포커스 상태에서 마우스 클릭
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_BlockManager.Hit_Info.transform.tag != "FarmLand")
                {
                    return;
                }
                // 씨앗 개수 부족
                if (Inven.MySeedCount <= 0)
                {
                    Text temptext = GameObject.Instantiate(EventMgr.ErrorMsg);
                    temptext.transform.SetParent(EventMgr.transform);
                    temptext.transform.localPosition = Vector3.zero;
                    temptext.transform.localScale = Vector3.one;
                    StartCoroutine(ErrorMsgCoroutine(temptext, EventMgr.SeedErrorMsg));
                    return;
                }

                ObjectManagement(Seed);
                --Inven.MySeedCount;
                Inven.Seed.text = string.Format(": {0}", Inven.MySeedCount);
            }
        }
    }
    
    // 메시지 색 변경하면서 투명해지면 오브젝트 삭제
    IEnumerator ErrorMsgCoroutine(Text p_text, string p_str)
    {
        Color tempcolor = p_text.color;
        while(true)
        {
            tempcolor = Color.Lerp(tempcolor, Color.clear, 0.2f);
            p_text.text = string.Format(p_str);

            yield return new WaitForSeconds(0.1f);

            p_text.color = tempcolor;
            if (p_text.color == Color.clear)
            {
                break;
            }
        }
        GameObject.Destroy(p_text.gameObject);
    }

    public float SeedYpos = 0.3f;
    FarmLand m_FarmLand = null;
    void ObjectManagement(GameObject p_obj)
    {
        // 선택한 블록 하위로 씨앗 오브젝트를 넣고 위치 설정 후 리턴
        if (p_obj == Seed)
        {
            m_FarmLand = m_BlockManager.Hit_Info.transform.GetComponent<FarmLand>();

            if (m_FarmLand.LandState == LANDSTATE.PLANTED)
            {
                return;
            }

            m_FarmLand.IS_Plant = true;

            GrowCrops seedinfo = p_obj.GetComponent<GrowCrops>();
            seedinfo.PlantState = E_PLANTSTATE.PLANT;

            Vector3 seedpos = Vector3.up * SeedYpos;
            GameObject copyseed = GameObject.Instantiate(p_obj);
            copyseed.SetActive(true);
            copyseed.transform.SetParent(m_BlockManager.Hit_Info.transform);
            copyseed.transform.position = m_BlockManager.Hit_Info.transform.position + seedpos;

            return;
        }

        /* 선택한 위치에 블록 복사,생성 후 해당 위치에 있던 블록 삭제 */
        GameObject copyobj = GameObject.Instantiate(p_obj);
        copyobj.SetActive(true);
        copyobj.transform.SetParent(m_BlockManager.FarmField);
        copyobj.transform.position = m_BlockManager.Hit_Info.transform.position;

        GameObject.Destroy(m_BlockManager.Hit_Info.transform.gameObject);
    }

    public Inventroy Inven = null;

    /* 작물 수확 */
    void FarmingProcess()
    {
        if (!m_BlockManager.IS_Focus)
        {
            return;
        }

        // 블록 포커스 상태에서 마우스 클릭
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_BlockManager.Hit_Info.transform.tag != "Crops")
                {
                    return;
                }

                // 작물의 부모오브젝트(FarmLand Pivot)에 접근해서 심겨져 있지 않은 상태(NONE)로 변경
                m_FarmLand = m_BlockManager.Hit_Info.transform.GetComponentInParent<FarmLand>();
                m_FarmLand.IS_Plant = false;

                SetSlotInfomation(Inven.SlotList);

                GameObject.Destroy(m_BlockManager.Hit_Info.transform.parent.gameObject);
            }
        }
    }

    // 슬롯 정보 설정
    void SetSlotInfomation(List<Slot> p_list)
    {
        // 수확한 작물 아이템 정보
        ItemInfo info = m_BlockManager.Hit_Info.transform.GetComponentInParent<ItemInfo>();

        // 인벤토리 슬롯 아이템 정보 설정, 이미지 변경
        foreach (var item in p_list)
        {
            if (item.SlotState == E_SLOTSTATE.EMPTY)
            {
                item.ItemInfo = info.CropsInfo; // 작물 정보
                item.SlotImage.sprite = info.CropsInfo.ItemSprite; // 이미지
                ++item.CropsCount; // 작물 개수
                item.SlotState = E_SLOTSTATE.FULL;
                break;
            }

            // 슬롯에 아이템이 이미 있는 경우
            else
            {
                // 수확한 작물과 슬롯의 아이템 정보가 일치하면 아이템 개수 증가시키고 반복문 종료
                if (item.ItemInfo == info.CropsInfo)
                {
                    ++item.CropsCount;
                    break;
                }

                else
                    continue;
            }
        }
    }
}

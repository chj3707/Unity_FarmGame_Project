using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                ObjectManagement(Seed);
            }
        }
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

                GameObject.Destroy(m_BlockManager.Hit_Info.transform.gameObject);
            }
        }
    }
}

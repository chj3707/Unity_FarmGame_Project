using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAMESTATE
{
    NONE = 0,
    STOP_WORK,
    WEEDING,
    PLOWING
}

public class GameManager : MonoBehaviour
{
    public static GAMESTATE GameState;

    public BlockManager m_BlockManager = null;

    public GameObject DirtBlock = null;
    public GameObject FarmLand = null;

    void Start()
    {
        GameState = GAMESTATE.NONE;

        DirtBlock.SetActive(false);
        FarmLand.SetActive(false);
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
        }
    }

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

    void ObjectManagement(GameObject p_obj)
    {
        /* 선택한 위치에 블록 복사,생성 후 해당 위치에 있던 블록 삭제 */
        GameObject copyobj = GameObject.Instantiate(p_obj);
        copyobj.SetActive(true);
        copyobj.transform.SetParent(m_BlockManager.FarmField);
        copyobj.transform.position = m_BlockManager.Hit_Info.transform.position;

        GameObject.Destroy(m_BlockManager.Hit_Info.transform.gameObject);
    }
}

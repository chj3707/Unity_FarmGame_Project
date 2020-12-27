using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    Transform m_WorkBtn = null;
    Transform m_Inventory = null;

    public Text ErrorMsg = null;
    public string SeedErrorMsg = "씨앗이 부족합니다.";

    void Start()
    {
        m_WorkBtn = transform.GetChild(0);
        m_Inventory = transform.GetChild(1);
    }

    void Update()
    {
        
    }

    // 풀뽑기 버튼 클릭
    public void _On_WeedingBtnClick()
    {
        GameManager.GameState = GAMESTATE.WEEDING;
    }

    // 밭갈기 버튼 클릭
    public void _On_PlowingBtnClick()
    {
        GameManager.GameState = GAMESTATE.PLOWING;
    }

    // 씨앗심기 버튼 클릭
    public void _On_PlantingBtnClick()
    {
        GameManager.GameState = GAMESTATE.PLANTING;
    }

    // 작물수확 버튼 클릭
    public void _On_FarmingBtnClick()
    {
        GameManager.GameState = GAMESTATE.FARMING;
    }

    // 작업중지 버튼 클릭
    public void _On_StopWorkBtnClick()
    {
        GameManager.GameState = GAMESTATE.STOP_WORK;
    }

    // 가방 클릭
    public void _On_BagBtnClick()
    {
        // 인벤토리 활성화
        m_Inventory.transform.gameObject.SetActive(true);

        for (int i = 0; i < m_WorkBtn.transform.childCount; i++)
        {
            // 자식 오브젝트(버튼들)에 접근해서 interactable 비활성화 
            m_WorkBtn.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
    
    // 가방->종료 버튼 클릭
    public void _On_Inven_CloseBtnClick()
    {
        // 인벤토리 비활성화
        m_Inventory.transform.gameObject.SetActive(false);

        for (int i = 0; i < m_WorkBtn.transform.childCount; i++)
        {
            // 자식 오브젝트(버튼들)에 접근해서 interactable 활성화 
            m_WorkBtn.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }
}

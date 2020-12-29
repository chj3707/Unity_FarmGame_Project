using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    Transform m_Inventory = null;
    Transform m_Store = null;

    public Text ErrorMsg = null;
    public string SeedErrorMsg = "씨앗이 부족합니다.";

    public Image BgroundImage = null;

    void Start()
    {
        m_Inventory = transform.GetChild(2);
        m_Store = transform.GetChild(3);

        BgroundImage.gameObject.SetActive(false);
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
        m_Inventory.gameObject.SetActive(true);
        BgroundImage.gameObject.SetActive(true);
    }
    
    // 가방->상점 버튼 클릭
    public void _On_Inven_StoreBtnClick()
    {
        m_Store.gameObject.SetActive(true);
        m_Inventory.Find("상점 버튼").GetComponent<Button>().gameObject.SetActive(false);
    }

    // 가방->종료 버튼 클릭
    public void _On_Inven_CloseBtnClick()
    {
        // 인벤토리 비활성화 , 상점이 열려있으면 비활성화
        m_Inventory.gameObject.SetActive(false);
        if (m_Store.gameObject.activeSelf)
        {
            m_Store.gameObject.SetActive(false);
            m_Inventory.Find("상점 버튼").GetComponent<Button>().gameObject.SetActive(true);
        }

        BgroundImage.gameObject.SetActive(false);

    }
}

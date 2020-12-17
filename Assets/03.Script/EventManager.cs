using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    void Start()
    {
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

    // 작업중지 버튼 클릭
    public void _On_StopWorkBtnClick()
    {
        GameManager.GameState = GAMESTATE.STOP_WORK;
    }
}

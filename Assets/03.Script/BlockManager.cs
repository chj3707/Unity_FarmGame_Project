using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 블록 상태 */
public enum BLOCKSTATE
{
    NONE = 0,
    FOCUS
}

public struct BlockPack
{
    public GameObject BlockObj;
    public BLOCKSTATE BlockState;
    public bool ISfocus;
}

public class BlockManager : MonoBehaviour
{
    public Transform FarmField = null;
    public GameObject BlockObj = null; // 복사할 오브젝트

    public static int FarmSize = 10;
    BlockPack[,] m_BlockPackArr = new BlockPack[FarmSize, FarmSize];

    void Start()
    {
        /* Init */
        for (int z = 0; z < FarmSize; z++)
        {
            for (int x = 0; x < FarmSize; x++)
            {
                Vector3 posvec = new Vector3(FarmField.position.x + x, 0f, FarmField.position.z + z);
                GameObject copyobj = GameObject.Instantiate(BlockObj);
                copyobj.tag = "Grass";
                copyobj.name = string.Format("블록:{0}{1}", z, x);
                copyobj.SetActive(true);
                copyobj.transform.GetChild(0).gameObject.SetActive(false);
                copyobj.transform.SetParent(FarmField);
                copyobj.transform.position = posvec;

                m_BlockPackArr[z, x].BlockObj = copyobj;
                m_BlockPackArr[z, x].BlockState = BLOCKSTATE.NONE;
                m_BlockPackArr[z, x].ISfocus = false;
            }
        }
    }

    void Update()
    {
        RayHitUpdate();
    }

    void RayHitUpdate()
    {
        // 메인카메라 에서 스크린 좌표의 마우스 위치로 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_info;

        // 메인카메라 에서 스크린 마우스 위치로 레이 발사
        if (Physics.Raycast(ray, out hit_info))
        {
            if (hit_info.transform == null)
            {
                return;
            }
            //Debug.Log(hit_info.transform.position);
        }

        if(hit_info.transform != null)
        {
            for (int z = 0; z < FarmSize; z++)
            {
                for (int x = 0; x < FarmSize; x++)
                {
                    // 블록 위에 마우스를 올리면 블록 배열에서 해당 블록을 찾아서 플래그를 켜줌
                    m_BlockPackArr[z, x].ISfocus = hit_info.transform.gameObject.name == m_BlockPackArr[z, x].BlockObj.name ? true : false;
                    
                    // 플래그가 켜지면 함수 호출
                    if (m_BlockPackArr[z, x].ISfocus)
                    {
                        BlockFocusing();
                    }
                }
            }
        }
    }

    // 블록 상태에 따라 하위 오브젝트 활성화, 비활성화
    void BlockFocusing()
    {
        for (int z = 0; z < FarmSize; z++)
        {
            for (int x = 0; x < FarmSize; x++)
            {     
                // 블록 배열에서 플래그에 따라서 블록 상태 변경
                m_BlockPackArr[z, x].BlockState = m_BlockPackArr[z, x].ISfocus ? BLOCKSTATE.FOCUS : BLOCKSTATE.NONE;

                // 블록 상태에 따라서 하위 오브젝트 활성화, 비활성화
                switch (m_BlockPackArr[z,x].BlockState)
                {
                    case BLOCKSTATE.NONE:
                        m_BlockPackArr[z, x].BlockObj.transform.GetChild(0).gameObject.SetActive(false);
                        break;
                    case BLOCKSTATE.FOCUS:
                        m_BlockPackArr[z, x].BlockObj.transform.GetChild(0).gameObject.SetActive(true);
                        break;
                }
            }
        }
    }
}

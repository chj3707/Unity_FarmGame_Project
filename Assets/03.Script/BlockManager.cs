using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Transform FarmField = null;
    public GameObject BlockObj = null; // 복사할 오브젝트

    public int FarmSize = 10;

    void Start()
    {
        Select.SetActive(false);
        CropsSelect.SetActive(false);
        SelectUnable.SetActive(false);
        BlockObj.SetActive(false);

        /* Init */
        for (int z = 0; z < FarmSize; z++)
        {
            for (int x = 0; x < FarmSize; x++)
            {
                Vector3 posvec = new Vector3(FarmField.position.x + x, 0f, FarmField.position.z + z);
                GameObject copyobj = GameObject.Instantiate(BlockObj);
                copyobj.SetActive(true);
                copyobj.transform.SetParent(FarmField);
                copyobj.transform.position = posvec;
            }
        }
    }

    void Update()
    {
        FocusUpdate();
    }

    public GameObject Select = null;
    public GameObject CropsSelect = null;
    public GameObject SelectUnable = null;
    public bool IS_Focus = false;

    public RaycastHit Hit_Info;

    void FocusUpdate()
    {
        if (GameManager.GameState == GAMESTATE.NONE || GameManager.GameState == GAMESTATE.STOP_WORK)
        {
            return;
        }

        // 메인카메라 에서 스크린 좌표의 마우스 위치로 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // https://mentum.tistory.com/253 LayerMask
        int layermask = 1 << LayerMask.NameToLayer("Select");
        layermask = ~layermask; // "Select" 레이어만 제외

        float maxdistance = Mathf.Abs(FarmField.position.magnitude - Camera.main.transform.position.magnitude);

        if (Physics.Raycast(ray, out Hit_Info, maxdistance, layermask))
        {
            IS_Focus = true;

            FarmLand farmland = Hit_Info.transform.GetComponent<FarmLand>();

            if (GameManager.GameState == GAMESTATE.WEEDING && Hit_Info.transform.tag != "Grass"
                || GameManager.GameState == GAMESTATE.PLOWING && Hit_Info.transform.tag != "Dirt"
                || GameManager.GameState == GAMESTATE.PLANTING && Hit_Info.transform.tag != "FarmLand" 
                || GameManager.GameState == GAMESTATE.PLANTING && farmland.LandState == LANDSTATE.PLANTED
                || GameManager.GameState == GAMESTATE.FARMING && Hit_Info.transform.tag != "Crops")
            {
                IS_Focus = false;
            }

            //Debug.Log(Hit_Info.transform);
            Debug.DrawRay(ray.origin, ray.direction * maxdistance, Color.red);

            /* 작물 오브젝트 */
            if (GameManager.GameState == GAMESTATE.FARMING && Hit_Info.transform.tag == "Crops")
            {
                Vector3 ypos = Vector3.up * 0.5f;
                CropsSelect.transform.position = Hit_Info.transform.position + ypos;

                Select.SetActive(false);
                SelectUnable.SetActive(false);
                CropsSelect.SetActive(true);
            }

            /* 선택 불가능한 오브젝트 */
            else if (!IS_Focus)
            {        
                SelectUnable.transform.position = Hit_Info.transform.position;

                Select.SetActive(false);
                SelectUnable.SetActive(true);
                CropsSelect.SetActive(false);
            }

            /* 선택 가능한 오브젝트 */
            else
            {
                Select.transform.position = Hit_Info.transform.position;

                Select.SetActive(true);
                SelectUnable.SetActive(false);
                CropsSelect.SetActive(false);
            }
        }
        else
        {
            IS_Focus = false;
            Select.SetActive(false);
            SelectUnable.SetActive(false);
            CropsSelect.SetActive(false);
        }
    }
}
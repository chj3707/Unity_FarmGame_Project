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
        SelectBlock.SetActive(false);
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

    public GameObject SelectBlock = null;
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
            if (GameManager.GameState == GAMESTATE.WEEDING && Hit_Info.transform.tag != "Grass")
            {
                OffFocus();
                return;
            }
            if (GameManager.GameState == GAMESTATE.PLOWING && Hit_Info.transform.tag != "Dirt")
            {
                OffFocus();
                return;
            }

            //Debug.Log(Hit_Info.transform);
            Debug.DrawRay(ray.origin, ray.direction * maxdistance, Color.red);

            IS_Focus = true;
            SelectBlock.transform.position = Hit_Info.transform.position;
            SelectBlock.SetActive(true);
        }
        else
        {
            OffFocus();
        }
    }

    private void OffFocus()
    {
        IS_Focus = false;
        SelectBlock.SetActive(false);
    }
}

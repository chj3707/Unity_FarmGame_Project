using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PLANTSTATE
{
    SEED = 0,
    PLANT,
    MATURE
}

public class GrowCrops : MonoBehaviour
{
    [Header("작물")] public GameObject[] CropsArr = null;
    public GameObject CropsProtoType = null;

    [Header("성장 시간")]public float GrowTime = 10.0f;

    public E_PLANTSTATE PlantState = E_PLANTSTATE.SEED;

    void Start()
    {
        foreach (var item in CropsArr)
        {
            item.SetActive(false);
        }
        
        CropsArraySetting();

        if (PlantState == E_PLANTSTATE.PLANT)
        {
            StartCoroutine(GrowCoroutine());
        }
    }

    /* 작물 성장 */
    IEnumerator GrowCoroutine()
    {
        float tempval = 0f;
        float temptime = 0.01f;
        Vector3 scale = new Vector3(transform.GetChild(0).localScale.x * 2f, transform.GetChild(0).localScale.y * 5f, transform.GetChild(0).localScale.z * 2f);

        GameObject cloneobj = GameObject.Instantiate(CropsProtoType);
        SetProtoType(cloneobj);

        Renderer temprenderer = transform.GetChild(0).GetComponent<Renderer>();
        temprenderer.material.color = Color.green;

        while (PlantState == E_PLANTSTATE.PLANT)
        {
            temprenderer.material.color = Color.Lerp(Color.green, Color.red, tempval / GrowTime);
            transform.GetChild(0).transform.localScale = Vector3.Lerp(transform.GetChild(0).localScale, scale, temptime / GrowTime);

            tempval += temptime;

            yield return new WaitForSeconds(temptime);

            if (tempval >= GrowTime)
            {
                break;
            }
        }

        // 성장 완료
        PlantState = E_PLANTSTATE.MATURE;

        cloneobj.transform.SetParent(this.gameObject.transform.parent);
        cloneobj.transform.position = this.transform.position;
        cloneobj.transform.GetChild(0).gameObject.SetActive(true);

        GameObject.Destroy(this.gameObject);
    }

    void Update()
    {
      

    }

    // 작물 배열 인덱스 설정
    void CropsArraySetting()
    {
        GameObject tempobj = null;

        for (int i = 0; i < CropsArr.Length; i++)
        {
            for (int j = 0; j < ItemManager.DataBase.ItemListData.Count; j++)
            {
                if (CropsArr[i].name == ItemManager.DataBase.ItemListData[j].ItemName)
                {
                    tempobj = CropsArr[i];
                    CropsArr[i] = CropsArr[j];
                    CropsArr[j] = tempobj;
                    break;
                }
            }
        }

    }

    // 프로토타입 설정
    void SetProtoType(GameObject p_prototype)
    {
        int randindex = Random.Range(0, ItemManager.DataBase.ItemListData.Count);

        GameObject copyobj = GameObject.Instantiate(CropsArr[randindex]);
        copyobj.transform.SetParent(p_prototype.transform);
        copyobj.transform.position = Vector3.zero;

        // 작물 정보 설정
        ItemInfo info = copyobj.GetComponentInParent<ItemInfo>();
        info.CropsInfo = ItemManager.DataBase.ItemListData[randindex];
        //Debug.Log(ItemManager.DataBase.ItemListData[randindex].ItemName);
    }
}

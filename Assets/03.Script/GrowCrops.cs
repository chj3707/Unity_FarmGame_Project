using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowCrops : MonoBehaviour
{
    [Header("작물")] public GameObject[] CropsArr = null;

    [Header("성장 시간")]public float GrowTime = 10.0f;
 
    public bool IS_Mature = false;

    void Start()
    {
        for (int i = 0; i < CropsArr.Length; i++)
        {
            CropsArr[i].SetActive(false);
        }
        
        StartCoroutine(GrowCoroutine());
    }

    IEnumerator GrowCoroutine()
    {
        float tempval = 0f;
        float temptime = 0.01f;
        Vector3 scale = new Vector3(transform.GetChild(0).localScale.x * 2f, transform.GetChild(0).localScale.y * 5f, transform.GetChild(0).localScale.z * 2f);

        GameObject copycrops = GameObject.Instantiate(CropsArr[Random.Range(0, CropsArr.Length)]);
      
        Renderer temprenderer = transform.GetChild(0).GetComponent<Renderer>();
        temprenderer.material.color = Color.green;

        while (!IS_Mature)
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

        IS_Mature = true;

        copycrops.transform.SetParent(this.gameObject.transform.parent);
        copycrops.transform.position = this.transform.position;
        copycrops.SetActive(true);

        GameObject.Destroy(this.gameObject);
    }

    void Update()
    {
      

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    SEED = 0,
    PLANT,
    MATURE
}


public class GrowSeed : MonoBehaviour
{
    public float GrowTime = 10.0f;
    public STATE PlantState;

    private void Awake()
    {
        PlantState = STATE.SEED;
    }

    void Start()
    {
        StartCoroutine(GrowCoroutine());
    }

    IEnumerator GrowCoroutine()
    {
        float tempval = 0f;
        float temptime = 0.01f;

        PlantState = STATE.PLANT;
        transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;

        while (PlantState == STATE.PLANT)
        {
            //Vector3 scale = new Vector3(Mathf.Lerp(transform.GetChild(0).localScale.x, transform.GetChild(0).localScale.x * 2, tempval),
            //    Mathf.Lerp(transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.y * 10, tempval),
            //    Mathf.Lerp(transform.GetChild(0).localScale.z, transform.GetChild(0).localScale.z * 2, tempval));
           
            //transform.GetChild(0).GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, tempval);
            //transform.GetChild(0).transform.localScale = scale;

            tempval += temptime;

            yield return new WaitForSeconds(temptime);      

            if(tempval >= GrowTime)
            {
                Debug.Log("완료");
                PlantState = STATE.MATURE;
            }
        }
        
    }

    void Update()
    {
        
    }
}

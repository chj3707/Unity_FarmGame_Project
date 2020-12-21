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
        Vector3 scale = new Vector3(transform.GetChild(0).localScale.x * 2f, transform.GetChild(0).localScale.y * 5f, transform.GetChild(0).localScale.z * 2f);

        PlantState = STATE.PLANT;
        transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;

        while (PlantState == STATE.PLANT)
        {

            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, tempval / GrowTime);
            transform.GetChild(0).transform.localScale = Vector3.Lerp(transform.GetChild(0).localScale, scale, temptime / GrowTime);

            tempval += temptime;

            yield return new WaitForSeconds(temptime);

            if (tempval >= GrowTime)
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

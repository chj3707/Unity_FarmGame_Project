using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBlock : MonoBehaviour
{
    public GameObject m_GrassBlock = null;
    public Transform m_BolockPos = null;

    public int Row = 10; // 행 크기
    public int Col = 10; // 열 크기
    void Start()
    {
        for (int z = 0; z < Col; z++)
        {
            for (int x = 0; x < Row; x++) 
            {
                Vector3 posvec = new Vector3(m_BolockPos.position.x + x, 0f, m_BolockPos.position.z + z);
                GameObject copyobj = GameObject.Instantiate(m_GrassBlock);
                copyobj.SetActive(true);
                copyobj.tag = "Grass";
                copyobj.transform.parent = m_BolockPos;
                copyobj.transform.Translate(posvec);
            }
        }
    }

    void Update()
    {
        
    }
}

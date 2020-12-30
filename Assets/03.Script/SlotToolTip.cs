using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    public Transform ToolTip = null;

    public Text ItemsName = null;
    public Text ItemsInfo = null;
    public Text ItemsTip = null;

    void Start()
    {
        ToolTip.gameObject.SetActive(false);
    }


    void Update()
    {
        
    }
}

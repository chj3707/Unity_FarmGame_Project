using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LANDSTATE
{
    NONE = 0,
    PLANTED
}

public class FarmLand : MonoBehaviour
{
    public LANDSTATE LandState = LANDSTATE.NONE;
    public bool IS_Plant = false;

    void Start()
    {
    
    }

    void Update()
    {
        LandState = IS_Plant ? LANDSTATE.PLANTED : LANDSTATE.NONE;
    }
}

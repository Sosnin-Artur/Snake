using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangePlane : Plane
{    
    [SerializeField] private ColorChanger colorChanger;
    
    public override void Place()
    {        
        colorChanger.ChangeColor();
    }    
}

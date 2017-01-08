using System;
using UnityEngine;
using System.Collections.Generic;  
using System.Collections;
 
public class UPanel : UINode
{
    public UPanel(GameObject go) : base(go)
    {
        
    }
    /// <summary>
    /// 新增一个层
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="depth"></param>
    public void addLayer(Transform transform, uint depth = 0)
    {
        UButton button = this.getChildByName("") as UButton;
    }
} 


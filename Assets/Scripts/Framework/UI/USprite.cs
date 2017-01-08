using System;
using UnityEngine;
using System.Collections.Generic;  
using System.Collections;

public class USprite : UINode
{
    private UISprite mSprite;

    public USprite(UIBase data) : base(data)
    {
        mSprite = gameObject.GetComponent<UISprite>();
    }

}  


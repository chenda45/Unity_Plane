using System;
using UnityEngine;
using System.Collections.Generic;  
using System.Collections;

public class ULabel : UINode
{
    private UILabel mLabel;

    public ULabel(UIBase data) : base(data)
    {
        mLabel = gameObject.GetComponent<UILabel>();

    }

    public string text 
    {
        get 
        {
            return mLabel.text;
        }
        set 
        {
            mLabel.text = text;
        }
    }
}  


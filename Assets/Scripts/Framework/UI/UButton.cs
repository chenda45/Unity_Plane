using System;
using UnityEngine;
using System.Collections.Generic;  
using System.Collections;
 
public class UButton : UINode
{
    private UIButton mButton;
    private USprite mSprite;
    private ULabel mLabel;

    public UButton(UIBase data) : base(data) 
    {
        mButton = gameObject.GetComponent<UIButton>();
        UISprite sprite = gameObject.GetComponent<UISprite>();
        if(sprite != null)
        {
            UIBase spriteData = GetChildDataByName(sprite.name);
            if (spriteData != null)
            {
                mSprite = new USprite(spriteData);
            } 
        }

        UILabel label = gameObject.GetComponentInChildren<UILabel>();
        if(label != null)
        {
            UIBase labelData = GetChildDataByName(sprite.name);
            if (labelData != null)
            {
                mLabel = new ULabel(labelData);
            } 
        }
    }


     
} 

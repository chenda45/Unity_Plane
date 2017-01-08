using UnityEngine;
using System;
using System.Collections.Generic;

public class UINode : UINodeBase
{
    private int mDepth;
    private UIBase mData;
    private List<UIBase> childList;

    public UINode() 
    {
        
    }

    public UINode(UIBase data) 
    {
        mTransform = data.transform;
        mObj = mTransform.gameObject;
        mName = mObj.name;
        mParent = mTransform.parent.gameObject;
        mBaseDepth = data.depth;
        childList = data.childList;
        mData = data;
    }

    public UIBase GetChildDataByName(string name) 
    {
        for (int i = 0; i < childList.Count; i++)
        {
            if(childList[i].name.EndsWith("/@" + name))
            {
                return childList[i];
            }
        }
        Debug.LogError("Error: can't find child data by name = " + name);
        return null;
    }
     

} 

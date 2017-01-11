using UnityEngine;
using System;
using System.Collections.Generic;

public class UINode : UINodeBase
{
    private int mBaseDepth;
    private UIBase mNodeData; 

    public UINode(UIBase data) 
    {
        mObj = data.transform.gameObject;
        mName = data.name;
        mNodeData = data;
        mBaseDepth = data.depth;
        mTransform = data.transform;
        if(mTransform.parent != null)
        {
            mParent = mTransform.parent.gameObject;
        }
         
    }

    /// <summary>
    /// 根据名字获取node数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected UIBase GetChildDataByName(string name) 
    {
        List<UIBase> childList = mNodeData.childList;
        for (int i = 0; i < childList.Count; i++)
        {
             if(childList[i].name.EndsWith("/@" + name))
             {
                
             }
        }
        return null;
    }
} 

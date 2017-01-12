//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;


/// <summary>
/// 节点数据
/// </summary>
[System.Serializable]
public class UIBase
{
    public int depth;                   //深度
    public string name;                 //控件名字
    public Transform parent;            //父节点
    public Transform transform;         //Transform 
    public List<UIBase> childList;      //子节点数据
}

/// <summary>
/// 
/// </summary>
public class UIDataBase : MonoBehaviour
{
    public List<string> dependList = new List<string>();            //依赖的prefab
    public List<string> atlasList = new List<string>();             //图集
    public List<UIBase> childList = new List<UIBase>();             //子节点

    /// <summary>
    /// 记录节点数据
    /// </summary>
    /// <param name="tf">节点Transform</param>
    /// <param name="depend">节点依赖的其他prefab</param>
    public void RecrodNodeData(Transform tf) 
    {
        RecordNodeChildData(tf);
    }

    /// <summary>
    /// 解析Node子节点数据
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="parentName">父类名字</param>
    /// <returns></returns>
    public void RecordNodeChildData(Transform tf) 
    {
        for (int i = 0; i < tf.childCount; i++ )
        {
            Transform trans = tf.GetChild(i);
            string name = NGUITools.GetHierarchy(trans.gameObject); 
            if (isExsit(name))
            {
                Debug.LogError("Error: 存在相同的子节点!!! >>>>> path = " + name );
                continue;
            }
            else 
            { 
                UIBase uibase = new UIBase();
                uibase.name = name;
                uibase.transform = trans;
                uibase.parent = trans.parent;
                UIWidget widget = trans.GetComponent<UIWidget>();
                if (widget != null)
                {
                    uibase.depth = widget.depth;
                }
                else 
                {
                    UIPanel panel = trans.GetComponent<UIPanel>();
                    if(panel != null)
                    {
                        uibase.depth = panel.depth;
                    }
                }
                childList.Add(uibase);
                if(trans.childCount > 0)
                {
                    RecordNodeChildData(trans);
                } 
            }
        }
    }

    /// <summary>
    /// 记录打包依赖的prefab
    /// </summary>
    /// <param name="depend"></param>
    public void RecordNodeDepend(List<string> depend) 
    {
        dependList.Clear();
        dependList.AddRange(depend);
    }

    /// <summary>
    /// 记录所需的atlas
    /// </summary>
    /// <param name="atlas"></param>
    public void RecordNodeAtlas(string name) 
    {
        for (int i = 0; i < atlasList.Count; i++ )
        {
            if (atlasList[i].Equals(name))
            {
                return;
            }
        }
        atlasList.Add(name);
    }
     
    /// <summary>
    /// 获取节点全名对应的UIBase
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, UIBase> GetNodeDict() 
    {
        Dictionary<string, UIBase> dict = new Dictionary<string, UIBase>();
        for (int i = 0; i < childList.Count; i++ )
        {
            if(!dict.ContainsKey(childList[i].name))
            {
                dict.Add(childList[i].name,childList[i]);
            }
        }
        return dict;
    }

    /// <summary>
    /// 检查是否存在相同的节点
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    private bool isExsit(string nodeName)
    {
        for (int i = 0; i < childList.Count; i++)
        {
            if (childList[i].name.Equals(nodeName))
            {
                return true;
            }
        }
        return false;
    }
}

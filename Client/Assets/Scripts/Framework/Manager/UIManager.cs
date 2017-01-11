using System;
using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
 
public class UIManager 
{
    //UI存放的根节点
    private GameObject uiRoot;
    //内存中缓存最大数量
    private readonly uint maxCount = 10;  
    //UI名字(或者路径)
    public Dictionary<UIName, string> pathDic;
    //UI名字对应的类名
    public Dictionary<UIName, Type> typeDic;
    //UI缓存
    public Dictionary<UIName, MonoBehaviour> cacheDic;
    //当前激活的UIView的List
    public List<MonoBehaviour> activeList ;

    public static UIManager Instance { get { return Singleton.GetInstance<UIManager>(); } }

    /// <summary>
    /// 初始化
    /// </summary>  
    public UIManager() 
    {
        uiRoot = GameObject.Find("UI Root/UI Panel");
        pathDic = new Dictionary<UIName, string>();
        typeDic = new Dictionary<UIName, Type>();
        cacheDic = new Dictionary<UIName, MonoBehaviour>();
        activeList = new List<MonoBehaviour>();
        InitUIPathDic();
        InitUIType();  
    }  

    //初始化路径
    private void InitUIPathDic()
    {
        pathDic.Add(UIName.LogoView, "dialog_ui_begin"); 
    }
    //初始化类型
    private void InitUIType()
    {
        typeDic.Add(UIName.LogoView, typeof(LogoView)); 
    }

    public MonoBehaviour OpenUI(UIName name,GameObject go,Type tp) 
    {
        return null;
    }
}

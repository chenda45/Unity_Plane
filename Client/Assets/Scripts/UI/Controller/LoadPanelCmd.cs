/// The only change in StartCommand is that we extend Command, not EventCommand 
using System;
using System.Text;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using System.Collections.Generic;
using strange.extensions.command.impl;
 
public class LoadPanelCmd : Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; } 
    [Inject]
    public UIName name { get; set; } //panel名字 
    [Inject]
    public UIOption option { get; set; }  //其他组件列表
    [Inject]
    public PoolManager poolManager {get;set;}  
    [Inject]
    public SignalManager signalManager{get;set;}
    [Inject]
    public LoadManager loadManager { get; set; }
    [Inject]
    public UIManager uiManager { get; set; }

    private uint loadCount = 0;

    public override void Execute()
    { 
        loadCount = 0; 
        //panel
        string pathName = uiManager.pathDic[name];
        UnityEngine.Object ab = loadManager.findAssetBundleByName(pathName);
        if (ab == null)
        {
            loadCount = loadCount + 1;
            loadManager.loadObject(UITool.GetPanelRelativePath(pathName), onLoadFinished);
        }
        
        //如果没有需要加载直接创建
        if (loadCount == 0)
        {
            signalManager.OpenView.Dispatch(name, option);
        }
        else 
        {
            //防止command被清掉
            Retain();
        }  
    }
     
    private void onLoadFinished() 
    {
        loadCount = loadCount - 1;
        if(loadCount == 0)
        {
            signalManager.OpenView.Dispatch(name, option);
            Release();
        }
    }

    private void findObjectByName(UIName uiname) 
    {
        ////先查找看是否内存池中有该名称对应的Object
        //string name = uiManager.pathDic[uiname];
        //UnityEngine.Object ab = loadManager.findAssetBundleByName(name);
        //if (ab == null)
        //{
        //    loadManager.loadObject(name);
        //} 

        //GameObject go = poolManager.findBundleByName(name);
        //if (go == null)
        //{
        //    loadManager.loadObject(name);
        //} 
    }

    private void Test() 
    {
        GameObject go = new GameObject();
        go.name = "TextView";
        //go.transform.parent = contextView.transform;
        go.transform.SetParent(contextView.transform, false);
        go.AddComponent<LogoView>();
    }
}

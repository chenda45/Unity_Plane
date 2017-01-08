
using System;
using UnityEngine;
using System.Collections.Generic; 
using strange.extensions.command.impl;

public class OpenUiCmd : Command
{
    [Inject]
    public UIName name { get; set; }  //面板名称
    [Inject]
    public UIOption option { get; set; } //组件名称列表
    [Inject]
    public PoolManager poolManager { get; set; }
    [Inject]
    public UIManager uiManager { get; set; }
    [Inject]
    public SignalManager signalManager { get; set; }

    public override void Execute()
    { 
        switch (option)
        {
            //case UIOption.CloseSelf:
            //    if (uiManager.cacheDic.ContainsKey(name))
            //        uiManager.cacheDic[name].gameObject.SetActive(false);
            //    return;
            //case UIOption.CloseFront:
            //    //GetFrontView(Model.ActiveViewList).SetActive(false);
            //    break;
            //case UIOption.CloseOther:
            //    for (int i = 0; i < uiManager.activeList.Count; i++)
            //        uiManager.activeList[i].gameObject.SetActive(false);
            //    break;
            case UIOption.Open:
                signalManager.OpenView.Dispatch(name, option);     
                break;
            default: 
                break;
        }
    }
}

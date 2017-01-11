using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using strange.extensions.signal.impl;
using strange.extensions.command.api;

public enum UIOpenEvent
{
    Close = 0,
    Open, 
}
 
public class SignalManager 
{
    /// <summary>
    /// 单例
    /// </summary>
    public static SignalManager Instance { get { return Singleton.GetInstance<SignalManager>(); } }

    //加载一个panel信号，主要用于打开一个panel前判定是否内存池中有了或者assetbundle已经加载到了内存
    public Signal<UIName, UIOption> OpenView = new Signal<UIName, UIOption>(); 
    //打开UI信号
    public Signal<UIName, UIOption> OpenUi = new Signal<UIName, UIOption>();


    public void init(ICommandBinder commandBinder) 
    {
        //commandBinder.Bind(OpenView).To<LoadPanelCmd>();
        commandBinder.Bind(OpenView).To<CreatePanelCmd>();
        commandBinder.Bind(OpenUi).To<OpenUiCmd>();
    }

   

}

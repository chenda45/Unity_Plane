using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

public class ClientMainContext : MVCSContext {

    public ClientMainContext (MonoBehaviour view) : base(view)
    {
    }

    public ClientMainContext(MonoBehaviour view, ContextStartupFlags flags)
        : base(view, flags)
    {
    }

    // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
    protected override void addCoreComponents()
    {
        base.addCoreComponents();
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
    }

    // Override Start so that we can fire the StartSignal 
    override public IContext Start()
    { 
        base.Start();
        ClientMainSignal startSignal = (ClientMainSignal)injectionBinder.GetInstance<ClientMainSignal>();
        startSignal.Dispatch();
        return this;
    }

    protected override void mapBindings()
    {
        base.mapBindings(); 

        injectionBinder.Bind<LoadManager>().ToValue(LoadManager.Instance).CrossContext().ToSingleton();                      
        injectionBinder.Bind<CameraManager>().CrossContext().ToSingleton();
        injectionBinder.Bind<UIManager>().CrossContext().ToSingleton();
        injectionBinder.Bind<DataManager>().CrossContext().ToSingleton();
        injectionBinder.Bind<PoolManager>().CrossContext().ToSingleton();
        injectionBinder.Bind<SignalManager>().CrossContext().ToSingleton(); 

        commandBinder.Bind<ClientMainSignal>().To<ClientMainCmd>().Once(); //启动入口
         
         
    }
     
}

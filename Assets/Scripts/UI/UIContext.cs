using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

public class UIContext : MVCSContext {

    public UIContext(MonoBehaviour view)
        : base(view)
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
        LogoSignal startSignal = (LogoSignal)injectionBinder.GetInstance<LogoSignal>();
        startSignal.Dispatch();
        return this;
    }

    protected override void mapBindings()
    {

        mediationBinder.Bind<LogoView>().To<LogoScene>();
        commandBinder.Bind<LogoSignal>().To<UICmd>().Once(); //启动入口 
         
    }
     
}

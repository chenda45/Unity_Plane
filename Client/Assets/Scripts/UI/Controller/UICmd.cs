/// The only change in StartCommand is that we extend Command, not EventCommand

using System;
using UnityEngine;
using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;

public class UICmd : Command
{

    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; } 
    [Inject]
    public UIManager uiManager { get; set; } 
    [Inject]
    public PoolManager poolManager { get; set; }
    [Inject]
    public SignalManager signalManager { get; set; }
      
    public override void Execute()
    {
        signalManager.init(commandBinder); 
        signalManager.OpenUi.Dispatch(UIName.LogoView,UIOption.Open);
    }

     

     
}

using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strange.extensions.context.api; 

public class LogoView : View
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; } 
      
    protected override void Start() 
    {
        base.Start(); 
    }

    [PostConstruct]
    public void onEnter()
    { 

    }
     
}

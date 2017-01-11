using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strange.extensions.context.api; 

public class LogoScene : Mediator
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }
    [Inject(ContextKeys.CONTEXT)]
    public IContext context { get; set; }
    [Inject]
    public LogoView startView { get; set; }
       
    void Start() 
    { 
    }
 
    public override void OnRegister()
    { 
        //startView.init(); 
    }

    public override void OnRemove()
    {
       
    }

    void Update() 
    {
         
    }

    void OnGUI()
    { 

    }
}

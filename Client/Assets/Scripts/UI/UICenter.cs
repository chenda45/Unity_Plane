using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.context.api;
public class UICenter : ContextView
{

    void Awake()
    {
        context = new UIContext(this);
    }

    void Update() 
    { 

    }
}

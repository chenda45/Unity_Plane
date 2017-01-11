using UnityEngine;
using System.Collections;
using strange.extensions.context.impl; 

public class ClientMainRoot : ContextView
{
    private bool isPause;

    private static ClientMainRoot instance; 
    public static ClientMainRoot Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        context = new ClientMainContext(this);
        DontDestroyOnLoad(gameObject);
    }

    void Update() 
    {
        //context. 
    }
     
}

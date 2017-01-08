using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class UINodeBase
{
    protected int mBaseDepth;
    protected string mName;             //控件名字
    protected Vector3 mPosition;        //控件坐标
    protected GameObject mObj;          //控件object        
    protected Transform mTransform;     //控件transform组件
    protected GameObject mParent;    //控件父控件 
 
    #region get/set
    //---------------------------get/set---------------------------//
    public string name { get { return mName; } }

    public Transform transform
    {
        set
        {
            mTransform = value;
        }
        get
        {
            return mTransform;
        }
    }

    public GameObject gameObject
    {
        get
        {
            return mObj;
        }
    }

    public GameObject parentObject
    {
        get
        {
            return mParent;
        }
    }

    public Vector3 position
    {
        set
        {
            transform.localPosition = value;
        }
        get
        {
            return mPosition;
        }
    }

    public float x
    {
        get
        {
            return transform.localPosition.x;
        }
        set
        {
            mPosition.x = value;
            position = mPosition;
        }
    }

    public float y
    {
        get
        {
            return transform.localPosition.y;
        }
        set
        {
            mPosition.y = value;
            position = mPosition;
        }
    }

    public float z
    {
        get
        {
            return transform.localPosition.z;
        }
        set
        {
            mPosition.z = value;
            position = mPosition;
        }
    }

    public Vector3 scale
    {
        get
        {
            return transform.localScale;
        }
        set
        {
            transform.localScale = value;
        }
    }

    public float scaleX
    {
        get
        {
            return mTransform.localScale.x;
        }
    }

    public float scaleY
    {
        get
        {
            return mTransform.localScale.y;
        }
    }

    public float scaleZ
    {
        get
        {
            return mTransform.localScale.z;
        }
    }

    //public Vector3 rotation
    //{ 

    //}

    //public float opacity 
    //{ 

    //} 

    #endregion
} 

using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class UINodeBase
{
    protected int mDepth;               //控件深度
    protected int mBaseDepth;
    protected string mName;             //控件名字
    protected Vector3 mPosition;        //控件坐标
    protected GameObject mObj;          //控件object        
    protected Transform mTransform;     //控件transform组件
    protected GameObject mParent;       //控件父控件  
     
    #region get/set
    //---------------------------get/set---------------------------//
    /// <summary>
    /// 控件名字
    /// </summary>
    public string name { get { return mName; } }

    /// <summary>
    /// 控件Transfrom
    /// </summary>
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

    /// <summary>
    /// 控件gameObject
    /// </summary>
    public GameObject gameObject
    {
        get
        {
            return mObj;
        }
    }

    /// <summary>
    /// 控件父节点
    /// </summary>
    public GameObject parentObject
    {
        get
        {
            return mParent;
        }
    }

    /// <summary>
    /// 控件位置
    /// </summary>
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

    /// <summary>
    /// 控件X坐标
    /// </summary>
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

    /// <summary>
    /// 控件Y坐标
    /// </summary>
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

    /// <summary>
    /// 控件Z坐标
    /// </summary>
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

    /// <summary>
    /// 控件大小
    /// </summary>
    public Vector3 scale
    {
        get
        {
            return mTransform.localScale;
        }
        set
        {
            mTransform.localScale = value;
        }
    }

    /// <summary>
    /// 控件 X 大小
    /// </summary>
    public float scaleX
    {
        get
        {
            return mTransform.localScale.x;
        }
    }

    /// <summary>
    /// 控件 Y 大小
    /// </summary>
    public float scaleY
    {
        get
        {
            return mTransform.localScale.y;
        }
    }

    /// <summary>
    /// 控件 Z 大小
    /// </summary>
    public float scaleZ
    {
        get
        {
            return mTransform.localScale.z;
        }
    }
     
    #endregion
} 

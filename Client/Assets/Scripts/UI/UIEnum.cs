using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl; 
/*
     UI下所有的枚举类型
  *Copyright(C) 2016 by #COMPANY#
  *All rights reserved.
  *FileName:     #SCRIPTFULLNAME#
  *Author:       #AUTHOR#
  *Date:         #DATE#
  *Description: 
*/ 
public enum UIName
{
    Null,       //无
    LogoView,   //logo
}

public enum UIOption
{
    Null,           //不做任何处理
    Open,        //打开新面板
    AddChild,       //面板添加子节点
    CloseSelf,      //关闭自身
    CloseFront,     //关闭最前面的View
    CloseOther,     //关闭其他View
    InFront,        //最前面
}
/// <summary>
/// 读取状态
/// </summary>
public enum eLoadState
{
    LOADSTATE_NONE,
    LOADSTATE_READY,//准备
    LOADSTATE_DOING,//LOADING中
    LOADSTATE_DOING_ENCRYPT,//LOADING解密中
    LOADSTATE_COMPLETED,//加载完成
}

/// <summary>
/// 读取场景状态
/// </summary>
public enum eLoadLevelState
{
    None = 0,
    Start,
    Loading,
    Completed,
}

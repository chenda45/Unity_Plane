using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationCurveTool  {

    /// <summary>
    /// 基本曲线类型枚举 （1，1）
    /// </summary>
    public enum CurveBaseType
    {
        LinerCurve,                     //直线曲线动画
        ConstantCurve,                  //匀速曲线动画
        AddCurve,                       //加速曲线动画
        SubCurve,                       //减速曲线动画
        VariableCurve,                  //变速曲线动画，（前段加速，中段匀速，后端减速）
    }

    private float beginTime = 0.0f;
    private float beginValue = 0.0f;
    private float endTime = 1.0f;
    private float endValue = 1.0f;

    //最基本的动画曲线，
    public AnimationCurve GetBaseCurve(CurveBaseType type)
    {
        AnimationCurve curve = null;
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(beginTime, beginValue);
        keys[1] = new Keyframe(endTime, endValue);

        switch (type)
        {
            case CurveBaseType.LinerCurve:
                keys[0].value = 1.0f;
                break;
            case CurveBaseType.ConstantCurve:
                keys[0].outTangent = 1.13f;
                keys[1].inTangent = 1.13f;
                break;
            case CurveBaseType.AddCurve:
                keys[1].inTangent = 2.0f;
                break;
            case CurveBaseType.SubCurve:
                keys[0].outTangent = 2.0f;
                break;
            case CurveBaseType.VariableCurve:
                break;
        }
        curve = new AnimationCurve(keys);
        return curve;
    }
    /// <summary>
    /// 根据Keyframe 的切线，获得（0 ，1）直接的简单动画曲线
    /// </summary>
    public AnimationCurve GetSingleCurve(int beginIn,int beginOut,int endIn,int endOut)
    {
        AnimationCurve curve = null;
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(beginTime, beginValue,beginIn,beginOut);
        keys[1] = new Keyframe(endTime, endValue,endIn,endOut);
        curve = new AnimationCurve(keys);
        return curve;
    }

    /// <summary>
    /// 动态的创建任意的 Keyframe 
    /// </summary>
    /// <returns></returns>
    public AnimationCurve GetMulCurve(List<float> times,List<float> values ,List<float> inTan,List<float>outTan)
    {
        AnimationCurve curve = null;
        Keyframe[] keys = new Keyframe[values.Count];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = new Keyframe(times[i],values[i]);
            keys[i].inTangent = inTan[i];
            keys[i].outTangent = outTan[i];
        }
        curve = new AnimationCurve(keys);
        return curve;
    }


    //最基本的动画曲线，静态
    public static AnimationCurve BaseCurve(CurveBaseType type)
    {
        AnimationCurve curve = null;
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0.0f, 0.0f);
        keys[1] = new Keyframe(1.0f, 1.0f);

        switch (type)
        {
            case CurveBaseType.LinerCurve:
                keys[0].value = 1.0f;
                break;
            case CurveBaseType.ConstantCurve:
                keys[0].outTangent = 1.13f;
                keys[1].inTangent = 1.13f;
                break;
            case CurveBaseType.AddCurve:
                keys[1].inTangent = 2.0f;
                break;
            case CurveBaseType.SubCurve:
                keys[0].outTangent = 2.0f;
                break;
            case CurveBaseType.VariableCurve:
                break;
        }
        curve = new AnimationCurve(keys);
        return curve;
    }
}


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

//将prefab资源打包

#if UNITY_EDITOR
public class BuildAsssetBundleForSelection 
{
    static void Exec(string Extension, BuildTarget target)
    {
        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            BuildObject(tmp, Extension, target);
        }
    }

    static void UpdateExec(string Extension, BuildTarget target, bool showTips = true)
    {
        ResourcesType resType = ResourcesType.Prefab;
        List<string> files = FileCtrl.GetCtrl().GetBuildFiles(resType);
        
        Common.ClearDirectory(Common.GetWindowPath("assets/uiresources/effect", ""));
        for (int i = 0, size = files.Count; i < size; ++i)
        {
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(files[i], typeof(UnityEngine.Object));
            if (null != obj)
            {
                BuildObject(obj, Extension, target);
            }
            else
            {
                //MyLog.LogError("资源加载失败：" + files[i]); ;
            }
        }
        if (showTips)
        {
            EditorUtility.DisplayDialog("提示", "打包成功", "确定");
        }
    }

    static void BuildObject(UnityEngine.Object tmp, string Extension, BuildTarget target)
    {
        string path = AssetDatabase.GetAssetPath(tmp);
        if (path.Contains(".prefab"))
        {
            //打包window版本
            string dstPath = Common.GetWindowPath(path, Extension);
            Debug.Log("dstPath = " + dstPath);

            GameObject o = (GameObject)UnityEngine.Object.Instantiate(tmp);

            UIEffectRendererQueueManage[] effects = o.GetComponentsInChildren<UIEffectRendererQueueManage>(true);
            if (effects != null && effects.Length > 0)
            {
                foreach (UIEffectRendererQueueManage ef in effects)
                {
                    //UIParticleRenderQuquq par = ef.gameObject.GetComponent<UIParticleRenderQuquq>();
                    ////if (par == null)
                    ////{
                    ////    par = ef.gameObject.AddComponent<UIParticleRenderQuquq>();
                    ////    par.Depth = ef.RendererQueue - 3000;
                    ////    ef.enabled = false;
                    ////}
                    //if (par != null)
                    //{
                    //    par.enabled = false;
                    //    Object.DestroyImmediate(par);
                    //}
                    //ef.enabled = true;
                }
            }

            Animator[] anis = o.GetComponentsInChildren<Animator>();
            foreach (Animator ani in anis)
            {
                ani.applyRootMotion = false;
            }

            AnimationDelay[] anidelays = o.GetComponentsInChildren<AnimationDelay>();
            foreach (AnimationDelay val in anidelays)
            {
                val.StopUpdate = true;
            }
            GameObject prefab = PrefabUtility.CreatePrefab(path, o);

            Common.CreatePath(dstPath);

            BuildPipeline.BuildAssetBundle((UnityEngine.Object)prefab, null, dstPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target);

            UnityEngine.Object.DestroyImmediate(o);
        }
        else
        {
            Debug.Log("please make sure you selected the prefab");
        }
    }

    [MenuItem("[Build Windows]/Build Prefab For  [Windows]")]
    static void ExecuteForWindows()
    {
#if UNITY_STANDALONE_WIN
        Exec(".x", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Prefab For  [Android]")]
    static void ExecuteForAndroid()
    {
#if UNITY_ANDROID
        Exec(".x.android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Prefab For  [IOS]")]
    static void ExecuteForIOS()
    {
#if UNITY_IOS
        Exec(".x.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Windows]/Build Update Prefab For  [Windows]", false, 1)]
    static void UpdateExecuteForWindows()
    {
#if UNITY_STANDALONE_WIN
        UpdateExec(".x", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Update Prefab For  [Android]", false, 1)]
    static void UpdateExecuteForAndroid()
    {
#if UNITY_ANDROID
        UpdateExec(".x.android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Update Prefab For  [IOS]", false, 1)]
    static void UpdateExecuteForIOS()
    {
#if UNITY_IOS
        UpdateExec(".x.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    public static void BuildAllForWindows()
    {
#if UNITY_STANDALONE_WIN
        UpdateExec(".x", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    public static void BuildAllForAndroid()
    {
#if UNITY_ANDROID
        UpdateExec(".x.android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }
    public static void BuildAllForIOS()
    {
#if UNITY_IOS
        UpdateExec(".x.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }
}

#endif

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

//将图片资源打包

#if UNITY_EDITOR
public class Build__Texture 
{
    static void Exec(string Extension, BuildTarget target)
    {
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            BuildObject(o, Extension, target);
        }
    }

    static void UpdateExec(string Extension, BuildTarget target, bool showTips = true)
    {
        ResourcesType resType = ResourcesType.Texture;
        List<string> files = FileCtrl.GetCtrl().GetBuildFiles(resType);
        
        Common.ClearDirectory(Common.GetWindowPath("assets/uiresources/texture", ""));
        for (int i = 0, size = files.Count; i < size; ++i)
        {
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(files[i], typeof(UnityEngine.Object));
            if (null != obj)
            {
                BuildObject(obj, Extension, target);
            }
            else
            {
                Debug.LogError("资源加载失败：" + files[i]); ;
            }
        }
        if (showTips)
        {
            EditorUtility.DisplayDialog("提示", "打包成功", "确定");
        }
    }

    static void BuildObject(UnityEngine.Object o, string Extension, BuildTarget target)
    {
        Debug.Log(o.GetType());
        if (!(o is Texture2D)) return;

        string path = AssetDatabase.GetAssetPath(o);

        string dstPath = Common.GetWindowPath(path, Extension);

        //打包window版本
        Common.CreatePath(dstPath);

        if (!path.Contains("2dmapcolour"))
        {
            MassSetTextureImporter.ChangeTextureFormat(o, false, TextureImporterFormat.RGBA32);
        }
        else
        {
            MassSetTextureImporter.ChangeTextureFormat(o, true, TextureImporterFormat.RGBA32);
        }


        Debug.Log("dstPath = " + dstPath);
        if (BuildPipeline.BuildAssetBundle((UnityEngine.Object)o, null, dstPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target))
        {
        }
    }

    [MenuItem("[Build Windows]/Build Texture For [Windows]")]
    static void ExecuteForWindows()
    {
#if UNITY_STANDALONE_WIN
        Exec(".tex", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Texture For [Android]")]
    static void ExecuteForAndroid()
    {
#if UNITY_ANDROID
        Exec(".tex.android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Texture For [IOS]")]
    static void ExecuteForIOS()
    {
#if UNITY_IOS
        Exec(".tex.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Windows]/Build Update Texture For [Windows]", false, 1)]
    static void UpdateExecuteForWindows()
    {
#if UNITY_STANDALONE_WIN
        UpdateExec(".tex", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Update Texture For [Android]", false, 1)]
    static void UpdateExecuteForAndroid()
    {
#if UNITY_ANDROID
        UpdateExec(".tex.android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Update Texture For [IOS]", false, 1)]
    static void UpdateExecuteForIOS()
    {
#if UNITY_IOS
        UpdateExec(".tex.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    /*[MenuItem("[Build IOS]/aaaaaaa", false, 1)]
    static void aaaaaaaaaa()
    {
        Transform anchorCenter = GameObject.Find("UI Root2D/UICamera/AnchorCenter").transform;
        if (null != anchorCenter)
        {
            for (int i = 0, icount = anchorCenter.transform.childCount; i < icount; ++i)
            {
                Transform obj = anchorCenter.GetChild(i);
                if (obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }*/

    public static void BuildAllForWindows()
    {
#if UNITY_STANDALONE_WIN
        UpdateExec(".tex", BuildTarget.WebPlayer, false);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    public static void BuildAllForAndroid()
    {
#if UNITY_ANDROID
        UpdateExec(".tex.android", BuildTarget.Android, false);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    public static void BuildAllForIOS()
    {
#if UNITY_IOS
        UpdateExec(".tex.iphone", BuildTarget.iPhone, false);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }
  
}


#endif
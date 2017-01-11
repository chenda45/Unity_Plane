using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildScene
{
    public static void Execute(string Extension, BuildTarget target)
    {
        Caching.CleanCache();
        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(tmp);
            if (path.Contains(".unity"))
            {
                string dstPath = Common.GetWindowPath(path, Extension);
                Common.CreatePath(dstPath);

                string[] levels = new string[] { path };
                BuildPipeline.BuildPlayer(levels, dstPath, target, BuildOptions.BuildAdditionalStreamedScenes);
            }
            
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("[Build Windows]/Build Scene for [Windows]")]
    public static void BuildLightMapForWindows()
    {
#if UNITY_STANDALONE_WIN
        Execute(".scene", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Scene for [Android]")]
    public static void BuildLightMapForAndroid()
    {
#if UNITY_ANDROID
        Execute(".scene.android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Scene for [IOS]")]
    public static void BuildLightMapForIOS()
    {
#if UNITY_IOS
        Execute(".scene.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

}

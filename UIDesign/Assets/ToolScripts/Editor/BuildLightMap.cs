using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildLightMap
{
    
    public static void Execute( string extension ,BuildTarget target)
    {
            // 制作Asset
        LightMapAsset lightmapAsset = ScriptableObject.CreateInstance<LightMapAsset>();
        int iCount = LightmapSettings.lightmaps.Length;
        lightmapAsset.lightmapFar = new Texture2D[iCount];
        lightmapAsset.lightmapNear = new Texture2D[iCount];


        for(int i=0; i<iCount; ++i)
        {
            // 这里把直接把lightmap纹理存起来
            lightmapAsset.lightmapFar[i] = LightmapSettings.lightmaps[i].lightmapFar;
            lightmapAsset.lightmapNear[i] = LightmapSettings.lightmaps[i].lightmapNear;
        }

        string tmpAssetPath = "Assets/tmp.asset";
        AssetDatabase.CreateAsset(lightmapAsset, tmpAssetPath);

        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(tmpAssetPath, typeof(LightMapAsset));


        // 打包
        string dest = Common.GetWindowPath("Assets/lightmap", extension);
        Common.CreatePath(dest);

         BuildPipeline.BuildAssetBundle(obj, null, dest,BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,target);


         // 删除临时文件
         AssetDatabase.DeleteAsset(tmpAssetPath);

        // 在游戏运行时恢复Lightmap数据就非常简单了，下面是风刀的测试代码片段
        //if (info.www.assetBundle.mainAsset is LightMapAsset)
        //{
        //    LightMapAsset lightmapAsset = info.www.assetBundle.mainAsset as LightMapAsset;
        //    int Count = lightmapAsset.lightmapFar.Length;
        //    LightmapData[] lightmapDatas = new LightmapData[Count];

        //     for(int i=0; i<Count; ++i)
        //   {
        //        LightmapData Lightmap = new LightmapData();
        //        Lightmap.lightmapFar = lightmapAsset.lightmapFar;
        //        Lightmap.lightmapNear = lightmapAsset.lightmapNear;
        //        lightmapDatas = Lightmap;
        //    }
        //    LightmapSettings.lightmaps = lightmapDatas;
        // }
    }


    [MenuItem("[Build Windows]/Build Lightmap for [Windows]")]
    public static void BuildLightMapForWindows()
    {
#if UNITY_STANDALONE_WIN
        Execute(".lightmap",BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Lightmap for [Android]")]
    public static void BuildLightMapForAndroid()
    {
#if UNITY_ANDROID
        Execute(".lightmap.android",BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Lightmap for [IOS]")]
    public static void BuildLightMapForIOS()
    {
#if UNITY_IOS
        Execute(".lightmap.iphone",BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

}


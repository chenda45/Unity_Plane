using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildLightMap
{
    
    public static void Execute( string extension ,BuildTarget target)
    {
            // ����Asset
        LightMapAsset lightmapAsset = ScriptableObject.CreateInstance<LightMapAsset>();
        int iCount = LightmapSettings.lightmaps.Length;
        lightmapAsset.lightmapFar = new Texture2D[iCount];
        lightmapAsset.lightmapNear = new Texture2D[iCount];


        for(int i=0; i<iCount; ++i)
        {
            // �����ֱ�Ӱ�lightmap���������
            lightmapAsset.lightmapFar[i] = LightmapSettings.lightmaps[i].lightmapFar;
            lightmapAsset.lightmapNear[i] = LightmapSettings.lightmaps[i].lightmapNear;
        }

        string tmpAssetPath = "Assets/tmp.asset";
        AssetDatabase.CreateAsset(lightmapAsset, tmpAssetPath);

        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(tmpAssetPath, typeof(LightMapAsset));


        // ���
        string dest = Common.GetWindowPath("Assets/lightmap", extension);
        Common.CreatePath(dest);

         BuildPipeline.BuildAssetBundle(obj, null, dest,BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,target);


         // ɾ����ʱ�ļ�
         AssetDatabase.DeleteAsset(tmpAssetPath);

        // ����Ϸ����ʱ�ָ�Lightmap���ݾͷǳ����ˣ������Ƿ絶�Ĳ��Դ���Ƭ��
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
        Debug.LogError("���л�ƽ̨�Ժ��ٴ��");
#endif
    }

    [MenuItem("[Build Android]/Build Lightmap for [Android]")]
    public static void BuildLightMapForAndroid()
    {
#if UNITY_ANDROID
        Execute(".lightmap.android",BuildTarget.Android);
#else
        Debug.LogError("���л�ƽ̨�Ժ��ٴ��");
#endif
    }

    [MenuItem("[Build IOS]/Build Lightmap for [IOS]")]
    public static void BuildLightMapForIOS()
    {
#if UNITY_IOS
        Execute(".lightmap.iphone",BuildTarget.iPhone);
#else
        Debug.LogError("���л�ƽ̨�Ժ��ٴ��");
#endif
    }

}


using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

//将数据包文件打包


#if UNITY_EDITOR

class BuildBundleConfig
{
    static void Exec(string extension,BuildTarget target)
    {
        foreach (UnityEngine.Object o in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(o);
            if (!path.Contains(".txt"))
            {
                continue;
            }

            CSV.CsvStreamReader csv = new CSV.CsvStreamReader(path);

            if (!csv.LoadCsvFile()) continue;

            string FileName = Path.GetFileNameWithoutExtension(path);


            FileStreamHolder holder = ScriptableObject.CreateInstance<FileStreamHolder>();
            holder.Init(csv.GetRowList());

            string time = Common.CurrTimeString;
            string p = "Assets" + Path.DirectorySeparatorChar + FileName + "_" + time + ".asset";

            AssetDatabase.CreateAsset(holder, p);
            UnityEngine.Object tmpObject = AssetDatabase.LoadAssetAtPath(p, typeof(FileStreamHolder));



            //打包版本
            string dest = Common.GetWindowPath(path, extension);
            Common.CreatePath(dest);

            dest = dest.ToLower();

            Debug.Log("dest Path = " + dest);
            tmpObject.name = FileName;

            if (BuildPipeline.BuildAssetBundle(tmpObject, null, dest, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target))
            {
            }

            AssetDatabase.DeleteAsset(p);

        }
    }

    [MenuItem("[Build Windows]/Build StaticData For [Windows]")]
    public static void BuildStaticForWindows()
    {
#if UNITY_STANDALONE_WIN
        Exec(".unity3d", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif

    }

    [MenuItem("[Build Android]/Build StaticData For [Android]")]
    public static void BuildStaticForAndroid()
    {
#if UNITY_ANDROID
        Exec(".unity3d.android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif

    }

    [MenuItem("[Build IOS]/Build StaticData For [IOS]")]
    public static void BuildStaticForIOS()
    {
#if UNITY_IOS
        Exec(".unity3d.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif

    }


}




#endif
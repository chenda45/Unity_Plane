
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

//��������Ч�ļ����

#if UNITY_EDITOR
public class BuildAsssetBundleForSound
{
    static void Exec(string Extension, BuildTarget target)
    {
        string[] SoundTypes = new string[3];
        SoundTypes[0] = ".wav";//��Ч
        SoundTypes[1] = ".ogg";//����
        SoundTypes[2] = ".mp3";//����

        UnityEngine.Object[] tmpObjs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        foreach (UnityEngine.Object tmp in tmpObjs)
        {
            string path = AssetDatabase.GetAssetPath(tmp);
            for (byte i = 0; i < SoundTypes.Length; ++i)
            {
                if (path.Contains(SoundTypes[i]))
                {
                    //���window�汾
                    string dstPath = Common.GetWindowPath(path, Extension);
                    Common.CreatePath(dstPath);
                    Debug.Log("dstPath =" + dstPath);
                    BuildPipeline.BuildAssetBundle((UnityEngine.Object)tmp, null, dstPath, BuildAssetBundleOptions.CollectDependencies, target);

                }
            }
        }
    }

    [MenuItem("[Build Windows]/Build Sound For  [Windows]")]
    static void BuildSoundForWindows()
    {
#if UNITY_STANDALONE_WIN
        Exec(".unity3d", BuildTarget.WebPlayer);
#else
        Debug.LogError("���л�ƽ̨�Ժ��ٴ��");
#endif
    }

    [MenuItem("[Build Android]/Build Sound For  [Android]")]
    static void BuildSoundForAndroid()
    {
#if UNITY_ANDROID
        Exec(".unity3d.android", BuildTarget.Android);
#else
        Debug.LogError("���л�ƽ̨�Ժ��ٴ��");
#endif
    }

    [MenuItem("[Build IOS]/Build Sound For  [IOS]")]
    static void BuildSoundForIOS()
    {
#if UNITY_IOS
        Exec(".unity3d.iphone", BuildTarget.iPhone);
#else
        Debug.LogError("���л�ƽ̨�Ժ��ٴ��");
#endif
    }
    
}


#endif
using System.IO;
using UnityEngine;
using System;
using System.Collections;
using System.Xml.Serialization; 
using System.Collections.Generic;
 
public class LoadManager
{
   private readonly int connectNum = 20;
    private string dataPath = Application.dataPath;
    private string persistentDataPath = Application.persistentDataPath;
    private string streamingAssetsPath = Application.streamingAssetsPath;

    private Dictionary<string, Coroutine> loadDict;
    private Dictionary<string, AssetBundle> assetDict;
    private Dictionary<string, UnityEngine.Object> objectDict;

    //单例
    public static LoadManager Instance 
    {
        get { return Singleton.GetInstance<LoadManager>(); }
    }

    public LoadManager() 
    {
        loadDict = new Dictionary<string, Coroutine>();
        assetDict = new Dictionary<string, AssetBundle>();
        objectDict = new Dictionary<string, UnityEngine.Object>();
    }

    /// <summary>
    /// 通过名字查找assetbundle
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnityEngine.Object findAssetBundleByName(string name) 
    {
        if (objectDict.ContainsKey(name))
        {
            return objectDict[name];
        }
        else 
        {
            return null;
        }
    }

    /// <summary>
    /// 读取资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <param name="isAssetSave"></param>
    public void loadObject(string path, Action callback = null,bool isAssetSave = false ) 
    {
        if (loadDict.ContainsKey(path))
        {

        }
        else
        {
            Coroutine load = ClientMainRoot.Instance.StartCoroutine(LoadObjectFromBundleAsync(path,isAssetSave,callback));
            loadDict[path] = load;
        }
    }

    /// <summary>
    /// 获取读取地址
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string getLoadUrlByPath(string name) 
    { 
        string url = string.Empty;
        string innerUrl = string.Empty;

    #if UNITY_ANDROID      
        if(Application.platform == RuntimePlatform.Android)
        {
            url = streamingAssetsPath + "/" + name + ".android"; 
        }
        else
        {
            //url = persistentDataPath + "/" + name + ".android";
            //if (isNeedUpdate)
            //{
            //    url = "file:///" + url;
            //    innerUrl = "file:///" + dataPath + "/StreamingAssets/" + name + ".android";
            //}
            //else
            //{
            url = "file://" + dataPath + "/StreamingAssets/" + name + ".android";
            //} 
        }  
    #elif UNITY_IPHONE
        //-------------------------IOS-----------------------------
        url = persistentDataPath + "/" + path + ".iphone"; 
        if (isNeedUpdate)
        {
            url = "file://" + url;
            innerUrl = "file://" + streamingAssetsPath + "/" + name + ".iphone";
            innerUrl = System.Uri.EscapeUriString(innerUrl);
        }
        else
        {
            url = "file://" + streamingAssetsPath + "/" + name + ".iphone"; 
            url = System.Uri.EscapeUriString(url);
        }

    #else
        if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
                    && File.Exists(dataPath + "/StreamingAssets/" + name))
        {
            url = "file://" + dataPath + "/StreamingAssets/" + name;
        }
        else
        {
            //url = ClientConfig.ResourceIP + path;
        }
    #endif
        return url;
    }


    /// <summary>
    /// 协同读取资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isAssetSave"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator LoadObjectFromBundleAsync(string path,bool isAssetSave,Action callback) 
    {
        string url = getLoadUrlByPath(path);
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            if (callback != null)
            {
                callback();
            }
        }
        else 
        {
            AssetBundle ab = www.assetBundle;
            if(ab == null)
            {
                byte[] bytes = www.bytes;
                if(bytes == null)
                {
                    Debug.LogError("Error : bytes == null url = " + path);
                    yield return null;
                }
                AssetsEncrypt.EncryptBytes(bytes);                    //解密
                AssetBundleCreateRequest abcr = AssetBundle.CreateFromMemory(bytes);
                yield return abcr;
                ab = abcr.assetBundle;
            }

            if(ab != null)
            {
                if (isAssetSave)
                {
                    assetDict[path] = ab;
                }
                else 
                {
                    objectDict[path] = ab.mainAsset;
                    ab.Unload(false);
                }
                if(callback != null)
                {
                    callback();
                }
            }
        }
        www.Dispose();
        www = null;
        loadDict.Remove(path);
    }

    /// <summary>
    /// 停止一个读取资源协同
    /// </summary>
    /// <param name="path"></param>
    public void stopLoadByName(string path) 
    {
        if(loadDict.ContainsKey(path))
        {
            ClientMainRoot.Instance.StopCoroutine(loadDict[path]);
            loadDict.Remove(path);
        }
    }

}

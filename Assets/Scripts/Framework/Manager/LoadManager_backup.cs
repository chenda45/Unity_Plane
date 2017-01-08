//using System.IO;
//using UnityEngine;
//using System.Collections;
//using System.Xml.Serialization;
//using System.Collections.Generic;

////读取回调
//public delegate void LoadDelegate();
///// <summary>
///// 读取加载的信息配置
///// </summary>
//public class LoadData
//{
//    public string interim;              //参数
//    public string originPath;           //原始路径
//    public string url;                  //
//    public string innerUrl;             //
//    public LoadDelegate asset;          //回调
//    public eLoadState loadState;        //读取状态
//    public UnityEngine.Object obj;      //对象
//    public float progress;              //
//    public byte byErrorCount;           //下载失败次数
//    public eLoadLevelState levelState;  //加载场景状态 
//    public WWW wwwRequest;
//    public AssetBundleCreateRequest abcRequest;
//    public AssetBundle ab;
//    public bool bDeterministic;//是否打包依赖
//    public bool bSaveInMemory;//是否保存在内存中
//    public bool bScene;//是否场景  
//}

//public class LoadManager
//{
//    private readonly int connectNum = 20;
//    private string dataPath = Application.dataPath;                             //项目代码根路径           E:/SVN/p1/Client_02_17/Assets
//    private string persistentDataPath = Application.persistentDataPath;         //数据存储路径             C:/Users/Admin/AppData/LocalLow/mokylin/____________
//    private string streamingAssetsPath = Application.streamingAssetsPath;       //项目Streaming路径        E:/SVN/p1/Client_02_17/Assets/StreamingAssets

//    private List<LoadData> loadingList = new List<LoadData>();                                               //正在读取的列表
//    private Dictionary<string, AssetBundle> assetDict = new Dictionary<string, AssetBundle>();                      //读取Object(场景或者UI)的时候所依赖的assetBundle 
//    private Dictionary<string, UnityEngine.Object> objectDict = new Dictionary<string, UnityEngine.Object>();

//    public static LoadManager Instance { get { return Singleton.GetInstance<LoadManager>(); } }

//    /// <summary>
//    /// 通过名字查找是否已经加载到内存
//    /// </summary>
//    /// <param name="name"></param>
//    /// <returns></returns>
//    public Object findAssetBundleByName(string name)
//    {
//        if (objectDict.ContainsKey(name))
//        {
//            return objectDict[name];
//        }
//        else
//        {
//            return null;
//        }
//    }

//    public void loadObject(string path, LoadDelegate callback)
//    {
//        //路径为空
//        if (string.IsNullOrEmpty(path) || path.Equals("0"))
//        {
//            Debug.LogError("Error: >>>>>> load path = null");
//            return;
//        }

//        string url = "";
//        string innerUrl = string.Empty;

//#if UNITY_ANDROID      
//        if(Application.platform == RuntimePlatform.Android)
//        {
//            url = streamingAssetsPath + "/" + path + ".android"; 
//        }
//        else
//        {
//            //url = persistentDataPath + "/" + path + ".android";
//            //if (isNeedUpdate)
//            //{
//            //    url = "file:///" + url;
//            //    innerUrl = "file:///" + dataPath + "/StreamingAssets/" + path + ".android";
//            //}
//            //else
//            //{
//            url = "file://" + dataPath + "/StreamingAssets/" + path + ".android";
//            //} 
//        }  
//#elif UNITY_IPHONE
//        //-------------------------IOS-----------------------------
//        url = persistentDataPath + "/" + path + ".iphone"; 
//        if (isNeedUpdate)
//        {
//            url = "file://" + url;
//            innerUrl = "file://" + streamingAssetsPath + "/" + path + ".iphone";
//            innerUrl = System.Uri.EscapeUriString(innerUrl);
//        }
//        else
//        {
//            url = "file://" + streamingAssetsPath + "/" + path + ".iphone"; 
//            url = System.Uri.EscapeUriString(url);
//        }

//#else
//        if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
//                    && File.Exists(dataPath + "/StreamingAssets/" + path))
//        {
//            url = "file://" + dataPath + "/StreamingAssets/" + path;
//        }
//        else
//        {
//            //url = ClientConfig.ResourceIP + path;
//        }
//#endif


//        LoadData info = new LoadData();
//        info.url = url;
//        info.asset = callback;
//        info.progress = 0;
//        info.innerUrl = innerUrl;
//        info.originPath = path;
//        info.wwwRequest = null;
//        info.abcRequest = null;
//        info.levelState = eLoadLevelState.Start;
//        info.loadState = eLoadState.LOADSTATE_READY;
//        info.byErrorCount = 0;
//        info.bScene = path.Contains(".scene");

//        if (!IsExistSameUrl(path))
//        {
//            loadingList.Add(info);
//        }
//        else
//        {
//            //if (callback != null)
//            //{
//            //    //此时waitlist正在处理删除添加操作,先锁定
//            //    m_waitingList.Add(info);
//            //}
//        }
//    }

//    /// <summary>
//    /// 是否存在相同的路径
//    /// </summary>
//    /// <param name="path"></param>
//    /// <returns></returns>
//    public bool IsExistSameUrl(string path)
//    {
//        for (int i = 0; i < loadingList.Count; ++i)
//        {
//            if (loadingList[i].originPath.Equals(path))
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    public void Update()
//    {
//        int loadInfoCount = (loadingList.Count > connectNum ? connectNum : loadingList.Count);
//        LoadData info;
//        for (int i = loadInfoCount - 1; i >= 0; i--)
//        {
//            info = loadingList[i];
//            if (info != null)
//            {
//                switch (info.loadState)
//                {
//                    case eLoadState.LOADSTATE_READY:
//                        {
//                            #region load ready 准备
//                            string path;
//                            if (info.byErrorCount > 0)
//                            {
//                                path = info.innerUrl;
//                            }
//                            else
//                            {
//                                path = info.url;
//                            }
//                            info.wwwRequest = new WWW(path);
//                            info.loadState = eLoadState.LOADSTATE_DOING;
//                            #endregion
//                        }
//                        break;
//                    case eLoadState.LOADSTATE_DOING:
//                        {
//                            #region load isDone 加载完成
//                            if (info.wwwRequest.isDone)
//                            {
//                                if (info.wwwRequest.error != null)
//                                {

//                                    info.byErrorCount++;
//                                    info.progress = 1;
//                                    if (info.asset != null)
//                                        info.asset();
//                                    //RemoveLoadInfo(info, true);

//                                    info.loadState = eLoadState.LOADSTATE_NONE;

//                                    info.wwwRequest.Dispose();
//                                    info.wwwRequest = null;
//                                    loadingList.RemoveAt(i);
//                                    Debug.LogError("Errpr: " + info.wwwRequest.error + "   can't load this url = " + info.url);
//                                }
//                                else
//                                {
//                                    info.ab = info.wwwRequest.assetBundle;
//                                    if (info.ab == null)//加密了
//                                    {
//                                        byte[] bytes = info.wwwRequest.bytes;
//                                        if (bytes == null)
//                                        {
//                                            Debug.LogError("Error : Load error bytes == null >> url = " + info.url);
//                                            return;
//                                        }
//                                        AssetsEncrypt.EncryptBytes(bytes);                    //解密
//                                        info.abcRequest = AssetBundle.CreateFromMemory(bytes);
//                                        info.loadState = eLoadState.LOADSTATE_DOING_ENCRYPT;
//                                        break;
//                                    }
//                                    else
//                                    {
//                                        info.loadState = eLoadState.LOADSTATE_COMPLETED;
//                                    }
//                                }

//                            }
//                            else
//                            {
//                                info.progress = info.wwwRequest.progress;
//                                if (info.progress >= 1) info.progress = 0.99f;
//                            }
//                            #endregion
//                        }
//                        break;
//                    case eLoadState.LOADSTATE_DOING_ENCRYPT:
//                        {
//                            #region load encrypt 解密
//                            if (info.abcRequest != null && info.abcRequest.isDone)
//                            {
//                                info.ab = info.abcRequest.assetBundle;

//                                info.loadState = eLoadState.LOADSTATE_COMPLETED;
//                            }
//                            #endregion
//                        }
//                        break;
//                    case eLoadState.LOADSTATE_COMPLETED:
//                        {
//                            #region load completed 读取完成
//                            if (info.ab != null)
//                            {
//                                if (!info.bDeterministic)
//                                {
//                                    info.obj = info.ab.mainAsset;
//                                }
//                            }
//                            else
//                            {
//                                Debug.LogError("Load error info.ab == null............." + info.url);
//                            }

//                            if (info.obj != null)
//                            {
//                                if (info.bSaveInMemory)
//                                {
//                                    if (!objectDict.ContainsKey(info.originPath))
//                                    {
//                                        objectDict.Add(info.originPath, info.obj);
//                                    }
//                                }
//                                if (info.asset != null)
//                                {
//                                    info.asset();
//                                }

//                            }

//                            if (info.bDeterministic)
//                            {
//                                if (!assetDict.ContainsKey(info.originPath))
//                                {
//                                    assetDict.Add(info.originPath, info.ab);
//                                }
//                            }
//                            else
//                            {
//                                info.ab.Unload(false);
//                            }

//                            info.wwwRequest.Dispose();
//                            info.wwwRequest = null;
//                            info.progress = 1;
//                            info.loadState = eLoadState.LOADSTATE_NONE;
//                            loadingList.RemoveAt(i);
//                            #endregion
//                        }
//                        break;
//                }
//            }
//        }
//    }

//}

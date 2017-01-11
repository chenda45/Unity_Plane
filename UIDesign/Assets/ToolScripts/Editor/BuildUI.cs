using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BuildUI
{
    static List<string> atlasList = new List<string>();

    static void Execute(string Extension, BuildTarget target)
    {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | 
            BuildAssetBundleOptions.CompleteAssets ;

        if (!Common.ClearDirectory("Assets/UIResources/component/"))
        {
             EditorUtility.DisplayDialog("错误", "请确保UIResources/component文件夹没有被占用", "OK");
            return;
        }
        if (!Common.ClearDirectory("Assets/UIResources/panel/"))
        {
             EditorUtility.DisplayDialog("错误", "请确保UIResources/panel文件夹没有被占用", "OK");
            return;
        }

        if (!Common.ClearDirectory("Assets/UIResources/config/"))
        {
            EditorUtility.DisplayDialog("错误", "请确保UIResources/config文件夹没有被占用", "OK");
            return;
        }

        if (!Common.ClearDirectory("assetbundles"))
        {
            EditorUtility.DisplayDialog("错误", "请确保assetbundles文件夹没有被占用", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("提示", "请选择UIResources/ui目录打包", "继续","取消"))
        {
            return;
        }

        Common.CreatePath("Assets/UIResources/component/");
        Common.CreatePath("Assets/UIResources/panel/");
        Common.CreatePath("Assets/UIResources/config/");

        Caching.CleanCache();

        atlasList.Clear();

        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            BuildObject(tmp, Extension, target, options, true);
        }

        EditorUtility.DisplayDialog("提示", "恭喜你,打包成功", "OK");

        AssetDatabase.Refresh();
    }

    static int UpdateExec(string Extension, BuildTarget target, bool showTips = true)
    {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies |
            BuildAssetBundleOptions.CompleteAssets;

        if (!Common.ClearDirectory("Assets/UIResources/component/"))
        {
            EditorUtility.DisplayDialog("错误", "请确保UIResources/component文件夹没有被占用", "OK");
            return -1;
        }
        if (!Common.ClearDirectory("Assets/UIResources/panel/"))
        {
            EditorUtility.DisplayDialog("错误", "请确保UIResources/panel文件夹没有被占用", "OK");
            return -1;
        }

        if (!Common.ClearDirectory("Assets/UIResources/config/"))
        {
            EditorUtility.DisplayDialog("错误", "请确保UIResources/config文件夹没有被占用", "OK");
            return -1;
        }

        if (!Common.ClearDirectory("assetbundles"))
        {
            EditorUtility.DisplayDialog("错误", "请确保assetbundles文件夹没有被占用", "OK");
            return -1;
        }

        Common.CreatePath("Assets/UIResources/component/");
        Common.CreatePath("Assets/UIResources/panel/");
        Common.CreatePath("Assets/UIResources/config/");

        Caching.CleanCache();

        atlasList.Clear();

        ResourcesType resType = ResourcesType.UI;
        List<string> files = FileCtrl.GetCtrl().GetBuildFiles(resType);
        if (0 == files.Count)
        {
            if (showTips)
            {
                EditorUtility.DisplayDialog("提示", "没有需要打包的" + resType.ToString(), "确定");
            }
            return 1;
        }
        for (int i = 0, size = files.Count; i < size; ++i)
        {
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(files[i], typeof(UnityEngine.Object));
            if (null != obj)
            {
                BuildObject(obj, Extension, target, options, false);
            }
            else
            {
                Debug.LogError("资源加载失败：" + files[i]); ;
            }
        }
        if (showTips)
        {
            EditorUtility.DisplayDialog("提示", "恭喜你,打包成功", "OK");
        }
        AssetDatabase.Refresh();
        return 1;
    }

    static void BuildObject(UnityEngine.Object tmp, string Extension, BuildTarget target, BuildAssetBundleOptions options, bool needBuildAtlas)
    {
        string path = AssetDatabase.GetAssetPath(tmp);
        if (path.Contains(".prefab"))
        {
            string dstPath = path.Replace("UIResources/ui", "UIResources/panel");
            dstPath = dstPath.Replace("_full", "");

            Object asset = AssetDatabase.LoadMainAssetAtPath(path);
            GameObject o = (GameObject)UnityEngine.Object.Instantiate(asset);
            o.name = System.IO.Path.GetFileNameWithoutExtension(dstPath);
            //
            UIDataBase db = o.GetComponent<UIDataBase>();
            if(db != null)
            {
                UnityEngine.Object.DestroyImmediate(db);
            }
            db = o.AddComponent<UIDataBase>();
            
            //生成component 
            List<string> list = BuildComponent("", o, Extension, options, target, needBuildAtlas);
            db.RecordNodeDepend(list);

            //剥离图集
            RemoveAtlasAndFont(o, Extension, options, target, needBuildAtlas, db);

            db.RecordNodeChildData(o.transform);

            //生成panel的prefab
            GameObject prefab = PrefabUtility.CreatePrefab(dstPath, o);

            //删除临时创建的物体
            UnityEngine.Object.DestroyImmediate(o);

            //打包资源
            string assetbundlePath = Common.GetWindowPath(dstPath, ".x" + Extension);
            Common.CreatePath(assetbundlePath);

            Debug.Log("assetbundlePath......" + assetbundlePath);
            BuildPipeline.BuildAssetBundle((UnityEngine.Object)prefab, null, assetbundlePath, options, target);

        }
    }
     
    static void RemoveAtlasAndFont(GameObject go, string Extension, BuildAssetBundleOptions options, BuildTarget target, bool needBuildAtlas,UIDataBase db)
    {

        string allSceneConfig = "Assets/UIResources/config/" + go.name + ".asset";

        List<UIConfigElement> list = new List<UIConfigElement>();

        UISprite[] sprites = go.GetComponentsInChildren<UISprite>(true);
        foreach (UISprite spri in sprites)
        {
            if (spri.atlas == null)
            {
                Debug.Log("界面:"+go.name+"的控件:"+spri.name+"没有图集!");
                spri.saveName = "";
                continue;
            }

            if (true)
            {
                BuildAtlas(spri.atlas.name, Extension, options, target);
            }
            spri.saveName = spri.atlas.name; 
            spri.atlas = null;
            //记录图集名字
            db.RecordNodeAtlas(spri.saveName);
        }

        UIPopupList[] pls = go.GetComponentsInChildren<UIPopupList>(true);
        foreach (UIPopupList pl in pls)
        {
            if (pl.atlas == null)
            {
                pl.saveName = "";
                Debug.Log("界面:" + go.name + "的控件:" + pl.name + "没有图集!");
                continue;
            }
            if (needBuildAtlas)
            {
                BuildAtlas(pl.atlas.name, Extension, options, target);
            }
            pl.saveName = pl.atlas.name;
            pl.atlas = null;
            pl.value = "";
            //记录图集名字
            db.RecordNodeAtlas(pl.saveName);
        }

        UITexture[] textures = go.GetComponentsInChildren<UITexture>(true);
        foreach (UITexture texture in textures)
        {
            texture.mainTexture = null;
            texture.material = null;
            texture.shader = null;
        }

        UILabel[] lable = go.GetComponentsInChildren<UILabel>(true);
        foreach (UILabel l in lable)
        { 
            l.bitmapFont = null;
            l.ambigiousFont = null;
            l.text = "";
        }
          
        //Hud[] huds = go.GetComponentsInChildren<H>(true);
        //foreach (HUDText hud in huds)
        //{
        //    hud.trueTypeFont = null;
        //    hud.bitmapFont = null;
        //}
        //-----------------------------------打包图集----------------------------
        

    }

    static void BuildAtlas(string atlasName, string Extension, BuildAssetBundleOptions options, BuildTarget target)
    {
        if (atlasList.Contains(atlasName))
        {
            return;
        }
        Debug.Log("BuildAtlas:" + atlasName);
        string path = "Assets/UIResources/atlas/" + atlasName +".prefab";

        string assetbundlePath = Common.GetWindowPath(path, ".x" + Extension);
        Common.CreatePath(assetbundlePath);

        UnityEngine.Object prefab = AssetDatabase.LoadMainAssetAtPath(path);

        Object[] dependObjects;
        dependObjects = EditorUtility.CollectDependencies(new Object[] { prefab });
        foreach (Object val in dependObjects)
        {
            if (val is Material)
            {
                Material m = (Material)val;

                if (target == BuildTarget.Android)
                {
                    m.shader = Shader.Find("Unlit/Transparent Colored");
                    m.SetTexture("_MainTex", (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/UIResources/atlas/" + atlasName + ".png"));
                    //m.SetTexture("_AlphaTex", (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/UIResources/atlas/" + atlasName + "_alpha" + ".png"));
                }
                else if (target == BuildTarget.iPhone)
                {
                    m.shader = Shader.Find("Unlit/Transparent Colored");
                    m.SetTexture("_MainTex", (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/UIResources/atlas/" + atlasName + ".png"));
                }
               
            }
        }
        AssetDatabase.Refresh();


        BuildPipeline.BuildAssetBundle((UnityEngine.Object)prefab, null, assetbundlePath, options, target);
        
        atlasList.Add(atlasName);
    }

    static List<string> BuildComponent(string ComponentIndex, GameObject go, string Extension, BuildAssetBundleOptions options, BuildTarget target, bool needBuildAtlas)
    {
        List<string> componentList = new List<string>();
        List<GameObject> deleteList = new List<GameObject>();

        UIGrid[] grids = go.GetComponentsInChildren<UIGrid>(true);
        foreach (UIGrid t in grids)
        {
            for (int i = 0, count = t.transform.childCount; i < count; ++i)
            {
                Transform child = t.transform.GetChild(i);

                if (!componentList.Contains(child.name))
                {
                    string destPath = "Assets/UIResources/component/" + child.name + ".prefab";

                    UIDataBase db = child.gameObject.GetComponent<UIDataBase>();
                    if (db != null)
                    {
                        UnityEngine.Object.DestroyImmediate(db);
                    }
                    db = child.gameObject.AddComponent<UIDataBase>();

                    //剥离图集和字体
                    RemoveAtlasAndFont(child.gameObject, Extension, options, target, needBuildAtlas, db);

                    //制作prefab
                    UnityEngine.Object prefab = PrefabUtility.CreatePrefab(destPath, child.gameObject);

                    //打包资源
                    string assetbundlePath = Common.GetWindowPath(destPath, ".x" + Extension);
                    Common.CreatePath(assetbundlePath);
                    BuildPipeline.BuildAssetBundle((UnityEngine.Object)prefab, null, assetbundlePath, options, target);

                    componentList.Add(child.name);
                }
                deleteList.Add(child.gameObject);
            }
        }

        UITable[] tables = go.GetComponentsInChildren<UITable>(true);
        foreach (UITable t in tables)
        {
            for (int i = 0, count = t.transform.childCount; i < count; ++i)
            {
                Transform child = t.transform.GetChild(i);

                if (!componentList.Contains(child.name))
                {
                    string destPath = "Assets/UIResources/component/" + child.name + ".prefab";

                    UIDataBase db = child.gameObject.AddComponent<UIDataBase>();

                    //剥离图集和字体
                    RemoveAtlasAndFont(child.gameObject, Extension, options, target, needBuildAtlas, db);

                    //制作prefab
                    UnityEngine.Object prefab = PrefabUtility.CreatePrefab(destPath, child.gameObject);

                    //打包资源
                    string assetbundlePath = Common.GetWindowPath(destPath, ".x" + Extension);
                    Common.CreatePath(assetbundlePath);
                    BuildPipeline.BuildAssetBundle((UnityEngine.Object)prefab, null, assetbundlePath, options, target);

                    componentList.Add(child.name);
                }
                deleteList.Add(child.gameObject);
            }
        }

        foreach (GameObject o in deleteList)
        {
            UnityEngine.Object.DestroyImmediate(o,true);
        }

        return componentList;
    }

    static void UpdateExecuteAtlas(string Extension, BuildTarget target)
    {
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies |
            BuildAssetBundleOptions.CompleteAssets;
        ResourcesType resType = ResourcesType.Atlas;
        List<string> files = FileCtrl.GetCtrl().GetBuildFiles(resType);
        for (int i = 0, size = files.Count; i < size; ++i)
        {
            string fileName = Path.GetFileName(files[i]);
            fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
            BuildAtlas(fileName, Extension, options, target);
        }
    }


    [MenuItem("[Build Windows]/Build UI for [Windows]")]
    public static void BuildSceneForWindows()
    {
#if UNITY_STANDALONE_WIN
        Execute("", BuildTarget.WebPlayer);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build UI for [Android]")]
    public static void BuildSceneForAndroid()
    {
#if UNITY_ANDROID
        Execute(".android", BuildTarget.Android);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build UI for [IOS]")]
    public static void BuildSceneForIOS()
    {
#if UNITY_IOS
        Execute(".iphone", BuildTarget.iPhone);
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Windows]/Build Update UI for [Windows]", false, 1)]
    public static void BuildUpdateSceneForWindows()
    {
#if UNITY_STANDALONE_WIN
        int ret = UpdateExec("", BuildTarget.WebPlayer);
        if (ret > 0)
        {
            UpdateExecuteAtlas("", BuildTarget.WebPlayer);
        }
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Update UI for [Android]", false, 1)]
    public static void BuildUpdateSceneForAndroid()
    {
#if UNITY_ANDROID
        int ret = UpdateExec(".android", BuildTarget.Android);
        if (ret > 0)
        {
            UpdateExecuteAtlas(".android", BuildTarget.Android);
        }
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Update UI for [IOS]", false, 1)]
    public static void BuildUpdateSceneForIOS()
    {
#if UNITY_IOS
        int ret = UpdateExec(".iphone", BuildTarget.iPhone);
        if (ret > 0)
        {
            UpdateExecuteAtlas(".iphone", BuildTarget.Android);
        }
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Windows]/Build Update All for [Windows]", false, 1)]
    public static void BuildUpdateAllForWindows()
    {
#if UNITY_STANDALONE_WIN
        int ret = UpdateExec("", BuildTarget.WebPlayer, false);
        if (ret > 0)
        {
            UpdateExecuteAtlas("", BuildTarget.WebPlayer);
            Build__Texture.BuildAllForWindows();
            BuildAsssetBundleForSelection.BuildAllForWindows();
        }
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build Android]/Build Update All for [Android]", false, 1)]
    public static void BuildUpdateAllForAndroid()
    {
#if UNITY_ANDROID
        int ret = UpdateExec(".android", BuildTarget.Android, false);
        if (ret > 0)
        {
            UpdateExecuteAtlas(".android", BuildTarget.Android);
            Build__Texture.BuildAllForAndroid();
            BuildAsssetBundleForSelection.BuildAllForAndroid();
        }
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }

    [MenuItem("[Build IOS]/Build Update All for [IOS]", false, 1)]
    public static void BuildUpdateAllForIOS()
    {
#if UNITY_IOS
        int ret = UpdateExec(".iphone", BuildTarget.iPhone, false);
        if (ret > 0)
        {
            UpdateExecuteAtlas(".iphone", BuildTarget.iPhone);
            Build__Texture.BuildAllForIOS();
            BuildAsssetBundleForSelection.BuildAllForIOS();
        }
#else
        Debug.LogError("请切换平台以后再打包");
#endif
    }


}

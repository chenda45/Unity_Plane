///// The only change in StartCommand is that we extend Command, not EventCommand

//using System;
//using UnityEngine;
//using System.Collections.Generic;
//using strange.extensions.context.api;
//using strange.extensions.context.impl;
//using strange.extensions.command.impl;
//using strange.extensions.dispatcher.eventdispatcher.impl;

//public class SceneLoadCmd : Command
//{

//    [Inject(ContextKeys.CONTEXT_VIEW)]
//    public GameObject contextView { get; set; }
//    [Inject(ContextKeys.CONTEXT)]
//    public IContext context{ get; set; }
//    [Inject]
//    public int sceneId { get; set; }    //场景ID
//    [Inject]
//    public LoadManager load { get; set; }

//    private string sceneName = "denglujiemian_new";

//    private List<string> textureList = new List<string>();

//    private Dictionary<string, SceneConfigHolder> sceneConfigDict = new Dictionary<string, SceneConfigHolder>();//场景动态物体分布配置,常驻内存
//    private Dictionary<string, DynamicDependenciesHolder> dynamicLoaderDict = new Dictionary<string, DynamicDependenciesHolder>();

//    public override void Execute()
//    {
//        Debug.LogError("----------------login load------------------");
//        // 读取配置config -> 读取预设组件配置DynamicConfig -> 加载Texture,Shader,Material -> 加载预设prefab ->
//        //LoadConfig();
//    }

//    /// <summary>
//    /// 读取配置
//    /// </summary>
//    private void LoadConfig() 
//    {
//        if (!sceneConfigDict.ContainsKey(sceneName))
//        {
//            sceneConfigDict.Add(sceneName, null);
//            //load.loadObject("resources/scenes/config/sceneconfig/" + sceneName + ".config", loadSceneConfigCompleted, false, false, sceneName);
//        }  
//        //预加载UDT
//        //if (m_pSceneLoader.WinCondition != 0)
//        //load.loadObject("resources/scenes/udt/" + sceneName + "_udt.x",null, false, true,"");
//    }

//    /// <summary>
//    /// 读取配置完成回调
//    /// </summary>
//    /// <param name="interim"></param>
//    /// <param name="asset"></param>
//    private void loadSceneConfigCompleted(string interim, UnityEngine.Object asset) 
//    {
//        SceneConfigHolder holder = asset as SceneConfigHolder;
//        if (sceneConfigDict.ContainsKey(interim))
//        {
//            sceneConfigDict[interim] = holder;
//        }
//        LoadDynamicConfig();
//    }

//    /// <summary>
//    /// 读取动态配置信息
//    /// </summary>
//    private void LoadDynamicConfig() 
//    {
//        SceneConfigHolder loaders = sceneConfigDict[sceneName]; 
//        if (null != loaders && null != loaders.content)
//        {
//            for (int i = 0, len = loaders.content.Count; i < len; ++i)
//            {
//                string fileName = loaders.content[i].sName;
//                if (dynamicLoaderDict.ContainsKey(fileName))
//                {
//                    continue;
//                }
//                dynamicLoaderDict.Add(fileName, null);//预先加进去
//                //load.loadObject("resources/scenes/config/dynamicdependencies/" + fileName + ".config",LoadAllDynamicCompleted, false, false,fileName);
//            }
//        }
//    } 

//    /// <summary>
//    /// 读取动态配置信息回调
//    /// </summary>
//    /// <param name="interim"></param>
//    /// <param name="asset"></param>
//    private void LoadAllDynamicCompleted(string interim, UnityEngine.Object asset)
//    {
//        DynamicDependenciesHolder holder = asset as DynamicDependenciesHolder; 

//        if (dynamicLoaderDict.ContainsKey(interim))
//        {
//            dynamicLoaderDict[interim] = holder;
//        }
//        loadTexture();
//    }

//    private void loadTexture() 
//    {
//        textureList.Clear(); 
//        List<string> textures;
//        //加载动态物体依赖资源
//        foreach (KeyValuePair<string, DynamicDependenciesHolder> kvp in dynamicLoaderDict)
//        {
//            if (kvp.Value == null) continue;
//            textures = kvp.Value.textureList;
//            if (null == textures) continue;
//            for (int j = 0, count2 = textures.Count; j < count2; ++j)
//            {
//                string texture = textures[j];
//                if (texture.Equals("0"))
//                {
//                    continue;
//                } 
//                if (!textureList.Contains(texture))
//                {
//                    textureList.Add(texture);
//                }
//            }
//        } 
//        for (int i = 0, count = textureList.Count; i < count; ++i)
//        {
//            //load.loadObject("resources/scenes/texture/" + textureList[i] + ".tex", LoadTextureListCompleted, true, false, textureList[i]);
//        }

//    }

//    private void LoadTextureListCompleted(string interim, UnityEngine.Object asset)
//    {

//        if (textureList.Contains(interim))
//        {
//            textureList.Remove(interim);
//        } 

//        if (textureList.Count <= 0)
//        {
//            loadShader();
//        } 
//    }

//    private void loadShader() 
//    { 
        
//    }

//}

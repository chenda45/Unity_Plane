using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class PoolManager 
{
    public delegate void OnLoadFinished(GameObject obj);

    [Inject]
    public LoadManager loadManager{get;set;} 
    private Dictionary<string, GameObject> panelDict = new Dictionary<string, GameObject>();    //panel对应的object
    private Dictionary<string ,GameObject> modeDict = new Dictionary<string,GameObject>();      //模型对应的GameObject
    private Dictionary<string, GameObject> weaponDict = new Dictionary<string, GameObject>();      //Weapon对应的GameObject
    private Dictionary<string, GameObject> clothesDict = new Dictionary<string, GameObject>();      //Clothes对应的GameObject
    private Dictionary<string, GameObject> controlDict = new Dictionary<string, GameObject>();      //Control对应的GameObject
     
    private GameObject root;
    private string[] childList = new string[] { "Panel Pool", "Model Pool", "Weapon Pool", "Clothes Pool", "Control Pool" };
    private Dictionary<string, GameObject> childDict = new Dictionary<string, GameObject>();

    public PoolManager() 
    { 
        GameObject go = new GameObject();
        go.name = "Pool"; 
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        UnityEngine.Object.DontDestroyOnLoad(go);  
        for (int i = 0; i < childList.Length; i++ )
        {
            GameObject childpool = new GameObject();
            childpool.name = childList[i];
            childpool.transform.parent = go.transform;
            UnityEngine.Object.DontDestroyOnLoad(childpool);
            childpool.transform.localScale = Vector3.one;
            childpool.transform.localPosition = Vector3.zero;
            childpool.transform.localRotation = Quaternion.identity;
            childDict[childList[i]] = childpool;
        }
        root = go; 
    }

    /// <summary>
    /// 从内存池中获取一个panel
    /// </summary>
    public void getTextureFromPool() 
    {
        
    }

    /// <summary>
    /// 从内存池中获取一个panel
    /// </summary>
    public void getModelFromPool() 
    {
        
    }

    public UIAtlas findAtlasByName(string name) 
    {
        return null;
    }

    /// <summary>
    /// 通过名字查找对应的gameobject
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject findBundleByName(string name) 
    { 
        if (panelDict.ContainsKey(name))
        {
            return panelDict[name]; 
        }
        else
        {
            //是否已经加载到内存，如果有则Instantiate这个预设
            Object ab = loadManager.findAssetBundleByName(name);
            if (ab != null)
            {
                GameObject newGo = (GameObject)UnityEngine.Object.Instantiate(ab); 
                newGo.SetActive(false);
                newGo.transform.parent = root.transform;
                panelDict[name] = newGo;
                return newGo;
            }
            else 
            {
                return null;
            } 
        } 
    } 

}

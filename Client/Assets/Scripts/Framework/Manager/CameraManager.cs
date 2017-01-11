using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class CameraModel
{ 
    public int idx;                 //相机号
    public int depth;               //深度
    public int layer;               //层级
    public uint count;              //当前存在的panel数
    public Camera camera;           //对应的相机
    public UICamera uicamera;       //对应的UI相机
    public GameObject root;         //对应的root obj  
}
 
public class CameraManager 
{
    private GameObject _uiRoot;          //UI总节点 包括UI和Camera
    private GameObject _panelRoot;       //各个UI Panel的父节点
    private Dictionary<int, CameraModel> cameraDict = new Dictionary<int, CameraModel>(); //相机列表对应的数据

    public CameraManager() 
    {
        CreateRoot();
        //
        GameObject cam = GameObject.Find("Main Camera");
        cam.SetActive(false);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void init() 
    {
       
    }

    /// <summary>
    /// 创建ROOT节点
    /// </summary>
    private void CreateRoot() 
    {
        GameObject obj = new GameObject("UI Root");
        UnityEngine.Object.DontDestroyOnLoad(obj); 
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        _uiRoot = obj; 

        //创建UIROOT
        UIRoot root = _uiRoot.AddComponent<UIRoot>();
        //root.scalingStyle = UIRoot.Scaling.FixedSize;
        //root.manualHeight = 768;
        root.adjustByDPI = false;
        root.shrinkPortraitUI = false; //是否竖屏    
 

        Create2DCamera("Camera(Bottom)_2D", 1, 0, 8);
        Create3DCamera("Camera(Center)_3D", 2, 5, 9);
        Create2DCamera("Camera(Center)_2D", 3, 15, 10);
        Create3DCamera("Camera(Top)_3D", 4, 20, 11);
        Create2DCamera("Camera(Top)_2D", 5, 25, 12);

        GameObject ui = new GameObject("UI Panel");
        UnityEngine.Object.DontDestroyOnLoad(ui);
        ui.transform.localScale = Vector3.one;
        ui.transform.localPosition = Vector3.zero;
        ui.transform.parent = obj.transform;
        _panelRoot = ui; 

    }

    /// <summary>
    /// 创建2D相机
    /// </summary>
    /// <param name="index">下标</param>
    /// <param name="depth">深度</param>
    /// <param name="layer">层级</param>
    private void Create2DCamera(string name,int index, int depth, int layer)
    { 
        //创建UI摄像机
        GameObject cameraObj = new GameObject(name);
        cameraObj.transform.parent = _uiRoot.transform;
        cameraObj.transform.localPosition = Vector3.zero;
        cameraObj.transform.localScale = Vector3.one;
        cameraObj.layer = layer;

        Camera camera = cameraObj.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.Depth;
        camera.isOrthoGraphic = true;
        camera.orthographicSize = 1;
        camera.nearClipPlane = -10;
        camera.farClipPlane = 10;
        camera.cullingMask = 1 << layer;
        camera.depth = depth;

        UICamera uiCamera = cameraObj.AddComponent<UICamera>();
        uiCamera.eventReceiverMask = 1 << layer;
        uiCamera.allowMultiTouch = false;
        uiCamera.eventReceiverMask = 1 << layer; 

        CameraModel cm = new CameraModel();
        cm.idx = index;
        cm.camera = camera;
        cm.uicamera = uiCamera; 
        cm.layer = layer;
        cm.depth = depth;
        if (!cameraDict.ContainsKey(index))
        {
            cameraDict.Add(index,cm);
        }  

    }

    /// <summary>
    /// 创建3DUI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="index"></param>
    /// <param name="depth"></param>
    /// <param name="layer"></param>
    private void Create3DCamera(string name,int index, int depth, int layer)
    {
        GameObject cameraObj = new GameObject(name);
        cameraObj.transform.parent = _uiRoot.transform;
        cameraObj.transform.localPosition = new Vector3(0,0,-700) ;
        cameraObj.transform.localScale = Vector3.one;
        cameraObj.layer = layer;

        //add camera component
        Camera camera3D = cameraObj.AddComponent<Camera>();
        camera3D.cullingMask = 1 << layer;
        camera3D.clearFlags = CameraClearFlags.Depth;
        camera3D.orthographic = false;
        camera3D.depth = depth;                                 //这个深度到时候根据DEFINE 的变量控制，根据相机多少
        camera3D.nearClipPlane = 0.1f;
        camera3D.farClipPlane = 1000.0f;
        camera3D.fieldOfView = 57.3f;
        camera3D.renderingPath = RenderingPath.Forward;

        //add uiCamera
        UICamera uiCamera = cameraObj.AddComponent<UICamera>();
        uiCamera.eventReceiverMask = 1 << layer;
        uiCamera.allowMultiTouch = false;
        uiCamera.eventType = UICamera.EventType.UI_3D;
        uiCamera.eventReceiverMask = 1 << layer;
         
        CameraModel cm = new CameraModel();
        cm.idx = index;
        cm.camera = camera3D;
        cm.uicamera = uiCamera;
        cm.layer = layer;
        cm.depth = depth;
        if (!cameraDict.ContainsKey(index))
        {
            cameraDict.Add(index, cm);
        }  

    }


    /// <summary>
    /// 根据下标获取相机信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CameraModel getCameraByIndex(int index) 
    {
        if (cameraDict.ContainsKey(index))
        {
            return cameraDict[index];
        }
        return null;
    }


    public GameObject uiRoot 
    {
        get
        {
            return _panelRoot;
        }
    }
}

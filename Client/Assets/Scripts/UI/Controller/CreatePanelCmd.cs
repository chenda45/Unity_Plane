/// The only change in StartCommand is that we extend Command, not EventCommand 
using System;
using UnityEngine;
using System.Collections.Generic; 
using strange.extensions.command.impl;

public class CreatePanelCmd : Command
{
    [Inject]
    public UIName name { get; set; }  //�������
    [Inject]
    public UIOption option { get; set; } //��������б�
    [Inject]
    public PoolManager poolManager { get; set; }
    [Inject]
    public LoadManager loadManager { get; set; }

    private Type panelType;
    private int loadNum = 0;
    private int loadCount = 0; 
    private string pathName;

    private GameObject obj;
    public override void Execute()
    {
        //����
        pathName = UIManager.Instance.pathDic[name];
        panelType = UIManager.Instance.typeDic[name];
        obj = poolManager.findBundleByName(UITool.GetPanelRelativePath(pathName));
        if (obj != null)
        {
            OpenUI(); 
        }
        else
        {
            Retain();
            obj = null;
            loadManager.loadObject(UITool.GetPanelRelativePath( pathName ), OnLoadObjectFinished);
        }

    }

    /// <summary>
    /// ���ض�����ɻص�
    /// </summary>
    public void OnLoadObjectFinished() 
    {
        //�ȴ��ڴ���в���
        obj = poolManager.findBundleByName(UITool.GetPanelRelativePath(pathName));
        if (obj != null)
        {
            UIDataBase data = obj.GetComponent<UIDataBase>();
            if (data != null)
            {
                loadNum = 0;
                loadCount = 0;
                //����ͼ��
                List<string> atlasList = data.atlasList;
                for (int i = 0; i < atlasList.Count; i++)
                {
                    if (poolManager.findAtlasByName(atlasList[i]) == null)
                    {
                        loadManager.loadObject(UITool.GetAtlasRelativePath(atlasList[i]), OnLoadFinished2Create);
                        loadCount = loadCount + 1;
                    }
                }
                //�������
                List<string> componentList = data.dependList;
                for (int i = 0; i < componentList.Count; i++)
                {
                    if (poolManager.findBundleByName(UITool.GetComponentRelativePath(componentList[i])) == null)
                    {
                        loadManager.loadObject(UITool.GetComponentRelativePath(componentList[i]), OnLoadFinished2Create);
                        loadCount = loadCount + 1;
                    }
                }

                if (loadCount == 0)
                {
                    OnLoadFinished2Create();
                }
                else
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Error : can't find node data by name = " + pathName);
            }
        }
        else 
        {
            Debug.LogError(" Error : create failed !!!");
        }
    }

    private void OnLoadFinished2Create() 
    {
        loadNum = loadNum + 1;
        if (loadNum < loadCount)
        {
            return;
        }
        else 
        {
            loadCount = 0;
            loadNum = 0;
        }
        obj.SetActive(false); 
        Release();
        OpenUI(); 
    }

    private void OpenUI() 
    {
        UIManager.Instance.OpenUI(name, obj, panelType);
        obj = null;
    }
}

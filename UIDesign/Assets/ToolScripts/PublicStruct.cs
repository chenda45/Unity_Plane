using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using UnityEngine;

public enum ResourcesType
{
    Start = -1,
    StaticData = 0,
    Texture,
    Prefab,
    Scene,
    Lightmap,
    Sound,
    UI,
    Material,
    Shader,
    Atlas,
    End
}
/// <summary>
/// 资源工程
/// </summary>
public enum ProjectType
{
    Start = -1,
    Project_StaticData = 0,
    Project_UiDesign_V2,
    Project_Sound,
    Project_Other,
    Project_Effect,
    Project_Model,
    Project_Scene,
    Project_CardBackGround,
    Project_End,
}

public class PathData
{
    public string path;
    public ResourcesType resType;
    public List<string> suffix;

    public PathData(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            string[] tmp = str.Split('$');
            if (tmp.Length == 2)
            {
                resType = FileData.ConvertResourcesType(tmp[0]);
                string[] ary = tmp[1].Split('~');
                if (null != ary && ary.Length > 1)
                {
                    path = ary[0];
                    suffix = new List<string>();
                    for (int i = 1, size = ary.Length; i < size; ++i)
                    {
                        suffix.Add(ary[i]);
                    }
                }
            }
            
            
        }
    }
    /// <summary>
    /// 检查文件是否是该目录下需要查看的文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool CheckFile(string fileName)
    {
        string fix = fileName.Substring(fileName.LastIndexOf('.'));
        if (string.IsNullOrEmpty(fix))
        {
            return false;
        }
        if (null == suffix)
        {
            return false;
        }
        if (suffix.Contains(fix))
        {
            return true;
        }
        return false;
    }

	public static string GetAssetPath(string path)
	{
		return path.Substring(Application.dataPath.Length - "Asset/".Length);
	}

	public static string GetAbsolutePath(string assetPath)
	{
		return Application.dataPath.Substring (0, Application.dataPath.Length - "Asset/".Length) + assetPath;
	}
}

public class FileData
{
    public ResourcesType resType;
    public bool needBuild;//是否会直接打包,true表示会直接打包的资源，false表示为依赖资源，不会对其进行直接打包
    public string path;
    public ProjectType projectType;
    public FileData(ProjectType type)
    {
        projectType = type;
        path = string.Empty;
        resType = ResourcesType.Start;
        needBuild = false;
    }

    public void Init(XmlElement xe)
    {
        path = xe.GetAttribute("file");
        resType = ConvertResourcesType(xe.GetAttribute("buildType"));
        needBuild = GetBuildState(path, projectType, resType);
    }

    public static bool GetBuildState(string path, ProjectType pjType, ResourcesType rType)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        bool flag = false;
        string suffix = path.Substring(path.IndexOf('.') + 1).ToLower();
        switch (pjType)
        {
            case ProjectType.Project_StaticData:
            case ProjectType.Project_Sound:
                flag = true;
                break;
            case ProjectType.Project_UiDesign_V2:
            case ProjectType.Project_Other:
            case ProjectType.Project_Effect:
            case ProjectType.Project_Model:
            case ProjectType.Project_Scene:
            case ProjectType.Project_CardBackGround:
                flag = GetBuildState(suffix, rType);
                break;
        }
        return flag;
    }

    public static bool GetBuildState(string suffix, ResourcesType rType)
    {
        bool flag = false;
        suffix = suffix.ToLower();
        switch (rType)
        {
            case ResourcesType.StaticData:
                if (suffix.Equals("txt"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Texture:
                if (suffix.Equals("png") || suffix.Equals("tga") || suffix.Equals("jpg"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Prefab:
                if (suffix.Equals("prefab"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Scene:
                if (suffix.Equals("unity"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Lightmap:
                if (suffix.Equals("exr"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Sound:
                if (suffix.Equals("ogg") || suffix.Equals("wav") || suffix.Equals("mp3"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.UI:
                if (suffix.Equals("prefab"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Material:
                if (suffix.Equals("mat"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Shader:
                if (suffix.Equals("shader"))
                {
                    flag = true;
                }
                break;
            case ResourcesType.Atlas:
                if (suffix.Equals("prefab"))
                {
                    flag = true;
                }
                break;
        }
        return flag;
    }

    public static ProjectType ConvertProjectType(string type)
    {
        ProjectType pjType = ProjectType.Start;
        switch (type)
        {
            case "Project_StaticData":
                pjType = ProjectType.Project_StaticData;
                break;
            case "Project_UiDesign_V2":
                pjType = ProjectType.Project_UiDesign_V2;
                break;
            case "Project_Sound":
                pjType = ProjectType.Project_Sound;
                break;
            case "Project_Other":
                pjType = ProjectType.Project_Other;
                break;
            case "Project_Effect":
                pjType = ProjectType.Project_Effect;
                break;
            case "Project_Model":
                pjType = ProjectType.Project_Model;
                break;
            case "Project_Scene":
                pjType = ProjectType.Project_Scene;
                break;
            case "Project_CardBackGround":
                pjType = ProjectType.Project_CardBackGround;
                break;
        }
        return pjType;
    }

    public static ResourcesType ConvertResourcesType(string type)
    {
        ResourcesType resType = ResourcesType.Start;
        switch (type)
        {
            case "StaticData":
                resType = ResourcesType.StaticData;
                break;
            case "Texture":
                resType = ResourcesType.Texture;
                break;
            case "Prefab":
                resType = ResourcesType.Prefab;
                break;
            case "Scene":
                resType = ResourcesType.Scene;
                break;
            case "Lightmap":
                resType = ResourcesType.Lightmap;
                break;
            case "Sound":
                resType = ResourcesType.Sound;
                break;
            case "UI":
                resType = ResourcesType.UI;
                break;
            case "Material":
                resType = ResourcesType.Material;
                break;
            case "Shader":
                resType = ResourcesType.Shader;
                break;
            case "Atlas":
                resType = ResourcesType.Atlas;
                break;
        }
        return resType;
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using System.Diagnostics;

public class FileCtrl
{
    private static FileCtrl sCtrl = null;
    private static object sLock = new object();
    public delegate void XmlElementDelegate(XmlElement xe, object type);
    private string mSvnToolPath = null;
    private static readonly string XmlPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Asset".Length - 1);
    private static readonly string DepXmlPath = "buildPath.xml";

    private ProjectType[] mNeedLoadProject = { ProjectType.Project_UiDesign_V2};

    private List<FileData>[] mNeedBuildFiles = null;//svn工具导出来的直接打包的文件
    private Dictionary<string, ResourcesType>[] mDependenceFiles = null;
    private List<PathData>[] mCheckDepPath = null;//需要检查依赖的路径
    private Dictionary<ResourcesType, List<string>>[] mBuildFiles = null;//最终生成的需要打包的文件，路径为AssetPath路径

    public static FileCtrl GetCtrl()
    {
		if (null == sCtrl)
        {
			lock (sLock)
            {
                if (null == sCtrl)
                {
                    sCtrl = new FileCtrl();
                }
            }
        }
        return sCtrl;
    }


    public static void ReadXml(string filePath, XmlElementDelegate dlgt, object type)
    {
        if (!File.Exists(filePath))
        {//文件不存在
            return;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        XmlElement xe = xmlDoc.DocumentElement;
        foreach (XmlNode node in xe.ChildNodes)
        {
            if (node is XmlElement)
            {
                if (null != dlgt)
                {
                    dlgt(node as XmlElement, type);
                }
            }
        }
        xe = null;
        xmlDoc = null;
    }

    public static List<string> GetFiles(string path, List<string> list = null)
    {
        if (null == list)
        {
            list = new List<string>();
        }
        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0, size = dirs.Length; i < size; ++i)
        {
            GetFiles(dirs[i], list);
        }
        string[] files = Directory.GetFiles(path);
        for (int i = 0, size = files.Length; i < size; ++i)
        {
            list.Add(files[i].Replace("\\", "/"));
        }
        return list;
    }

    public List<string> GetBuildFiles(ResourcesType rType)
    {
        if (1 == mNeedLoadProject.Length)
        {
            return GetBuildFiles(mNeedLoadProject[0], rType);
        }
        return null;
    }

    public List<string> GetBuildFiles(ProjectType pjType, ResourcesType rtype)
    {
        int index = (int)pjType;
        if (null != mBuildFiles[index] && mBuildFiles[index].ContainsKey(rtype))
        {
            return mBuildFiles[index][rtype];
        }
        InitBuildFiles(index, rtype);
        SaveBuildXml(mBuildFiles[index], pjType);
        return mBuildFiles[index][rtype];
    }
    private void InitBuildFiles(int index, ResourcesType rtype)
    {
        if (null == mBuildFiles[index])
        {
            mBuildFiles[index] = new Dictionary<ResourcesType, List<string>>();
        }
        for (int i = 0, size = (int)ResourcesType.End; i < size; ++i)
        {
            ResourcesType tmpType = (ResourcesType)i;
            if (!mBuildFiles[index].ContainsKey(tmpType))
            {
                mBuildFiles[index].Add(tmpType, new List<string>());
            }
            else
            {
                mBuildFiles[index].Clear();
            }
        }
        List<FileData> list = new List<FileData>();
        Dictionary<string, int> fileList = new Dictionary<string, int>();
        if (null != mNeedBuildFiles && null != mNeedBuildFiles[index])
        {
            for (int i = 0, size = mNeedBuildFiles[index].Count; i < size; ++i)
            {
                list.Add(mNeedBuildFiles[index][i]);
                fileList.Add(mNeedBuildFiles[index][i].path, 1);
                ResourcesType resType = mNeedBuildFiles[index][i].resType;
                mBuildFiles[index][resType].Add(/*PathData.GetAssetPath(*/mNeedBuildFiles[index][i].path/*)*/);
            }
        }
        if (null != mDependenceFiles && null != mDependenceFiles[index] && null != mCheckDepPath[index])
        {
            for (int i = 0, size = mCheckDepPath[index].Count; i < size; ++i)
            {
                PathData data = mCheckDepPath[index][i];
                List<string> files = FileCtrl.GetFiles(PathData.GetAbsolutePath(data.path));
                for (int j = 0, size1 = files.Count; j < size1; ++j)
                {
                    if (data.CheckFile(files[j]))
                    {
                        string path = PathData.GetAssetPath(files[j]);
                        string[] tmpDependence = AssetDatabase.GetDependencies(new string[] { path });
                        if (null != tmpDependence)
                        {
                            for (int k = 0, tmpSize = tmpDependence.Length; k < tmpSize; ++k)
                            {
                                string assetPath = tmpDependence[k];
                                if (mDependenceFiles[index].ContainsKey(assetPath) && (/*assetPath.EndsWith(".shader") || */data.resType == mDependenceFiles[index][assetPath]))
                                {
                                    if (!fileList.ContainsKey(path))
                                    {
                                        FileData fileData = new FileData((ProjectType)index);
                                        fileData.resType = data.resType;
                                        fileData.needBuild = true;
                                        fileData.path = path;
                                        list.Add(fileData);
                                        fileList.Add(path, 1);
                                        mBuildFiles[index][data.resType].Add(/*PathData.GetAssetPath(*/fileData.path/*)*/);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    [MenuItem("[SvnTool]/Open SvnTool")]
    public static void OpenSvnTool()
    {
        Process[] process = Process.GetProcesses();//获取所有启动进程
        if (null != process)
        {
            for (int i = 0, size = process.Length; i < size; ++i)
            {
                try
                {
                    if (process[i].ProcessName.Equals("SvnTool"))
                    {
                        EditorUtility.DisplayDialog("提示", "SvnTool正在运行中！", "OK");
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                	
                }
            }
        }        
        try
        {
            if (null != FileCtrl.GetCtrl().mSvnToolPath)
            {
                Process.Start(FileCtrl.GetCtrl().mSvnToolPath);
                //Application.OpenURL(XmlPath + "SvnTool/bin/Release/SvnTool.exe");
            }
            else
            {
                string path = Application.dataPath.Substring(0, Application.dataPath.IndexOf("AngelEmpire") + "AngelEmpire".Length) + "/Program/SvnTool/";
                Process.Start(path + "SvnTool/bin/Release/SvnTool.exe");
            }
            
        }
        catch (System.IO.FileNotFoundException)
        {//文件不存在
            EditorUtility.DisplayDialog("提示", "SvnTool不存在，请去svn下载该程序", "OK");
        }
        catch (System.Exception ex)
        {
        	
        }

        
    }



    private FileCtrl()
    {
        mNeedBuildFiles = new List<FileData>[(int)ProjectType.Project_End];
        mDependenceFiles = new Dictionary<string, ResourcesType>[(int)ProjectType.Project_End];
        mCheckDepPath = new List<PathData>[(int)ProjectType.Project_End];
        mBuildFiles = new Dictionary<ResourcesType, List<string>>[(int)ProjectType.Project_End];
        ReadXmlInfo();
    }

    private void SaveBuildXml(Dictionary<ResourcesType, List<string>> data, ProjectType pjType)
    {
        string path = Application.dataPath.Substring(0, Application.dataPath.Length - "Asset".Length - 1) + pjType.ToString() + ".xml";
        XmlDocument doc = new XmlDocument();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        XmlElement root = doc.CreateElement("Root");
        doc.AppendChild(root);
        foreach (KeyValuePair<ResourcesType, List<string>> pair in data)
        {
            for (int i = 0, size = pair.Value.Count; i < size; ++i)
            {
                XmlElement node = doc.CreateElement("node");
                node.SetAttribute("resType", pair.Key.ToString());
                node.SetAttribute("file", pair.Value[i]);
                root.AppendChild(node);
            }
        }
        doc.Save(path);
    }

    private void ReadXmlInfo()
    {
        for (int i = 0, size = mNeedLoadProject.Length; i < size; ++i)
        {
            string filePath = XmlPath + "svn_" + mNeedLoadProject[i].ToString() + ".xml";
            ReadXml(filePath, InitXmlElementDelegate, mNeedLoadProject[i]);
        }

        ReadXml(XmlPath + DepXmlPath, InitCheckDepDelegate, 0);
    }

    private void InitXmlElementDelegate(XmlElement xe, object type)
    {
        ProjectType project = (ProjectType)type;
        int index = (int)project;
        FileData data = new FileData(project);
        data.Init(xe);
        if (data.needBuild)
        {
            if (null == mNeedBuildFiles[index])
            {
                mNeedBuildFiles[index] = new List<FileData>();
            }
            mNeedBuildFiles[index].Add(data);

        }
        else
        {
            if (null == mDependenceFiles[index])
            {
                mDependenceFiles[index] = new Dictionary<string, ResourcesType>();
            }
            mDependenceFiles[index].Add(data.path, data.resType);
        }
    }

    private void InitCheckDepDelegate(XmlElement xe, object type)
    {
        string projectType = xe.GetAttribute("ProjectType");
        string value = xe.GetAttribute("value");
        if (projectType.Equals("SvnTool"))
        {
            mSvnToolPath = value;
            return;
        }
        ProjectType ptype = FileData.ConvertProjectType(projectType);
        int index = (int)ptype;
        if (string.IsNullOrEmpty(value))
        {
            mCheckDepPath[index] = null;
        }
        else
        {
            string[] str = value.Split(';');
            if (null != str)
            {
                List<PathData> list = new List<PathData>();
                for (int i = 0, size = str.Length; i < size; ++i)
                {
                    list.Add(new PathData(str[i]));
                }
                mCheckDepPath[index] = list;
            }
            else
            {
                mCheckDepPath[index] = null;
            }
        }
    }



}
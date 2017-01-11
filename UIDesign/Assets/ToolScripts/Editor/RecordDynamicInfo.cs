using UnityEngine;
using UnityEditor;
using System.Collections;

public class RecordDynamicInfo 
{
    [MenuItem("Scripts/RecordDynamicInfo")]
    static void Execute()
    {
        GameObject[] objects = Selection.gameObjects;

        string rootName = Selection.activeGameObject.transform.root.name;

        CSV.CsvStreamWriter writer = new CSV.CsvStreamWriter("Assets/" + rootName +".txt");

        int row = 1;
        foreach (GameObject o in objects)
        {
            Record(o, writer, ref row);
        }

        writer.Save();
    }

    static void Record( GameObject o , CSV.CsvStreamWriter writer,ref int row)
    {
        MeshRenderer mr = o.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            int col = 1;
            //记录坐标,旋转,缩放和光照信息
            writer[row, col] = o.name;
            col++;
            writer[row, col] = TransVector3ToString(o.transform.localPosition);
            col++;
            writer[row, col] = TransVector3ToString(o.transform.localRotation.eulerAngles);
            col++;
            writer[row, col] = TransVector3ToString(o.transform.localScale);
            col++;
            writer[row, col] = mr.lightmapIndex.ToString();
            col++;
            writer[row, col] = TransVector4ToString(mr.lightmapTilingOffset);
            col++;

            row++;
        }

        int nCount = o.transform.GetChildCount();
        for (int i = 0; i < nCount; ++i)
        {
            Record(o.transform.GetChild(i).gameObject, writer,ref row);
        }
    }

    static string TransVector3ToString(Vector3 v)
    {
        string s = "";
        s += v.x + "~";
        s += v.y + "~";
        s += v.z;
        return s;
    }

    static string TransVector4ToString(Vector4 v)
    {
        string s = "";
        s += v.x + "~";
        s += v.y + "~";
        s += v.z + "~";
        s += v.w ;
        return s;
    }

    //static string GetPath(GameObject o)
    //{
    //    string path = o.name;

    //    GameObject tmp = o;
    //    Transform parent = tmp.transform.parent;
    //    while (parent != null)
    //    {
    //        path = parent.name + "/" + path;
    //        parent = parent.parent;
    //    }

    //    return path;
    //}
}

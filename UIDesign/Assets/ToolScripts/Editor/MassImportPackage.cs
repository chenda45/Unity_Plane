using UnityEngine;
using UnityEditor;
using System.Collections;

public class MassImportPackage
{
    [MenuItem("Scripts/Mass Import Package")]
    static void Execute()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            string path = AssetDatabase.GetAssetPath(o);

            Debug.Log("path = " + path);
            AssetDatabase.ImportPackage(path, false);
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class MaterialTidyUp
{

	[MenuItem("[TidyUp]/Material TidyUp")]
    static void Execute()
    {
        string path = "";
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is Material)) continue;

            path = AssetDatabase.GetAssetPath(o);
            path = Path.GetDirectoryName(path);
            break;
        }

        path = path.Replace('\\', '/');
        string[] ds = path.Split('/');

        if(path.Length < 2)
        {
            Debug.LogError("error path!!!!");
            return;
        }
		
#if !UNITY_FLASH
        string MaterialPath = ds[0] + Path.DirectorySeparatorChar + ds[1] + Path.DirectorySeparatorChar + "Materials";

        Common.CreatePath(MaterialPath);

        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is Material)) continue;
            string FilePath = AssetDatabase.GetAssetPath(o);

            if (FilePath.Replace("\\", "/").Contains(MaterialPath.Replace("\\", "/"))) continue;

            string FileName = Path.GetFileName(FilePath);

            string MetaPath = Path.GetDirectoryName(FilePath);
            string MetaName = FileName + ".meta";

            while( File.Exists( MaterialPath + Path.DirectorySeparatorChar + FileName ))
            {
                FileName = Path.GetFileNameWithoutExtension(FileName);
                FileName += "_backup";
                FileName += ".mat";
            }
            File.Move(FilePath, MaterialPath + Path.DirectorySeparatorChar + FileName);

            try
            {
                File.Move(MetaPath + Path.DirectorySeparatorChar + MetaName, MaterialPath + Path.DirectorySeparatorChar + FileName + ".meta");
            }
            catch 
            {
            	
            }
            
            
            AssetDatabase.Refresh();
        }
#endif
    }
}

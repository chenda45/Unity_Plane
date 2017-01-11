#if !UNITY_FLASH
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class PrefabTidyUp
{
    //将该prefab所用到的所有资源分类储存在相应的文件夹中
	[MenuItem("[TidyUp]/Prefab TidyUp")]
    static void Execute()
    {
        foreach (GameObject tmp in Selection.gameObjects)
        {
            string FilePath = AssetDatabase.GetAssetPath(tmp);

            if (!FilePath.Contains(".prefab")) continue;

            string CurrPath = Path.GetDirectoryName(FilePath);

            string[] paths = new string[1];
            paths[0] = FilePath;

            //取得联系在该prefab上的所有资源路径
            string[] AssetPaths = AssetDatabase.GetDependencies(paths);

            foreach(string AssetPath in AssetPaths)
            {
                if (AssetPath.Contains(".prefab")) continue;

                Object o = AssetDatabase.LoadMainAssetAtPath(AssetPath);
                string DestPath = CurrPath + Path.DirectorySeparatorChar;
                string FileName = Path.GetFileName(AssetPath);

                if( o is Texture2D)
                {
                    DestPath += "Textures";
                }
                else if( o is Material)
                {
                    DestPath += "Materials";
                }
                else if( o is GameObject)
                {
                    DestPath += "GameObjects";
                }
                else if( o is AnimationClip)
                {
                    DestPath += "Animations";
                }
                else if( o is Shader)
                {
                    continue;
                }
                else if (o is MonoScript)
                {
                    continue;
                }
                else if( o is Cubemap)
                {
                    DestPath += "Cubemaps";
                }
                else if(o is TerrainData)
                {
                    DestPath += "TerrainData";
                }
                else
                {
                    Debug.LogError("other = " + AssetPath);
                    Debug.LogError(o.GetType());
                    continue;
                }

                Common.CreatePath(DestPath);

                DestPath = DestPath + Path.DirectorySeparatorChar + FileName;
                DestPath = DestPath.Replace("\\", "/");

                string error = AssetDatabase.MoveAsset(AssetPath, DestPath);

                try
                {
                    AssetDatabase.MoveAsset(AssetPath + ".meta", DestPath + ".meta");
                }
                catch 
                {
                	
                }
                

            }
            

        }

        AssetDatabase.Refresh();
    }

}
#endif
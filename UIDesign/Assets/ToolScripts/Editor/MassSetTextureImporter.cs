using UnityEngine;
using UnityEditor;
using System.Collections;

public class MassSetTextureImporter
{
    [MenuItem("Scripts/Mass Set TextureImporter")]
    static void Execute()
    {
        TextureImporterType TextureType = TextureImporterType.Advanced;
        TextureImporterFormat TextureFormat = TextureImporterFormat.RGBA32;
        
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is Texture2D)) continue;
            string path = AssetDatabase.GetAssetPath(o);

            Debug.Log(path);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.mipmapEnabled = false;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.textureType = TextureType;
            importer.isReadable = false;
            importer.filterMode = FilterMode.Trilinear;
            importer.alphaIsTransparency = false;

#if UNITY_IPHONE
			Texture2D t = o as Texture2D;
            if (fun(t.width) && fun(t.height) && t.width == t.height)
            {
                if (importer.DoesSourceTextureHaveAlpha())
                {
                TextureFormat = TextureImporterFormat.PVRTC_RGBA4;
                }
                else
                {
                TextureFormat = TextureImporterFormat.PVRTC_RGB4;
                }
            }
            else
            {
                TextureFormat = TextureImporterFormat.ETC2_RGBA8;
                    
            }
#elif UNITY_ANDROID
            if (importer.DoesSourceTextureHaveAlpha())
            {
                 TextureFormat = TextureImporterFormat.RGBA32;

            }
            else
            {
                TextureFormat = TextureImporterFormat.ETC_RGB4;
            }
#else          
            TextureFormat = TextureImporterFormat.RGBA32;
#endif

            importer.textureFormat = TextureFormat;

            AssetDatabase.ImportAsset(path);
           
        }
        AssetDatabase.Refresh();
    }


    public static void ChangeTextureFormat(Object obj, bool readable = false, TextureImporterFormat androidAlpha = TextureImporterFormat.RGBA16)
    {
        Object[] dependObjects;
        dependObjects = EditorUtility.CollectDependencies(new Object[] { obj });
        foreach (Object val in dependObjects)
        {
            if (val is Texture2D)
            {
                TextureImporterFormat TextureFormat;

                string path = AssetDatabase.GetAssetPath(val);

                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.textureType = TextureImporterType.Advanced;
                importer.isReadable = readable;
                importer.filterMode = FilterMode.Trilinear;
                importer.alphaIsTransparency = false;

                if (readable)
                {
                    TextureFormat = TextureImporterFormat.RGBA32;
                }
                else
                {

#if UNITY_IPHONE
			    Texture2D t = val as Texture2D;
                if (fun(t.width) && fun(t.height) && t.width == t.height)
                {
                     if (importer.DoesSourceTextureHaveAlpha())
                     {
                        TextureFormat = TextureImporterFormat.PVRTC_RGBA4;
                     }
                     else
                     {
                        TextureFormat = TextureImporterFormat.PVRTC_RGB4;
                     }
                }
                else
                {
                     TextureFormat = TextureImporterFormat.ETC2_RGBA8;
                    
                }
#elif UNITY_ANDROID
                if (importer.DoesSourceTextureHaveAlpha())
                {
                    TextureFormat = androidAlpha;

                }
                else
                {
                    TextureFormat = TextureImporterFormat.ETC_RGB4;
                }
#else          
                TextureFormat = TextureImporterFormat.RGBA32;
#endif
                }



                importer.textureFormat = TextureFormat;

                AssetDatabase.ImportAsset(path);
            }
        }
        AssetDatabase.Refresh();
    }

    static bool fun(int v)
    {
        bool flag = false;
        if ((v > 0) && (v & (v - 1)) == 0)
            flag = true;
        return flag;
    }  


}

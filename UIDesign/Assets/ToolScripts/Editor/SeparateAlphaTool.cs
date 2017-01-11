using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SeparateAlphaTool : Editor {

	[MenuItem("ETCA Tool/Split Alpha")]
	static void SplitAlpha()
	{
		Texture2D[] ts = FilterTexture2D();
		string tempPath = "";
		TextureImporter tempTextureImport;
		Texture2D tempTexture2d;
		Texture2D tempAlphaTexture2D;
		Color32[] tempColor32;
		Color32 tempColor;
		byte[] bytes;
		foreach(Texture2D tex in ts)
		{
			tempPath = AssetDatabase.GetAssetPath(tex);
			tempTextureImport = TextureImporter.GetAtPath(tempPath) as TextureImporter;
			tempTextureImport.isReadable = true;
			tempTextureImport.SetPlatformTextureSettings("Android",4096,TextureImporterFormat.RGBA32);
			AssetDatabase.ImportAsset(tempPath,ImportAssetOptions.ForceUpdate);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			EditorUtility.SetDirty(tex);
			tempTexture2d = AssetDatabase.LoadAssetAtPath(tempPath,typeof(Texture2D)) as Texture2D;
			tempColor32 = tempTexture2d.GetPixels32();
			tempAlphaTexture2D = new Texture2D(tempTexture2d.width,tempTexture2d.height);
			for(int i = 0;i<tempTexture2d.height;i++)
			{
				for(int j =0;j<tempTexture2d.width;j++)
				{
					tempColor = tempColor32[i*tempTexture2d.width+j];
					tempColor.r = tempColor.a;
					tempColor.g = tempColor.b = 0;
					tempColor.a = 255;
					tempColor32[i*tempTexture2d.width+j] = tempColor;
				}
			}
			tempAlphaTexture2D.SetPixels32(tempColor32);
			tempPath = tempPath.Substring(0,tempPath.LastIndexOf("."));
			tempPath += "_alpha.png";
			bytes = tempAlphaTexture2D.EncodeToPNG();
			System.IO.File.WriteAllBytes(tempPath,bytes);
			bytes = null;
			tempTextureImport.isReadable = false;
			tempTextureImport.SetPlatformTextureSettings("Android",4096,TextureImporterFormat.ETC_RGB4);
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex),ImportAssetOptions.ForceUpdate);
			AssetDatabase.ImportAsset(tempPath,ImportAssetOptions.ForceUpdate);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			Debug.Log(System.IO.Directory.GetCurrentDirectory());
		}

		Debug.Log("Select Texture2D num "+ts.Length);
	}

	static Texture2D[] FilterTexture2D()
	{
		List<Texture2D> textures = new List<Texture2D>();
		if(Selection.objects!=null&&Selection.objects.Length!=0)
		{
			object[] selectObjs = Selection.objects;
			Texture2D tempTexture2d;
			foreach(object obj in selectObjs)
			{
				tempTexture2d = obj as Texture2D;
				if(tempTexture2d!=null)
				{
					textures.Add(tempTexture2d);
				}
			}
		}
		return textures.ToArray();
	}
}
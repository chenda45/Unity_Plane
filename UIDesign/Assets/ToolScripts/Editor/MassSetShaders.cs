using UnityEngine;
using UnityEditor;
using System.Collections;

public class MassSetShaders 
{

    [MenuItem("Scripts/Mass Set Shaders")]

    static void Set()
    {
        GameObject father = Selection.activeGameObject;

        MeshRenderer[] mrs = father.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] smrs = father.GetComponentsInChildren<SkinnedMeshRenderer>();

        //Shader shader1 = Shader.Find("Transparent/Diffuse");
        Shader shader2 = Shader.Find("Transparent/Cutout/Cross");

        foreach (MeshRenderer mr in mrs)
        {
            foreach (Material material in mr.sharedMaterials)
            {
                if(material.shader.name == "Diffuse" || 
					material.shader.name == "Transparent/Cutout/Diffuse"||
				material.shader.name == "VertexLit"||
				material.shader.name == "Transparent/Cutout/Cross")
                {
                    material.shader = shader2;
					material.color = new Color(160/255.0f,160/255.0f,160/255.0f,1.0f);
                }
            }
        }

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            foreach (Material material in smr.sharedMaterials)
            {
                if(material.shader.name == "Diffuse" || 
					material.shader.name == "Transparent/Cutout/Diffuse"||
				material.shader.name == "VertexLit"||
				material.shader.name == "Transparent/Cutout/Cross")
                {
                    material.shader = shader2;
					material.color = new Color(160/255.0f,160/255.0f,160/255.0f,1.0f);
                }
            }
        }
        
    }
}

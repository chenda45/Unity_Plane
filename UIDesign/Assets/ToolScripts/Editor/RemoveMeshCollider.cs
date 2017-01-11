using UnityEngine;
using UnityEditor;
using System.Collections;

public class RemoveMeshCollider : MonoBehaviour
{

	// Use this for initialization
    [MenuItem("Scripts/Remove Mesh Collider")]
	static void Execute()
    {
        GameObject[] objects = Selection.gameObjects;
        foreach(GameObject o in objects)
        {
            MeshCollider[] colliders = o.transform.GetComponentsInChildren<MeshCollider>();
            for (int i = 0; i < colliders.Length; ++i)
            {

                UnityEngine.Object.DestroyImmediate(colliders[i]);
            }
        }


	}

}

using UnityEngine;
using UnityEditor;
using System.Collections;



public class BatchOperation : EditorWindow
{
    [MenuItem("Batch/" + "≤‚ ‘Lightmapping–≈œ¢ ")]
    static void TestLightmapingInfo()
    {
        GameObject[] tempObject;
        if (Selection.activeGameObject)
        {
            tempObject = Selection.gameObjects;
            for (int i = 0; i < tempObject.Length; i++)
            {
                Debug.Log("Object name: "  + tempObject[i].name);
                Debug.Log("Lightmaping Index: " + tempObject[i].renderer.lightmapIndex);
                Debug.Log("Lightmaping Offset: " + tempObject[i].renderer.lightmapTilingOffset);
            }
        }
    }
}



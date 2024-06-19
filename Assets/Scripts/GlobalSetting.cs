using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    public bool refresh;



    [MenuItem("MyHelperTools/Global/Edit All Renderers")]
    static void EditAllRenderers()
    {
        Object[] objs = GameObject.FindObjectsOfType(typeof(Renderer)); //returns Object[]
        Debug.Log(objs.Length);

        foreach(Object obj in objs)
        {
            Renderer rr = (obj as Renderer);
            rr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            
        }
    }

    [MenuItem("MyHelperTools/Global/Select All Renderer")]
    static void SelectAllRenderer()
    {
        Object[] objs = GameObject.FindObjectsOfType(typeof(Renderer)); //returns Object[]
        Selection.objects = objs;
    }

    [MenuItem("MyHelperTools/Global/Select All Materials")]
    static void SelectAllMaterials()
    {
        Object[] objs = GameObject.FindObjectsOfType(typeof(Material)); //returns Object[]
        Selection.objects = objs;
    }
}

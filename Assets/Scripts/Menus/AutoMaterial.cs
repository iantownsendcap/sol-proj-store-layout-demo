//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class AutoMaterial : MonoBehaviour
//{
    


//    private Material Test;


//    // Add a menu item named "Do Something" to MyMenu in the menu bar.
//    [MenuItem("MyHelperTools/Edit Project Asset/Auto Set Material")]
//    static void AutoSetMaterial()
//    {
//        if (Selection.assetGUIDs.Length > 0)
//        {
//            //if (Selection.activeObject is Material)
//            //{
//            //    Material mat = Selection.activeObject as Material;
                
//            //    // Texture tex = mat.GetTexture("_albedo");
//            //    // mat.SetTexture("_albedo", tex);
//            //}

//            Material mat = null;
//            Dictionary<string, Texture> texs = new Dictionary<string, Texture>();
//            bool multi_mat_error = false;

//            int n = Selection.count;
//            for (int i = 0; i < n; i++)
//            {
//                if (Selection.objects[i] is Material)
//                {
//                    if (mat.Equals(null)) mat = Selection.activeObject as Material;
//                    else { multi_mat_error = true; break; }
//                }
//                if (Selection.objects[i] is Texture)
//                {
//                    Texture tex = Selection.activeObject as Texture;
//                    tex.name.Split("_");
//                    texs.Add("", tex);
//                }
//            }

//            if (multi_mat_error)
//            {
//                Debug.Log("Error: Multiple Materials Selected");
//                return;
//            }

            
//        }
//    }


//    //private void OnSelectionChange()
//    //{
//    //    if (Selection.activeObject is Material)
//    //    {

//    //        if (Selection.assetGUIDs.Length > 0)
//    //        {

//    //            Test = Selection.activeObject as Material;
//    //        }
//    //    }

//    //}


//    /*
//        // Specular vs Metallic workflow
//        _WorkflowMode("WorkflowMode", Float) = 1.0

//        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
//        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)

//        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

//        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
//        _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

//        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
//        _MetallicGlossMap("Metallic", 2D) = "white" {}

//        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
//        _SpecGlossMap("Specular", 2D) = "white" {}

//        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
//        [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

//        _BumpScale("Scale", Float) = 1.0
//        _BumpMap("Normal Map", 2D) = "bump" {}

//        _Parallax("Scale", Range(0.005, 0.08)) = 0.005
//        _ParallaxMap("Height Map", 2D) = "black" {}

//        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
//        _OcclusionMap("Occlusion", 2D) = "white" {}

//        [HDR] _EmissionColor("Color", Color) = (0,0,0)
//        _EmissionMap("Emission", 2D) = "white" {}

//        _DetailMask("Detail Mask", 2D) = "white" {}
//        _DetailAlbedoMapScale("Scale", Range(0.0, 2.0)) = 1.0
//        _DetailAlbedoMap("Detail Albedo x2", 2D) = "linearGrey" {}
//        _DetailNormalMapScale("Scale", Range(0.0, 2.0)) = 1.0
//        [Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}

//        // SRP batching compatibility for Clear Coat (Not used in Lit)
//        [HideInInspector] _ClearCoatMask("_ClearCoatMask", Float) = 0.0
//        [HideInInspector] _ClearCoatSmoothness("_ClearCoatSmoothness", Float) = 0.0
//*/ 

//}

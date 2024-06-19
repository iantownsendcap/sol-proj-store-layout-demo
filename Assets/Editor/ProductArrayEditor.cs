//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(ProductArray))]
//public class ProductArrayEditor : Editor
//{
//    ProductArray productArray;

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();


//        // DrawSettingsEditor(productArray.pap, productArray.OnSettingsUpdated, ref productArray.foldout);
//    }

//    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout)
//    {
//        using (var check = new EditorGUI.ChangeCheckScope())
//        {
//            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

//            if (foldout)
//            {
//                Editor editor = CreateEditor(settings);
//                editor.OnInspectorGUI();

//                if (check.changed)
//                {
//                    if (onSettingsUpdated != null)
//                    {
//                        onSettingsUpdated();
//                    }
//                }
//            }
//        }
//    }

//    private void OnEnable()
//    {
//        productArray = (ProductArray)target;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using Unity.VisualScripting;

public class ProductDataWindow : EditorWindow
{
    private static ModularMenu.ProductDataLine productData;
    public bool refresh = true;
    private static Object[] previousObj;

    [MenuItem("MyHelperTools/Modulars/Product Data Tool")]
    static void Init()
    {
        // productData = new ModularMenu.ProductDataLine();
        previousObj = null;
        var window = GetWindowWithRect<global::ProductDataWindow>(new Rect(200, 200, 400, 400));
        window.minSize = new Vector2(400, 400); 
        window.maxSize = new Vector2(1200, 400);
        window.Show();

        EnableDisableOnSelection();
    }



    static bool EnableDisableOnSelection()
    {
        //if (Selection.objects == null || Selection.objects.Length == 0)
        //{
        //    productData = null;
        //    return true;
        //}

        if (previousObj == null) previousObj = new Object[1];
        if (Selection.objects.Length != 1) return false;
        if (Selection.objects[0] == previousObj[0]) return false;
        previousObj = Selection.objects;

        productData = null;
        //Transform productGroup = PrefabUtility.GetCorrespondingObjectFromSource((Selection.objects[0] as GameObject).transform);
        //if (productGroup == null) return false;

        if (Selection.objects[0] == null) return true;
        GameObject selobj = (Selection.objects[0] as GameObject);
        if (selobj == null) return true;
        ProductArray pa = selobj.GetComponent<ProductArray>();
        GameObject product = null;
        if (pa == null)
        {
            product = Selection.objects[0] as GameObject;
        }
        else
        {
            product = pa.Product;
            if (product == null) return true;
        }
        ModularMenu.AllProductDataLines = null;
        productData = ModularMenu.GetProductDataByName(product.name);
        if (productData == null) return true;
        productData.obj = product;

        return true;
    }

    private void OnInspectorUpdate()
    {
        if (EnableDisableOnSelection()) Repaint();
    }

    void OnGUI()
    {
        if (!refresh) return;

        if (productData == null)
        {
            productData = new ModularMenu.ProductDataLine();
        }

        EditorGUILayout.BeginVertical(); // 0 the main list of controls

        EditorGUILayout.LabelField("filename", productData.filename);
        EditorGUILayout.LabelField("path", productData.path);

        EditorGUILayout.LabelField("------------------");

        productData.UPC = EditorGUILayout.TextField("UPC", productData.UPC);
        productData.CATEGORY = EditorGUILayout.TextField("category", productData.CATEGORY);
        productData.KEYWORDS = new List<string>(EditorGUILayout.TextField("Keywords", string.Join(",", productData.KEYWORDS)).ToLower().Split(","));
        productData.DEPARTMENT = (ModularMenu.Departments)EditorGUILayout.EnumPopup("department", productData.DEPARTMENT);

        EditorGUILayout.LabelField("------------------");

        string p = EditorGUILayout.TextField("price", productData.price.ToString("c2")) ;
        float.TryParse(p.Replace("$", ""), out productData.price);

        productData.sales = EditorGUILayout.FloatField("sales", productData.sales);

        productData.shrink = EditorGUILayout.FloatField("shrink", productData.shrink);

        EditorGUILayout.LabelField("------------------");

        EditorGUILayout.ObjectField("Object", productData.obj, typeof(GameObject), false);

        productData.dimensions = EditorGUILayout.Vector3Field("dimensions", productData.dimensions);
        if (GUILayout.Button("Get Dimensions"))
        {
            ModularMenu.UpdateBounds(Selection.objects);
            ModularMenu.UpdateProductData(productData);
            refresh = true;
        }



        EditorGUILayout.LabelField("line index", productData.lineIndex.ToString());

        if (GUILayout.Button("Save"))
        {
            ModularMenu.UpdateProductData(productData);
        }

        EditorGUILayout.EndVertical(); // 0 the main list of controls

    }
}

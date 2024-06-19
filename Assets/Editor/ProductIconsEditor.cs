using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

[CustomEditor(typeof(ProductIcons))]
public class ProductIconsEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        // Debug.Log("Selected");

        ProductIcons icon = target as ProductIcons;

        // icon.OnSelected();

        // base.OnInspectorGUI();

        
    }

}

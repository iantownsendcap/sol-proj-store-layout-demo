using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;

[CustomEditor(typeof(ProductIcons))]
public class ModularIconsEditor : Editor
{
    ModIcons icon;
    public override void OnInspectorGUI()
    {
        // Debug.Log("Selected");

        icon = target as ModIcons;

        // icon.OnSelected();

        // base.OnInspectorGUI();
    }

    // this needs to be on the modular
    //private void OnEnable()
    //{
    //    if (icon != null)
    //    {
    //        Renderer[] icons = icon.transform.GetComponentsInChildren<Renderer>();
    //        if (icons != null) foreach (Renderer renderer in icons) { renderer.enabled = true; }
    //        //ISetHighlight
    //        //UI.Selectable.IsHighlighted;
    //    }
    //}
}
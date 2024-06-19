using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static ModularMenu;

public class HeatMappingWindow : EditorWindow
{
    public static class HeatMapSettings
    {
        //public bool Enabled = false;
        public static bool price_enable = false;
        public static bool price_invert = false;
        public static bool shrink_enable = false;
        public static bool shrink_invert = false;
        public static bool sales_enable = false;
        public static bool sales_invert = false;

        internal static void Reset()
        {
            price_enable = false;
            price_invert = false;
            shrink_enable = false;
            shrink_invert = false;
            sales_enable = false;
            sales_invert = false;
        }

        //internal static void Copy(HeatMapSettings other)
        //{
        //    //this.Enabled = other.Enabled;
        //    this.price_enable = other.price_enable;
        //    this.price_invert = other.price_invert;
        //    this.shrink_enable = other.shrink_enable;
        //    this.sales_enable = other.sales_enable;
        //    this.shrink_invert = other.shrink_invert;
        //    this.sales_invert = other.sales_invert;
        //}
        //internal static bool Equals(HeatMapSettings other)
        //{
        //    //if (this.Enabled != other.Enabled) return false;
        //    if (this.price_enable != other.price_enable) return false;
        //    if (this.price_invert != other.price_invert) return false;
        //    if (this.shrink_enable != other.shrink_enable) return false;
        //    if (this.sales_enable != other.sales_enable) return false;
        //    if (this.shrink_invert != other.shrink_invert) return false;
        //    if (this.sales_invert != other.sales_invert) return false;
        //    return true;
        //}

    }
    //public static HeatMapSettings HeatMapSettings = new HeatMapSettings();
    //public static HeatMapSettings LastHeatMapSettings = new HeatMapSettings();

    //public delegate void NotifyHandler(bool HeatMapSettingsping, EventArgs args);  // delegate
    //public event NotifyHandler Notify;

    public static HeatMappingWindow window;

    //public class EnableHeatMapSettings
    //{
    //    public event Notify ProcessCompleted; // event

    //}

    public delegate void ValueChanged();
    public static event ValueChanged OnValueChanged;

    // public UnityEvent HeatMapSettingsUpdated;


    // Start is called before the first frame update
    [MenuItem("MyHelperTools/Modulars/Heat Mapping")]
    static void Init()
    {
        window = GetWindowWithRect<global::HeatMappingWindow>(new Rect(10, 10, 240, 200));
        window.Show();
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(); // 0 the main list of controls

        //HeatMapSettings.Enabled = EditorGUILayout.Toggle("Enable Heat Mapping: ", HeatMapSettings.Enabled);

        // min max header
        EditorGUILayout.BeginHorizontal(
            //new GUILayoutOption[] {
            //GUILayout.MaxWidth(10)
            //    }
            );
        GUILayout.Space(60);
        EditorGUILayout.LabelField("enable", GUILayout.Width(60));
        EditorGUILayout.LabelField("invert", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        // ---
        EditorGUILayout.BeginHorizontal();
        //EditorGUIUtility.labelWidth
        EditorGUILayout.LabelField("price: ", GUILayout.Width(60));
        HeatMapSettings.price_enable = EditorGUILayout.Toggle("", HeatMapSettings.price_enable, GUILayout.Width(60));
        HeatMapSettings.price_invert = EditorGUILayout.Toggle("", HeatMapSettings.price_invert, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("sales: ", GUILayout.Width(60));
        HeatMapSettings.sales_enable = EditorGUILayout.Toggle("", HeatMapSettings.sales_enable, GUILayout.Width(60));
        HeatMapSettings.sales_invert = EditorGUILayout.Toggle("", HeatMapSettings.sales_invert, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("shrink: ", GUILayout.Width(60));
        HeatMapSettings.shrink_enable = EditorGUILayout.Toggle("", HeatMapSettings.shrink_enable, GUILayout.Width(60));
        HeatMapSettings.shrink_invert = EditorGUILayout.Toggle("", HeatMapSettings.shrink_invert, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(60);
        ModularMenu.ProductDataSummary.minColor = EditorGUILayout.ColorField(ModularMenu.ProductDataSummary.minColor, GUILayout.Width(60));
        ModularMenu.ProductDataSummary.maxColor = EditorGUILayout.ColorField(ModularMenu.ProductDataSummary.maxColor, GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical(); // 0 the main list of controls

        //if (!LastHeatMapSettings.Equals(HeatMapSettings))
        //{
        //if (DragVolume.HeatMapSettings != null)
        //{
        //    DragVolume.HeatMapSettings.Copy(HeatMapSettings);
        //OnNotify();
        //     LastHeatMapSettings.Copy(HeatMapSettings);
        //    SceneView.RepaintAll();
        // }
        //}

        if (GUI.changed)
        {
            OnValueChanged?.Invoke();
            // HeatMapSettingsUpdated.Invoke();
            SceneView.RepaintAll();
        }
    }

    void OnDestroy()
    {
        ProductDataSummary.Reset();
        HeatMapSettings.Reset();
        OnValueChanged?.Invoke();
    }

    //protected virtual void OnNotify()
    //{
    //    if(Notify!= null)
    //    {
    //        Notify(HeatMapSettingsping, EventArgs.Empty);
    //    }
    //}
}

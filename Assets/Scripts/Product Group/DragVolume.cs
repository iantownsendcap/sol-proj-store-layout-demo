// using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
//using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.PlayerLoop;
//using UnityEngine.Rendering.HighDefinition;
//using static Unity.VisualScripting.Metadata;

[ExecuteInEditMode]

public class DragVolume : MonoBehaviour
{
    public Transform endNodeWidthHeight;
    public Transform endNodeDepth;

    public Transform boundry;
    public ShelfGroup Shelf;
    public int ShelfIndex = -1;

    public Transform iconsgroup;

    // public bool HeatMapSubscribed = false;

    public float handlesize = 0.1f;

    public float scale = 1;

    public Material originalmaterial;



    //public Color DefaultFillColor = Color.blue;
    //public Color DefaultEdgeColor = Color.blue;

    private void Start()
    {
        //if (!HeatMapSubscribed) {  HeatMapSubscribed = true; }
    }

    private void OnEnable()
    {
        HeatMappingWindow.OnValueChanged += UpdateHeatMap;
    }

    private void UpdateHeatMap()
    {
        Debug.Log("Heat Map Updating...");
        if (boundry == null || boundry.transform == null) return;
        Renderer rend = boundry.transform.GetComponentInChildren<Renderer>();
        if (rend != null)
        {

            bool heatmapon = HeatMappingWindow.HeatMapSettings.sales_enable || HeatMappingWindow.HeatMapSettings.shrink_enable || HeatMappingWindow.HeatMapSettings.price_enable;

            rend.enabled = heatmapon;
            if (heatmapon)
            {
                float H = 0;
                ProductArray pa = gameObject.GetComponent<ProductArray>();
                if (pa != null)
                {
                    ModularMenu.ProductDataLine productData = ModularMenu.GetProductDataByName(pa.Product.name);
                    if (productData != null)
                    {
                        rend.enabled = true;

                        int nums = 0;
                        
                        if (HeatMappingWindow.HeatMapSettings.price_enable)
                        {
                            if (HeatMappingWindow.HeatMapSettings.price_invert)
                            {
                                H += (ModularMenu.ProductDataSummary.price - productData.price) / ModularMenu.ProductDataSummary.price;
                            } else H += productData.price / ModularMenu.ProductDataSummary.price;
                            nums++;
                        }
                        // ---

                        if (HeatMappingWindow.HeatMapSettings.sales_enable)
                        {
                            if (HeatMappingWindow.HeatMapSettings.sales_invert)
                            {
                                H += (ModularMenu.ProductDataSummary.sales - productData.sales) / ModularMenu.ProductDataSummary.sales;
                            } else H += productData.sales / ModularMenu.ProductDataSummary.sales;

                            nums++;
                        }
                        
                        // ---
                        if (HeatMappingWindow.HeatMapSettings.shrink_enable)
                        {
                            if (HeatMappingWindow.HeatMapSettings.shrink_invert)
                            {
                                H += (ModularMenu.ProductDataSummary.shrink - productData.shrink) / ModularMenu.ProductDataSummary.shrink;
                            } else H += productData.shrink / ModularMenu.ProductDataSummary.shrink;
                            nums++;
                        }
                        
                        // ---
                        if (H > 0)
                        {
                            H /= nums;
                            H = Mathf.Clamp(H, 0, 1);
                        }

                        Color CenterColor = Color.white;
                        CenterColor = Color.Lerp(ModularMenu.ProductDataSummary.minColor, ModularMenu.ProductDataSummary.maxColor, H);

                        Color EdgeColor = Color.white;
                        EdgeColor = Color.Lerp(ModularMenu.ProductDataSummary.minColor, ModularMenu.ProductDataSummary.maxColor, H);
                        
                        // CenterColor.a = EdgeColor.a = 0.6f;

                        rend.material.SetColor("_CenterColor", CenterColor);
                        rend.material.SetColor("_EdgeColor", EdgeColor);
                        
                    }
                }
            }else
            {
                rend.material = originalmaterial;
                rend.sharedMaterial = originalmaterial;
            }

        }
    }


    public void SetSize(Vector3 size)
    {
        endNodeWidthHeight.localPosition = new Vector3(size.x, size.y, 0);
        endNodeDepth.localPosition = new Vector3(0, 0, size.z);
        Refresh();
    }

    private GameObject[] GetChildren(GameObject obj, string nm)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform trans in obj.transform)
        {
            if (trans == null) continue;
            if (trans.name == nm) { children.Add(trans.gameObject); }
        }
        return children.ToArray();
    }

    public void Refresh()
    {
        boundry.localScale = new Vector3(endNodeWidthHeight.localPosition.x, endNodeWidthHeight.localPosition.y, endNodeDepth.localPosition.z);
    }

    private void Update()
    {
        Refresh();

        
    }
}

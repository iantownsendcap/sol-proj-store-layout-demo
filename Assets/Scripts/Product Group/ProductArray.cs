using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

[ExecuteInEditMode]

public class ProductArray : MonoBehaviour
{
    //[HideInInspector]
    //public ProductArrayProperties pap;
    //[HideInInspector]
    //public bool foldout;

    public GameObject Product;
    [HideInInspector] public GameObject LastProduct;

    public Vector3Int Grid;
    [HideInInspector] public Vector3Int LastGrid;

    // [SerializeField]
    // bool refresh = false;

    public int MaxProduct = 100;
    public int NumberOfProducts;

    [Header("Transform")]
    public Vector3 Space = new Vector3(6, 18, 8);
    public Vector3 Offset = new Vector3(3.6f, 0, 4.11f);
    //[Range(0, 360)]
    //public float Rotate;
    public Vector3 direction = new Vector3(-1, 1, 1);
    public Quaternion Rotation = Quaternion.Euler(0, 180, 0);

    [Header("Random")]
    //[Range(0, 180)]
    //public float RotateRandom = 8;
    public Quaternion RotationRandom = Quaternion.Euler(0, 8, 0);
    public Vector3 PositionRandom = new Vector3(0.05f, 0, 0.05f);
    public int seed;

    // GameObject[] items;

    // public int ShelfNumber;

    const float meter_to_inch = 39.3701f;
    const float inch_to_meter = 0.0254f;

    GameObject[] DestroyItems;

    bool rr = false;


    private void OnValidate()
    {
        if (Product != null && Product != LastProduct) ModularMenu.UpdateBounds(new GameObject[1] { gameObject });
        LastProduct = Product;
        rr = true;
        // Debug.Log("OnValidate");
    }

    // ProductArrayProperties LastProductArray;

    //private void OnValidate()
    //{
    //    // refresh = true;

    //    //UnityEditor.EditorApplication.delayCall += () =>
    //    //{
    //    //    MakeIt();
    //    //};
    //    MakeIt();
    //}


    //IEnumerator Destroy(GameObject go)
    //{
    //    yield return new WaitForEndOfFrame();
    //    DestroyImmediate(go);
    //}

    Transform dyngrp;
    private void Start()
    {
        
    }


    //// Update is called once per frame
    void Update()
    {

        GetDynGrp();

        
        if (!LastGrid.Equals(Grid))
        {
            // Debug.Log("Grid Changed");
            rr = true;
        }
        LastGrid = Grid;


        // Product changes are triggered by onvalidate

        DestroyAllChildren();

        if (!rr) return;
        rr = false;


        dyngrp.localPosition = Vector3.zero;
        dyngrp.localScale = Vector3.one;

        // If the number of products is less than the max
        // make more until it reaches the desired value
        // if (items == null) items = new GameObject[0];
        List<GameObject> newitems = new List<GameObject>();
        int prodnum = Grid.x * Grid.y * Grid.z;
        NumberOfProducts = 0;
        while (newitems.Count < prodnum)
        {
            NumberOfProducts++;

            if (NumberOfProducts > MaxProduct && MaxProduct != -1) break;
            GameObject c = PrefabUtility.InstantiatePrefab(Product) as GameObject;
            if (c != null && dyngrp != null)
            {
                c.transform.parent = dyngrp;
                c.AddComponent<BoxCollider>();
                newitems.Add(c);
            }
        }

        // Get a random seed based on position
        Random.InitState(seed + Mathf.RoundToInt(transform.position.magnitude));

        if (newitems != null)
        {

            int n = 0;
            bool allbreak = false;
            for (int y = 0; y < Grid.y; y++)
            {
                if (allbreak) break;
                for (int z = 0; z < Grid.z; z++)
                {
                    if (allbreak) break;
                    for (int x = 0; x < Grid.x; x++)
                    {

                        if (n > newitems.Count - 1 || newitems[n] == null) allbreak = true;

                        if (allbreak) break;

                        // if (newitems == null || newitems.Count - 1 < n || newitems[n] == null) { newitems = null; return; }

                        //ress r = CalculateBounds(c);

                        // I want the min on the floor and the width half
                        // the min and max from the pivot

                        Vector3 sp = Space * 0.01f;
                        Vector3 of = Offset * 0.01f;

                        Vector3 randp = new Vector3(Random.Range(-PositionRandom.x, PositionRandom.x), Random.Range(-PositionRandom.y, PositionRandom.y), Random.Range(-PositionRandom.z, PositionRandom.z));
                        of += randp * 0.1f;

                        Vector3 pos = new Vector3(of.x + (sp.x * x), of.y + (sp.y * y), of.z + (sp.z * z));
                        pos = Vector3.Scale(pos, direction);

                        Vector3 m = pos;
                        newitems[n].transform.localPosition = m;

                        Vector3 rv = RotationRandom.eulerAngles;
                        newitems[n].transform.localRotation = Rotation * Quaternion.Euler(Random.Range(-rv.x, rv.x), Random.Range(-rv.y, rv.y), Random.Range(-rv.z, rv.z));

                        n++;
                    }
                }
            }

        }

        bool mergeresult = MergeObjArray.MergeThem(dyngrp.gameObject, dyngrp.gameObject);

        //catch { Debug.Log("Could not merge the Meshes"); }

        DestroyAllChildren();

        dyngrp.localPosition = Vector3.zero;
        dyngrp.localScale = Vector3.one;

        Resources.UnloadUnusedAssets();
        EditorUtility.SetDirty(this);

    }

    private void GetDynGrp()
    {
        if (dyngrp == null)
        {
            dyngrp = transform.Find("DynamicGroup");
            if (dyngrp == null)
            {
                Transform[] tr = transform.GetComponentsInChildren<Transform>();
                for (int i = 0; i < tr.Length; i++)
                {
                    if (dyngrp == null && tr[i].name == "DynamicGroup")
                    {
                        dyngrp = tr[i];
                    }
                }
            }

            if (dyngrp == null)
            {
                dyngrp = new GameObject("DynamicGroup").transform;
                dyngrp.parent = transform;
            }

            if (dyngrp == null) return;
        }
    }

    private void DestroyAllChildren()
    {
        GetDynGrp();

        int cc = dyngrp.childCount;

        while (dyngrp.childCount > 0)
        {
#if UNITY_EDITOR
            DestroyImmediate(dyngrp.GetChild(0).gameObject);
#else
            Destroy(dyngrp.GetChild(0).gameObject);
#endif

            if (dyngrp.childCount == cc) { Debug.Log("Could not delete child of dyngrp"); break; }
            cc = dyngrp.childCount;

        }
    }

}

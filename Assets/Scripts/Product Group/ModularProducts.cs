using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class ModularProducts : MonoBehaviour
{

    public GameObject shelfAsset;
    public Vector3[] shelves;

    public bool hasBaseDeck = true;

    public Vector3 dimensions;

    //public class PG
    //{
    //    public ProductGroupConnector[] pg;

    //    public PG()
    //    {
    //        this.pg = new ProductGroupConnector[1];
    //    }
    //}

    //public PG[] ProductsPerShelf;

    public GameObject ProductGroup;
    public int[] NumberOfProductGroups;

    // Start is called before the first frame update
    void Start()
    {
        // if (shelves == null || shelves.Length == 0) shelves = new float[1] { 0.185f };
    }

    // Update is called once per frame
    void Update()
    {
        if (shelves == null || shelves.Length == 0) shelves = new Vector3[1] { new Vector3(1.2f, 0.185f, 1) };

        Transform shelfdyngrp = MakeShelfGrp("ShelfDynamicGroup", shelves.Length);

        int current;
        int total;
        int diff;

        if (shelfAsset != null)
        {
            current = shelfdyngrp.childCount;
            total = shelves.Length;
            //if (hasBaseDeck) total -= 1;
            diff = total - current;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    GameObject c;
                    if (hasBaseDeck && i == 0) { c = new GameObject("BaseDeck"); }
                    else
                    {
                        c = PrefabUtility.InstantiatePrefab(shelfAsset) as GameObject;
                    }

                    GameObject loc = new GameObject("loc");
                    loc.transform.parent = shelfdyngrp;
                    c.transform.parent = loc.transform;
                    // c.AddComponent<BoxCollider>();
                }
            }

            // ---

            List<int> nl = new List<int>(NumberOfProductGroups) { };
            while (nl.Count > shelves.Length)
            {
                nl.RemoveAt(nl.Count - 1);
            }

            while (nl.Count < shelves.Length)
            {
                nl.Add(0);
            }
            NumberOfProductGroups = nl.ToArray();

            // move the shelves

            for (int i = 0; i < shelves.Length; i++)
            {
                Transform loc = shelfdyngrp.GetChild(i);
                Transform c = loc.GetChild(0);

                //int g = i;
                //if (hasBaseDeck) g = i + 1;
                loc.localPosition = new Vector3(-dimensions.z, shelves[i].y, (shelves[i].x / 2));
                
                // 0.55f base, 1 is 0.45f
                c.localScale = new Vector3(shelves[i].z * 2.22f, 1, shelves[i].x * 0.5f);
                loc.localRotation = Quaternion.identity;

                // connect product groups to the shelves

                if (ProductGroup != null)
                {
                    current = loc.childCount-1; // ignore the shelf
                    total = NumberOfProductGroups[i];
                    diff = total - current;
                    if (diff > 0)
                    {
                        for (int j = 0; j < diff; j++)
                        {
                            GameObject pg = PrefabUtility.InstantiatePrefab(ProductGroup) as GameObject;
                            pg.transform.parent = loc;
                        }
                    }
                }

                // ---

                for (int j = 1; j <= NumberOfProductGroups[i]; j++)
                {
                    Transform pg = loc.GetChild(j);

                    pg.transform.localPosition = new Vector3(shelves[i].z, 0.015f, -shelves[i].x/2);
                    pg.transform.localRotation = Quaternion.Euler(0, -90, 0);
                }

                // ---
            }
        }


        // ---



    }

    private Transform MakeShelfGrp(string nm, int total)
    {
        Transform dyngrp = transform.Find(nm);
        if (dyngrp == null)
        {
            dyngrp = new GameObject(nm).transform;
            dyngrp.parent = transform;
        }

        if (dyngrp == null) return null;

        dyngrp.localPosition = Vector3.zero;
        dyngrp.localScale = Vector3.one;
        dyngrp.localRotation = Quaternion.Euler(0, 90, 0);

        // only delete ones that exceed the max

        int b = dyngrp.childCount;
        int diff = b - total;

        if (diff > 0)
        {
            for (int i = diff; i < 0; i++)
            {
                GameObject gm = dyngrp.GetChild(b - i).gameObject;
                if (gm != null) DestroyImmediate(gm);
                else { break; }
            }
        }

        return dyngrp;
    }

}

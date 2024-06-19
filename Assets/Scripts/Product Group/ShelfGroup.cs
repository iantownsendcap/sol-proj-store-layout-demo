using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

[ExecuteInEditMode]

public class ShelfGroup : MonoBehaviour
{
    // public GameObject[] Products;

    [HideInInspector]
    public GameObject ProductGroupPrefab;
    [HideInInspector]
    public GameObject ShelfPrefab;
    public bool HasShelf = true;

    public float shelflength = 48;
    public bool freeShelfLength = false;

    [HideInInspector]
    public List<GameObject> ProductGroups;
    // [HideInInspector]
    // public GameObject[] ProductGroupsLast;

    [HideInInspector]
    public GameObject shelf;
    [HideInInspector]
    public Transform ProductGroupTransform;

    const float inch_to_meter = 0.0254f;

    [HideInInspector]
    public Transform iconsgroup;

    bool rr = false;

    // Start is called before the first frame update
    void Start()
    {
        rr = true;
    }

    //private void OnValidate()
    //{
    //    Debug.Log("Property Changed");
    //}


    private void OnValidate()
    {
        rr = true;

        // if it needs a shelf and there isn't one: add one
        if (shelf == null && ShelfPrefab != null && HasShelf)
        {
            shelf = PrefabUtility.InstantiatePrefab(ShelfPrefab) as GameObject;//GameObject.Instantiate<GameObject>(ShelfPrefab);
            shelf.transform.SetParent(transform);
        }

        // show / hide the shelf
        if (shelf != null && shelf.activeSelf != HasShelf)
        {
            shelf.SetActive(HasShelf);
        }

        // ---

    }

    // Update is called once per frame
    void Update()
    {

        if (!rr) return;

        if (ProductGroupPrefab == null) return;

        if (ProductGroupTransform == null)
        {
            ProductGroupTransform = transform.Find("ProductGroups");
            if (ProductGroupTransform == null)
            {
                Transform[] cch = transform.GetComponentsInChildren<Transform>();
                for (int i = 0; i < cch.Length; i++)
                {
                    if (ProductGroupTransform == null && cch[i].name == "ProductGroups")
                    {
                        ProductGroupTransform = cch[i];
                    }
                }
            }
            if (ProductGroupTransform == null) return;
        }


        ProductGroups = new List<GameObject>();
        foreach (Transform child in ProductGroupTransform.GetComponentInChildren<Transform>())
        {
            ProductGroups.Add(child.gameObject);
        }

        List<GameObject> newObjects = new List<GameObject>();
        // Debug.Log("RUN");

        // check for a null add a new prefab if found
        for (int i = 0; i < ProductGroups.Count; i++)
        {
            if (ProductGroups[i] == null)
            {
                GameObject newObject = PrefabUtility.InstantiatePrefab(ProductGroupPrefab) as GameObject;
                newObject.transform.SetParent(ProductGroupTransform);
                ProductGroups[i] = newObject;
                newObjects.Add(newObject);
            }
            DragVolume dv = ProductGroups[i].GetComponent<DragVolume>();
            if (dv != null) { dv.Shelf = this; dv.ShelfIndex = i; }
        }

        // Show / Hide icons if there are no Product Groups
        Transform[] tr = transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < tr.Length; i++)
        {
            if (iconsgroup == null && tr[i].name == "icons")
            {
                iconsgroup = tr[i];
            }
        }

        if (iconsgroup != null)
        {
            Renderer[] icons = iconsgroup.transform.GetComponentsInChildren<Renderer>();
            if (icons != null) foreach (Renderer renderer in icons) { renderer.enabled = ProductGroups.Count <= 0; }
        }


        // place the product groups in position order
        float dist = 0;
        for (int i = 0; i < ProductGroups.Count; i++)
        {
            DragVolume dv = ProductGroups[i].GetComponent<DragVolume>();
            ProductArray pa = ProductGroups[i].GetComponent<ProductArray>();
            ProductGroupConnector pgc = ProductGroups[i].GetComponent<ProductGroupConnector>();

            pgc.shelfGroup = this;

            // if (Products.Length > i && Products[i] != null) pa.Product = Products[i];
            // pa.refresh = true;

            // dv.Update();
            ProductGroups[i].transform.localPosition = new Vector3(dist, 0, 0);
            ProductGroups[i].transform.localRotation = Quaternion.identity;
            dist += dv.endNodeWidthHeight.localPosition.x;
        }

        if (!freeShelfLength) { dist = shelflength * inch_to_meter; }
        if (shelf != null)
        {
            shelf.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
            shelf.transform.localScale = new Vector3(1, 1, dist / 2);
            shelf.transform.localPosition = new Vector3(dist / 2, -0.012f, 0.445f);
            transform.Find("SelectionCube").localPosition = new Vector3(dist / 2, -0.02f, 0);
        }
    }

    internal GameObject[] Make()
    {
        GameObject newObject = PrefabUtility.InstantiatePrefab(ProductGroupPrefab) as GameObject;
        newObject.transform.SetParent(ProductGroupTransform);
        newObject.GetComponent<DragVolume>().Shelf = this;
        rr = true;
        return new GameObject[1] { newObject };
    }


    //internal void Delete()
    //{
    //    DestroyImmediate(gameObject);
    //    rr = true;
    //}

    internal void MoveLeft(Transform trans)
    {
        trans.SetSiblingIndex(trans.GetSiblingIndex() - 1);
        rr = true;
    }

    internal void MoveRight(Transform trans)
    {
        trans.SetSiblingIndex(trans.GetSiblingIndex() + 1);
        rr = true;
    }
}

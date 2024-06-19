using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

[ExecuteInEditMode]

public class MyModular : MonoBehaviour
{
    public int numberOfShelves;

    //public GameObject[] ShelfGroups;
    //[HideInInspector]
    // public List<GameObject> CreatedShelfGroups;

    [HideInInspector]
    public GameObject shelfGroupPrefab;
    [HideInInspector]
    public Transform BackLeft;
    [HideInInspector]
    public Transform FrontLeft;
    //[HideInInspector]
    //public Transform ModularMesh;
    [HideInInspector]
    public Transform MeshSelector;

    public Vector3 Dimensions = new Vector3(48, 78, 24);
    const float inch_to_meter = 0.0254f;

    // how do I make an offset
    public Transform Connection;

    public bool pause = false;
    // public bool fix = false;

    // Start is called before the first frame update

    // public GameObject ModularGrp;

    bool rr = true;

    private void OnValidate()
    {
        rr = true;
    }

    private void Start()
    {
        
    }

    public void AlignOffset(Transform target)
    {
        Transform offset = transform.Find("OffsetGroup");

        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        transform.position = target.position;
        transform.rotation = target.rotation;

        offset.position = pos;
        offset.rotation = rot;
    }

    public void ResetOffset()
    {
        Transform offset = transform.Find("OffsetGroup");

        Vector3 pos = offset.position;
        Quaternion rot = offset.rotation;

        offset.localPosition = Vector3.zero;
        offset.localRotation = Quaternion.identity;

        transform.position = pos;
        transform.rotation = rot;
    }


    // Update is called once per frame
    void Update()
    {
        if (!rr) return;
        rr = false;

        if (pause) return;

        // --- TEST
        //if (ModularGrp == null)
        //{
        //    ModularGrp = new GameObject("MyModularGenerated");
        //    EditorUtility.SetDirty(ModularGrp);
        //    ModularGrp.transform.SetParent(transform);
        //    ModularGrp = ModularGrp.transform.gameObject;
        //}
        // --- TEST

        if (FrontLeft == null || BackLeft == null || MeshSelector == null)
        {
            Transform[] tr = transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < tr.Length; i++)
            {
                if (BackLeft == null && tr[i].name == "BackLeft")
                {
                    BackLeft = tr[i];
                }
                if (FrontLeft == null && tr[i].name == "FrontLeft")
                {
                    FrontLeft = tr[i];
                }
                if (MeshSelector == null && tr[i].name == "MeshSelector")
                {
                    MeshSelector = tr[i];
                }
            }
        }

        //if (BackLeft == null) { BackLeft = transform.Find("BackLeft"); }
        //if (FrontLeft == null) { FrontLeft = transform.Find("FrontLeft"); }
        //if (MeshSelector == null) { MeshSelector = transform.Find("MeshSelector"); }

        if (FrontLeft == null) { Debug.LogError("FrontLeft is null"); return; }
        if (MeshSelector == null) { Debug.LogError("MeshSelector is null"); return; }
        if (shelfGroupPrefab == null) { Debug.LogError("shelfGroupPrefab is null"); return; }

        // ModularMesh.localScale = Dimensions * inch_to_meter;
        Vector3 imdims = Dimensions * inch_to_meter;

        MeshSelector.transform.localPosition = new Vector3(imdims.x / 2, 0, 0);
        MeshSelector.transform.localScale = new Vector3(imdims.x - 0.1f, 0.05f, 0.05f);

        // if (CreatedShelfGroups == null) CreatedShelfGroups = new List<GameObject>();

        List<GameObject> CreatedShelfGroupsTmp = new List<GameObject>();
        foreach (Transform obj in FrontLeft)
        {
            CreatedShelfGroupsTmp.Add(obj.gameObject);
        }


        // Trying to remove all the null ones

        //foreach (GameObject gm in ShelfGroups)
        //{
        //    // add all the values that are not null
        //    if (gm != null) { CreatedShelfGroupsTmp.Add(gm); }
        //}

        List<GameObject> newObject = new List<GameObject>();
        // make as many as needed
        for (int i = 0; i < 100; i++) // instead of a while loop
        {
            if (CreatedShelfGroupsTmp.Count < numberOfShelves)
            {
                GameObject newShelfGroup = PrefabUtility.InstantiatePrefab(shelfGroupPrefab) as GameObject;
                newShelfGroup.transform.SetParent(FrontLeft);
                newShelfGroup.transform.localPosition = Vector3.zero + (Vector3.up * 0.2f);
                newShelfGroup.transform.localRotation = Quaternion.identity;
                newObject.Add(newShelfGroup);
                CreatedShelfGroupsTmp.Add(newShelfGroup);
            }
            else break;
        }

        for (int i = 0; i < 100; i++) // instead of a while loop
        {
            if (CreatedShelfGroupsTmp.Count > numberOfShelves)
            {
                DestroyImmediate(CreatedShelfGroupsTmp[CreatedShelfGroupsTmp.Count - 1]);
            }
            else break;
        }


        // ---

        // Remove/Add components that are already in the transform
        for (int i = 0; i < 100; i++) // instead of a while loop
        {
            ShelfGroup[] ch = FrontLeft.GetComponentsInChildren<ShelfGroup>();
            if (ch.Length > numberOfShelves)
            {
                Object v = ch[ch.Length - 1].gameObject;
                DestroyImmediate(v);
            }
            else break;
        }



        //ShelfGroups = new GameObject[CreatedShelfGroupsTmp.Count];
        //for (int i = 0; i < CreatedShelfGroupsTmp.Count; i++)
        //{
        //    ShelfGroups[i] = CreatedShelfGroupsTmp[i];
        //}


        // fix

        //if (fix)
        //{
        //    ShelfGroup[] ch = FrontLeft.GetComponentsInChildren<ShelfGroup>();
        //    CreatedShelfGroups.Clear();
        //    for (int i = 0; i < numberOfShelves; i++)
        //    {
        //        CreatedShelfGroups.Add(ch[i].gameObject);
        //    }
        //}
        //if (fix == true) fix = false;

        //

        // ShelfGroups = CreatedShelfGroups.ToArray();

        Selection.objects = newObject.ToArray();

        if (Connection != null)
        {
            transform.position = Connection.position;
            transform.rotation = Connection.rotation;
        }
    }

    internal void AddShelf()
    {
        numberOfShelves++;
        rr = true;
    }
}

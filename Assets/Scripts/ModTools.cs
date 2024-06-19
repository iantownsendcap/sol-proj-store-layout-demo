using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

//[ExecuteInEditMode]

public class ModTools : MonoBehaviour
{
    public GameObject anchor;

    [SerializeField]
    public static Transform[] roots;

    public List<Transform> assets;
    public List<Transform> nodes;

    // Start is called before the first frame update
    void Start()
    {

    }

    //[MenuItem("MyHelperTools/Furniture Layouts/Mods/Assign Roots")]
    //public static void SetRoots()
    //{
    //    string names = "";
    //    List<Transform> gets = new List<Transform>();
    //    foreach (object obj in Selection.objects)
    //    {
    //        gets.Add(((GameObject)obj).transform);
    //        if (names != "") { names += ", "; }
    //        names += ((GameObject)obj).name;
    //    }
    //    roots = gets.ToArray();
    //    // Debug.Log(roots.Length);

    //    if (EditorUtility.DisplayDialog("Roots Assigned",
    //                names, "OK"))
    //    {

    //    }
    //}


    //[MenuItem("MyHelperTools/Furniture Layouts/Mods/Attach a Mod")]
    //public static void MakeMod()
    //{
    //    // Debug.Log("HI");
    //    Object[] objects = Selection.objects;
    //    MakeMod(objects);
    //}


    //static List<string> getConnected()
    //{
    //    List<string> gg = new List<string>();

    //    GameObject modgrp = GameObject.Find("Mod Group");
    //    if (modgrp == null) return gg;

    //    foreach(Transform t in modgrp.transform)
    //    {
    //        ModTools mt = t.gameObject.GetComponent<ModTools>();
    //        if (mt == null) continue;

    //        gg.Add(mt.anchor.name);
    //    }

    //    return gg;
    //}

    //[MenuItem("MyHelperTools/Furniture Layouts/Mods/Select Connected Mod")]
    //public static void SelectConnectedMod()
    //{
    //    GameObject mod = getMod();
    //    if (mod != null) Selection.activeGameObject = mod;
    //}

    //public static GameObject getMod()
    //{
    //    GameObject modgrp = GameObject.Find("Mod Group");
    //    if (modgrp == null) return null;

    //    GameObject go = Selection.activeGameObject;
    //    if (go == null) return null;

    //    foreach (Transform t in modgrp.transform)
    //    {
    //        ModTools mt = t.gameObject.GetComponent<ModTools>();
    //        if (mt == null) continue;

    //        if (GameObject.ReferenceEquals(mt.anchor, go))
    //        {
    //            return t.gameObject;
    //        }
    //    }
    //    return null;
    //}



    ////public static void MakeMod(Object[] objects) 
    ////{

    ////    List<GameObject> newobjs = new List<GameObject>();

    ////    if (roots == null)
    ////    {
    ////        GameObject r = GameObject.Find("Products");
    ////        if (r != null)
    ////        {
    ////            roots = new Transform[1] { r.transform };
    ////        } else return;
    ////    }

    ////    if (roots == null || roots.Length <= 0)
    ////    {
    ////        if (EditorUtility.DisplayDialog("Make Mod Error",
    ////                "You need to assign the roots that contains the items to attach.\nThe roots may also be disabled.", "OK"))
    ////        {

    ////        }
    ////        return;
    ////    }

    ////    List<string> connected = getConnected();

    ////    GameObject modgroup = GameObject.Find("Mod Group");
    ////    if (modgroup == null) modgroup = new GameObject("Mod Group");

    ////    foreach (Object obj in objects)
    ////    {

    ////        if (connected.Contains(obj.name)) {
    ////            Debug.Log("Object has mode already.");
    ////            continue;
    ////        }

    ////        Selection.objects = new Object[1] { obj };

    ////        //Create a blank gameobject
    ////        GameObject modobj = new GameObject("Mod_"+obj.name);
    ////        // modobj.tag = "Mod";

    ////        modobj.transform.parent = modgroup.transform;

    ////        //Add the collider
    ////        modobj.AddComponent<BoxCollider>();

    ////        // Add the mod tools
    ////        ModTools modtools = modobj.AddComponent<ModTools>();

    ////        // Start the connections
    ////        modtools.setAnchor((GameObject)obj);
    ////        Physics.SyncTransforms();
    ////        modtools.attach((GameObject)obj);

    ////        if (modtools.nodes.Count <= 0) { //modobj
    ////            DestroyImmediate(modobj);
    ////            continue;
    ////        }

    ////        newobjs.Add(modobj);
    ////        // break;
    ////    }

    ////    Selection.objects = newobjs.ToArray();
    ////}

    //// ---

    ////public void attach(GameObject obj)
    ////{

    ////    //List<Transform> chtrans = new List<Transform>(transform.GetComponentsInChildren<Transform>());
    ////    //chtrans.Remove(transform);

    ////    //foreach (Transform f in chtrans)
    ////    //{
    ////    //    DestroyImmediate(f.gameObject);
    ////    //}

    ////    this.nodes = new List<Transform>();
    ////    Bounds bound = gameObject.GetComponent<BoxCollider>().bounds;

    ////    if (bound == null) return;

    ////    assets = new List<Transform>();

    ////    foreach (Transform root in roots)
    ////    {
    ////        Transform[] children = root.GetComponentsInChildren<Transform>();

    ////        foreach (Transform ch in children)
    ////        {

    ////            if (ch.gameObject.GetComponent<Renderer>() == null) continue;

    ////            // if (ch == root || ch.parent == root) continue;
    ////            // the bounds should not contain the parents

    ////            if (bound.Contains(ch.position))
    ////            {
    ////                GameObject go1 = new GameObject(ch.gameObject.name);
    ////                go1.transform.parent = transform;
    ////                go1.transform.position = ch.position;
    ////                go1.transform.rotation = ch.rotation;

    ////                assets.Add(ch);

    ////                // dummy, product
    ////                this.nodes.Add(go1.transform);
    ////            }
    ////        }

    ////    }
    ////}


    ////public void setAnchor(GameObject obj)
    ////{
    ////    this.anchor = obj;

    ////    transform.position = anchor.transform.position;
    ////    transform.rotation = anchor.transform.rotation;

    ////    Bounds targetbounds = DoStuff.CalculateBounds(this.anchor).bounds;
    ////    BoxCollider bo = gameObject.GetComponent<BoxCollider>();
    ////    bo.center = targetbounds.center;
    ////    bo.size = targetbounds.size;
    ////}


    ////[MenuItem("MyHelperTools/Furniture Layouts/Mods/Select Attached")]
    //public static void selectAttached()
    //{
    //    GameObject go = Selection.activeGameObject;
    //    if (go == null) return;
    //    ModTools mt = go.GetComponent<ModTools>();
    //    if (mt == null) return;

    //    List<GameObject> gos = new List<GameObject>();
    //    foreach (Transform tr in mt.assets)
    //    {
    //        gos.Add(tr.gameObject);
    //    }

    //    if (mt.assets != null) Selection.objects = gos.ToArray();
    //}



    ////[MenuItem("MyHelperTools/Furniture Layouts/Mods/Write Mods to File")]
    //public static void WriteModsToFile()
    //{
    //    GameObject modgrp = GameObject.Find("Mod Group");

    //    GameObject[] roots = new GameObject[1] { modgrp };

    //    List<string> products = new List<string>();

    //    // Make a list of all the products used
    //    // Associate the product with the mod

    //    StringBuilder sb = new StringBuilder();
    //    sb.Append($"{{{System.Environment.NewLine}");

    //    StringBuilder sbproducts = new StringBuilder();
    //    sbproducts.Append("    \"product\" : {" + System.Environment.NewLine);

    //    StringBuilder sbmods = new StringBuilder();
    //    sbmods.Append("    \"mods\" : {" + System.Environment.NewLine);

    //    string nl = "";
    //    foreach (GameObject root in roots)
    //    {
    //        foreach(Transform mod in root.transform) {

    //            ModTools mt = mod.GetComponent<ModTools>();
    //            if (mt == null) continue;

    //            // get the product related to the mod

    //            sbmods.Append(nl);
    //            sbmods.Append($"        \"{mod.name}\": {{ \"id\": {mod.name}, \"products\": {{{System.Environment.NewLine}");

    //            StringBuilder modproducts = new StringBuilder();

    //            for (int j = 0; j < mt.assets.Count; j++)
    //            {
    //                GameObject product = mt.assets[j].gameObject;
    //                Transform node = mt.nodes[j];


    //                Transform prefab = PrefabUtility.GetCorrespondingObjectFromSource(product.transform);
    //                string asset = product.name;
    //                if (prefab != null) asset = prefab.name;
    //                else Debug.Log("No prefab found: " + asset);

    //                if (j > 0) modproducts.Append("," + System.Environment.NewLine);
    //                modproducts.Append($"            \"{asset}\": {{ \"position\": {{ \"x\": {node.localPosition.x}, \"y\": {node.localPosition.y}, \"z\": {node.localPosition.z} }} }}");

    //                if (!products.Contains(asset))
    //                {
    //                    DoStuff.ress b = DoStuff.CalculateBounds(product);
    //                    string dimensionline = DoStuff.dimensionsLine(b);
    //                    dimensionline = dimensionline.Replace("dimensions", asset);
    //                    dimensionline = dimensionline.Remove(dimensionline.Length - 1, 1);

    //                    if (products.Count > 0) sbproducts.Append("," + System.Environment.NewLine);

    //                    sbproducts.Append( dimensionline );
    //                    products.Add(asset);
    //                }
    //            }

    //            sbmods.Append($"{modproducts.ToString()}{System.Environment.NewLine}        }}");
    //            nl = $",{System.Environment.NewLine}";
    //        }
    //    }


    //    sbproducts.Append(System.Environment.NewLine + "    }," + System.Environment.NewLine);
    //    sbmods.Append(System.Environment.NewLine + "    }" + System.Environment.NewLine);

    //    sb.Append($"{sbproducts.ToString()}{sbmods.ToString()}");
    //    sb.Append($"}}{System.Environment.NewLine}");

    //    System.IO.File.WriteAllText(@"C:\Users\ian.townsend\Desktop\py\moddata.txt", sb.ToString());


    //    Debug.Log("Done");


    //}



    // Update is called once per frame





    void Update()
    {

        //if (anchor != null)
        //{
        //    transform.position = anchor.transform.position;
        //    transform.rotation = anchor.transform.rotation;
        //}

        //if (nodes != null && nodes.Count > 0)
        //{
        //    for (int i = 0; i < nodes.Count; i++)
        //    {
        //        assets[i].transform.position = nodes[i].position;
        //        assets[i].transform.rotation = nodes[i].rotation;
        //    }
        //}

    }
}

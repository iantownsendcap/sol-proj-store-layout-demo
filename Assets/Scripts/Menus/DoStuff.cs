using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System.IO;
using System.Text;
using System.Linq;

using System.Text.Json;

[ExecuteInEditMode]

public class DoStuff : MonoBehaviour
{
    
    // public GameObject[] Roots;

    // GameObject[] allObjects;

    public bool autoRefresh = false;

    public static string OutFileStatic;
    public string DataFile = @"C:\Users\ian.townsend\Desktop\py\data.txt";

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("MyHelperTools/Furniture Layouts/Write Data")]
    static void WriteDataButton()
    {
        GameObject fixturegrp = GameObject.Find("Fixtures");
        GameObject[] Roots = new GameObject[1] { fixturegrp };
        WriteBlockData(Roots, OutFileStatic);
    }
    [MenuItem("MyHelperTools/Furniture Layouts/Refresh from Data")]
    static void RefreshDataButton()
    {
        RefreshFromBlockData();
    }


    [MenuItem("MyHelperTools/Show CAD Lines")]
    static void ShowCADLines()
    {
        Transform Fixtures = GameObject.Find("Fixtures").transform;
        Transform CAD = GameObject.Find("CAD").transform;

        while (CAD.childCount > 0)
        {
            DestroyImmediate(CAD.GetChild(0));
            break;
        }

        for (int i = 0; i < Fixtures.childCount; i++)
        {
            // get the position
            Vector3 pos = Fixtures.GetChild(i).position;
            // get teh orientation
            Quaternion rot = Fixtures.GetChild(i).rotation;
            // get the bounds
            MeshRenderer mr = Fixtures.GetChild(i).gameObject.GetComponent<MeshRenderer>();
            if (mr == null) continue;
            Bounds b = mr.bounds;

            string[] CADLines = AssetDatabase.FindAssets("CADLine t:Prefab");
            if (CADLines.Length == 0) return;

            string pth = AssetDatabase.GUIDToAssetPath(CADLines[0]);
            GameObject CADLine = AssetDatabase.LoadAssetAtPath(pth, typeof(GameObject)) as GameObject;

            GameObject CADLineObj = PrefabUtility.InstantiatePrefab(CADLine) as GameObject;
            CADLineObj.transform.SetParent(CAD);
            CADLineObj.transform.localPosition = pos;
            CADLineObj.transform.localRotation = rot;

            break;
        }


    }


    public static void WriteBlockData(GameObject[] roots, string output)
    {

        StringBuilder allkeys = new StringBuilder();

        Dictionary<string, List<string>> AssetList = new Dictionary<string, List<string>>();

        StringBuilder sb = new StringBuilder();
        sb.Append("{");

        Dictionary<string, ress> AssetBounds = new Dictionary<string, ress>();

        foreach (GameObject g in roots)
        {
            Transform t = g.GetComponent<Transform>();

            for (int i = 0; i < t.childCount; i++)
            {
                Transform ch = t.GetChild(i);

                string id = ch.name;
                string asset = ch.name;

                Transform prefab = PrefabUtility.GetCorrespondingObjectFromSource(ch);
                if (prefab != null) asset = prefab.name;

                if (!AssetBounds.ContainsKey(asset)) AssetBounds[asset] = CalculateBounds(ch.gameObject);

                if (asset == "") asset = ch.name;

                if (!AssetList.ContainsKey(asset)) AssetList.Add(asset, new List<string>());

                AssetList[asset].Add(blockline(ch, asset));

            }
        }

        List<string> keys = AssetList.Keys.ToList(); ;
        keys.Sort();

        string comma = "";

        foreach (string key in keys)
        {
            sb.Append($"{System.Environment.NewLine}    \"{key}\" : {{{System.Environment.NewLine}");
            comma = "";
            allkeys.Append(key + System.Environment.NewLine);

            float width = AssetBounds[key].bounds.size.z;
            float length = AssetBounds[key].bounds.size.x;

            sb.Append(dimensionsLine(AssetBounds[key]));
            sb.Append($"{System.Environment.NewLine}        \"blocks\":{{{System.Environment.NewLine}");

            foreach (string line in AssetList[key])
            {
                sb.Append(comma);
                sb.Append(line);
                if (comma == "") comma = "," + System.Environment.NewLine;
            }

            sb.Append($"{System.Environment.NewLine}        }}{System.Environment.NewLine}    }}");
            if (keys.Count - 1 != keys.IndexOf(key)) sb.Append(",");
        }


        sb.Append(System.Environment.NewLine + "}" + System.Environment.NewLine);
        File.WriteAllText(output, sb.ToString());
        // File.WriteAllText(@"C:\Users\ian.townsend\Desktop\gh.log", allkeys.ToString());

        Debug.Log("Done");
    }

    public static string dimensionsLine(ress ress)
    {
        return $"        \"dimensions\": {{ \"width\": {ress.bounds.size.x}, \"length\": {ress.bounds.size.z}, \"pivot\": {{\"x\": {ress.offset.z}, \"y\": {ress.offset.x}}} }},";
    }

    public static string blockline(Transform ch, string asset)
    {
        return $"            \"{ch.name}\": {{\"id\": \"{ch.name}\", \"asset\": \"{asset}\", \"position\": {{\"x\": {ch.position.x}, \"y\": {ch.position.z}, \"z\": {ch.position.y}}}, \"scale\": {{\"x\": {ch.localScale.x}, \"y\": {ch.localScale.z}, \"z\": {ch.localScale.y}}}, \"rotation\": {ch.rotation.eulerAngles.y}}}";
    }

    class block
    {
        public string asset;
        public string id;
        public Vector3 position; // Dictionary<string,float>
        public float rotation;
        public Vector3 scale;
    }
    class dimension
    {
        public float width;
        public float length;
        public Dictionary<string, float> pivot;
    }
    class mainobj
    {
        public dimension dimensions;
        public Dictionary<string, block> blocks;
    }


    static Dictionary<string, string> GetKeyValues(string line)
    {
        Dictionary<string, string> output = new Dictionary<string, string>();

        // cut to just the data
        string ln = line;
        int st = ln.IndexOf("{") + 1;
        int en = ln.LastIndexOf("}");
        ln = ln[st..en].Trim();

        st = 0;

        // loop until the end of line
        while(true)
        {
            // get the key
            string key = ln;
            st = key.IndexOf("\"", st);
            if (st == -1) break;
            st += 1;
            en = key.IndexOf("\"", st);
            if (en == -1) break;
            key = key[st..en].Trim();

            // get the value
            string value = ln;
            int st2 = value.IndexOf(":", en + 1);
            if (st2 == -1) break;
            en = value.IndexOf(",", st2+1);
            if (en == -1) en = ln.Length;
            value = value[(st2+1)..en].Trim();

            if (value.Contains("{"))
            {

                value = ln;
                st = value.IndexOf("{", st2);
                en = value.IndexOf("}", st2+1)+1;
                value = ln[st..en].Trim();

            }
            st = en;

            output.Add(key, value);
        }

        return output;
    }

    static void RefreshFromBlockData()
    {
        Dictionary<string, mainobj> data = ReadBlockData();

        //GameObject r = GameObject.Find("Products");
        //if (r == null)
        //{
        //    if (EditorUtility.DisplayDialog("Make Mod Error",
        //            "The roots may be disabled.", "OK"))
        //    {

        //    }
        //    return;
        //}
    
        foreach (string k in data.Keys)
        {
            // Debug.Log(k);
            mainobj mainobj = data[k];
            foreach (string assetname in data[k].blocks.Keys)
            {
                block block = data[k].blocks[assetname];

                //if (block.asset != "Checkout_gueqmj") continue;
                //Debug.Log("--- TEST Checkout_gueqmj ---");

                // Debug.Log(block.id);
                // Debug.Log("Anti-theftDoor_9qs8y9 (1)");
                
                GameObject c = GameObject.Find(block.id);
                if (c == null)
                {
                    c = AddAsset(block);
                    if (c == null) { Debug.Log("Could not find asset: " + block.id); continue; }
                }
                else
                {

                    Vector3 newpos = block.position;
                    if (Vector3.Distance(c.transform.position, newpos) > 0)
                    {
                        Debug.Log("Moved:");
                        Debug.Log(c.name);
                    }

                    c.transform.position = newpos;
                    c.transform.rotation = Quaternion.Euler(0, block.rotation, 0);

                    // Debug.Log(c);

                    // Selection.activeObject = c;

                    // break;
                }

                
                // ModTools.MakeMod(new Object[1] { c });
            }
            // break;
        }

        /*
        
        var newLink = EditorUtility.InstantiatePrefab(NodePrefab) as GameObject;
        newLink.transform.position = target.transform.position;

        */
    }

    static Dictionary<string, mainobj> ReadBlockData() {

        string[] lines = File.ReadAllLines(OutFileStatic);

        Dictionary<string, mainobj> data = new Dictionary<string, mainobj>();

        int depth = 0;
        mainobj mainobj = null;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "{" || lines[i] == "}" || lines[i] == "") continue;

            string line = lines[i];

            if (line == "        }") { depth++; continue; }
            if (line == "    }") { depth = 0; continue; }
            if (line == "    },") { depth = 0; continue; }

            if (depth == 0)
            {
                mainobj = new mainobj();
                mainobj.dimensions = new dimension();
                mainobj.blocks = new Dictionary<string, block>();

                //     "Anti-theftDoor_9qs8y9" : {

                string asset = line;
                int st = asset.IndexOf("\"") + 1;
                int en = asset.IndexOf("\"", st);
                asset = asset.Substring(st, en - st);
                data.Add(asset, mainobj);

                depth++;
                continue;
            }

            if (depth == 1)
            {
                //"dimensions": { "width": 0.3811517, "length": 0.09065947, "pivot": {"x": 6.705523E-08, "y": 0} },
                Dictionary<string, string> keyval = GetKeyValues(line);

                string pivotstring = keyval["pivot"];

                Dictionary<string, string> pivotval = GetKeyValues(pivotstring);

                mainobj.dimensions.width = float.Parse(keyval["width"]);
                mainobj.dimensions.length = float.Parse(keyval["length"]);
                mainobj.dimensions.pivot = new Dictionary<string, float>() {
                    { "x", float.Parse(pivotval["x"]) },
                    { "y", float.Parse(pivotval["y"]) }
                };

                depth++;
                continue;
            }

            if (depth == 2)
            {
                //        "blocks":{
                depth++;
                continue;
            }

            if (depth == 3)
            {
                //             "Anti-theftDoor_9qs8y9 (1)": {"id": "Anti-theftDoor_9qs8y9 (1)", "asset": "Anti-theftDoor_9qs8y9", "position": {"x": 2.6, "y": -1.4},"rotation": 0},

                // Debug.Log(line);

                Dictionary<string, string> keyval = GetKeyValues(line);
                block block = new block();

                foreach (string key in keyval.Keys)
                {
                    Debug.Log(key);

                    switch (key)
                    {
                        case "id":
                            block.id = keyval[key].Replace("\"", "");
                            break;
                        case "asset":
                            block.asset = keyval[key].Replace("\"", "");
                            break;
                        case "rotation":
                            block.rotation = float.Parse( keyval[key] );
                            break;
                        case "position":
                            Dictionary<string, string> posval = GetKeyValues(keyval[key]);
                            //block.position = new Dictionary<string, float>() {
                            //    { "x", float.Parse(posval["x"]) },
                            //    { "y", float.Parse(posval["y"]) }
                            //};
                            block.position = new Vector3(float.Parse(posval["x"]), float.Parse(posval["z"]), float.Parse(posval["y"]));
                            break;
                        case "scale":
                            Dictionary<string, string> scval = GetKeyValues(keyval[key]);
                            //block.position = new Dictionary<string, float>() {
                            //    { "x", float.Parse(posval["x"]) },
                            //    { "y", float.Parse(posval["y"]) }
                            //};
                            block.scale = new Vector3(float.Parse(scval["x"]), float.Parse(scval["z"]), float.Parse(scval["y"]));
                            break;
                    }
                }

                if (mainobj != null) mainobj.blocks.Add(block.id, block);
                //depth++;
                continue;
            }

            


        }

        Debug.Log("Done");

        return data;

        // -- Dictionary<string, mainobj> data = new Dictionary<string, mainobj>();
        //mainobj mainobj = new mainobj();
        //mainobj.dimensions = new dimension();
        //mainobj.blocks = new Dictionary<string, block>();
        //block block = new block() {
        //    id = "Anti-theftDoor_9qs8y9 (1)",
        //    position = new Vector2(),
        //};
        //mainobj.blocks.Add(block.id, block);
        //data.Add("Anti-theftDoor_9qs8y9", mainobj);



        /*

        "Anti-theftDoor_9qs8y9" : {
            "dimensions": { "width": 0.3811517, "length": 0.09065947, "pivot": {"x": 6.705523E-08, "y": 0} },
            "blocks":{
                "Anti-theftDoor_9qs8y9 (1)": {"id": "Anti-theftDoor_9qs8y9 (1)", "asset": "Anti-theftDoor_9qs8y9", "position": {"x": 2.6, "y": -1.4},"rotation": 0},
                "Anti-theftDoor_9qs8y9 (2)": {"id": "Anti-theftDoor_9qs8y9 (2)", "asset": "Anti-theftDoor_9qs8y9", "position": {"x": 2.55, "y": 1.4},"rotation": 0},
                "Anti-theftDoor_9qs8y9 (3)": {"id": "Anti-theftDoor_9qs8y9 (3)", "asset": "Anti-theftDoor_9qs8y9", "position": {"x": 2.55, "y": -4.5},"rotation": 0},
                "Anti-theftDoor_9qs8y9 (4)": {"id": "Anti-theftDoor_9qs8y9 (4)", "asset": "Anti-theftDoor_9qs8y9", "position": {"x": 2.55, "y": -7.5},"rotation": 0}
            }
        },

        */

    }

    static GameObject AddAsset(block block)
    {
        //var newLink = EditorUtility.InstantiatePrefab(block.) as GameObject;
        //newLink.transform.position = target.transform.position;

        Debug.Log("ADD ASSET:");
        Debug.Log(block.id);

        //PrefabUtility.LoadPrefabContentsIntoPreviewScene("ModernSupermarket/Prefabs/Furniture/Checkout_gueqmj", )

        //GameObject myPrefab = GetComponent<block.asset>;

        //PrefabUtility.InstantiatePrefab();

        // GameObject c = (GameObject)Resources.Load("ModernSupermarket/Prefabs/Furniture/" + block.asset, typeof(GameObject));

        // Assets/ModernSupermarket/Prefabs/Furniture/Anti-theftDoor_9qs8y9.prefab

        Object Obj = AssetDatabase.LoadAssetAtPath($"Assets/ModernSupermarket/Prefabs/Furniture/{block.asset}.prefab", typeof(Object));

        if (Obj == null) { return null; }

        GameObject par = GameObject.Find("Fixtures");
        if (par == null) par = new GameObject("Fixtures");

        GameObject c = Obj as GameObject;
        c = PrefabUtility.InstantiatePrefab(c, par.transform) as GameObject;
        Debug.Log("Added:");
        Debug.Log(block.id);
        c.name = block.id;
        c.transform.position = block.position;
        c.transform.rotation = Quaternion.Euler(0, block.rotation, 0);
        c.transform.localScale = block.scale;

        return c;
    }


    public class ress
    {
        public Bounds bounds;
        public Vector3 offset;
    }

    public static ress CalculateBounds(GameObject d)
    {

        ress res = new ress();

        Quaternion q = d.transform.rotation;
        d.transform.rotation = Quaternion.identity;

        Vector3 t = d.transform.position;
        d.transform.position = new Vector3();

        Bounds bounds = new Bounds();
        bounds.size = Vector3.zero; // reset

        Renderer rend = d.GetComponent<Renderer>();
        if (rend != null) bounds.Encapsulate(rend.bounds);

        foreach (Renderer r in d.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(r.bounds);
        }

        res.offset = d.transform.position - bounds.center;
        res.bounds = bounds;

        //Collider col1 = d.GetComponent<Collider>();
        //if (col1 != null) bounds.Encapsulate(col1.bounds);

        //Collider[] colliders = d.GetComponentsInChildren<Collider2D>();
        //foreach (Collider col in colliders)
        //{
        //    bounds.Encapsulate(col.bounds);
        //}

        d.transform.position = t;
        d.transform.rotation = q;

        return res;
    }


    // Update is called once per frame
    void Update()
    {
        if (OutFileStatic != DataFile) OutFileStatic = DataFile;

        if (autoRefresh)
        {
            RefreshFromBlockData();

            //foreach (GameObject obj in allObjects)
            //{

            //if (obj.name == "Neon_dwd6wi(5)")
            //{
            //Debug.Log("HI");
            //}

            //Renderer rr = obj.GetComponent<Renderer>();
            //if (rr == null) continue;

            //Selection.activeGameObject = obj;

            //Debug.Log(obj.name);
            //rr.staticShadowCaster = false;
            // break;



            //}

            // refresh = false;
        }


    }
}

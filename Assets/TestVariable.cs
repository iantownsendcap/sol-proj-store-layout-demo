using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]

public class TestVariable : MonoBehaviour
{
    //public GameObject ngo;
    public GameObject PrefabObj;

    public int n = 0;

    GameObject[] newobj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (ngo == null)
        //{
        //    ngo = new GameObject("MyModularGenerated");
        //}
        //ngo = ngo.transform.gameObject;
        if (PrefabObj == null) return;

        List<GameObject> tmp = new List<GameObject>();
        foreach(GameObject obj in newobj)
        {
            if (!obj.Equals(null)) tmp.Add(obj);
        }
        newobj = tmp.ToArray();

        for (int i = 0; i < 100; i++) {
            if (newobj.Length > n) break;
            List<GameObject> stuff = newobj.ToList();
            GameObject gg = PrefabUtility.InstantiatePrefab(PrefabObj) as GameObject;
            stuff.Add(gg);
            gg.transform.SetParent(transform);
            newobj = stuff.ToArray();
        }

        for (int i = 0; i < 100; i++)
        {
            if (newobj.Length <= n) break;
            List<GameObject> stuff = newobj.ToList();
            DestroyImmediate(stuff[stuff.Count - 1]);
            newobj = stuff.ToArray();
        }

        for (int i = 0; i < newobj.Length; i++)
        {
            newobj[i].transform.position = Vector3.left * i * 2;
        }
    }
}

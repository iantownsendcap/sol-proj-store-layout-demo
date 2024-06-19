using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;


[ExecuteInEditMode]

public class boxLine : MonoBehaviour
{
    public bool refresh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        if (refresh) { refresh = false; }
        else { return; }

        LineRenderer lineRenderer = null;
        if (!TryGetComponent<LineRenderer>(out lineRenderer)) { lineRenderer = gameObject.AddComponent<LineRenderer>(); }

        // ---

        List<GameObject> gos = GetAllObjectsOnlyInScene();
        
        // ---

        List<UnityEngine.Vector3> points = new List<UnityEngine.Vector3>();
        List<float> linedims = new List<float>();

        foreach (GameObject go in gos)
        {
            // skip this object
            if (go.Equals(gameObject)) continue;

            // GameObject product = AssetDatabase.LoadAssetAtPath(p, typeof(GameObject)) as GameObject;
            //if (product == null) continue;
            Renderer rend = go.GetComponent<Renderer>();
            if (rend == null || rend.bounds == null) continue;
            // if (UnityEngine.Vector3.Distance(dims, UnityEngine.Vector3.zero) < 2) { dims *= 100f; }

            //UnityEngine.Vector3[] ps = new UnityEngine.Vector3[4];
            //ps[0] = rend.bounds.size/2f;
            //ps[1] = rend.bounds.size/2f;
            //ps[2] = rend.bounds.size/2f;
            //ps[3] = rend.bounds.size/2f;
            //Bounds bounds = GeometryUtility.CalculateBounds(ps, transform.localToWorldMatrix);

            UnityEngine.Vector3 p = go.transform.InverseTransformPoint(rend.bounds.size/2f);
            UnityEngine.Vector3 c = rend.bounds.center;

            UnityEngine.Vector3 t = p;
            t.y = -1;
            points.Add(t);

            points.Add(c);
            points.Add(c + p);

            p = c + p;
            p.y = -1;
            points.Add(p);

        }

        //lineRenderer.widthCurve = 
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

    }


    public Bounds CalculateBounds(GameObject obj)
    {
        Bounds bounds = new Bounds();
        
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            bounds.Encapsulate(col.bounds);
        }

        return bounds;
    }




    List<GameObject> GetAllObjectsOnlyInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }
        Debug.Log($"Found: {objectsInScene.Count}");
        return objectsInScene;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]

[RequireComponent(typeof(LineRenderer))]

public class BoundingBox : MonoBehaviour
{
    const float inch_to_meter = 0.0254f;
    [HideInInspector] public Vector3 Dimensions = Vector3.zero;
    [HideInInspector] public Vector3 pos = Vector3.zero;
    [HideInInspector] public Quaternion rot = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnValidate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Redraw();
    }

    private void Redraw()
    {
        
        MyModular mymod = GetComponentInParent<MyModular>();
        if (mymod == null) return;

        LineRenderer lr = transform.GetComponent<LineRenderer>();
        if (lr == null) return;

        List<Vector3> poss = new List<Vector3>();

        if (mymod.Dimensions == null) { return; }

        if (Dimensions.Equals(mymod.Dimensions) && pos.Equals(transform.position) && rot.Equals(transform.rotation)) return;
        Dimensions = mymod.Dimensions;
        pos = transform.position;
        rot = transform.rotation;

        Vector3 imdims = mymod.Dimensions * inch_to_meter;

        Vector3[] c = new Vector3[8] {
            new Vector3(0,0,0),
            new Vector3(0,imdims.y,0),
            new Vector3(imdims.x,imdims.y,0),
            new Vector3(imdims.x,0,0),
            new Vector3(0,0,imdims.z),
            new Vector3(0,imdims.y,imdims.z),
            new Vector3(imdims.x,imdims.y,imdims.z),
            new Vector3(imdims.x,0,imdims.z),
        };

        poss.Add(transform.TransformPoint(c[0]));
        poss.Add(transform.TransformPoint(c[1]));
        poss.Add(transform.TransformPoint(c[2]));
        poss.Add(transform.TransformPoint(c[3]));
        poss.Add(transform.TransformPoint(c[0]));

        poss.Add(transform.TransformPoint(c[4]));
        poss.Add(transform.TransformPoint(c[7]));
        poss.Add(transform.TransformPoint(c[4]));

        poss.Add(transform.TransformPoint(c[5]));
        poss.Add(transform.TransformPoint(c[1]));
        poss.Add(transform.TransformPoint(c[5]));
        poss.Add(transform.TransformPoint(c[6]));
        poss.Add(transform.TransformPoint(c[2]));
        poss.Add(transform.TransformPoint(c[6]));
        poss.Add(transform.TransformPoint(c[7]));
        poss.Add(transform.TransformPoint(c[3]));

        lr.startWidth = lr.endWidth = 0.015f;
        lr.positionCount = 0;
        lr.positionCount = poss.Count;
        lr.SetPositions(poss.ToArray());
    }
}

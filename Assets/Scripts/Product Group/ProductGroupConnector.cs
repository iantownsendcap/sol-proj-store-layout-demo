using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ProductArray))]
[RequireComponent(typeof(DragVolume))]
[ExecuteInEditMode]

public class ProductGroupConnector : MonoBehaviour
{
    public ProductArray pg;
    public DragVolume dv;

    // [HideInInspector]
    // public Object[] selectedobjs;

    // public LayerMask layerMask;

    public ShelfGroup shelfGroup;

    // Start is called before the first frame update
    void Start()
    {
        pg = GetComponent<ProductArray>();
        dv = GetComponent<DragVolume>();
    }

    //static void SetAllRefreshOff()
    //{
    //    foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
    //    {
    //        ProductGroupConnector f = go.GetComponent<ProductGroupConnector>();
    //        if (f != null)
    //        {
    //            ProductArray pa = f.GetComponent<ProductArray>();
    //            if (pa != null)
    //            {
    //                pa.refresh = false;
    //            }
    //        }
    //    }
    //}

    //// enable disable when selected
    //void EnableDisableOnSelection()
    //{
    //    if (selectedobjs == null) selectedobjs = Selection.objects;
    //    List<Object> objs = new List<Object>(Selection.objects);

    //    ProductArray pa = GetComponent<ProductArray>();
    //    if (pa == null) return;

    //    if (objs.Contains(gameObject))
    //    {
    //        SetAllRefreshOff();
    //        pa.refresh = true;
    //    }
    //    else
    //    {
    //        pa.refresh = false;
    //    }
        
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = new Color(1, 0, 0, 0.5f);
    //    Vector3 p = transform.TransformDirection(Vector3.forward);
    //    Vector3 pp = transform.localScale / 2;
    //    p.Scale(pp);
    //    Gizmos.DrawCube(transform.position + (p), transform.localScale * 0.5f);
    //}

    //void FindNeighbors()
    //{
    //    // Bit shift the index of the layer (8) to get a bit mask
    //    // int layerMask = 1 << 12;

    //    // This would cast rays only against colliders in layer 8.
    //    // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
    //    // layerMask = ~layerMask;


    //    //Vector3 t = dv.endNodeDepth.position;
    //    //Vector3 t2 = dv.endNodeWidthHeight.position;

    //    //Vector3 p = transform.TransformDirection(Vector3.forward);
    //    //Vector3 pp = transform.localScale / 2;
    //    //p.Scale(pp);



    //    //Collider[] colls = Physics.OverlapBox(transform.position, new Vector3(t2.x, t2.y, t.z), transform.rotation, layerMask.value);
    //    //foreach(Collider c in colls)
    //    //{
    //    //    Transform tr = c.transform;
    //    //    if (tr == null) continue;
    //    //    tr = tr.parent;
    //    //    if (tr == null) continue;
    //    //    tr = tr.parent;
    //    //    if (tr == null) continue;
    //    //    if (transform == tr) continue;

    //    //    Debug.Log(gameObject.name);
    //    //}


    //    //// Bit shift the index of the layer (8) to get a bit mask
    //    //int layerMask = 1 << 8;

    //    //// This would cast rays only against colliders in layer 8.
    //    //// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
    //    //layerMask = ~layerMask;

    //    //RaycastHit hit;
    //    //// Does the ray intersect any objects excluding the player layer

    //    //Vector3 OffsetRay = new Vector3(0.03f, 0.03f, 0.03f);
    //    //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 2, layerMask.value))
    //    //{
    //    //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
    //    //    Debug.Log("Did Hit");
    //    //}
    //    //else
    //    //{
    //    //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 2, Color.white);
    //    //    Debug.Log("Did not Hit");
    //    //}
    //}


    // Update is called once per frame
    
    void Update()
    {

        if (pg == null) return;
        if (dv == null) return;
        //Debug.Log(dv.boundry.localScale.x / 2f / (pg.Space.x * 0.01f));
        // 

        // if (pg.Space.x == 0 || pg.Space.y == 0 || pg.Space.z == 0) { pg.Grid = Vector3Int.zero; return; }


        pg.Grid.x = Mathf.FloorToInt(dv.boundry.localScale.x / (pg.Space.x * 0.01f));
        pg.Grid.y = Mathf.FloorToInt(dv.boundry.localScale.y / (pg.Space.y * 0.01f));
        pg.Grid.z = Mathf.FloorToInt(dv.boundry.localScale.z / (pg.Space.z * 0.01f));

        int max = 30;
        pg.Grid.x = pg.Grid.x > max ? max : pg.Grid.x;
        pg.Grid.y = pg.Grid.y > max ? max : pg.Grid.y;
        pg.Grid.z = pg.Grid.z > max ? max : pg.Grid.z;

        pg.Grid.x = pg.Grid.x < 1 ? 1 : pg.Grid.x;
        pg.Grid.y = pg.Grid.y < 1 ? 1 : pg.Grid.y;
        pg.Grid.z = pg.Grid.z < 1 ? 1 : pg.Grid.z;

        // EnableDisableOnSelection();
    }
}

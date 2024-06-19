using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]

public class SelectParent : MonoBehaviour
{
    public GameObject target;
    public bool Enable = false;

    Object[] selectedobjs;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // return;

        if (!Enable) return;
        if (target == null) return;

        if (selectedobjs == null) selectedobjs = Selection.objects;

        List<Object> objs = new List<Object>(Selection.objects);
        if (objs.Contains(gameObject))
        {
            //if (target == null) { target = transform.parent.gameObject; }
            Selection.objects = new Object[1] { target };
        }

    }
}

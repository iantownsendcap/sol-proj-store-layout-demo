using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[ExecuteInEditMode]

public class ModIcons : MonoBehaviour
{
    private void Update()
    {
        if (Selection.objects.Length != 1) return;
        foreach (Object obj in Selection.objects)
        {
            if (obj.Equals(gameObject))
            {
                OnSelected();
                break;
            }
        }
    }

    public void OnSelected()
    {
        string iconSel = gameObject.name;
        Debug.Log(iconSel);


        // ShelfIndex = 0;
        switch (iconSel)
        {
            case "Add":
                MyModular mm = gameObject.GetComponentInParent<MyModular>();
                mm.AddShelf();
                break;



            default:
                break;
        }

    }

}

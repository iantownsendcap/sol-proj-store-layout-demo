using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[ExecuteInEditMode]

public class ProductIcons : MonoBehaviour
{

    // public ShelfGroup Shelf;
    // public DragVolume DV;
    // public int ShelfIndex = -1;


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
        ShelfGroup Shelf = null;
        DragVolume DV = null;
        //int ShelfIndex = -1;

        ShelfGroup[] ShelfGroups = transform.GetComponentsInParent<ShelfGroup>(); 
        if (ShelfGroups.Length > 0) Shelf = ShelfGroups[0];
        
        DragVolume[] DVs = transform.GetComponentsInParent<DragVolume>(); 
        if (DVs.Length > 0) DV = DVs[0];
        if (DV != null && Shelf == null) { Shelf = DV.Shelf; }
        //if (DV != null) ShelfIndex = DV.ShelfIndex;

        // if (ShelfIndex)
        // make sure the prefab version doesn't work

        string iconSel = gameObject.name;
        Debug.Log(iconSel);


        if (Shelf == null) return;


        //List<GameObject> pgs = Shelf.ProductGroups.ToList();
        // int ind = pgs.IndexOf(gameObject);

        GameObject[] newobject = null;
        // ShelfIndex = 0;
        switch (iconSel)
        {
            case "Add":
                //pgs.Insert(ShelfIndex+1, null);
                //Shelf.ProductGroups = pgs;
                newobject = Shelf.Make();
                Selection.objects = newobject;
                break;


            case "Delete":
                Selection.objects = new Object[1] { Shelf.gameObject };
                //pgs.RemoveAt(ShelfIndex);
                //Shelf.ProductGroups = pgs;
                DestroyImmediate(DV.gameObject);
                //Shelf.Delete();
                break;


            case "MoveLeft":
                Selection.objects = new Object[1] { DV.gameObject };
                //if (ShelfIndex > 0)
                //{
                    //GameObject g = pgs[ShelfIndex];
                    //pgs[ShelfIndex] = pgs[ShelfIndex - 1];
                    //pgs[ShelfIndex - 1] = g;
                    //ShelfIndex -= 1;
                    //Shelf.ProductGroups = pgs;
                    Shelf.MoveLeft(DV.transform);
                //}
                //else Debug.Log("Can't move");
                break;


            case "MoveRight":
                Selection.objects = new Object[1] { DV.gameObject };
                //if (ShelfIndex < Shelf.ProductGroups.Count - 1)
                //{
                    //GameObject g = pgs[ShelfIndex];
                    //pgs[ShelfIndex] = pgs[ShelfIndex + 1];
                    //pgs[ShelfIndex + 1] = g;
                    //ShelfIndex += 1;
                    //Shelf.ProductGroups = pgs;
                    Shelf.MoveRight(DV.transform);
                //}
                //else Debug.Log("Can't move");

                break;


            default:
                break;
        }

    }

}

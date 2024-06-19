using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

// [CanEditMultipleObjects]
public class ProductSearchWindow : EditorWindow
{
    bool refresh = true;

    public static Object[] ObjectList;

    static public ProductSearchObj productSearch;
    //[System.Serializable]
    public class ProductSearchObj
    {
        public string Keywords = "";
        public string UPC = "";
        public ModularMenu.Departments Department;
    }

    Vector2 scrollPos = new Vector2();
    [MenuItem("MyHelperTools/Modulars/Product Search Tool")]
    static void Init()
    {
        productSearch = new ProductSearchObj();
        var window = GetWindowWithRect<global::ProductSearchWindow>(new Rect(0, 0, 400, 300));
        window.Show();
    }

    void OnGUI()
    {
        if (!refresh) return;

        if (productSearch == null) productSearch = new ProductSearchObj();

        Rect r = EditorGUILayout.BeginVertical(); // 0 the main list of controls

        // EditorGUILayout.PropertyField(SerializedObject.FindProperty("productSearch"))


        productSearch.Keywords = EditorGUILayout.TextField("Keywords", productSearch.Keywords).ToLower();

        productSearch.UPC = EditorGUILayout.TextField("UPC", productSearch.UPC);
       
        productSearch.Department = (ModularMenu.Departments)EditorGUILayout.EnumPopup("Department", productSearch.Department);

        if (GUILayout.Button("Search"))
        {
            if (productSearch.Keywords == null)
            {
                ShowNotification(new GUIContent("No object selected for searching"));
            }
            else
            {
                // look up the keywords

                ModularMenu.ProductDataLine[] pdls = ModularMenu.FindProdDataLines();
                ObjectList = ModularMenu.MatchingObjectsFromLines(pdls);
                refresh = true;
            }
        }

        int res = 0;
        if (ObjectList != null)
        {
            res = ObjectList.Length;
        }
        EditorGUILayout.LabelField($"Result: {res}");
        
        var errorStyleText = new GUIStyle
        {
            normal =
            {
                textColor = Color.red
            }
        };

        if (ObjectList != null)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < ObjectList.Length; i++)
            {
                if (ObjectList[i] == null)
                {
                    EditorGUILayout.LabelField($"Error finding object {i}", errorStyleText);
                }
                else
                {
                    EditorGUILayout.ObjectField(ObjectList[i].name, ObjectList[i],
                        ObjectList[i].GetType(), false);
                }
            }

            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.EndVertical(); // 0

        // ---

        //if (r.Contains(Event.current.mousePosition))
        //{
            //switch (Event.current.type)
            //{
            //    case EventType.MouseDown:
            //        Event.current.Use();
            //        break;
            //    case EventType.DragPerform:
            //        Event.current.Use();
            //        break;
            //    case EventType.DragUpdated:
            //        Event.current.Use();
            //        break;
            //    case EventType.DragExited:
            //        Event.current.Use();
            //        break;

            //    default: break;
            //}
        //}

    }
}

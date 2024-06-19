//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class ShelfEditorWindow : EditorWindow
//{
//    private static Object previousObj;

//    // Start is called before the first frame update
//    [MenuItem("MyHelperTools/Modulars/Shelf Editor Tool")]
//    static void Init()
//    {
//        previousObj = null;
//        var window = GetWindowWithRect<global::ShelfEditorWindow>(new Rect(0, 0, 400, 300));
//        window.Show();
//    }


//    private void OnGUI()
//    {
//        EditorGUILayout.BeginVertical(); // 0 the main list of controls

//        EditorGUILayout.LabelField("filename", "HI");
//        EditorGUILayout.LabelField("path", "HO");
//    }
//}

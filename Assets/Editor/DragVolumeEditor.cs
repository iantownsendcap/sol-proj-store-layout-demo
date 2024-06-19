using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

[CustomEditor(typeof(DragVolume))]

public class DragVolumeEditor : Editor
{
    DragVolume dragvolume;
    private bool shapeChangedSinceLastRepaint;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnEnable()
    {
        //Tools.hidden = true;
        dragvolume = target as DragVolume;

        // Have the renderer enable when selected
        Renderer rend = dragvolume.boundry.transform.GetComponentInChildren<Renderer>();
        if (rend != null) {
            rend.enabled = true;
            // rend[1].enabled = DragVolume.Heatmap.Enabled;
        }

        if (dragvolume.iconsgroup != null)
        {
            Renderer[] icons = dragvolume.iconsgroup.transform.GetComponentsInChildren<Renderer>();
            if (icons != null) foreach (Renderer renderer in icons) { renderer.enabled = true; }
            //ISetHighlight
            //UI.Selectable.IsHighlighted;
        }

        //SceneView.duringSceneGui += SceneView_duringSceneGui;
        //SceneView.onSceneGUIDelegate += (SceneView.OnSceneFunc)Delegate.Combine(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(CustomOnSceneGUI));

    }

    //private void SceneView_duringSceneGui(SceneView obj)
    //{
    //    DragVolume dragvolume = (DragVolume)target;

    //    Handles.color = Color.white;
    //    //Handles.FreeMoveHandle(dragvolume.transform.position, dragvolume.handlesize * 0.5F, snap, Handles.SphereHandleCap);

    //    //Handles.CapFunction rightFunc = (id, position, rotation, size, type) => Handles.CubeHandleCap(0, dragvolume.transform.position, Quaternion.identity, dragvolume.handlesize * 0.5f, EventType.Repaint);
    //    //Handles.Button(dragvolume.transform.position, Quaternion.identity, dragvolume.handlesize * 0.5f, dragvolume.handlesize, rightFunc);
    //    // Handles.Label(dragvolume.transform.position, "TEST");
    //    //int bb;
    //    //HandleUtility.PickGameObject(dragvolume.transform.position, out bb);
    //    // Selection.objects = new GameObject[1] { dragvolume.transform.gameObject };
    //}

    private void OnDisable()
    {
        // Have the renderer hidden when not selected
        Renderer rend = dragvolume.boundry.transform.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            if (!HeatMappingWindow.HeatMapSettings.price_enable && !HeatMappingWindow.HeatMapSettings.sales_enable && !HeatMappingWindow.HeatMapSettings.shrink_enable) { 
                rend.enabled = false; }
        }


        if (dragvolume.iconsgroup != null)
        {
            Renderer[] icons = dragvolume.iconsgroup.transform.GetComponentsInChildren<Renderer>();
            if (icons != null) foreach (Renderer renderer in icons) { renderer.enabled = false; }
        }
        //Tools.hidden = false;
    }






    private void OnSceneGUI()
    {

        Event guiEvent = Event.current;

        dragvolume = (DragVolume)target;
        Vector3 snap = Vector3.one * .5f;
        //float size = HandleUtility.GetHandleSize(dragvolume.worldspace) * dragvolume.handlesize;

        EditorGUI.BeginChangeCheck();

        //dragvolume.worldspace
        Handles.color = Color.blue;
        Vector3 a = dragvolume.transform.TransformPoint(dragvolume.endNodeWidthHeight.localPosition);
        Vector3 newWorldSpaceA = Handles.FreeMoveHandle(a, dragvolume.handlesize, snap, Handles.SphereHandleCap);

        Vector3 b = dragvolume.transform.TransformPoint(dragvolume.endNodeDepth.localPosition);
        Vector3 newWorldSpaceB = Handles.FreeMoveHandle(b, dragvolume.handlesize, snap, Handles.SphereHandleCap);

        //Vector3 sz = new Vector3(dragvolume.endNodeWidthHeight.localPosition.x, dragvolume.endNodeWidthHeight.localPosition.y, dragvolume.endNodeDepth.localPosition.z);
        //Handles.color = Color.yellow;
        //Handles.DrawWireCube(dragvolume.transform.position, sz);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(dragvolume, "Change Target Position");

            //

            dragvolume.endNodeWidthHeight.position = newWorldSpaceA;
            dragvolume.endNodeWidthHeight.localPosition = Vector3.Scale(dragvolume.endNodeWidthHeight.localPosition, new Vector3(
                (dragvolume.endNodeWidthHeight.localPosition.x < 0) ? 0 : 1,
                (dragvolume.endNodeWidthHeight.localPosition.y < 0) ? 0 : 1,
                0)
                );

            dragvolume.endNodeDepth.position = newWorldSpaceB;
            dragvolume.endNodeDepth.localPosition = Vector3.Scale(dragvolume.endNodeDepth.localPosition, new Vector3(
                0,
                0,
                (dragvolume.endNodeDepth.localPosition.z < 0) ? 0 : 1)
                );


            //Vector3 sz = dragvolume.transform.TransformPoint();


            //g = newWorldSpace;

            dragvolume.Refresh();
        }
    }
}
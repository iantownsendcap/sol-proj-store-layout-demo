using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sebastian.Geometry;

[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator shapeCreator;
    bool shapeChangedSinceLastRepaint;
    SelectionInfo selectionInfo;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string helpmessage = "Left click to add a point.\nShift Left click to delete a point.\nShift-left click an empty space to start a new shape.";
        EditorGUILayout.HelpBox(helpmessage, MessageType.Info);

        int deletebutton = -1;
        shapeCreator.showShapeslist = EditorGUILayout.Foldout(shapeCreator.showShapeslist, "Shapes List");
        if (shapeCreator.showShapeslist)
        {
            for (int i = 0; i < shapeCreator.shapes.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Shapes: " + (i + 1));

                GUI.enabled = i != selectionInfo.selectedShapeindex;
                if (GUILayout.Button("select"))
                {
                    selectionInfo.selectedShapeindex = i;
                }
                GUI.enabled = true;

                if (GUILayout.Button("delete"))
                {
                    deletebutton = i;
                }
                GUILayout.EndHorizontal();
            }
        }

        if (deletebutton != -1)
        {
            Undo.RecordObject(shapeCreator, "Delete Shape");
            shapeCreator.shapes.RemoveAt(deletebutton);
            selectionInfo.selectedShapeindex = -1;
        }

        if (GUI.changed)
        {
            shapeChangedSinceLastRepaint = true;
            SceneView.RepaintAll();
        }
    }


    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        else if (guiEvent.type == EventType.Layout)
        {
            // disable the selection change wehn left click
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            HandleInput(guiEvent);
            if (shapeChangedSinceLastRepaint)
            {
                HandleUtility.Repaint();
            }
        }
    }

    void CreateNewShape()
    {
        Undo.RecordObject(shapeCreator, "Create Shape");
        shapeCreator.shapes.Add(new Shape());
        selectionInfo.selectedShapeindex = shapeCreator.shapes.Count - 1;
    }

    void CreateNewPoint(Vector3 position)
    {
        bool mouseisoverSelectedShape = selectionInfo.selectedShapeindex == selectionInfo.mouseOverShapeIndex;
        int newPointIndex = (selectionInfo.mouseIsOverLine && mouseisoverSelectedShape) ? selectionInfo.lineindex + 1 : SelectedShape.points.Count;
        Undo.RecordObject(shapeCreator, "Add Point");
        SelectedShape.points.Insert(newPointIndex, position);
        selectionInfo.pointIndex = newPointIndex;
        selectionInfo.mouseOverShapeIndex = selectionInfo.selectedShapeindex;
        shapeChangedSinceLastRepaint = true;

        SelectPointUnderMouse();
    }

    void DeletePointUndermouse()
    {
        Undo.RecordObject(shapeCreator, "Delete Point");
        SelectedShape.points.RemoveAt(selectionInfo.pointIndex);
        selectionInfo.pointIsSelected = false;
        selectionInfo.mouseIsOverPoint = false;
        shapeChangedSinceLastRepaint = true;
    }

    void SelectPointUnderMouse()
    {
        selectionInfo.pointIsSelected = true;
        selectionInfo.mouseIsOverPoint = true;
        selectionInfo.mouseIsOverLine = false;
        selectionInfo.lineindex = -1;

        selectionInfo.positionAtStartOfDrag = SelectedShape.points[selectionInfo.pointIndex];
        shapeChangedSinceLastRepaint = true;
    }

    void SelectShapeUnderMouse()
    {
        if (selectionInfo.mouseOverShapeIndex != -1)
        {
            selectionInfo.selectedShapeindex = selectionInfo.mouseOverShapeIndex;
            shapeChangedSinceLastRepaint = true;
        }
    }

    void HandleInput(Event guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane); // mouseRay.origin + mouseRay.direction * dstToDrawPlane;


        if (guiEvent.button == 0)
        {
            if (guiEvent.modifiers == EventModifiers.None)
            {
                if (guiEvent.type == EventType.MouseDown)
                {
                    HandleLeftMouseDown(mousePosition);
                }
                if (guiEvent.type == EventType.MouseDrag)
                {
                    HandleLeftMouseDrag(mousePosition);
                }
            }

            if (guiEvent.modifiers == EventModifiers.Shift)
            {
                if (guiEvent.type == EventType.MouseDown)
                {
                    HandleShiftLeftMouseDown(mousePosition);
                }
            }

            if (guiEvent.type == EventType.MouseUp)
            {
                HandleLeftMouseUp(mousePosition);
            }
        }


        if (!selectionInfo.pointIsSelected)
        {
            UpdateMouseOverInfo(mousePosition);
        }
    }


    void HandleShiftLeftMouseDown(Vector3 mousePosition)
    {
        if (selectionInfo.mouseIsOverPoint)
        {
            SelectShapeUnderMouse();
            DeletePointUndermouse();
        }
        else {
            CreateNewShape();
            CreateNewPoint(mousePosition);
        }
    }

    void HandleLeftMouseDown(Vector3 mousePosition)
    {
        if (shapeCreator.shapes.Count == 0)
        {
            CreateNewShape();
        }

        SelectShapeUnderMouse();

        if (selectionInfo.mouseIsOverPoint)
        {
            SelectPointUnderMouse();
        }
        else
        {
            CreateNewPoint(mousePosition);
        }

    }
    void HandleLeftMouseUp(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            SelectedShape.points[selectionInfo.pointIndex] = selectionInfo.positionAtStartOfDrag;
            Undo.RecordObject(shapeCreator, "Point Moved");
            SelectedShape.points[selectionInfo.pointIndex] = mousePosition;

            selectionInfo.pointIndex = -1;
            selectionInfo.pointIsSelected = false;
            shapeChangedSinceLastRepaint = true;
        }
    }
    void HandleLeftMouseDrag(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            SelectedShape.points[selectionInfo.pointIndex] = mousePosition;
            shapeChangedSinceLastRepaint = true;
        }
    }


    void UpdateMouseOverInfo(Vector3 mousePosition)
    {
        int mouseOverPointIndex = -1;
        int mouseOverShapeIndex = -1;

        for (int shapeIndex = 0; shapeIndex < shapeCreator.shapes.Count; shapeIndex++)
        {
            Shape currentShape = shapeCreator.shapes[shapeIndex];
            

            for (int i = 0; i < currentShape.points.Count; i++)
            {
                if (Vector3.Distance(mousePosition, currentShape.points[i]) < shapeCreator.handleRadius)
                {
                    mouseOverPointIndex = i;
                    mouseOverShapeIndex = shapeIndex;
                    break;
                }
            }
        }

        if (selectionInfo.pointIndex != mouseOverPointIndex || selectionInfo.mouseOverShapeIndex != mouseOverShapeIndex)
        {
            selectionInfo.mouseOverShapeIndex = mouseOverShapeIndex;
            selectionInfo.pointIndex = mouseOverPointIndex;
            selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

            shapeChangedSinceLastRepaint = true;
        }

        if (selectionInfo.mouseIsOverPoint)
        {
            selectionInfo.mouseIsOverLine = false;
            selectionInfo.lineindex = -1;
        }
        else
        {
            int mouseOveLineIndex = -1;
            for (int shapeIndex = 0; shapeIndex < shapeCreator.shapes.Count; shapeIndex++)
            {
                Shape currentShape = shapeCreator.shapes[shapeIndex];

                float closest = shapeCreator.handleRadius;
                for (int i = 0; i < currentShape.points.Count; i++)
                {
                    Vector3 nextPoint = currentShape.points[(i + 1) % currentShape.points.Count];
                    float dst = HandleUtility.DistancePointToLineSegment(mousePosition.ToXZ(), currentShape.points[i].ToXZ(), nextPoint.ToXZ());
                    if (dst < closest)
                    {
                        dst = closest;
                        mouseOveLineIndex = i;
                        mouseOverShapeIndex = shapeIndex;
                    }
                }

            }

            if (selectionInfo.lineindex != mouseOveLineIndex || selectionInfo.mouseOverShapeIndex != mouseOverShapeIndex)
            {
                selectionInfo.mouseOverShapeIndex = mouseOverShapeIndex;
                selectionInfo.lineindex = mouseOveLineIndex;
                selectionInfo.mouseIsOverLine = mouseOveLineIndex != -1;
                shapeChangedSinceLastRepaint = true;
            }
        }

    }

    void Draw()
    {
        for (int shapeIndex = 0; shapeIndex < shapeCreator.shapes.Count; shapeIndex++)
        {
            Shape currentShape = shapeCreator.shapes[shapeIndex];
            bool shapeIsSelected = shapeIndex == selectionInfo.selectedShapeindex;
            bool mouseIsOverShape = shapeIndex == selectionInfo.mouseOverShapeIndex;

            Color deselectedColor = Color.grey;

            for (int i = 0; i < currentShape.points.Count; i++)
            {
                // Handles.FreeMoveHandle(new Vector3(), Quaternion.identity, 0.5f, Vector3.zero, Handles.SphereHandleCap);

                Vector3 nextPoint = currentShape.points[(i + 1) % currentShape.points.Count];
                
                if (selectionInfo.lineindex == i && mouseIsOverShape)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(currentShape.points[i], nextPoint, shapeCreator.lineRadius);
                }
                else
                {
                    Handles.color = (shapeIsSelected)?Color.black: deselectedColor;
                    Handles.DrawDottedLine(currentShape.points[i], nextPoint, shapeCreator.lineRadius);
                }

                if (selectionInfo.pointIndex == i && mouseIsOverShape)
                {
                    Handles.color = (selectionInfo.pointIsSelected) ? Color.black : Color.red;
                }
                else
                {
                    Handles.color = (shapeIsSelected)? Color.white: deselectedColor;
                }
                Handles.DrawSolidDisc(currentShape.points[i], Vector3.up, shapeCreator.handleRadius);
            }
        }

        if (shapeChangedSinceLastRepaint)
        {
            shapeCreator.UpdateMeshDisplay();
        }

        shapeChangedSinceLastRepaint = false;
    }

    private void OnEnable()
    {
        shapeChangedSinceLastRepaint = true;
        // when the object is selected
        shapeCreator = target as ShapeCreator;
        selectionInfo = new SelectionInfo();
        Undo.undoRedoPerformed += OnUndoOrRedo;
        Tools.hidden = true; // hide the transform
    }

    private void OnDisable()
    {
        // when the object is deselected
        Undo.undoRedoPerformed -= OnUndoOrRedo;
        Tools.hidden = false;
    }

    void OnUndoOrRedo()
    {
        if (selectionInfo.selectedShapeindex >= shapeCreator.shapes.Count)
        {
            selectionInfo.selectedShapeindex = shapeCreator.shapes.Count - 1;
        }
        shapeChangedSinceLastRepaint = true;
    }

    Shape SelectedShape
    {
        get
        {
            return shapeCreator.shapes[selectionInfo.selectedShapeindex];
        }
    }

    public class SelectionInfo
    {
        public int selectedShapeindex = -1;
        public int mouseOverShapeIndex = -1;
        public int pointIndex = -1;
        public int lineindex = -1;
        public bool mouseIsOverPoint;
        public bool mouseIsOverLine;
        public bool pointIsSelected;
        public Vector3 positionAtStartOfDrag;
    }
}

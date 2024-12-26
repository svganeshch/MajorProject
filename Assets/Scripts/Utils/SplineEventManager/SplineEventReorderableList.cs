using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Splines;

public class SplineEventReorderableList : ReorderableList
{
    SplineEventManager manager;
    
    float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    
    static readonly GUIContent eventNameLabel = EditorGUIUtility.TrTextContent("Event Name");
    static readonly GUIContent enterTriggerPositionLabel = EditorGUIUtility.TrTextContent("Enter Trigger Position");
    static readonly GUIContent exitTriggerPositionLabel = EditorGUIUtility.TrTextContent("Exit Trigger Position");
    static readonly GUIContent switchCameraToLabel = EditorGUIUtility.TrTextContent("Switch to camera");
    static readonly GUIContent cameraLabel = EditorGUIUtility.TrTextContent("Camera");
    
    public static SplineEventReorderableList Get(SerializedProperty splineArrayElement)
    {
        var list = new SplineEventReorderableList(splineArrayElement.serializedObject, splineArrayElement);
        list.Init(splineArrayElement);
        return list;
    }
    
    void Init(SerializedProperty splineEventArrayElement)
    {
        serializedProperty = splineEventArrayElement;

        if (splineEventArrayElement.serializedObject.targetObject is SplineEventManager splineEventManager)
        {
            manager = splineEventManager;
        }
    }
    
    public SplineEventReorderableList(
        SerializedObject serializedObject,
        SerializedProperty serializedArrayElement) : base(serializedObject, serializedArrayElement, true, false, true,
        true)
    {
        Init(serializedArrayElement);
        drawElementCallback += DrawElement;
        elementHeightCallback += GetElementHeight;
        onSelectCallback += OnSelect;
        onReorderCallbackWithDetails += OnReorder;
        onAddCallback += OnAdd;
        onRemoveCallback += OnRemove;
    }
    
    float GetElementHeight(int i)
    {
        return EditorGUI.GetPropertyHeight(serializedProperty.GetArrayElementAtIndex(i));
    }

    private void DrawElement(Rect position, int listIndex, bool isactive, bool isfocused)
    {
        SerializedProperty splineEvent = serializedProperty.GetArrayElementAtIndex(listIndex);
        var eventName = splineEvent.FindPropertyRelative("eventName");
        var enterTriggerPosition = splineEvent.FindPropertyRelative("EnterTriggerPosition");
        var exitTriggerPosition = splineEvent.FindPropertyRelative("ExitTriggerPosition");
        var switchCameraTo = splineEvent.FindPropertyRelative("switchCameraTo");
        var camera = splineEvent.FindPropertyRelative("camera");
        
        ++EditorGUI.indentLevel;
        EditorGUI.BeginChangeCheck();
        
        EditorGUIUtility.labelWidth = 0;
        var titleRect = ReserveSpace(lineHeight, ref position);
        titleRect.width = EditorGUIUtility.labelWidth;
        splineEvent.isExpanded = EditorGUI.Foldout(titleRect, splineEvent.isExpanded, eventName.stringValue);

        if (splineEvent.isExpanded)
        {
            EditorGUI.PropertyField(ReserveSpaceForLine(ref position), eventName, eventNameLabel);
            EditorGUI.PropertyField(ReserveSpaceForLine(ref position), enterTriggerPosition, enterTriggerPositionLabel);
            EditorGUI.PropertyField(ReserveSpaceForLine(ref position), exitTriggerPosition, exitTriggerPositionLabel);
            EditorGUI.PropertyField(ReserveSpaceForLine(ref position), switchCameraTo, switchCameraToLabel);

            var isFixedCamera = (CameraState)switchCameraTo.enumValueIndex != CameraState.DollyCamera;
            EditorGUI.BeginDisabledGroup(!isFixedCamera);
            
            var rect = ReserveSpaceForLine(ref position);
            var buttonWidth = 60f;
            var cameraFieldRect = new Rect(rect.x, rect.y, rect.width - buttonWidth - 5, rect.height);
            var buttonRect = new Rect(cameraFieldRect.xMax + 5, rect.y, buttonWidth, rect.height);
            
            EditorGUI.PropertyField(cameraFieldRect, camera, cameraLabel);
            
            var isCameraSet = camera.objectReferenceValue == null;
            EditorGUI.BeginDisabledGroup(!isCameraSet);
            if (GUI.Button(buttonRect, "Set Cam"))
            {
                manager.InstantiateCamera(listIndex);
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.EndDisabledGroup();
        }

        EditorGUI.EndChangeCheck();
        --EditorGUI.indentLevel;
    }
    
    private void OnSelect(ReorderableList reorderableList)
    {
        if (reorderableList.index >= 0 && reorderableList.index < serializedProperty.arraySize)
        {
            manager.SetSelectedSplineEvent(reorderableList.index);
            Debug.Log("selected index : " + reorderableList.index);
        }
    }
    
    private void OnAdd(ReorderableList reorderableList)
    {
        manager.AddSplineEvent(reorderableList.index);
        serializedProperty.serializedObject.ApplyModifiedProperties();

        reorderableList.index = serializedProperty.arraySize;
        manager.SetSelectedSplineEvent(reorderableList.index);
    }
    
    private void OnRemove(ReorderableList reorderableList)
    {
        if (reorderableList.index >= 0 && reorderableList.index < serializedProperty.arraySize)
        {
            manager.RemoveSplineEvent(reorderableList.index);
            serializedProperty.DeleteArrayElementAtIndex(reorderableList.index);
            serializedProperty.serializedObject.ApplyModifiedProperties();

            reorderableList.index = Mathf.Clamp(reorderableList.index, 0, serializedProperty.arraySize);
            
            ClearSelection();
        }
    }

    private void OnReorder(ReorderableList reorderableList, int oldIndex, int newIndex)
    {
        manager.SplineEventIndexChanged(oldIndex, newIndex);
        serializedProperty.serializedObject.ApplyModifiedProperties();
        serializedProperty.serializedObject.Update();
    }
    
    public Rect ReserveSpace(float height, ref Rect total)
    {
        Rect current = total;
        current.height = height;
        total.y += height;
        return current;
    }
    
    public Rect ReserveSpaceForLine(ref Rect total)
    {
        var height = EditorGUIUtility.wideMode ? lineHeight : 2f * lineHeight;
        return ReserveSpace(height, ref total);
    }
}
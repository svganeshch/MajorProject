using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
using UnityEditor.Splines;
using UnityEngine.Splines;

[CustomEditor(typeof(SplineEventManager))]
public class SplineEventManagerEditor : Editor
{
    private SplineEventManager manager;
    private List<Spline> splines = new List<Spline>();

    private SplineEventReorderableList splineReorderableList;
    private SerializedProperty splineEventsCollection;
    
    private int selectedSplineEventIndex = 0;
    private float previousSplineLength;

    private void Awake()
    {
        manager = (SplineEventManager)target;
        manager.SetSplineContainer();
        splines = manager.splineContainer.Splines.ToList();
    }
    
    private void OnEnable()
    {
        splineEventsCollection = serializedObject.FindProperty("splineEventsCollection");
        splineReorderableList = SplineEventReorderableList.Get(splineEventsCollection);

        manager.splineEventSelected += OnSplineEventSelected;
        manager.splineEventRemoved += OnSplineEventRemoved;
        EditorSplineUtility.AfterSplineWasModified += OnSplineModified;
        
        SetSplineLength();
    }

    private void OnDisable()
    {
        manager.splineEventSelected -= OnSplineEventSelected;
        manager.splineEventRemoved -= OnSplineEventRemoved;
        EditorSplineUtility.AfterSplineWasModified -= OnSplineModified;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Spline Events Collection", EditorStyles.boldLabel);
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedCamerasParent"));

        splineReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSplineEventSelected(int selectedIndex)
    {
        selectedSplineEventIndex = selectedIndex;
        
        SceneView.RepaintAll();
    }

    private void OnSplineEventRemoved(int removedIndex)
    {
        selectedSplineEventIndex = Mathf.Clamp(removedIndex, 0, manager.splineEventsCollection.Count - 1);
    }
    
    private void OnSplineModified(Spline spline)
    {
        float currentSplineLength = spline.CalculateLength(float4x4.identity);

        if (Math.Abs(currentSplineLength - previousSplineLength) > Mathf.Epsilon && currentSplineLength > Mathf.Epsilon)
        {
            float inverseCurrentLength = 1 / currentSplineLength;

            foreach (SplineEvent splineEvent in splineEventsCollection)
            {
                splineEvent.EnterTriggerPosition = 
                    Mathf.Clamp01((splineEvent.EnterTriggerPosition * previousSplineLength) * inverseCurrentLength);
                
                splineEvent.ExitTriggerPosition = 
                    Mathf.Clamp01((splineEvent.ExitTriggerPosition * previousSplineLength) * inverseCurrentLength);
            }

            previousSplineLength = currentSplineLength;
        }
        
        SceneView.RepaintAll();
    }
    
    private void SetSplineLength()
    {
        if (manager.splineContainer == null || manager.splineContainer.Splines.Count == 0)
        {
            previousSplineLength = 0;
            return;
        }

        previousSplineLength = manager.splineContainer.Splines[0].CalculateLength(float4x4.identity);
    }

    private void OnSceneGUI()
    {
        if (manager.splineContainer != null && manager.splineEventsCollection.Count > 0)
        {
            SplineEvent splineEvent = manager.splineEventsCollection[selectedSplineEventIndex];
            Spline spline = splines[0];

            if (splineEvent == null) return;
            
            // Gizmos for EnterTriggerPosition
            Handles.color = Color.green;
            Vector3 enterPosition = spline.EvaluatePosition(splineEvent.EnterTriggerPosition);
            Handles.Label(new Vector3(enterPosition.x, enterPosition.y + 0.25f, enterPosition.z), splineEvent.eventName + " Entry");
            Handles.SphereHandleCap(0, enterPosition, Quaternion.identity, 0.25f, EventType.Repaint);
            
            // Gizmos for ExitTriggerPosition
            Handles.color = Color.red;
            Vector3 exitPosition = spline.EvaluatePosition(splineEvent.ExitTriggerPosition);
            Handles.Label(new Vector3(exitPosition.x, exitPosition.y + 0.25f, exitPosition.z), splineEvent.eventName + " Exit");
            Handles.SphereHandleCap(0, exitPosition, Quaternion.identity, 0.25f, EventType.Repaint);
        }
    }
}
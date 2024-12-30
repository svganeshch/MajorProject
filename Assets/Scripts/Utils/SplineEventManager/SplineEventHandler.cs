using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class SplineEventHandler : MonoBehaviour
{
    public CinemachineCamera dollyCamera;
    public CinemachineSplineDolly cinemachineSplineDolly;
    [HideInInspector] public SplineEventManager splineEventManager;

    private readonly SortedDictionary<float, SplineEvent> sortedEventsByKnot = new SortedDictionary<float, SplineEvent>();
    private float lastCheckedPosition = -1f;

    private CameraState defaultCameraType;
    private CameraState currentCameraState;
    
    private SplineEvent previousSplineEvent;

    private void Awake()
    {
        defaultCameraType = CameraState.DollyCamera;
        currentCameraState = defaultCameraType;
        
        splineEventManager = GetComponent<SplineEventManager>();

        DisableAllFixedCameras();
    }

    private void Start()
    {
        GroupEventsByKnots();
    }

    private void Update()
    {
        float cameraKnotPosition = cinemachineSplineDolly.CameraPosition;
    
        if (Mathf.Approximately(lastCheckedPosition, cameraKnotPosition))
            return;
    
        lastCheckedPosition = cameraKnotPosition;
    
        float knotLowerBoundary = Mathf.Floor(cameraKnotPosition);
        float knotUpperBoundary = Mathf.Ceil(cameraKnotPosition);
    
        var eventsInRange = sortedEventsByKnot
            .Where(pair => pair.Key >= knotLowerBoundary && pair.Key <= knotUpperBoundary);
    
        foreach (var kvp in eventsInRange)
        {
            var splineEvent = kvp.Value;
    
            float enterTrigger = cinemachineSplineDolly.Spline.Spline.ConvertIndexUnit(
                splineEvent.EnterTriggerPosition, PathIndexUnit.Knot);
            float exitTrigger = cinemachineSplineDolly.Spline.Spline.ConvertIndexUnit(
                splineEvent.ExitTriggerPosition, PathIndexUnit.Knot);
    
            if (cameraKnotPosition >= enterTrigger && cameraKnotPosition <= exitTrigger)
            {
                if (currentCameraState == splineEvent.switchCameraTo) return;
                HandleCameraSwitch(splineEvent);
                return;
            }
        }
        
        if (currentCameraState != defaultCameraType)
        {
            ResetCamera();
        }
    }

    private void HandleCameraSwitch(SplineEvent splineEvent)
    {
        if (currentCameraState == splineEvent.switchCameraTo) return;
        
        if (splineEvent.switchCameraTo == CameraState.DollyCamera)
        {
            ResetCamera();
        }
        else
        {
            SetCamera(splineEvent);
        }
        
        previousSplineEvent = splineEvent;
        currentCameraState = splineEvent.switchCameraTo;
    }

    private void SetCamera(SplineEvent splineEvent)
    {
        DisablePreviousCamera();

        if (splineEvent.switchCameraTo == CameraState.FixedTrackingCamera)
        {
            splineEvent.camera.Follow = FindFirstObjectByType<Player>().playerCombatManager.lockOnTransform;
        }
        
        splineEvent.camera.gameObject.SetActive(true);
        currentCameraState = splineEvent.switchCameraTo;
    }

    private void ResetCamera()
    {
        DisablePreviousCamera();

        currentCameraState = CameraState.DollyCamera;
    }

    private void DisablePreviousCamera()
    {
        if (previousSplineEvent != null)
        {
            previousSplineEvent.camera.gameObject.SetActive(false);
        }
    }

    private void DisableAllFixedCameras()
    {
        foreach (var splineEvent in splineEventManager.splineEventsCollection.Where(splineEvent => splineEvent.camera))
        {
            splineEvent.camera.gameObject.SetActive(false);
        }
    }
    
    private void GroupEventsByKnots()
    {
        foreach (var splineEvent in splineEventManager.splineEventsCollection)
        {
            float knotEnterTriggerPosition = cinemachineSplineDolly.Spline.Spline.ConvertIndexUnit(
                splineEvent.EnterTriggerPosition, 
                PathIndexUnit.Knot
            );
            
            sortedEventsByKnot.Add(knotEnterTriggerPosition, splineEvent);
            
            float knotExitTriggerPosition = cinemachineSplineDolly.Spline.Spline.ConvertIndexUnit(
                splineEvent.ExitTriggerPosition, 
                PathIndexUnit.Knot
            );
            
            sortedEventsByKnot.Add(knotExitTriggerPosition, splineEvent);
        }
    }
}
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public class SplineEvent
{
    public string eventName;
    public CameraState switchCameraTo;
    [Range(0.0f, 1.0f)] public float EnterTriggerPosition;
    [Range(0.0f, 1.0f)] public float ExitTriggerPosition;
    public CinemachineCamera camera;
}

[ExecuteAlways]
public class SplineEventManager : MonoBehaviour
{
    public GameObject fixedCamerasParent;
    public SplineContainer splineContainer;
    public List<SplineEvent> splineEventsCollection = new List<SplineEvent>();

    public event Action<int> splineEventRemoved;
    public event Action<int> splineEventSelected;

    private void Reset()
    {
        SetSplineContainer();
    }

    public void SetSplineContainer()
    {
        splineContainer = GetComponent<SplineContainer>();
    }

    public void SetSelectedSplineEvent(int index)
    {
        splineEventSelected?.Invoke(index);
    }

    public void AddSplineEvent(int selectedIndex)
    {
        var selectedEvent = new SplineEvent();
        
        try
        {
            selectedEvent = splineEventsCollection[selectedIndex];
        }
        catch
        {
            selectedEvent = null;
        }

        var calculatedEnterTriggerPosition = (selectedEvent != null) ? selectedEvent.ExitTriggerPosition + 0.1f : 0f;
        var calculatedExitTriggerPosition = (selectedEvent != null) ? calculatedEnterTriggerPosition + 0.1f : 0.25f;
        
        SplineEvent newEvent = new SplineEvent
        {
            eventName = "Event " + splineEventsCollection.Count,
            switchCameraTo = CameraState.DollyCamera,
            EnterTriggerPosition = Mathf.Clamp01(calculatedEnterTriggerPosition),
            ExitTriggerPosition = Mathf.Clamp01(calculatedExitTriggerPosition),
        };

        splineEventsCollection.Insert(selectedIndex + 1, newEvent);
    }

    public bool RemoveSplineEvent(int index)
    {
        if (index >= 0 && index < splineEventsCollection.Count)
        {
            splineEventsCollection.RemoveAt(index);
            splineEventRemoved?.Invoke(index);
            return true;
        }
        return false;
    }

    public bool SplineEventIndexChanged(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || oldIndex >= splineEventsCollection.Count ||
            newIndex < 0 || newIndex >= splineEventsCollection.Count)
            return false;
        
        var splineEvents = new List<SplineEvent>(splineEventsCollection);

        var splineEvent = splineEvents[newIndex];
        
        if (newIndex > 0)
        {
            var previousEvent = splineEvents[newIndex - 1];
            splineEvent.EnterTriggerPosition = Mathf.Clamp01(previousEvent.ExitTriggerPosition + 0.1f);
            splineEvent.ExitTriggerPosition = Mathf.Clamp01(splineEvent.EnterTriggerPosition + 0.1f);
        }
        else if (newIndex < splineEvents.Count - 1)
        {
            var nextEvent = splineEvents[newIndex + 1];
            splineEvent.ExitTriggerPosition = Mathf.Clamp01(nextEvent.EnterTriggerPosition - 0.1f);
            splineEvent.EnterTriggerPosition = Mathf.Clamp01(splineEvent.ExitTriggerPosition - 0.1f);
        }
        
        // splineEvents.RemoveAt(oldIndex);
        // splineEvents.Insert(newIndex, splineEvent);
        splineEventsCollection = splineEvents;

        return true;
    }

    public void InstantiateCamera(int splineIndex, Transform sceneViewTransform)
    {
        if (splineIndex < 0 || splineIndex >= splineEventsCollection.Count) return;

        SplineEvent splineEvent = splineEventsCollection[splineIndex];
        GameObject cameraGameObject = new GameObject(splineEvent.eventName + " Cam");

        var cameraCinemachine = cameraGameObject.AddComponent<CinemachineCamera>();
        cameraGameObject.AddComponent<CinemachineHardLookAt>();
        cameraGameObject.transform.position = sceneViewTransform.position;
        cameraGameObject.transform.rotation = sceneViewTransform.rotation;
        
        splineEvent.camera = cameraCinemachine;

        if (fixedCamerasParent != null)
            cameraGameObject.transform.parent = fixedCamerasParent.transform;
    }

    public void ResetCameraPosition(int index, Transform sceneViewTransform)
    {
        SplineEvent splineEvent = splineEventsCollection[index];
        
        var cameraGameObj = splineEvent.camera.gameObject;
        cameraGameObj.transform.position = sceneViewTransform.position;
        cameraGameObj.transform.rotation = sceneViewTransform.rotation;
    }

    public void RenameCamera(int index)
    {
        var splineEvent = splineEventsCollection[index];
        
        splineEvent.camera.gameObject.name = splineEvent.eventName + " Cam";
    }
}
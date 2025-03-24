using UnityEngine;
using System.Collections.Generic;

public class CameraZoneHandler : MonoBehaviour
{
    [System.Serializable]
    public class CameraZone
    {
        public string zoneName;
        public TriggerZone triggerZone1;
        public TriggerZone triggerZone2;
        public Camera assignedCamera;
    }

    public List<CameraZone> cameraZones;
    public Camera defaultCamera;

    private Camera currentCamera;
    private CameraZone activeZone = null;

    private void Start()
    {
        ActivateCamera(defaultCamera);
    }

    // Called when a trigger is entered
    public void OnZoneEntered(TriggerZone trigger)
    {
        foreach (var zone in cameraZones)
        {
            if (trigger.zone == zone)
            {
                activeZone = zone;
                ActivateCamera(zone.assignedCamera);
                return;
            }
        }
    }

    // Called when a trigger is exited
    public void OnZoneExited(TriggerZone trigger)
    {
        if (activeZone != null && trigger.zone == activeZone)
        {
            activeZone = null;
            ActivateCamera(defaultCamera);
        }
    }

    private void ActivateCamera(Camera newCamera)
    {
        if (currentCamera != null) currentCamera.gameObject.SetActive(false);
        if (newCamera != null) newCamera.gameObject.SetActive(true);
        currentCamera = newCamera;
    }
}
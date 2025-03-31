using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CameraZoneHandler : MonoBehaviour
{
    [System.Serializable]
    public class CameraZone
    {
        public string zoneName;
        public TriggerZone triggerZone1;
        public TriggerZone triggerZone2;
        public CinemachineCamera assignedCamera;
    }

    public List<CameraZone> cameraZones;
    public CinemachineCamera defaultCamera;

    private CinemachineCamera currentCamera;
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
            if (trigger.gameObject == zone.triggerZone1.gameObject
                || trigger.gameObject == zone.triggerZone2.gameObject)
            {
                if (activeZone != null && activeZone == zone)
                {
                    activeZone = null;
                    ActivateCamera(defaultCamera);
                    
                    Debug.LogWarning("Trigger zone: " + zone.zoneName + " is already active so exiting");
                    return;
                }
                
                activeZone = zone;
                ActivateCamera(zone.assignedCamera);
                return;
            }
        }
    }

    private void ActivateCamera(CinemachineCamera newCamera)
    {
        if (currentCamera != null) currentCamera.gameObject.SetActive(false);
        if (newCamera != null) newCamera.gameObject.SetActive(true);
        currentCamera = newCamera;
        
        newCamera.Follow = FindFirstObjectByType<Player>().playerCombatManager.lockOnTransform;
    }
}
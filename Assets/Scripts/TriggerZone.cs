using System;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    private CameraZoneHandler cameraHandler;

    private void Start()
    {
        cameraHandler = FindFirstObjectByType<CameraZoneHandler>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraHandler.OnZoneEntered(this);
            
            Debug.Log("Zone entered");
        }
    }
}
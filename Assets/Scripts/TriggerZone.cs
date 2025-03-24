using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public CameraZoneHandler.CameraZone zone;
    private CameraZoneHandler cameraHandler;

    private void Start()
    {
        cameraHandler = FindFirstObjectByType<CameraZoneHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraHandler.OnZoneEntered(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraHandler.OnZoneExited(this);
        }
    }
}
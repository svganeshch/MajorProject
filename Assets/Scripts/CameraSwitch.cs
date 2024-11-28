using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineCamera vCamera;
    public GameObject camFollow;

    private void Awake()
    {
        //camFollow = FindObjectOfType<PlayerFollowCam>();
        vCamera = GetComponentInChildren<CinemachineCamera>();
        vCamera.enabled = false;
    }

    private void OnTriggerExit(Collider collidedWith)
    {
        Debug.Log("collided : " +  collidedWith);

        if (collidedWith != null)
        {
            if (collidedWith.gameObject.CompareTag("Player"))
            {
                Debug.Log("Collided with player");
                GameObject player = collidedWith.gameObject;

                if (Vector3.Dot(player.transform.forward, transform.forward) > 0)
                {
                    Debug.Log("player facing forward");
                    OnCameraSwitch(vCamera);
                }
                else
                {
                    Debug.Log("player facing backward");
                    ResetCamera();
                }
            }
        }
    }

    public void OnCameraSwitch(CinemachineCamera camera)
    {
        camera.Follow = camFollow.transform;
        camera.LookAt = camFollow.transform;

        camera.enabled = true;
    }

    public void ResetCamera()
    {
        vCamera.enabled = false;
    }
}

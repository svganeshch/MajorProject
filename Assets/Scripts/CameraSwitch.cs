using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera vCamera;
    public GameObject camFollow;

    private void Awake()
    {
        //camFollow = FindObjectOfType<PlayerFollowCam>();
        vCamera = GetComponentInChildren<CinemachineVirtualCamera>();
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

    public void OnCameraSwitch(CinemachineVirtualCamera camera)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
    Player player;
    Camera mainCamera;

    private void Awake()
    {
        player = FindObjectOfType<Player>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.position = mainCamera.transform.forward + new Vector3(player.transform.position.x, 2, -0.5f);
    }
}

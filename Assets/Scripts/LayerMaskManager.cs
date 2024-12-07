using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMaskManager : MonoBehaviour
{
    public static LayerMaskManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask characterLayer;
    public LayerMask groundLayer;
    public LayerMask vaultObstacleLayer;
}

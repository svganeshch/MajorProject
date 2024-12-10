using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        obstacleLayer = groundLayer | parkourObstacleLayer;
    }

    public LayerMask characterLayer;
    public LayerMask groundLayer;
    public LayerMask parkourObstacleLayer;
    public LayerMask obstacleLayer;
}

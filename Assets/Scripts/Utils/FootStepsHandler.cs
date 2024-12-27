using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsHandler : MonoBehaviour
{
    public static FootStepsHandler instance;
    
    string currentSurfaceMaterialName;
    string onSurfaceMaterialName;

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

    public FootStepsData CheckSurface(FootStepsData[] surfaceFootStepsData)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
        {
            if (hit.transform.TryGetComponent<Renderer>(out Renderer renderer))
            {
                onSurfaceMaterialName = renderer.sharedMaterial.name;
                //Debug.Log("On Material : " + onSurfaceMaterialName);

                if (currentSurfaceMaterialName != onSurfaceMaterialName)
                {
                    currentSurfaceMaterialName = onSurfaceMaterialName;

                    foreach (FootStepsData footStepsData in surfaceFootStepsData)
                    {
                        if (footStepsData.materialNames.Contains(currentSurfaceMaterialName))
                        {
                            return footStepsData;
                        }
                    }
                }
            }
        }
        
        return surfaceFootStepsData[0];
    }
}
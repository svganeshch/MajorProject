using System;
using UnityEngine;

public class SwapVanishMaterial : MonoBehaviour
{
    public Material vanishMaterial;
    public SkinnedMeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void SwapMaterial()
    {
        meshRenderer.material = vanishMaterial;
    }
}

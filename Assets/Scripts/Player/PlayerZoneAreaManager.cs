using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneAreaManager : MonoBehaviour
{
    public float zoneRadius = 5f;

    public List<IZoneItem> enabledZoneItems = new List<IZoneItem>();

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, zoneRadius);
        HashSet<IZoneItem> currentZoneItems = new HashSet<IZoneItem>();
        
        foreach (var zoneItemCollider in colliders)
        {
            if (zoneItemCollider.TryGetComponent<IZoneItem>(out IZoneItem zoneItem))
            {
                currentZoneItems.Add(zoneItem);

                if (!enabledZoneItems.Contains(zoneItem))
                {
                    zoneItem.EnableZoneItem();
                    enabledZoneItems.Add(zoneItem);
                }
            }
        }

        for (int i = enabledZoneItems.Count - 1; i >= 0; i--)
        {
            IZoneItem zoneItem = enabledZoneItems[i];
            if (!currentZoneItems.Contains(zoneItem))
            {
                zoneItem.DisableZoneItem();
                enabledZoneItems.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, zoneRadius);
    }
}
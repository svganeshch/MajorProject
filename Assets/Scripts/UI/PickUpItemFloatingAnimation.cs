using System;
using System.Collections;
using UnityEngine;

public class PickUpItemFloatingAnimation : MonoBehaviour
{
    private GameObject pickUpItem;
    
    private void Start()
    {
        pickUpItem = transform.GetChild(0).gameObject;
        StartCoroutine(PickUpFloatingAnimation());
    }

    private IEnumerator PickUpFloatingAnimation()
    {
        while (true)
        {
            pickUpItem.transform.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
            pickUpItem.transform.localPosition = new Vector3(0, Mathf.Sin(Time.time) * 0.5f, 0);
            
            yield return null;
        }
    }
}

using System;
using UnityEngine;

public class PowerUpCollectable : MonoBehaviour, ICollectable
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect();
        }
    }

    public void OnCollect()
    {
        PowerUpHandler.Instance.SetPowerUpStatus(true);
        gameObject.SetActive(false);
        
        InputManager.Instance.chainAttackAction.Enable();
        
        Debug.Log("Power up enabled");
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Action", menuName = "Actions / Weapon Actions")]
public class WeaponItemAction : ScriptableObject
{
    public virtual void AttemptToPerformAction(Character characterPerformingAction, WeaponItem weaponPerformingAction)
    {
        
    }
}

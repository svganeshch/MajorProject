using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    Character character;
    
    public Character currentTarget;
    
    public Transform lockOnTransform;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }
    protected virtual void Start() {}
    protected virtual void Update() {}
    protected virtual void FixedUpdate() {}

    public virtual void PerformWeaponBasedAction(WeaponItemAction weaponAction)
    {
        weaponAction.AttemptToPerformAction(character);
    }
}

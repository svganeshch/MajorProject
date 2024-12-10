using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    public Character currentTarget;
    
    public Transform lockOnTransform;
    
    protected virtual void Awake() {}
}

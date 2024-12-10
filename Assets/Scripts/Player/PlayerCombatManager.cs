public class PlayerCombatManager : CharacterCombatManager
{
    private Player player;
    
    public WeaponItem currentWeapon;

    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<Player>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weapon)
    {
        weaponAction.AttemptToPerformAction(player, weapon);
    }
}
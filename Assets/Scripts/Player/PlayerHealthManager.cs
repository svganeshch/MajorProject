public class PlayerHealthManager : CharacterHealthManager
{
    protected override void Start()
    {
        base.Start();

        PlayerHUDManager.instance.SetHealthBar(currentHealth);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        PlayerHUDManager.instance.SetHealthBar(currentHealth);
    }
}
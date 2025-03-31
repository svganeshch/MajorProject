public class PlayerHealthManager : CharacterHealthManager
{
    protected override void Start()
    {
        base.Start();

        //PlayerHUDManager.instance.SetHealthBar(currentHealth);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        // calculate health percentage
        var healthPercentage = (float) currentHealth / character.health;
        PlayerHUDManager.instance.SetHealthBar(healthPercentage);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        
        MenuHandler.Instance.deathMenuPanel.SetActive(true);
    }
}
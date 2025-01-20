public class EnemyHealthManager : CharacterHealthManager
{
    SwapVanishMaterial swapVanishMaterial;
    
    protected override void Awake()
    {
        base.Awake();
        
        swapVanishMaterial = GetComponentInChildren<SwapVanishMaterial>();
    }

    protected override void OnDeath()
    {
        swapVanishMaterial.SwapMaterial();
        
        base.OnDeath();
    }
}
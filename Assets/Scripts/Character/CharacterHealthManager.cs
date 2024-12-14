using UnityEngine;

public class CharacterHealthManager : MonoBehaviour
{
    Character character;
    
    SwapVanishMaterial swapVanishMaterial;

    int currentHealth;

    private void Awake()
    {
        character = GetComponent<Character>();
        swapVanishMaterial = GetComponentInChildren<SwapVanishMaterial>();

        currentHealth = character.health;
    }

    public void TakeDamage(int damage)
    {
        if (character.isDead) return;

        currentHealth -= damage;
        character.health = currentHealth;

        if (currentHealth < 0)
        {
            character.isDead = true;
            character.characterMovementManager.enabled = false;
            character.characterAnimatorManager.PlayDeathAction();
            
            swapVanishMaterial.SwapMaterial();

            Destroy(character.gameObject, 5f);
        }
    }
}

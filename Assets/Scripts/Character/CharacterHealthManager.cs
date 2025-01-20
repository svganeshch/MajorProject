using UnityEngine;

public class CharacterHealthManager : MonoBehaviour
{
    Character character;

    protected int currentHealth;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();

        currentHealth = character.health;
    }

    protected virtual void Start() { }

    public virtual void TakeDamage(int damage)
    {
        if (character.isDead) return;

        currentHealth -= damage;
        character.health = currentHealth;

        if (currentHealth <= 0)
            OnDeath();
    }

    protected virtual void OnDeath()
    {
        character.isDead = true;
        character.characterMovementManager.enabled = false;
        character.characterAnimatorManager.PlayDeathAction();

        Destroy(character.gameObject, 5f);
    }
}

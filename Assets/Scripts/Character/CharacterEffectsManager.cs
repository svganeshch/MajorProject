using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    // instant effects

    // timed effects (lighting attack etc)

    Character character;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }
}

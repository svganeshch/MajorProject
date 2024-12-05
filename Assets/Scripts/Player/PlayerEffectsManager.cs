using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;

            // Instantiate and process effect
        }
    }
}

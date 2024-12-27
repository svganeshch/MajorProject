using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Footstep Collection", menuName = "Create new footstep data")]
public class FootStepsData : ScriptableObject
{
    public AudioClip[] walkingFootstepSounds;
    public AudioClip[] runningFootstepSounds;
    
    public AudioClip[] jumpSounds;
    public AudioClip[] landingSounds;

    public List<string> materialNames;
}
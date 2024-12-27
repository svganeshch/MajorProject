using UnityEngine;

public class Enums : MonoBehaviour
{
    
}

public enum CameraState
{
    DollyCamera,
    FixedCamera,
    FixedTrackingCamera,
}

public enum ParkourActionType
{
    Vault,
    Climb
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHand,
}

public enum PlayerMovementState
{
    Walking,
    Running,
    Sprinting,
}

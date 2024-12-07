using System.Collections;
using Actions;
using UnityEngine;

public class MatchTargetParameters
{
    public Vector3 matchPos;
    public Vector3 matchPosWeight;

    public AvatarTarget matchBodyPart;

    public float matchStartTime;
    public float matchTargetTime;
}

public class PlayerParkourManager : CharacterParkourManager
{
    Player player;
    
    [Header("Vault Settings")]
    public VaultAction vaultAction;
    private RaycastHit forwardHitData;
    private RaycastHit heightHitData;
    
    public Vector3 forwardRayOffset;
    public float forwardRayLength;
    public float heightRayLength;

    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<Player>();
    }
    
    public bool IsVaultObject()
    {
        var forwardOrigin = player.transform.position + forwardRayOffset;

        bool vaultObstacleDetected = Physics.Raycast(forwardOrigin, player.transform.forward, out forwardHitData,
            forwardRayLength, LayerMaskManager.instance.vaultObstacleLayer);
        
        Debug.DrawRay(forwardOrigin, player.transform.forward * forwardRayLength,
            vaultObstacleDetected ? Color.green : Color.red);

        bool obstacleHeightDetected = false;

        if (vaultObstacleDetected)
        {
            var heightOrigin = forwardHitData.point + Vector3.up * heightRayLength;
            
            obstacleHeightDetected = Physics.Raycast(heightOrigin, Vector3.down, out heightHitData,
                heightRayLength, LayerMaskManager.instance.vaultObstacleLayer);
            
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength,
                obstacleHeightDetected ? Color.green : Color.red);
        }

        if (vaultObstacleDetected && obstacleHeightDetected)
        {
            return vaultAction.CanVault(forwardHitData, heightHitData, player.transform);
        }
        
        return false;
    }

    public void PerformVaultActionCoroutine()
    {
        if (player.performingAction) return;
        
        StartCoroutine(PerformVaultAction());
    }
    
    private IEnumerator PerformVaultAction()
    {
        player.isVaulting = true;
        player.playerAnimatorManager.SetAnimatorParameters(0, 0);
        
        MatchTargetParameters matchTargetParameters = null;

        if (vaultAction.EnableTargetMatching)
        {
            matchTargetParameters = new MatchTargetParameters()
            {
                matchPos = vaultAction.MatchPos,
                matchBodyPart = vaultAction.MatchBodyPart,
                matchPosWeight = vaultAction.MatchPosWeight,
                matchStartTime = vaultAction.MatchStartTime,
                matchTargetTime = vaultAction.MatchTargetTime
            };
        }
        
        player.playerAnimatorManager.PlayVaultAction();

        yield return null;
        
        var animState = player.animator.GetNextAnimatorStateInfo(1);
        float rotateStartTime = (matchTargetParameters != null) ? matchTargetParameters.matchStartTime : 0;
        
        float timeElapsed = 0;
        while (timeElapsed <= animState.length)
        {
            timeElapsed += Time.deltaTime;
            float normalizedTime = timeElapsed / animState.length;

            if (vaultAction.RotateTowardsObstacle && normalizedTime > rotateStartTime)
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, 
                    vaultAction.TargetRotation, player.rotationDampTime * Time.deltaTime);
            }

            if (matchTargetParameters != null)
                MatchTarget(matchTargetParameters);
            
            yield return null;
        }
    }

    private void MatchTarget(MatchTargetParameters matchTargetParameters)
    {
        if (player.animator.isMatchingTarget || player.animator.IsInTransition(1)) return;
        
        player.animator.MatchTarget(matchTargetParameters.matchPos, player.transform.rotation, matchTargetParameters.matchBodyPart,
            new MatchTargetWeightMask(matchTargetParameters.matchPosWeight, 0.0f), matchTargetParameters.matchStartTime,
            matchTargetParameters.matchTargetTime);
    }
}

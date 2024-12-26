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
    
    [SerializeField] private ParkourAction currentParkourAction;
    
    [Header("Parkour Actions")]
    public ParkourAction[] parkourActions;
    
    [Header("Parkour Settings")]
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
    
    public bool IsParkourObstacle()
    {
        var forwardOrigin = player.transform.position + forwardRayOffset;

        bool parkourObstacleDetected = Physics.Raycast(forwardOrigin, player.transform.forward, out forwardHitData,
            forwardRayLength, LayerMaskManager.instance.parkourObstacleLayer);
        
        Debug.DrawRay(forwardOrigin, player.transform.forward * forwardRayLength,
            parkourObstacleDetected ? Color.green : Color.red);

        bool obstacleHeightDetected = false;

        if (parkourObstacleDetected)
        {
            var heightOrigin = forwardHitData.point + Vector3.up * heightRayLength;
            
            obstacleHeightDetected = Physics.Raycast(heightOrigin, Vector3.down, out heightHitData,
                heightRayLength, LayerMaskManager.instance.parkourObstacleLayer);
            
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength,
                obstacleHeightDetected ? Color.green : Color.red);
        }

        if (!parkourObstacleDetected || !obstacleHeightDetected) return false;

        foreach (var parkourAction in parkourActions)
        {
            if (!parkourAction.CanParkourObstacle(forwardHitData, heightHitData, player.transform)) continue;
            currentParkourAction = parkourAction;
            return true;
        }
        
        return false;
    }

    public void PerformParkourActionCoroutine()
    {
        if (player.performingAction) return;
        
        StartCoroutine(PerformParkourAction());
    }
    
    private IEnumerator PerformParkourAction()
    {
        player.performingParkour = true;
        player.playerAnimatorManager.SetAnimatorParameters(0, 0);
        
        MatchTargetParameters matchTargetParameters = null;

        if (currentParkourAction.EnableTargetMatching)
        {
            matchTargetParameters = new MatchTargetParameters()
            {
                matchPos = currentParkourAction.MatchPos,
                matchBodyPart = currentParkourAction.MatchBodyPart,
                matchPosWeight = currentParkourAction.MatchPosWeight,
                matchStartTime = currentParkourAction.MatchStartTime,
                matchTargetTime = currentParkourAction.MatchEndTime
            };
        }
        
        player.playerAnimatorManager.PlayParkourAction(currentParkourAction.ParkourActionAnimation);

        yield return null;
        
        var animState = player.animator.GetNextAnimatorStateInfo(1);
        float timeElapsed = 0;
        
        while (timeElapsed <= animState.length)
        {
            timeElapsed += Time.deltaTime;

            if (currentParkourAction.RotateTowardsObstacle)
            {
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, 
                    currentParkourAction.TargetRotation, player.rotationDampTime * Time.deltaTime);
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
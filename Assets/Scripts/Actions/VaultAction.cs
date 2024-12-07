using UnityEngine;

namespace Actions
{
    [CreateAssetMenu(fileName = "VaultAction", menuName = "Actions / VaultAction")]
    public class VaultAction : ScriptableObject
    {
        [Header("Height Information")]
        [SerializeField] private float minHeight;
        [SerializeField] private float maxHeight;
        
        [SerializeField] private bool rotateTowardsObstacle;
        
        [Header("Target Matching")]
        [SerializeField] private bool enableTargetMatching;

        [SerializeField] protected AvatarTarget matchBodyPart;
        [SerializeField] private Vector3 matchPosWeight = new Vector3(0, 1.0f, 1.0f);

        [SerializeField] private float matchStartTime;
        [SerializeField] private float matchTargetTime;

        public Quaternion TargetRotation { get; private set; }
        public Vector3 MatchPos { get; private set; }
        public bool RotateTowardsObstacle => rotateTowardsObstacle;
        public bool EnableTargetMatching => enableTargetMatching;
        public AvatarTarget MatchBodyPart => matchBodyPart;
        public Vector3 MatchPosWeight => matchPosWeight;
        public float MatchStartTime
        {
            get => matchStartTime;
            set => matchStartTime = value;
        }

        public float MatchTargetTime
        {
            get => matchTargetTime;
            set => matchTargetTime = value;
        }

        public bool CanVault(RaycastHit forwardHitData, RaycastHit heightHitData, Transform playerTransform)
        {
            var height = heightHitData.point.y - playerTransform.position.y;
            if (height < minHeight || height > maxHeight) return false;
            
            if (rotateTowardsObstacle)
                TargetRotation = Quaternion.LookRotation(-forwardHitData.normal);
            
            if (enableTargetMatching)
                MatchPos = heightHitData.point;
            
            var hitPoint = forwardHitData.transform.InverseTransformPoint(forwardHitData.point);

            /*if ((hitPoint.x < 0.0f && hitPoint.x > -0.3f) || (hitPoint.x > 0.0f && hitPoint.x < 0.3f))
            {
                matchBodyPart = AvatarTarget.RightHand;
                matchStartTime = 0f;
                matchTargetTime = 0f;
                
                // Perform center vault
            }*/
            
            return true;
        }
    }
}
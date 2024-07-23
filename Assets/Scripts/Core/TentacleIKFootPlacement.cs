using UnityEngine;

public class TentacleIKFootPlacement : MonoBehaviour
{
    [System.Serializable]
    public class Tentacle
    {
        public Transform footTransform; // The end effector of the tentacle
        public Transform upperLegTransform; // The upper part of the tentacle
        public Transform lowerLegTransform; // The lower part of the tentacle
        public float stepHeight = 0.5f; // Maximum height the foot can step
        public float stepSpeed = 5f; // Speed of the foot adjustment
    }

    [SerializeField] private Tentacle[] tentacles;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float footOffset = 0.1f; // Offset to prevent feet from clipping into the ground

    private void Update()
    {
        foreach (var tentacle in tentacles)
        {
            AdjustTentacle(tentacle);
        }
    }

    private void AdjustTentacle(Tentacle tentacle)
    {
        RaycastHit hit;
        Vector3 footPosition = tentacle.footTransform.position;
        Vector3 direction = Vector3.down;

        if (Physics.Raycast(footPosition + Vector3.up * tentacle.stepHeight, direction, out hit, tentacle.stepHeight * 2, groundLayer))
        {
            Vector3 targetPosition = hit.point + Vector3.up * footOffset;

            tentacle.footTransform.position = Vector3.Lerp(tentacle.footTransform.position, targetPosition, Time.deltaTime * tentacle.stepSpeed);

            // Rotate the foot to align with the ground normal
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * tentacle.footTransform.rotation;
            tentacle.footTransform.rotation = Quaternion.Slerp(tentacle.footTransform.rotation, targetRotation, Time.deltaTime * tentacle.stepSpeed);
        }

        AdjustLeg(tentacle, footPosition);
    }

    private void AdjustLeg(Tentacle tentacle, Vector3 targetFootPosition)
    {
        Vector3 upperLegPosition = tentacle.upperLegTransform.position;
        Vector3 lowerLegPosition = tentacle.lowerLegTransform.position;

        float upperLegLength = Vector3.Distance(upperLegPosition, lowerLegPosition);
        float lowerLegLength = Vector3.Distance(lowerLegPosition, tentacle.footTransform.position);

        Vector3 direction = (targetFootPosition - upperLegPosition).normalized;
        Vector3 middlePoint = upperLegPosition + direction * (upperLegLength / (upperLegLength + lowerLegLength));

        tentacle.lowerLegTransform.position = middlePoint;
        tentacle.lowerLegTransform.LookAt(targetFootPosition);

        tentacle.upperLegTransform.LookAt(tentacle.lowerLegTransform.position);
    }
}

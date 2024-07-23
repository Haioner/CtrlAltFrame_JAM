using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 300f;
    [SerializeField] private float stopDistance = 1f;

    private void Update()
    {
        if(target != null)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = target.position;

            targetPosition.y = currentPosition.y;
            Vector3 direction = (targetPosition - currentPosition).normalized;

            float distanceToTarget = Vector3.Distance(currentPosition, targetPosition);
            if(distanceToTarget > stopDistance)
            {
                transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
         
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEvent;
    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Tocou " + gameObject.name);
            triggerEvent?.Invoke();
            hasTriggered = true;
            PlayerManager.checkPointPos = transform.position;
        }
    }
}

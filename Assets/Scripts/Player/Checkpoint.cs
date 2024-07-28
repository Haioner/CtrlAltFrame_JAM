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
            triggerEvent?.Invoke();
            hasTriggered = true;
            //PlayerManager.checkPointPos = transform.position;
            PlayerPrefs.SetFloat("xPos", transform.position.x);
            PlayerPrefs.SetFloat("yPos", transform.position.y);
            PlayerPrefs.SetFloat("zPos", transform.position.z);
        }
    }
}

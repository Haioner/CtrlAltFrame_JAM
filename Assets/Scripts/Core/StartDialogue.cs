using UnityEngine;
using UnityEngine.Events;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEvent;
    private static bool hasTriggered;

    public void TriggerDialogue()
    {
        if (!hasTriggered)
        {
            triggerEvent?.Invoke();
            hasTriggered = true;
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEvent;

    public void TriggerDialogue()
    {
        if (!PlayerPrefs.HasKey("StartTrigger"))
        {
            PlayerPrefs.SetInt("StartTrigger", 1);
            triggerEvent?.Invoke();
        }
    }
}

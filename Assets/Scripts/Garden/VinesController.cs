using System.Collections.Generic;
using UnityEngine;

public class VinesController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private List<VinesController> otherVines = new List<VinesController>();
    private AudioSource audioSource;
    private bool isOpened;

    private void Start()
    {
        if(GetComponent<AudioSource>() != null)
            audioSource = GetComponent<AudioSource>();
    }

    public void SwitchState()
    {
        isOpened = !isOpened;

        if (audioSource != null)
            audioSource.Play();

        anim.SetBool("State", isOpened);
        SwitchOtherVines();
    }

    private void SwitchOtherVines()
    {
        if (otherVines.Count > 0)
        {
            foreach (var otherAnim in otherVines)
            {
                otherAnim.SwitchState();
            }
        }
    }
}

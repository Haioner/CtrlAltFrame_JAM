using System.Collections.Generic;
using UnityEngine;

public class VinesController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private List<VinesController> otherVines = new List<VinesController>();

    private bool isOpened;

    public void SwitchState()
    {
        isOpened = !isOpened;
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

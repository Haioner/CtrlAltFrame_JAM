using UnityEngine;

public class VinesController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private bool isOpened;

    public void SwitchState()
    {
        isOpened = !isOpened;
        anim.SetBool("State", isOpened);
    }
}

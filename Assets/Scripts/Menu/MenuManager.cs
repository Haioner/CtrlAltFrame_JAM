using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        TransitionController.instance.TransitionToSceneName("Game");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}

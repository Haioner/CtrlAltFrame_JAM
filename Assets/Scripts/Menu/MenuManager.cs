using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        TransitionController.instance.TransitionToSceneName("Game");
        FindFirstObjectByType<InputState>().SetCanPauseGame(true);
    }

    public void OptionsMenu()
    {
        FindFirstObjectByType<InputState>().SwitchCanvasGroup();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}

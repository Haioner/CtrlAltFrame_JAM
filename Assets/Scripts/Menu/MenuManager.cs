using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteKey("StartTrigger");
    }

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

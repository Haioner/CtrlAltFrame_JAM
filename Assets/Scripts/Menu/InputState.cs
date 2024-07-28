using UnityEngine;

public class InputState : MonoBehaviour
{
    [SerializeField] private KeyCode inputKey, inputKeyTwo;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private bool canPauseGame;
    private bool canvasState;

    public void SwitchCanvasGroup()
    {
        canvasState = !canvasState;
        PauseGame();
        if (canvasState)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) || Input.GetKeyDown(inputKeyTwo))
        {
            SwitchCanvasGroup();
        }
    }

    private void PauseGame()
    {
        if (canPauseGame)
        {
            Time.timeScale = canvasState ? 0 : 1;
        }
    }

    public void SetCanPauseGame(bool newValue)
    {
        canPauseGame = newValue;
    }

    public void BackToMenu()
    {
        if (FindFirstObjectByType<PlayerManager>() != null)
            FindFirstObjectByType<PlayerManager>().ClearCheckpoint();
        PlayerPrefs.DeleteKey("StartTrigger");
        TransitionController.instance.TransitionToSceneName("Menu");
        SetCanPauseGame(false);
    }
}

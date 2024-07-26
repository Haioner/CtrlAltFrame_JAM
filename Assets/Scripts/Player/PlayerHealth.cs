using UnityEngine.Rendering;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Volume deathVolume;
    private bool hasTrigger;

    private void Update()
    {
        if (hasTrigger)
        {
            deathVolume.weight += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        SoundManager.PlayAudioClip(deathClip);
        playerManager.SetPlayerControl(false);
        hasTrigger = true;
        CinemachineShake.instance.ShakeCamera(15, 0.1f);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.5f);

        string sceneName = SceneManager.GetActiveScene().name;
        TransitionController.instance.TransitionToSceneName(sceneName);
    }
}

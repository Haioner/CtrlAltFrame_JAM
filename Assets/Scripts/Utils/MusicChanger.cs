using UnityEngine;
using System.Collections;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private float crossfadeDuration = 1.0f;

    private AudioSource currentSource;
    private AudioSource nextSource;

    private void OnEnable()
    {
        currentSource = audioSource1;
        nextSource = audioSource2;
    }

    public void PlayMusic(AudioClip newClip)
    {
        StartCoroutine(CrossfadeMusic(newClip));
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        nextSource.clip = newClip;
        nextSource.Play();

        float timer = 0f;

        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeDuration;

            currentSource.volume = Mathf.Lerp(1, 0, t);
            nextSource.volume = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        currentSource.Stop();
        currentSource.volume = 1;
        nextSource.volume = 1;

        // Swap the sources
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}

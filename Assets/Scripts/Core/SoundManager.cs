using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlayAudioClip(AudioClip clip)
    {
        _audioSource.volume = 1f;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(clip);
    }

    public static void PlayAudioClipPitch(AudioClip clip, float pitch)
    {
        _audioSource.volume = 1f;
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip);
    }

    public static void PlayAudioClipVolume(AudioClip clip, float volume)
    {
        _audioSource.volume = volume;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(clip);
    }
}

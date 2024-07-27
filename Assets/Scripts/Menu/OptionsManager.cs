using UnityEngine.Localization.Settings;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using PixelCrushers;

[System.Serializable]
public class AudioSettings
{
    public AudioMixer AudioMixer;
    public Slider SoundSlider;
    public Slider MusicSlider;
    public TextMeshProUGUI SoundText;
    public TextMeshProUGUI MusicText;
    public float currentSound;
    public float currentMusic;
}

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private AudioSettings audioSettings;

    private void Start()
    {
        LoadAudioSettings();
    }

    public void BrazilButton()
    {
        SetLanguage("pt-BR");
        UpdateDialogueLocalize();
    }

    public void EUAButton()
    {
        SetLanguage("en");
        UpdateDialogueLocalize();
    }

    private void UpdateDialogueLocalize()
    {
        if (FindFirstObjectByType<UILocalizationManager>() != null)
            FindFirstObjectByType<UILocalizationManager>().currentLanguage = LocalizationSettings.SelectedLocale.Identifier.Code;
    }

    private void SetLanguage(string languageCode)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == languageCode);
    }

    #region Audio
    private void LoadAudioSettings()
    {
        LoadSoundVolume();
        LoadMusicVolume();
    }

    private float AudioValueText(float value)
    {
        return value * 100;
    }

    #region Sound
    public void SoundVolume()
    {
        audioSettings.currentSound = audioSettings.SoundSlider.value;
        audioSettings.AudioMixer.SetFloat("SoundVolume", Mathf.Log10(audioSettings.SoundSlider.value) * 20);
        PlayerPrefs.SetFloat("SVolume", audioSettings.SoundSlider.value);
        SoundText();
    }

    private void SoundText()
    {
        audioSettings.SoundText.text = AudioValueText(audioSettings.currentSound).ToString("F0");
    }

    private void LoadSoundVolume()
    {
        if (PlayerPrefs.HasKey("SVolume"))
        {
            audioSettings.currentSound = PlayerPrefs.GetFloat("SVolume");
            audioSettings.SoundSlider.SetValueWithoutNotify(audioSettings.currentSound);
        }
        SoundVolume();
    }
    #endregion

    #region Music
    public void MusicVolume()
    {
        audioSettings.currentMusic = audioSettings.MusicSlider.value;
        audioSettings.AudioMixer.SetFloat("MusicVolume", Mathf.Log10(audioSettings.MusicSlider.value) * 20);
        PlayerPrefs.SetFloat("MuVolume", audioSettings.MusicSlider.value);
        MusicText();
    }

    private void MusicText()
    {
        audioSettings.MusicText.text = AudioValueText(audioSettings.currentMusic).ToString("F0");
    }

    private void LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey("MuVolume"))
        {
            audioSettings.currentMusic = PlayerPrefs.GetFloat("MuVolume");
            audioSettings.MusicSlider.SetValueWithoutNotify(audioSettings.currentMusic);
        }
        MusicVolume();
    }
    #endregion

    #endregion
}

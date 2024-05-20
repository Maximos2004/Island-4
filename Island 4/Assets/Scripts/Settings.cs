using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [Space(10)]
    [HideInInspector] public float volumee;
    [HideInInspector] public float volumeSFX;
    [HideInInspector] public float volumeMusic;
    [Space(10)]
    [SerializeField] private Slider SliderMasterVolume;
    [SerializeField] private Slider SliderSFXVolume, SliderMusicVolume;

    [Header("Quality")]
    [SerializeField] private TMP_Dropdown QualityControl;
    [HideInInspector] public int QualityLevelVAR;

    private const string resolutionWidthPlayerPrefKey = "ResolutionWidth";
    private const string resolutionHeightPlayerPrefKey = "ResolutionHeight";
    private const string resolutionRefreshRateNumeratorPlayerPrefKey = "RefreshRateNumerator";
    private const string resolutionRefreshRateDenominatorPlayerPrefKey = "RefreshRateDenominator";
    private const string fullScreenPlayerPrefKey = "FullScreen";
    private const string VSyncPlayerPrefKey = "VSync";

    [Header("Toggle Fullscreen")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Toggle VSync")]
    [SerializeField] private Toggle VSyncToggle;

    [Header("Resolutions")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    Resolution selectedResolution;
    [SerializeField] private AudioListener audioListener;

    [Header("FPSCounter")]
    [SerializeField] private Toggle showFPSToggle;
    [SerializeField] private GameObject FPSCounter;
    [HideInInspector] public bool ShowFPS;

    [Header("Texture Quality")]
    [SerializeField] private TMP_Dropdown TextureQualityControl;
    [HideInInspector] public int TextureQualityLevelVAR;

    // Start is called before the first frame update
    IEnumerator AL()
    {
        yield return new WaitForSeconds(1f);
        audioListener.enabled = true;
    }

    [System.Obsolete]
    void Start()
    {
        StartCoroutine(AL());
        resolutions = Screen.resolutions;
        LoadSettings();
        CreateResolutionDropdown();

        if (fullScreenToggle != null)
        {
            fullScreenToggle.onValueChanged.AddListener(SetFullscreen);
            VSyncToggle.onValueChanged.AddListener(SetVSync);
            showFPSToggle.onValueChanged.AddListener(SetShowFPS);
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
            TextureQualityControl.onValueChanged.AddListener(delegate { TextureQualityChangeCheck(); });
        }

        SettingsData data = SaveSystem.LoadSettings();

        if (data != null)
        {
            volumee = data.volumee;
            volumeSFX = data.volumeSFX;
            volumeMusic = data.volumeMusic;
            QualityLevelVAR = data.QualityLevelVAR;
            TextureQualityLevelVAR = data.TextureQualityLevelVAR;

            if (data.ShowFPS)
            {
                if (showFPSToggle != null)
                {
                    showFPSToggle.isOn = data.ShowFPS;
                }
            }
        }

        if (QualityControl != null)
        {
            QualityControl.value = QualityLevelVAR;
            QualitySettings.SetQualityLevel(QualityLevelVAR);
        }

        if (TextureQualityControl != null)
        {
            TextureQualityControl.value = TextureQualityLevelVAR;
        }

        if (SliderMasterVolume != null)
        {
            SliderMasterVolume.value = volumee;
            SliderSFXVolume.value = volumeSFX;
            SliderMusicVolume.value = volumeMusic;
        }
    }

    private void LoadSettings()
    {
        if (fullScreenToggle != null)
        {
            selectedResolution = new Resolution();
            selectedResolution.width = PlayerPrefs.GetInt(resolutionWidthPlayerPrefKey, Screen.currentResolution.width);
            selectedResolution.height = PlayerPrefs.GetInt(resolutionHeightPlayerPrefKey, Screen.currentResolution.height);
            selectedResolution.refreshRateRatio = new RefreshRate
            {
                numerator = (uint)PlayerPrefs.GetInt(resolutionRefreshRateNumeratorPlayerPrefKey, (int)Screen.currentResolution.refreshRateRatio.numerator),
                denominator = (uint)PlayerPrefs.GetInt(resolutionRefreshRateDenominatorPlayerPrefKey, (int)Screen.currentResolution.refreshRateRatio.denominator)
            };

            fullScreenToggle.isOn = PlayerPrefs.GetInt(fullScreenPlayerPrefKey, Screen.fullScreen ? 1 : 0) > 0;
            VSyncToggle.isOn = PlayerPrefs.GetInt(VSyncPlayerPrefKey, QualitySettings.vSyncCount > 0 ? 1 : 0) > 0;

            Screen.SetResolution(
                selectedResolution.width,
                selectedResolution.height,
                fullScreenToggle.isOn
            );
        }
    }

    private void CreateResolutionDropdown()
    {
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                // Calculate the refresh rate in Hz
                float refreshRateHz = (float)resolutions[i].refreshRateRatio.numerator / resolutions[i].refreshRateRatio.denominator;
                string option = resolutions[i].width + " x " + resolutions[i].height + ", " + Mathf.RoundToInt(refreshRateHz) + " Hz";
                options.Add(option);
                if (Mathf.Approximately(resolutions[i].width, selectedResolution.width) && Mathf.Approximately(resolutions[i].height, selectedResolution.height))
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(fullScreenPlayerPrefKey, isFullscreen ? 1 : 0);
        FindObjectOfType<AudioManager>().Play("Click");
    }

    public void SetVSync(bool isVSyncOn)
    {
        QualitySettings.vSyncCount = isVSyncOn ? 1 : 0;
        PlayerPrefs.SetInt(VSyncPlayerPrefKey, isVSyncOn ? 1 : 0);
        FindObjectOfType<AudioManager>().Play("Click");
    }

    public void SetResolution(int resolutionIndex)
    {
        selectedResolution = resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(resolutionWidthPlayerPrefKey, selectedResolution.width);
        PlayerPrefs.SetInt(resolutionHeightPlayerPrefKey, selectedResolution.height);
        PlayerPrefs.SetInt(resolutionRefreshRateNumeratorPlayerPrefKey, (int)selectedResolution.refreshRateRatio.numerator);
        PlayerPrefs.SetInt(resolutionRefreshRateDenominatorPlayerPrefKey, (int)selectedResolution.refreshRateRatio.denominator);
        FindObjectOfType<AudioManager>().Play("Click");
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
        volumee = volume;
        FindObjectOfType<AudioManager>().Play("Click");
        SaveSystem.SaveSettings(this);
    }

    public void SetSFXVolume(float volume1)
    {
        audioMixer.SetFloat("SFX", volume1);
        volumeSFX = volume1;
        FindObjectOfType<AudioManager>().Play("Click");
        SaveSystem.SaveSettings(this);
    }

    public void SetMusicVolume(float volume2)
    {
        audioMixer.SetFloat("Music", volume2);
        volumeMusic = volume2;
        FindObjectOfType<AudioManager>().Play("Click");
        SaveSystem.SaveSettings(this);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        QualitySettings.globalTextureMipmapLimit = TextureQualityControl.value;
        QualityLevelVAR = qualityIndex;
        FindObjectOfType<AudioManager>().Play("Click");
        SaveSystem.SaveSettings(this);
    }

    public void TextureQualityChangeCheck()
    {
        // Change the texture quality according to the dropdown's value
        QualitySettings.globalTextureMipmapLimit = TextureQualityControl.value;
        TextureQualityLevelVAR = TextureQualityControl.value;
        FindObjectOfType<AudioManager>().Play("Click");
        SaveSystem.SaveSettings(this);
    }

    public void Update()
    {
        if (FPSCounter != null)
        {
            if (ShowFPS)
            {
                FPSCounter.GetComponent<RectTransform>().anchoredPosition = new Vector3(-10, -10, 0);
            }
            else
            {
                FPSCounter.GetComponent<RectTransform>().anchoredPosition = new Vector3(1000, -10, 0);
            }
        }
    }

    public void SetShowFPS(bool IsShowFPSOn)
    {
        ShowFPS = IsShowFPSOn;
        FindObjectOfType<AudioManager>().Play("Click");
        SaveSystem.SaveSettings(this);
    }
}

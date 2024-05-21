[System.Serializable]
public class SettingsData
{
    public float volumee;
    public float volumeSFX;
    public float volumeMusic;
    public int QualityLevelVAR;
    public int TextureQualityLevelVAR;
    public bool ShowFPS;

    public SettingsData(Settings S)
    {
        volumee = S.volumee;
        volumeSFX = S.volumeSFX;
        volumeMusic = S.volumeMusic;
        QualityLevelVAR = S.QualityLevelVAR;
        TextureQualityLevelVAR = S.TextureQualityLevelVAR;
        ShowFPS = S.ShowFPS;
    }
}

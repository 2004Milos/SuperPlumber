using UnityEngine;
using UnityEngine.UI;

public class SerttingsManager : MonoBehaviour
{
    public GameObject SettingsPanel;
    public AudioSource As;
    public Scrollbar SB;

    public void openSettings() {
        SettingsPanel.SetActive(true);
        SB.value = As.volume;
    }

    public void closeSettings() {
        SettingsPanel.SetActive(false);
    }

    public void resetHighScore() {
        PlayerPrefs.SetInt("HighScorePref", 0);
    }

    public void AudioVolume() {
        As.volume = SB.value;
        PlayerPrefs.SetFloat("Volume", SB.value);
        DontDestroyOnLoad(SB);
        
    }
}

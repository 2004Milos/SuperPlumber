using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public GameObject CreditsPanel;

    public void Open() {
        CreditsPanel.SetActive(true);
    }

    public void Close() {
        CreditsPanel.SetActive(false);
    }

    public void LinkTunePolo() {
        Application.OpenURL("https://tunepolo.itch.io/");
    }

    public void LinkCC() {
        Application.OpenURL("https://creativecommons.org/licenses/by/4.0/");
    }

    public void LinkAutorMuzike() {
        Application.OpenURL("https://www.playonloop.com/2018-music-loops/wacky-race/");
    }
}

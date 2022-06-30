using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifeMenager : MonoBehaviour
{
    public GameObject[] zivoti;

    public Collider2D policaZaKameru;

    public GameObject PanelKraj;
    public GameObject PanelStart;

    public Text scoreText;
    public Text HighScore;
    public int brZivota;
    public static double BestScore = 0;
    public double Score;

    #region naPanelu
    public Text scoreTextPnaPanelu;
    public Text BestScoreText;
    #endregion

    private void Start()
    {
        PanelStart.SetActive(true);
        policaZaKameru.enabled = true;
        HighScore.gameObject.SetActive(false);
    }
    void Update()
    {
        for (int i = 2; i > brZivota-1; i--)
        {
            zivoti[i].SetActive(false);
        }
        if (Score > BestScore)
        {
            BestScore = Score;
            PlayerPrefs.SetInt("HighScorePref", Convert.ToInt32(BestScore));
            HighScore.gameObject.SetActive(true);
        }
        if (brZivota == 0)
        {
            PanelKraj.SetActive(true);
            scoreTextPnaPanelu.text = "Score: " + Score.ToString("00");
            BestScoreText.text = "High score: " + BestScore.ToString("00");
            FindObjectOfType<BGScroll>().enabled = false;
            foreach (KretanjeCevi KC in FindObjectsOfType<KretanjeCevi>())
                KC.enabled = false;
        }
        scoreText.text = Score.ToString("00");
    }
    public void PlayAgain()
    {
        PanelKraj.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartPlay()
    {
        BestScore = PlayerPrefs.GetInt("HighScorePref", 0); //0 je default, tj ako je igra pokenuta 1. put
        FindObjectOfType<KretanjeCevi>().As.PlayOneShot(FindObjectOfType<KretanjeCevi>().Clip);
        PanelStart.SetActive(false);
        policaZaKameru.enabled = false;
    }
}

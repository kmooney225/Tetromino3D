using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public static UIHandler instance;
    public Text scoreText;
    public Text levelText;
    public Text layersText;

    public Text highScoreText;

    public GameObject gameOverWindow;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameOverWindow.SetActive(false); }

    public void UpdateUI(int score, int level, int layers, int highScore)
    {
        scoreText.text = "Score: " + score.ToString("D9");
        levelText.text = "Level: " + level.ToString("D2");
        layersText.text = "Layers: " + layers.ToString("D9");
        highScoreText.text = "High Score: " + highScore.ToString();
    }

   // public void UpdateHSUI(int highScore)
    //{
     //   highScoreText.text = "High Score: " + highScore.ToString();
    //}

    public void ActivateGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public Text highScore;

    public void GameOverScript() {
        highScore.text = PlayerPrefs.GetInt("High Score",0).ToString();
    }
}

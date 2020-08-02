using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int score;
    int levels;
    int layersCleared;

    int highScore;

    bool gameIsOver;

    float fallSpeed;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetScore(score);
        PlayerPrefs.GetInt("High Score", highScore);
    }

    public void SetScore(int amount)
    {
        score += amount;
        if (highScore < score)
        {

            PlayerPrefs.SetInt("High Score", score);

        }


        CalculateLevel();
        SetHighScore();
        UIHandler.instance.UpdateUI(score,levels,layersCleared, highScore);
        //Update UI
    }

    public void SetHighScore()
    {
        if (highScore < score)
        {
            highScore = score;
            PlayerPrefs.SetInt("High Score", highScore);
            PlayerPrefs.Save();

        }
        //Update UI
    }

    public float ReadFallSpeed()
    {
        return fallSpeed;
    }

    public void LayersCleared(int amount)
    {
        if(amount == 1)
        {
            SetScore(400);
        }

        else if (amount == 2)
        {
            SetScore(800);
        }

        else if (amount == 3)
        {
            SetScore(1600);
        }

        else if (amount == 4)
        {
            SetScore(3200);
        }

        layersCleared += amount;
        UIHandler.instance.UpdateUI(score, levels, layersCleared, highScore);
        //Update UI
    }

    void CalculateLevel()
    {
        if(score <= 7000)
        {
            levels = 1;
            fallSpeed = 3f;
        }

        else if(score>7000 && score <= 12000)
        {
            levels = 2;
            fallSpeed = 2.7f;
        }
        else if (score > 12000 && score <= 20000)
        {
            levels = 3;
            fallSpeed = 2.4f;
        }
        else if (score > 20000 && score <= 32000)
        {
            levels = 4;
            fallSpeed = 2.15f;
        }
        else if (score > 32000 && score <= 45000)
        {
            levels = 5;
            fallSpeed = 1.9f;
        }
        else if (score > 45000 && score <= 60000)
        {
            levels = 6;
            fallSpeed = 1.65f;
        }
        else if (score > 60000 && score <= 89000)
        {
            levels = 7;
            fallSpeed = 1.4f;
        }
        else if (score > 89000 && score <= 107000)
        {
            levels = 8;
            fallSpeed = 1.2f;
        }
        else if (score > 107000 && score <= 134000)
        {
            levels = 9;
            fallSpeed = 1f;
        }
        else if (score > 134000 && score <= 150000)
        {
            levels = 10;
            fallSpeed = 0.8f;
        }


    }

    public bool ReadGameIsOver()
    {
        return gameIsOver;
    }

    public void SetGameIsOver()
    {
        gameIsOver = true;

        UIHandler.instance.ActivateGameOverWindow();
    }
}

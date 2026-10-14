using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    
    public IntVariable gameScore; // trying out the Scriptable Object

    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    private int score = 0;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        gameScore.Value = 0; // testing SO
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        SetScore(score);
        gameScore.ApplyChange(1); // testing SO
    } 

    public void SetScore(int score)
    {
        // invoke score change event with current score to update HUD
        scoreChange.Invoke(gameScore.Value); // testing SO
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        Debug.Log("is this going too fast");
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
    
    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        SetScore(score);
    }
}
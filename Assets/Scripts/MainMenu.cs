using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public GameObject highscoreText;
    public IntVariable gameScore;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetHighScore();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetHighScore()
    {
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
    }

    public void GoToLoadScene()
    {
        SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Single);
    }

    public void ResetHighscore()
    {
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        gameScore.ResetHighestValue();
        //set highscore
        SetHighScore();
    }




}

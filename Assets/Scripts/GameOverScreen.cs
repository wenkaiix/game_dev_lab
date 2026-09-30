using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // no intellisense for this which is weird 



public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI GameOver_score;


    
    public void Setup(int score){
        gameObject.SetActive(true);
        GameOver_score.text = "Score: " + score.ToString();

    }

    public void Retry(){
        SceneManager.LoadScene("Scene1");
        Time.timeScale = 1.0f;

    }
}

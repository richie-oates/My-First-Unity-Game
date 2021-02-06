using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
       public TextMeshProUGUI scoreText;
    public int score;
    public TextMeshProUGUI hiScoreText;
    public int hiScore;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    private bool gameActive;
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        hiScore = PlayerPrefs.GetInt ("highscore", hiScore);
        hiScoreText.text = "Hi-Score: " + hiScore;
        gameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
       
    }   
     public void ScoreChange(int points) 
    { 
        if (gameActive == true) 
        {
            score += points;
            scoreText.text = "Score: " + score;
            if (score > hiScore)
            {
            hiScore = score;
            }
            hiScoreText.text = "Hi-Score: " + hiScore;
        }
    }

    void OnDestroy()
    {
        // Saves highscore
        PlayerPrefs.SetInt ("highscore", hiScore);
        PlayerPrefs.Save();
    }

    public void GameOver()
    {
        gameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

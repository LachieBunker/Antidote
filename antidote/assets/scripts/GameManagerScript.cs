using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

    public string gameState;
    public int levelNum;

    public Canvas menuCanvas;

    public Canvas gameWonCanvas;

    public Image snakeImmuneImage;
    public Image vineImmuneImage;
    public Image smokeImmuneImage;
    
    public Image attemptNum1;
    public Image attemptNum2;
    public List<Sprite> numImages;

	// Use this for initialization
	void Start ()
    {
        Time.timeScale = 1.0f;
        gameState = "Playing";
        menuCanvas.gameObject.SetActive(false);
        gameWonCanvas.gameObject.SetActive(false);

        snakeImmuneImage.gameObject.SetActive(false);
        vineImmuneImage.gameObject.SetActive(false);
        smokeImmuneImage.gameObject.SetActive(false);
        

        SetAttemptsImages(ProgressManager.numAttempts);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameState == "Won")
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space))
            {
                NextLevel();
            }
        }
	}

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        gameState = "Paused";
        menuCanvas.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        gameState = "Playing";
        menuCanvas.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        ProgressManager.numAttempts++;
        SceneManager.LoadScene(levelNum);
    }

    public void Menu()
    {
        ProgressManager.numAttempts = 0;
        SceneManager.LoadScene(0);
    }

    public void GameWon()
    {
        print("GameWon");
        Time.timeScale = 0.0f;
        if (levelNum > ProgressManager.highestCompletedLevel)
        {
            ProgressManager.highestCompletedLevel = levelNum;
        }
        gameState = "Won";
        gameWonCanvas.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        ProgressManager.numAttempts = 0;
        //Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (SceneManager.sceneCountInBuildSettings <= levelNum + 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(levelNum + 1);
        }
        
        
    }

    public void SetAttemptsImages(int attemptsNum)
    {
        //Debug.Log(attemptsNum);

        NumImageScript numScript = GameObject.Find("UICanvas").GetComponent<NumImageScript>();

        int digit1 = attemptsNum % 10;
        //Debug.Log("Digit1 " + digit1);
        int digit2 = ((attemptsNum - digit1) % 100)/10;
        //Debug.Log("Digit2 " + digit2);
        numScript.attemptNum1.sprite = numScript.numImages[digit1];
        numScript.attemptNum2.sprite = numScript.numImages[digit2];
    }
}

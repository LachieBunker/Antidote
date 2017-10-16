using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {

    public List<GameObject> LevelButtons;
    public List<GameObject> NextButtons;

	// Use this for initialization
	void Start ()
    {
        SetActiveLevelButtons();
        SetNextPageButtons();

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void PlayLevel(int leveNum)
    {
        ProgressManager.numAttempts = 0;
        SceneManager.LoadScene(leveNum);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetActiveLevelButtons()
    {
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            if (i <= ProgressManager.highestCompletedLevel)
            {
                LevelButtons[i].SetActive(true);
            }
            else
            {
                LevelButtons[i].SetActive(false);
            }
        }
    }

    public void SetNextPageButtons()
    {
        
        for (int i = 0; i < NextButtons.Count; i++)
        {
            if (((float)ProgressManager.highestCompletedLevel / 10) >= i + 1)
            {
                NextButtons[i].SetActive(true);
                //Debug.Log(((float)ProgressManager.highestCompletedLevel / 10));
            }
            else
            {
                NextButtons[i].SetActive(false);
            }
        }
    }

    public void ResetProgress()
    {
        ProgressManager.highestCompletedLevel = 0;
        ProgressManager.numAttempts = 0;
    }

}

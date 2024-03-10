using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // Start is called before the first frame update

    public GameObject controlsMenu;
    

    public static bool isPaused;


    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {  
        if(Input.GetKeyDown(KeyCode.Escape))
        {


            if(isPaused)
            {
                ResumeGame();
            }
            if(!isPaused)
            {
                PauseGame();
            }

        }
        
    }

    public void openControls()
    {
        controlsMenu.SetActive(true);
    }

    public void closeControls()
    {
        controlsMenu.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        isPaused = false;

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public static PauseMenu Instance;

    private InputManager mInput;

    private bool GamePaused = false;

    [SerializeField] private AudioSource BackgroundMusic;

    private void Start()
    {
        mInput = GetComponent<InputManager>();
        Instance = this;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GamePaused = false;

        BackgroundMusic.Play();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GamePaused = true;

        BackgroundMusic.Pause();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void OnPause(InputValue value)
    {
        if (!value.isPressed){
            return;
        }

        if (GameOverMenu.Instance.gameOverMenuUI.activeInHierarchy){
            return;
        }
        
        if (GamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public static PauseMenu Instance;

    private InputManager mInput;

    private bool GamePaused = false;

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
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GamePaused = true;
    }

    public void OnPause(InputValue value)
    {
        if (!value.isPressed){
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

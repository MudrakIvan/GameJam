using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private InputManager mInput;

    private bool GamePaused = false;

    private void Start()
    {
        mInput = GetComponent<InputManager>();
    }

    void ResumeGame(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GamePaused = false;
    }

    void PauseGame(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GamePaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(mInput.Escape){
            if(GamePaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;
    [SerializeField] private AudioSource BackgroundMusic;
    [SerializeField] private AudioSource DeathBackgroundMusic;


    public static GameOverMenu Instance;
    
    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOverShow(){
        gameOverMenuUI.SetActive(true);
        BackgroundMusic.Stop();
        DeathBackgroundMusic.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

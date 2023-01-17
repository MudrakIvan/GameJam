using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }


    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        
#if UNITY_EDITOR
        // Quitting in Unity Editor: 
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER || UNITY_WEBGL
        // Quitting in the WebGL build: 
        //Application.OpenURL(Application.absoluteURL);
        Application.ExternalEval("window.open('" + Application.absoluteURL + "','_self')");
#else // !UNITY_WEBPLAYER
        // Quitting in all other builds: 
        Application.Quit();
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void LoadPlayerVSAI()
    {
        SceneManager.LoadScene("Scenes/PlayerVSAI");
        Time.timeScale = 1;
    }
    
    public void LoadPlayerVSZombies()
    {
        SceneManager.LoadScene("Scenes/PlayerVSZombies");
        Time.timeScale = 1;
    }

    public void LoadTargetRange()
    {
        SceneManager.LoadScene("Scenes/TargetRange");
        Time.timeScale = 1;
    }
    
    public void ReloadGame()
    {
        SceneManager.LoadScene("Scenes/Menu");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
        Time.timeScale = 1;
    }
}

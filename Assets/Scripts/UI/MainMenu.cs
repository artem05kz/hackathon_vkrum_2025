using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Sounds
{
    
    public void PlayGame()
    {
        PlaySound(sounds[1]);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        PlaySound(sounds[1]);
        Application.Quit();
    }

    void Start()
    {
        PlaySound(sounds[0], 0.15f);
    }
    void Update()
    {
        
    }
}

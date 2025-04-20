using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryManager : Sounds
{
    public GameObject victoryPanel;
    public float menuDelay = 5f;

    void Awake()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void ShowVictory()
    {
        PlaySound(sounds[0], 0.3f);
        Time.timeScale = 0f;
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        StartCoroutine(WaitAndLoadMenu());
    }

    private IEnumerator WaitAndLoadMenu()
    {
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(menuDelay);
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
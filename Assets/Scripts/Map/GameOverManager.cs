using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : Sounds
{
    [Header("UI Game Over")]
    public GameObject gameOverPanel;
    public Button rewindButton;

    [Tooltip("Сколько секунд даётся на Rewind")]
    public float rewindWindow = 3f;

    private PersonScript playerToRevive;
    private bool canRewind = false;

    void Awake()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(PersonScript player)
    {
        playerToRevive = player;
        gameOverPanel.SetActive(true);
        canRewind = true;

        rewindButton.onClick.AddListener(OnRewindClicked);

        StartCoroutine(RewindWindowRoutine());
    }

    private IEnumerator RewindWindowRoutine()
    {
        yield return new WaitForSeconds(rewindWindow);

        if (canRewind)
        {
            EndGame();
        }
    }

    private void OnRewindClicked()
    {
        if (!canRewind) return;
        canRewind = false;
        PlaySound(sounds[0], 0.2f);

        HideGameOver();

        TimeManager.Instance.StartTimeRewind();

        playerToRevive.Revive();
    }

    private void EndGame()
    {
        HideGameOver();
        LoadMainMenu();
    }

    private void HideGameOver()
    {
        rewindButton.onClick.RemoveListener(OnRewindClicked);
        gameOverPanel.SetActive(false);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

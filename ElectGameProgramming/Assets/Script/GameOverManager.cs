using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public tankSpawner allyTankSpawner;
    public tankSpawner enemyTankSpawner;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winnerAnnouncementText; // Text element to announce the winner
    public Button restartButton; // Reference to the restart button

    private float gameDuration = 30f; // Duration of the game in seconds

    void Start()
    {
        // Initialize UI elements
        winnerAnnouncementText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        StartCoroutine(GameDurationCountdown());
    }

    IEnumerator GameDurationCountdown()
    {
        float remainingTime = gameDuration;
        while (remainingTime > 0)
        {
            timerText.text = FormatTime(remainingTime);
            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        AnnounceWinner();
        PauseGame();
    }

    private void AnnounceWinner()
    {
        bool allyWins = allyTankSpawner.currentActivePercentage > enemyTankSpawner.currentActivePercentage;
        winnerAnnouncementText.text = allyWins ? "Allies Win!" : "Enemies Win!";
        winnerAnnouncementText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true); // Show the restart button
    }

    private void PauseGame()
    {
        
        Time.timeScale = 0; 
    }

    public void RestartGame()
    {
        
         SceneManager.LoadScene(0); // Reload the current scene
         Time.timeScale =1;
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int fraction = (int)((time * 100) % 100);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, fraction);
    }
}

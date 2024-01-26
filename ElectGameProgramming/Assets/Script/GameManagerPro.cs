using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // or using TMPro; if you're using TextMeshPro
using UnityEngine.SceneManagement;


public class GameManagerPro : MonoBehaviour
{
     public int playerMoney = 100;
    public Text moneyText; // or public TextMeshProUGUI moneyText; for TextMeshPro

    void Start()
    {
        UpdateMoneyUI();
    }

    public void ChooseSide(bool isAlly)
    {
        playerMoney -= 20; // Deduct the bet amount
        UpdateMoneyUI();

        // Save the player's choice (Ally or Enemy)
        PlayerPrefs.SetInt("PlayerChoice", isAlly ? 1 : 0);
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);

       // Load the next scene in sequence
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + playerMoney;
    }

    public void CheckGameOutcome(bool allyWon)
    {
        int playerChoice = PlayerPrefs.GetInt("PlayerChoice");
        if ((allyWon && playerChoice == 1) || (!allyWon && playerChoice == 0))
        {
            playerMoney += 40; // Win double the bet
        }
        // Loss is already accounted for in ChooseSide method

        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
        UpdateMoneyUI();

        // Handle game end, e.g., show end screen or return to the start screen
    }
}

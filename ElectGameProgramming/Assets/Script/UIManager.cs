using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class UIManager : MonoBehaviour
{
    public tankSpawner allyTankSpawner; // Reference to the ally tankSpawner script
    public tankSpawner enemyTankSpawner; // Reference to the enemy tankSpawner script

    public TextMeshProUGUI allyPercentageTMP; // Reference to the TMP element for allies
    public TextMeshProUGUI enemyPercentageTMP; // Reference to the TMP element for enemies

    void Update()
    {
        if (allyTankSpawner != null && allyPercentageTMP != null)
        {
            // Update the TMP Text with the current active percentage for allies
            allyPercentageTMP.text = "Ally Spawn Percentage: " + allyTankSpawner.currentActivePercentage.ToString("F2") + "%";
        }

        if (enemyTankSpawner != null && enemyPercentageTMP != null)
        {
            // Update the TMP Text with the current active percentage for enemies
            enemyPercentageTMP.text = "Enemy Spawn Percentage: " + enemyTankSpawner.currentActivePercentage.ToString("F2") + "%";
        }
    }
}

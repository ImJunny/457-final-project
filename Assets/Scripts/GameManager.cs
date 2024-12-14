using UnityEngine;
using TMPro; // Add this for TextMeshPro

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Reference to the TextMeshProUGUI UI element
    public EnemySpawner enemySpawner; // Reference to the EnemySpawner to stop spawning enemies

    private void Start()
    {
        if (messageText != null)
        {
            messageText.text = ""; // Initially, no message is displayed
        }
    }

    public void GameOver()
    {
        // Change the text to "Game Over"
        if (messageText != null)
        {
            messageText.text = "Game Over";
        }

        // Stop the enemy spawner from spawning new enemies
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        // Destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy); // Destroy all enemies
        }
    }
}
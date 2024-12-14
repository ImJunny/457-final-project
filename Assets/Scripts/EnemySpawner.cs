using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the Enemy prefab
    public Transform goal; // Reference to the Goal object
    public float spawnInterval = 2f; // Time between enemy spawns
    public float spawnRange = 10f; // Maximum radius for spawn area (distance from the goal)
    public float spawnHeight = 5f; // Height offset for spawn area (how high above or below the goal)
    public float minSpeed = 3f; // Minimum speed for enemy
    public float maxSpeed = 8f; // Maximum speed for enemy
    public int enemiesToSpawn = 3; // Number of enemies to spawn per interval

    private float spawnTimer;
    private bool canSpawn = true; // Flag to control if spawning is allowed

    void Update()
    {
        if (canSpawn)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemies(); // Spawn multiple enemies
                spawnTimer = 0f;
            }
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Randomly select an angle within a 180-degree arc above the goal, from -90 to 90 degrees
            float randomAngle = Random.Range(0, Mathf.PI); // Random angle between -90 and 90 degrees

            // Convert the angle to radians
            float angleInRadians = randomAngle * Mathf.Deg2Rad;

            // The radius is fixed as spawnRange, so no randomness here
            float radius = spawnRange;

            // Calculate the spawn position in world space
            float spawnX = goal.position.x + radius * Mathf.Cos(randomAngle);
            float spawnY = goal.position.y + radius * Mathf.Sin(randomAngle);

            // Z should remain fixed at -5
            float spawnZ = -5;

            // Spawn the enemy at the calculated position
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ); // Adjust X and Y, Z is fixed
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Set the random speed for the enemy
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            newEnemy.GetComponent<Enemy>().SetGoal(goal);
            newEnemy.GetComponent<Enemy>().SetSpeed(randomSpeed); // Set the speed on the enemy
        }
    }



    public void StopSpawning()
    {
        canSpawn = false; // Stop spawning new enemies
    }
}
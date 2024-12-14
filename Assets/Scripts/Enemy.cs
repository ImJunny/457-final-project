using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float moveSpeed; // Speed of the enemy
    private Transform goal;
    private Rigidbody rb; // Rigidbody reference for physics interactions
    private bool shouldMove = true; // Flag to control if the enemy should move towards the goal
    public float maxDistance = 15f; // Max distance from the goal at which the enemy will be destroyed
    private Vector3 spawnPosition; // Store spawn position to compare distance

    // Reference to GameManager to trigger Game Over
    public GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component at the start
        spawnPosition = transform.position; // Save the spawn position

        // Find the GameManager at runtime
        gameManager = FindObjectOfType<GameManager>();

        // You can check if the GameManager is null to avoid errors
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    public void SetGoal(Transform targetGoal)
    {
        goal = targetGoal;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed; // Set the speed for the enemy
    }

    void Update()
    {
        if (goal == null || !shouldMove) return; // If we shouldn't move, do nothing

        // Check if the enemy is too far from the goal and destroy it if it is
        float distanceToGoal = Vector3.Distance(transform.position, goal.position);
        if (distanceToGoal > maxDistance)
        {
            Destroy(gameObject); // Destroy the enemy if it's too far
            return; // Exit the method to avoid further calculations
        }

        // Move towards the goal
        Vector3 direction = (goal.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Make the enemy face the goal
        if (direction != Vector3.zero) // Prevent errors when direction is zero
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation; // Align the forward vector with the direction to the goal
        }
    }

    // This method is called when the enemy collides with another object
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            // Debug log to confirm collision with the goal
            Debug.Log("Enemy collided with Goal!");

            // Trigger game over
            if (gameManager != null)
            {
                gameManager.GameOver();
            }

            // destroy the enemy after reaching the goal
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // stop moving towards the goal temporarily
            shouldMove = false;

            // bounce away from the player using physics forces
            Vector3 bounceDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(bounceDirection * moveSpeed, ForceMode.Impulse);

            // destroy the enemy after bouncing
            Destroy(gameObject, 3f);
        }
    }
}
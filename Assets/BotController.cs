using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public float maxSpeed = 5f; // Maximum speed of the bot
    public float reactionTime = 0.5f; // Time before the bot reacts to the ball (optional)
    public float ballCheckInterval = 2f; // Time interval to check for new balls
    public float lerpSpeed = 10f; // Speed at which the bot lerps to the target position

    // Min and max bounds for movement
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    // To switch between defending axis (X or Z)
    public enum DefendingAxis { X, Z }
    public DefendingAxis defendingAxis = DefendingAxis.X;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Vector3 targetVelocity;
    private Vector3 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentVelocity = Vector3.zero;
    }

    void Update()
    {
        // Access the balls list from GameManager Singleton
        var balls = GameManager.Instance.pool.GetAllActiveBalls();

        // If there are no balls in the list, do nothing
        if (balls.Count == 0)
        {
            rb.linearVelocity = Vector3.zero; // Stop movement if no balls are present
            return;
        }

        // Find the closest ball
        Transform closestBall = FindClosestBall(balls);

        if (closestBall != null)
        {
            // Calculate the target position based on the defending axis
            switch (defendingAxis)
            {
                case DefendingAxis.X:
                    targetPosition = new Vector3(closestBall.position.x, transform.position.y, transform.position.z);
                    targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                    break;
                case DefendingAxis.Z:
                    targetPosition = new Vector3(transform.position.x, transform.position.y, closestBall.position.z);
                    targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);
                    break;
            }

            // Move the bot smoothly using Rigidbody
            MoveToTarget();
        }
    }

    // Smooth movement towards the target position
    void MoveToTarget()
    {
        // Calculate direction to the target and clamp it to the max speed
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Only move along the selected axis (X or Z)

        // Get the desired velocity based on the direction and max speed
        targetVelocity = direction.normalized * maxSpeed;

        // Smoothly adjust the velocity
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * lerpSpeed);

        // Apply the smoothed velocity to the Rigidbody
        rb.linearVelocity = currentVelocity;
    }

    // Find the closest ball from the GameManager's list
    Transform FindClosestBall(List<GameObject> balls)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject ball in balls)
        {
            float distance = Vector3.Distance(transform.position, ball.transform.position);

            if (distance < minDistance)
            {
                closest = ball.transform;
                minDistance = distance;
            }
        }

        return closest;
    }
}

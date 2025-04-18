using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneParentHandler : MonoBehaviour
{
    float rotation = 0f;
    public Transform playerTransform; // Reference to the player
    public float followDistance = 0f; // How close the rune gets to the player
    public float verticalOffset = 2.0f; // Adjust this to position the rune vertically
    public float horizontalOffset = 0.0f; // Add horizontal offset if needed
    public float maxMoveSpeed = 15.0f; // Increased for faster movement
    public float acceleration = 20.0f; // Increased for quicker response
    public float deceleration = 12.0f; // How quickly it slows down

    private float currentZ; // Store the Z value to keep it unchanged
    private Vector3 currentVelocity = Vector3.zero; // Current velocity vector

    void Start()
    {
        // Find the exported knight specifically if not set
        if (playerTransform == null)
        {
            // Try to find the exported knight directly for more accurate positioning
            Transform knightTransform = GameObject.FindGameObjectWithTag("Knight")?.transform;
            if (knightTransform != null)
            {
                // Try to find the actual knight model if possible
                Transform exportedKnight = knightTransform.Find("exported knight");
                if (exportedKnight != null)
                {
                    playerTransform = exportedKnight;
                }
                else
                {
                    playerTransform = knightTransform;
                }
            }
        }

        // Store the initial Z value
        currentZ = transform.position.z;
    }

    void Update()
    {
        // Update the rotation
        gameObject.transform.rotation = Quaternion.Euler(rotation, -90, 0);
        rotation++;
        rotation %= 360;

        if (playerTransform != null)
        {
            // Calculate target position with both vertical and horizontal offset
            Vector3 targetPosition = new Vector3(
                playerTransform.position.x + horizontalOffset,
                playerTransform.position.y + verticalOffset,
                playerTransform.position.z
            );
            targetPosition.z = currentZ; // Keep the original Z value

            // Direction and distance to target
            Vector3 directionToTarget = targetPosition - transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            // Calculate speed based on distance with a more aggressive curve
            float targetSpeed = Mathf.Clamp(distanceToTarget * 2.0f, 0f, maxMoveSpeed);

            // Calculate current speed
            float currentSpeed = currentVelocity.magnitude;

            // Apply acceleration or deceleration
            float newSpeed;
            if (targetSpeed > currentSpeed)
            {
                // Speed up (accelerate)
                newSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, targetSpeed);
            }
            else
            {
                // Slow down (decelerate)
                newSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, targetSpeed);
            }

            // Apply the new velocity
            if (distanceToTarget > 0.01f) // Prevent division by zero
            {
                currentVelocity = directionToTarget.normalized * newSpeed;
            }
            else
            {
                currentVelocity = Vector3.zero;
            }

            // Apply movement using the calculated velocity
            transform.position += currentVelocity * Time.deltaTime;

            // Ensure Z stays constant
            Vector3 pos = transform.position;
            pos.z = currentZ;
            transform.position = pos;
        }
    }
}
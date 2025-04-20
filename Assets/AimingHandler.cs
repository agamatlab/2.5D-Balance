using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingHandler : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Speed at which the aim rotates (higher = faster)")]
    public float rotationSpeed = 10f;
    [Tooltip("Whether to use smoothing for aim rotation")]
    public bool useSmoothing = true;
    [Tooltip("If not assigned in the Inspector, will grab Camera.main")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    float shift = 90f;

    [Header("Fireball Settings")]
    [Tooltip("The fireball prefab to instantiate")]
    [SerializeField]
    private GameObject fireballPrefab;
    [Tooltip("Speed of the fireball")]
    [SerializeField]
    private float fireballSpeed = 10f;
    [Tooltip("Reference to the aim child object (where fireball will spawn)")]
    [SerializeField]
    private Transform aimTransform;
    [Tooltip("Cooldown between shots in seconds")]
    [SerializeField]
    private float shootCooldown = 0.5f;

    private float nextFireTime = 0f;
    private Vector3 currentAimDirection;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // If aim transform not set, try to find the child named "aim"
        if (aimTransform == null)
        {
            Transform childAim = transform.Find("aim");
            if (childAim != null)
                aimTransform = childAim;
            else
                Debug.LogWarning("No aim transform assigned and no child named 'aim' found!");
        }

        // Reset to a neutral rotation (pointing right along +X)
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        HandleAiming();

        // Check for input to shoot
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            ShootFireball();
            nextFireTime = Time.time + shootCooldown;
        }
    }

    void HandleAiming()
    {
        // 1) Mouse in screen space
        Vector3 mouseScreen = Input.mousePosition;
        // 2) Compute the Z distance from camera to this object
        float objectScreenZ = mainCamera.WorldToScreenPoint(transform.position).z;
        // 3) Convert that screen point into world space
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(mouseScreen.x, mouseScreen.y, objectScreenZ)
        );
        // 4) Direction vector from this object to the mouse
        Vector3 dir = mouseWorld - transform.position;

        // Store the current aim direction for shooting
        currentAimDirection = dir.normalized;

        // 5) Compute angle in degrees (2D plane: Z‑axis rotation)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(-angle + shift, 90, 0);
        // 6) Apply rotation (smoothed or immediate)
        if (useSmoothing)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        else
            transform.rotation = targetRot;
    }

    void ShootFireball()
    {
        // Check if we have both a fireball prefab and an aim transform
        if (fireballPrefab == null)
        {
            Debug.LogError("Fireball prefab not assigned!");
            return;
        }

        if (aimTransform == null)
        {
            Debug.LogError("Aim transform not assigned!");
            return;
        }

        // Capture the direction at the moment of firing
        Vector3 firingDirection = currentAimDirection;

        // Create the fireball at the aim position
        GameObject fireball = Instantiate(fireballPrefab, aimTransform.position, Quaternion.identity);

        // Add a FireballMover component to handle movement
        FireballMover mover = fireball.AddComponent<FireballMover>();
        mover.Initialize(firingDirection, fireballSpeed);
    }
}

// Separate component to handle fireball movement
public class FireballMover : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifetime = 5f;
    private float timer = 0f;

    public void Initialize(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;

        // Set the rotation to match the direction
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }

    void Update()
    {
        // Move the fireball in the fixed direction
        transform.position += direction * speed * Time.deltaTime;

        // Handle lifetime
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;
using System.Collections;

/// <summary>
/// Controls time slowdown effects on collisions for a more impactful game feel.
/// </summary>
public class HitSlowMo : MonoBehaviour
{
    #region Singleton
    public static HitSlowMo Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("Slowdown Settings")]
    [Range(0.01f, 1f)]
    [Tooltip("How slow time becomes during slowdown (e.g., 0.2 = 20% speed)")]
    public float slowdownFactor = 0.2f;

    [Tooltip("Duration of the slowdown effect in seconds")]
    public float slowdownDuration = 0.15f;

    [Tooltip("Time to transition into slowdown in seconds")]
    public float fadeInTime = 0.05f;

    [Tooltip("Time to transition back to normal speed in seconds")]
    public float fadeOutTime = 0.1f;

    [Header("Collision Settings")]
    [Tooltip("How strong the collision must be to trigger maximum slowdown")]
    public float maxCollisionForce = 50f;

    [Tooltip("The minimum slowdown factor when collision is detected")]
    [Range(0.01f, 1f)]
    public float minSlowdownFactor = 0.5f;

    [Header("Cooldown")]
    [Tooltip("Time in seconds before another slowdown can occur")]
    public float cooldownTime = 0.5f;

    [Header("Camera Effects")]
    [Tooltip("Apply a small camera shake on hit")]
    public bool useCameraShake = true;

    [Tooltip("Maximum camera shake intensity")]
    [Range(0.01f, 1f)]
    public float maxShakeIntensity = 0.3f;

    [Tooltip("Camera shake duration in seconds")]
    public float shakeDuration = 0.2f;

    [Header("Debug")]
    [Tooltip("Show debug information in the console")]
    public bool debugMode = false;

    // Private variables
    private float defaultTimeScale = 1.0f;
    private float currentSlowdownFactor;
    private bool isInSlowdown = false;
    private bool isInCooldown = false;
    private Camera mainCamera;
    private Vector3 originalCameraPosition;

    private void Start()
    {
        defaultTimeScale = Time.timeScale;
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Call this method when a collision occurs to trigger the slowdown effect.
    /// </summary>
    /// <param name="collisionForce">Optional: The force of the collision to scale the slowdown effect.</param>
    public void TriggerSlowmotion(float collisionForce = 0f)
    {
        if (isInCooldown) return;

        // Calculate slowdown based on collision force if provided
        if (collisionForce > 0)
        {
            float forceFactor = Mathf.Clamp01(collisionForce / maxCollisionForce);
            currentSlowdownFactor = Mathf.Lerp(minSlowdownFactor, slowdownFactor, forceFactor);

            if (debugMode)
                Debug.Log($"Collision force: {collisionForce}, Slowdown factor: {currentSlowdownFactor}");
        }
        else
        {
            currentSlowdownFactor = slowdownFactor;
        }

        // Start the slowdown effect
        if (!isInSlowdown)
        {
            StopAllCoroutines();
            StartCoroutine(DoSlowmotion());

            if (useCameraShake && mainCamera != null)
            {
                float shakeIntensity = collisionForce > 0
                    ? Mathf.Lerp(0.1f, maxShakeIntensity, Mathf.Clamp01(collisionForce / maxCollisionForce))
                    : maxShakeIntensity;

                StartCoroutine(ShakeCamera(shakeIntensity));
            }
        }
    }

    /// <summary>
    /// Handle the slow motion effect with smooth transitions.
    /// </summary>
    private IEnumerator DoSlowmotion()
    {
        isInSlowdown = true;

        // Fade into slow motion
        float startTime = Time.unscaledTime;
        float endTime = startTime + fadeInTime;

        while (Time.unscaledTime < endTime)
        {
            float t = (Time.unscaledTime - startTime) / fadeInTime;
            Time.timeScale = Mathf.Lerp(defaultTimeScale, currentSlowdownFactor, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust physics timestep
            yield return null;
        }

        // Hold the slow motion
        Time.timeScale = currentSlowdownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(slowdownDuration);

        // Fade back to normal
        startTime = Time.unscaledTime;
        endTime = startTime + fadeOutTime;

        while (Time.unscaledTime < endTime)
        {
            float t = (Time.unscaledTime - startTime) / fadeOutTime;
            Time.timeScale = Mathf.Lerp(currentSlowdownFactor, defaultTimeScale, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        // Return to normal
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = 0.02f * defaultTimeScale;
        isInSlowdown = false;

        // Start cooldown
        StartCoroutine(Cooldown());
    }

    /// <summary>
    /// Simple camera shake effect to enhance the impact.
    /// </summary>
    private IEnumerator ShakeCamera(float intensity)
    {
        if (mainCamera == null) yield break;

        originalCameraPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float damping = 1.0f - (elapsed / shakeDuration);
            float x = Random.Range(-1f, 1f) * intensity * damping;
            float y = Random.Range(-1f, 1f) * intensity * damping;

            mainCamera.transform.localPosition = new Vector3(
                originalCameraPosition.x + x,
                originalCameraPosition.y + y,
                originalCameraPosition.z
            );

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalCameraPosition;
    }

    /// <summary>
    /// Prevents slowdown effects from occurring too frequently.
    /// </summary>
    private IEnumerator Cooldown()
    {
        isInCooldown = true;
        yield return new WaitForSecondsRealtime(cooldownTime);
        isInCooldown = false;
    }
}
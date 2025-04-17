
using UnityEngine;
using Cinemachine;


public class HitHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticlePrefab;
    [SerializeField] private LayerMask targetLayers;

    // Reference to the sword's collider (assign in inspector)

    // Optional: Different particle effects for different surface types
    [SerializeField] private ParticleSystem[] surfaceSpecificParticles;
    [SerializeField] private string[] surfaceTags;
    //CinemachineImpulseSource impulseSource;
    private void Start()
    {
        //impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    [Tooltip("Minimum velocity required to trigger slowdown")]
    public float minImpactVelocity = 2.0f;

    [Tooltip("Whether to use the collision force to scale the slowdown effect")]
    public bool useCollisionForce = true;

    [Tooltip("Scale the collision force by this multiplier")]
    public float collisionForceMultiplier = 1.0f;

    [Tooltip("Tags that this object will trigger slowdown when colliding with")]
    public string[] triggerTags = { "Player", "Enemy", "Destructible" };


    private void OnCollisionEnter(Collision collision)
    {
        // Check if we should react to this collision
        bool shouldTrigger = false;

        if (triggerTags.Length == 0)
        {
            shouldTrigger = true; // Trigger on any collision if no specific tags
        }
        else
        {
            foreach (string tag in triggerTags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    shouldTrigger = true;
                    break;
                }
            }
        }

        if (!shouldTrigger) return;

        // Check if the impact is strong enough
        //if (collision.relativeVelocity.magnitude < minImpactVelocity) return;

        // Calculate collision force based on velocity
        float collisionForce = 0f;
        if (useCollisionForce)
        {
            collisionForce = collision.relativeVelocity.magnitude * collisionForceMultiplier;
        }

        // Trigger the slowdown effect
        if (HitSlowMo.Instance != null)
        {
            HitSlowMo.Instance.TriggerSlowmotion(collisionForce);
        }
        // Check if we hit something in our target layers
        if (((1 << collision.gameObject.layer) & targetLayers) != 0)
        {
            // Get the exact collision contact point
            ContactPoint contact = collision.GetContact(0);
            Vector3 hitPoint = contact.point;
            Vector3 hitNormal = contact.normal;

            // Spawn the appropriate particle effect
            SpawnHitEffect(hitPoint, hitNormal, collision.gameObject);

            //if (collision.gameObject.TryGetComponent<KnockBack>( out KnockBack kb))
            //{
            //    impulseSource.GenerateImpulseWithForce(1);
            //    print("Call knocback");
            //    kb.CallKnockback(transform.right, Vector2.zero, 0);
            //}else if(collision.gameObject.TryGetComponent<MannequinTilt>( out MannequinTilt mt))
            //{
            //    impulseSource.GenerateImpulseWithForce(1);
            //    mt.OnHit(1, transform.right);
            //}

            // You can add more effects here (camera shake, hit stop, etc.)
        }
    }

    private void SpawnHitEffect(Vector3 position, Vector3 normal, GameObject hitObject)
    {
        // Create a rotation that faces along the normal direction
        Quaternion rotation = Quaternion.LookRotation(normal);

        // Check for surface-specific particles
        ParticleSystem particleToSpawn = hitParticlePrefab;

        for (int i = 0; i < surfaceTags.Length; i++)
        {
            if (hitObject.CompareTag(surfaceTags[i]) && i < surfaceSpecificParticles.Length)
            {
                particleToSpawn = surfaceSpecificParticles[i];
                break;
            }
        }

        // Instantiate the particle system at the exact hit point
        ParticleSystem spawnedParticles = Instantiate(particleToSpawn, position, rotation);

        // Set it to destroy itself after playing (if it doesn't already)
        Destroy(spawnedParticles.gameObject, spawnedParticles.main.duration + 0.5f);
    }
}
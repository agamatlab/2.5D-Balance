using UnityEngine;

public class KnightRagdollManager : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Collider[] originalColliders;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        // Get all Rigidbodies and Colliders in children
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        // Get original colliders (on root object)
        originalColliders = GetComponents<Collider>();

        // Disable ragdoll by default
        SetRagdollState(false);
    }

    void SetRagdollState(bool enabled)
    {
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = !enabled;
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = enabled;
        }

        foreach (var col in originalColliders)
        {
            col.enabled = !enabled;
        }

        if (characterController != null)
        {
            characterController.enabled = !enabled;
        }
    }

    public void Die()
    {
        animator.enabled = false;
        SetRagdollState(true);
        gameObject.layer = LayerMask.NameToLayer("deadEnemy");
    }
    private void Update()
    {
        // For testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Space key pressed. Triggering die animation.");
            Die();
        }
    }
}

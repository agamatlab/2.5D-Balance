using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHandler : MonoBehaviour
{
    Animator zombieAnimator;
    GameObject knight;

    public float moveSpeed = 2f;
    public float attackRange = 2f;

    public bool facingLeft = true;
    public bool attack = false;

    private Rigidbody rb;

    void Start()
    {
        zombieAnimator = GetComponentInChildren<Animator>();
        knight = GameObject.FindWithTag("Knight");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (knight == null) return;

        // Check direction to player
        bool shouldFaceLeft = knight.transform.position.x < transform.position.x;

        // Change direction if needed
        if (facingLeft != shouldFaceLeft)
        {
            facingLeft = shouldFaceLeft;
            transform.rotation = Quaternion.Euler(0, facingLeft ? 180 : 0, 0);
        }

        // Get distance to player
        float distance = Vector3.Distance(transform.position, knight.transform.position);

        // Attack if close, walk if far
        if (distance <= attackRange)
        {
            // Attack
            attack = true;
            zombieAnimator.SetBool("Attack", true);
            zombieAnimator.SetBool("Walking", false);
            rb.velocity = Vector3.zero;
        }
        else
        {
            // Walk
            attack = false;
            zombieAnimator.SetBool("Attack", false);
            zombieAnimator.SetBool("Walking", true);

            // Move toward player
            Vector3 direction = knight.transform.position - transform.position;
            direction.z = 0; // Keep on 2D plane
            direction.Normalize();

            rb.velocity = new Vector3(direction.x * moveSpeed, 0, 0);
        }
    }
}
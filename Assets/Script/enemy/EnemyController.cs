using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool alive;
    public int health;
    public PlayerAnimator playerAnimator;
    public GameObject enemy;
    public GameObject player;
    public bool hasCollided;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        //enemy = transform.gameObject;
        //enemy = GameObject.Find("Banana Man");
        //player =GameObject.FindWithTag("Player");
        Transform exportedKnight = player.transform.Find("exported knight");
        playerAnimator = exportedKnight.GetComponent<PlayerAnimator>();
        alive = true;
        health = 100;
        hasCollided = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (health <= 0)
        {
            alive = false;
            Destroy(enemy);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasCollided)
        {
            return;
        }

        if (timer > 1.5f && playerAnimator.isSwinging && (other.gameObject.CompareTag("weaponR") || other.gameObject.CompareTag("weaponL")))
        {
            Rigidbody weaponRb = other.attachedRigidbody;
            float collisionForce = 30f; // Default force

            if (weaponRb != null)
            {
                // Calculate force based on velocity magnitude
                collisionForce = weaponRb.velocity.magnitude * weaponRb.mass;
            }
            HitSlowMo.Trigger(collisionForce);
            health -= 40;
            hasCollided = true;
            timer = 0;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("weaponR") || other.gameObject.CompareTag("weaponL")){
        hasCollided = false;}
    }
}

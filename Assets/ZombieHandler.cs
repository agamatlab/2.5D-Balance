using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHandler : MonoBehaviour
{
    Animator zombieAnimator;
    GameObject knight;

    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public int damage = 10;

    public bool facingLeft = true;
    public bool attack = false;

    private Rigidbody rb;

    public enum AttackState
    {
        Started, Finished
    }

    public bool hasDealtDamage;

    public AttackState currentState = AttackState.Finished;

    public void AttackStart()
    {
        hasDealtDamage = false;
        currentState = AttackState.Started;
    }

    public void AttackFinish()
    {
        currentState = AttackState.Finished;
    }


    void Start()
    {
        zombieAnimator = GetComponent<Animator>();
        knight = GameObject.FindWithTag("Knight");
        
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, knight.transform.position) <attackRange)
        {
            zombieAnimator.SetBool("attack", true);
        }
        else
        {
            zombieAnimator.SetBool("attack", false);
        }
    }
}
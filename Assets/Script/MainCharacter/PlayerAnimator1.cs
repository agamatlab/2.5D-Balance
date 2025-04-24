using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimator2 : MonoBehaviour
{

    float direction = 0;

    [SerializeField]
    private float animationSpeed = 50;

    float animationConstant;
    public bool isSwinging;

    Animator playerAnimator;
    PlayerMovement playerMovementScript;

    public GameObject StrongAttackMiniGame;

    // Used for attacks and combos


    public float comboTimerLimit = 2f;
    private float comboTimer;
    public float attackCooldownLimit;
    private float attackCooldown = 0; 
    private int currentCombo = 0;

    // Start is called before the first frame update
    void Start()
    {
        animationConstant = 1 / animationSpeed;
        playerAnimator = GetComponent<Animator>();
        playerMovementScript = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

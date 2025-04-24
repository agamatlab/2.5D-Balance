using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    float direction = 0;

    [SerializeField]
    private float animationSpeed = 50;

    float animationConstant;
    public bool isSwinging;

    Animator playerAnimator;
    MyPlayerMovement playerMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        animationConstant = 1 / animationSpeed;
        playerAnimator = GetComponent<Animator>();
        playerMovementScript = GetComponentInParent<MyPlayerMovement>();
        playerAnimator.SetFloat("direcitonRange", 0);
    }

    // Used for attacks and combos
    public float comboTimerLimit = 2f;
    private float comboTimer;
    public float attackCooldownLimit;
    private float attackCooldown = 0;
    private int currentCombo = 0;

    // Update is called once per frame
    void Update()
    {

        // Canceling all previously  set animations
        playerAnimator.SetBool("swing1", false);
        playerAnimator.SetBool("swing2", false);
        playerAnimator.SetBool("swing3", false);

        // Timer for attack cooldown and combo
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        if (comboTimer > 0) comboTimer -= Time.deltaTime;

        if (comboTimer <= 0)
        {
            currentCombo = 0;
        }


        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("hit") )
        {
            playerAnimator.SetBool("hit", false);
        }   
        
        if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("stop") || playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("stop backwards"))
        {
            GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            playerAnimator.SetBool("directionChange", false);
        }

        if (isSwinging)
        {
            playerAnimator.SetBool("swing1", false);
        }


        isSwinging = playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("swing normal");

        if (isSwinging)
        {
            playerAnimator.SetBool("swing1", false);

        }

        float horizontal = Input.GetAxis("Horizontal");
        if (Input.GetMouseButtonDown(0) && !isSwinging && (playerMovementScript.balancePoint == 6 || playerMovementScript.isHoldingRight))
        {
            if (currentCombo == 0 && attackCooldown <= 0)
            {
                playerAnimator.SetBool("swing1", true);
                attackCooldown = attackCooldownLimit;
                comboTimer = comboTimerLimit;
                currentCombo++;
                Debug.Log("first attack");
            }
            else if (currentCombo == 1 && attackCooldown <= 0)
            {
                playerAnimator.SetBool("swing2", true);
                attackCooldown = attackCooldownLimit;
                comboTimer = comboTimerLimit;
                currentCombo++;

                Debug.Log("second attack");

            }
            else if (currentCombo == 2 && attackCooldown <= 0)
            {

                playerAnimator.SetBool("swing3", true);
                attackCooldown = attackCooldownLimit;
                currentCombo = 0;

                Debug.Log("third attack");
            }
            else
            {
                return;
            }
            //playerAnimator.SetBool("swing1", true);
            
            Vector2 mousePosition = Input.mousePosition;

            float screenWidth = Screen.width;
            playerAnimator.SetFloat("directionBinary", 1);
            playerMovementScript.balancePoint +=1;
            if (mousePosition.x < screenWidth / 2)
            {
                playerAnimator.SetFloat("directionBinary", 0);
                playerMovementScript.facingLeft = true;
            }
            else
            {
                playerAnimator.SetFloat("directionBinary", 1);
                playerMovementScript.facingLeft = false;

            }

        }
        else
        {
            if(horizontal > 0)
            {
                playerMovementScript.facingLeft = false;
            }else if(horizontal < 0)
            {
                playerMovementScript.facingLeft = true;
            }


        }



        float directionSlow = 5;
        if (horizontal > 0)
        {
            
            if(direction < 0)
            {
                playerAnimator.SetBool("directionChange", true);
            }
            direction += animationConstant/directionSlow;
        }
        else if (horizontal < 0)
        {
            if (direction > 0)
            {
                playerAnimator.SetBool("directionChange", true);
            }
            direction -= animationConstant/ directionSlow;

        }
        else
        {
            if (direction != 0)
            {
                direction -=  (direction/Mathf.Abs(direction)) *animationConstant/ directionSlow;
            }
        }


        playerAnimator.SetFloat("directionRange", direction);


        direction = Mathf.Clamp(direction, -1, 1);
        playerAnimator.SetBool("facingLeft", playerMovementScript.facingLeft);
        if (horizontal != 0)
        {

            playerAnimator.SetBool("running", true);
        }
        else
        {
            playerAnimator.SetBool("running", false);

        }





    }
}

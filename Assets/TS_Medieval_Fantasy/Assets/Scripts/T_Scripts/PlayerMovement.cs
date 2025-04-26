// Description : Manage Player animation and player movement
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed = 5;                                               // Player speed                
    private Rigidbody rb;
    public float jumpForce = 7;                                               // Jump force
    public float gravity = .6f;                                              // Player gravity

    public bool b_Jump = true;
    private Animator anim;
    private int RunHash = Animator.StringToHash("b_run");                        // Refers to Animator Parameters "b_run".
    private int JumpHash = Animator.StringToHash("b_Jump");                    // Refers to Animator Parameters "b_Jump".
    private int JumpingStateHash = Animator.StringToHash("Base.Jum");                    // Refers to Animator State Jum

    public GameObject obj_Player;
    public GameObject obj_Pivot_Player;

    private CapsuleCollider col;
    private float colCenterY;
    private float colHeight;


    public bool b_timer = true;
    private float timer = 0;
    public float target_Timer = .15f;

    public float Input_Value;
    public float smoothInputValue = 1;
    public float smoothInputValue2 = 1;

    public bool b_buttonLeft = false;
    public bool b_buttonRight = false;
    public bool b_buttonJump = false;
    public Animator playerAnimator;

    // New variables for smooth stopping
    private float lastNonZeroInputValue = 0f;
    private bool isStoppingAnimation = false;

    // Use this for initialization
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        anim = obj_Player.GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        col.height = colHeight;
        col.center = new Vector3(0, colCenterY, 0);
    }


    void FixedUpdate()
    {
        // Check if we're in a stopping animation state
        isStoppingAnimation = playerAnimator.GetBool("changeDirection") ||
                              playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("stop") ||
                              playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("stop backwards") ||
                              playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("swing normal");

        if (isStoppingAnimation)
        {
            // Apply smooth deceleration instead of immediate stop
            Vector3 currentVelocity = rb.velocity;
            float decelerationRate = 2.5f; // Lower value for more gradual stopping

            // Gradually reduce velocity to zero
            float newHorizontalVelocity = Mathf.Lerp(currentVelocity.x, 0f, Time.fixedDeltaTime * decelerationRate);

            // Apply the new velocity, keeping vertical component unchanged
            rb.velocity = new Vector3(newHorizontalVelocity, rb.velocity.y, rb.velocity.z);

            // Debug to verify deceleration is happening

            return;
        }

        // If not in stopping animation, apply normal movement
        rb.velocity = new Vector3(Input_Value * Speed * smoothInputValue * smoothInputValue, rb.velocity.y, rb.velocity.z);        // Move the player rigidbody

        if (b_timer)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);            // PLayer jump

            anim.SetBool(JumpHash, true);                                            // Play an animation
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - gravity, rb.velocity.z);            // Apply gravity to the player                                            
        }
    }

    public void PressButtonLeft()
    {
        b_buttonLeft = true;
        b_buttonRight = false;
    }

    public void ReleaseButtonLeft()
    {
        b_buttonLeft = false;
    }

    public void PressButtonRight()
    {
        b_buttonLeft = false;
        b_buttonRight = true;
    }

    public void ReleaseButtonRight()
    {
        b_buttonRight = false;
    }

    public void PressButtonJump()
    {
        b_buttonJump = true;
    }

    public void ReleaseButtonJump()
    {
        b_buttonJump = false;
    }


    void Update()
    {
        // Store the last non-zero input value and handle input
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            lastNonZeroInputValue = horizontalInput;
            Input_Value = horizontalInput * smoothInputValue;
        }
        else if (b_buttonLeft)
        {
            lastNonZeroInputValue = -1;
            Input_Value = -1;
        }
        else if (b_buttonRight)
        {
            lastNonZeroInputValue = 1;
            Input_Value = 1;
        }
        else
        {
            // Check if we should maintain momentum for stopping animations
            if (isStoppingAnimation)
            {
                // Don't zero out Input_Value yet - let the FixedUpdate deceleration handle it
                // This is key - we're not immediately zeroing velocity
                Input_Value = lastNonZeroInputValue * 0.5f; // Reduced input but not zero
            }
            else
            {
                // Normal case - not in a stop animation
                Input_Value = 0;
            }
        }

        if (Input.GetButtonDown("Jump") && b_Jump || b_buttonJump && b_Jump)
        {                                    // Player Jump
            Debug.Log("Jump");
            playerAnimator.SetTrigger("Jump");
            b_timer = true;
        }
        else if (Input.GetButtonUp("Jump") || !b_buttonJump && Input.GetButtonUp("Jump"))
        {
            b_timer = false;
            timer = 0;
        }

        if (b_timer)
        {
            timer = Mathf.MoveTowards(timer, target_Timer, Time.deltaTime);
            if (timer == target_Timer) { b_timer = false; timer = 0; }
        }

        RaycastHit hit;
        float distanceToGround = 0;                                                // Check distance to the ground
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0f))
        {
            distanceToGround = hit.distance;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);    // know what animation is active
        if (stateInfo.fullPathHash == JumpingStateHash && distanceToGround < .7f)
        {    // If the active state is MoveStateHash ("Base.Movement")
            if (!b_timer) b_Jump = true;
            anim.SetBool(JumpHash, false);

            colCenterY = Mathf.MoveTowards(colCenterY, 0.28f, Time.deltaTime * 5);
            colHeight = Mathf.MoveTowards(colHeight, 1.57f, Time.deltaTime * 5);
        }

        if (distanceToGround < .7f)
        {                                                // if player touch the ground
            if (!b_timer) b_Jump = true;
            anim.SetBool(JumpHash, false);

            colCenterY = Mathf.MoveTowards(colCenterY, 0.28f, Time.deltaTime * 5);
            colHeight = Mathf.MoveTowards(colHeight, 1.57f, Time.deltaTime * 5);
        }
        else
        {
            b_Jump = false;
            colCenterY = Mathf.MoveTowards(colCenterY, 0.58f, Time.deltaTime * 5);
            colHeight = Mathf.MoveTowards(colHeight, 1.22f, Time.deltaTime * 5);
        }

        if (Input.GetAxis("Horizontal") != 0 || b_buttonRight || b_buttonLeft) { anim.SetBool(RunHash, true); }
        else { anim.SetBool(RunHash, false); }

        col.height = colHeight;
        col.center = new Vector3(0, colCenterY, 0);
    }
}
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
    

    IEnumerator ResetEmoteTrigger(string trigger)
    {
        yield return null; // wait one frame
        playerAnimator.SetBool("trigger", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        animationConstant = 1 / animationSpeed;
        playerAnimator = GetComponent<Animator>();
        playerMovementScript = GetComponentInParent<MyPlayerMovement>();
        playerAnimator.SetFloat("direcitonRange", 0);
        //playerAnimator.speed = animationSpeed; // Plays animations at 2x speed

    }

    // Used for attacks and combos
    public float comboTimerLimit = 2f;
    private float comboTimer;
    public float attackCooldownLimit;
    //private float attackCooldown = 0;
    //private int currentCombo = 0;

    // Update is called once per frame
    void Update()
    {
        float balancePoint = playerMovementScript.balancePoint;

        // Canceling all previously  set animations
        playerAnimator.SetBool("swing1", false);
        playerAnimator.SetBool("swing2", false);
        playerAnimator.SetBool("swing3", false);

        // Timer for attack cooldown and combo
        //if (attackCooldown > 0) attackCooldown -= Time.deltaTime;


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
        if (Input.GetMouseButtonDown(0) && !isSwinging )
        {
            if (balancePoint==0)
            {
                playerAnimator.SetBool("swing1", true);

            }
            else if (balancePoint < 1 && balancePoint>0)
            {
                playerAnimator.SetBool("swing2", true);


            }
            else if (balancePoint < 2 && balancePoint>=1)
            {

                playerAnimator.SetBool("swing3", true);



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

            /*if(direction < 0)
            {
                playerAnimator.SetBool("directionChange", true);
            }
            direction += animationConstant/directionSlow;*/
            direction = 1;
        }
        else if (horizontal < 0)
        {
            /*if (direction > 0)
             {
                 playerAnimator.SetBool("directionChange", true);
             }
             direction -= animationConstant/ directionSlow;*/
            direction = -1;

        }
        else
        {
            if (direction != 0)
            {
                //direction -=  (direction/Mathf.Abs(direction)) *animationConstant/ directionSlow;
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

        /*if(Input.GetButtonDown("Jump") && playerMovementScript.isGrounded)
        {
            playerAnimator.SetBool("jump", true);
            playerMovementScript.isGrounded = false;
        }
        else
        {
            playerAnimator.SetBool("jump", false);
        }*/


        if (Input.GetKeyDown(KeyCode.L))
        {
            playerAnimator.SetTrigger("Emote1");
        }


    }
}

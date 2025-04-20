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
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if (Input.GetMouseButtonDown(0) && !isSwinging && (playerMovementScript.balancePoint == 6 || playerMovementScript.isHoldingRight))
        {
            playerAnimator.SetBool("swing1", true);
            Vector2 mousePosition = Input.mousePosition;

            float screenWidth = Screen.width;
            playerAnimator.SetFloat("directionBinary", 1);
            //playerMovementScript.balancePoint +=1;
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


        playerAnimator.SetFloat("directionRange", direction);

        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            if(direction < -0.5f)
            {
                playerAnimator.SetBool("directionChange", true);
                direction = -0.05f;
            }
            else
            {
                direction += animationConstant;
            }
        }
        else if (horizontal < 0)
        {
            if (direction > 0.5f)
            {
                playerAnimator.SetBool("directionChange", true);
                direction = 0.05f;
            }
            else
            {
                direction -= animationConstant;
            }

        }
        else
        {
            if (direction < -0.001 || direction > 0.001)
            {
                float step = (direction / Mathf.Abs(direction)) * animationConstant;
                direction -= step;
            }
        }

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

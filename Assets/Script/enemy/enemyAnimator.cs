using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class enemyAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private float Timer = 0f;
    public bool alert = false;
    private Canvas enemyCanvas;
    public Image alertIndicator;

    public Vector3 UIoffset = new Vector3(0, 2, 0);
    public Camera mainCamera;

    public float patrolSpeed;
    public float alertSpeed;
    public float alertRange;
    public Transform playerBody;
    public bool runningLeft;
    public bool attack;
    public EnemyController enemyController;
    public TextMeshProUGUI breakIndicator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("runningLeft", true);
        animator.SetBool("break", false);
        Timer = -1f;
        alert = false;
        enemyCanvas = GetComponentInChildren<Canvas>();
        UIoffset = new Vector3(0, 2, 0);
        patrolSpeed = 0.008f;
        alertSpeed = 0.016f;
        runningLeft = false;
        alertRange = 4f;
        //playerBody = GameObject.Find("Player").transform;
        //initialize UI for alert
        RectTransform rectTransform = alertIndicator.GetComponent<RectTransform>();
        //rectTransform.anchoredPosition = new Vector2(20, -100);
        rectTransform.sizeDelta = new Vector2(16, 16);
        alertIndicator.color = Color.green;
        attack = false;
        breakIndicator.enabled = false;

        rectTransform = breakIndicator.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0.05f * Screen.width, -0.2f * Screen.height);
        rectTransform.sizeDelta = new Vector2(0.1f * Screen.width, 0.02f * Screen.width);
    }


    bool checkPlayerInRange()
    {

        float x1 = transform.position.x;
        float x2 = playerBody.position.x;
        float diff = x1 - x2;
        if (diff > 0 && diff < alertRange && runningLeft)
        {
            return true;
        }
        if (diff < 0 && diff > -alertRange && !runningLeft)
        {
            return true;
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {        
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + UIoffset);
        alertIndicator.transform.position = screenPosition;
        alertIndicator.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);


        if(enemyController.health <= 0){
            breakIndicator.enabled = true;
            alertIndicator.enabled = false;
            animator.SetBool("break", true);
                    breakIndicator.transform.position = screenPosition;
            breakIndicator.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
            return;
        }

        //check for player

            transform.localRotation = Quaternion.Euler(0, 90, 0);



        if (!alert)
        {
            Timer += Time.deltaTime;
            alert = checkPlayerInRange();
            if (Timer < 3f && Timer >= 0f)
            {

                transform.position += new Vector3(patrolSpeed, 0, 0);
                animator.SetBool("runningLeft", false);
                runningLeft = false;
            }
            else if (Timer >= 3f && Timer < 6f)
            {
                transform.position += new Vector3(-patrolSpeed, 0, 0);
                animator.SetBool("runningLeft", true);
                runningLeft = true;
            }
            else if (Timer >= 6f)
            {
                Timer = 0;
            }
            alertIndicator.color = Color.green;
        }
        else
        {
            alertIndicator.color = Color.red;
            if (transform.position.x - playerBody.position.x > 0)
            {
                if (transform.position.x - playerBody.position.x > 1)
                {
                    animator.SetBool("attack", false);
                    attack = false;
                    transform.position += new Vector3(-alertSpeed, 0, 0);
                    animator.SetBool("runningLeft", true);
                    runningLeft = true;
                }
                else
                {
                    attack = true;
                    animator.SetBool("attack", true);
                }
            }
            else
            {
                if (transform.position.x - playerBody.position.x < -1)
                {
                    attack = false;
                    animator.SetBool("attack", false);
                    transform.position += new Vector3(alertSpeed, 0, 0);
                    animator.SetBool("runningLeft", false);
                    runningLeft = false;
                }
                else
                {
                    attack = true;
                    animator.SetBool("attack", true);
                }
            }
        }
    }
}

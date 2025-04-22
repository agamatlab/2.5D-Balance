using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZombieHandler : MonoBehaviour
{
    Animator zombieAnimator;
    GameObject knight;
    private float Timer = 0f;
    public float attackRange;
    public int damage = 10;
    public float alertRange;
    public bool alert = false;
    public bool facingLeft = true;
    public bool attack = false;

    private Rigidbody rb;
    public float health = 100;
    public Image alertIndicator;
    public Camera mainCamera;
    public Vector3 UIoffset = new Vector3(0, 2, 0);

    public Animator animator;
    //public float alertSpeed = 0.008f;
    public bool hasCollided = false;
    public PlayerAnimator playerAnimator;
    public TextMeshProUGUI breakIndicator;

    public enum AttackState
    {
        Started, Finished
    }

    public bool hasDealtDamage;

    public AttackState currentState = AttackState.Finished;
    public GameObject zombie;
    public void AttackStart()
    {
        hasDealtDamage = false;
        currentState = AttackState.Started;
    }

    public void AttackFinish()
    {
        currentState = AttackState.Finished;
    }

    bool checkPlayerInRange()
    {

        float x1 = transform.position.x;
        float x2 = knight.transform.position.x;
        float diff = x1 - x2;
        if (diff > 0 && diff < alertRange && facingLeft)
        {
            return true;
        }
        if (diff < 0 && diff > -alertRange && !facingLeft)
        {
            return true;
        }
        return false;
    }
    void Start()
    {
        zombieAnimator = GetComponent<Animator>();
        knight = GameObject.FindWithTag("Knight");
        //initialize UI for alert
        RectTransform rectTransform = alertIndicator.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0.05f * Screen.width, -0.2f * Screen.height);
        rectTransform.sizeDelta = new Vector2(0.02f * Screen.width, 0.02f * Screen.width);
        rectTransform = breakIndicator.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0.05f * Screen.width, -0.2f * Screen.height);
        rectTransform.sizeDelta = new Vector2(0.1f * Screen.width, 0.02f * Screen.width);
        alertIndicator.color = Color.green;

        attackRange = 0.6f;
        alertRange = 3f;
        animator = GetComponent<Animator>();
        breakIndicator.enabled = false;
        alertIndicator.enabled = true;
        
    }

    void Update()
    {


        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + UIoffset);
        alertIndicator.transform.position = screenPosition;
        alertIndicator.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        
        if(health <= 0){
            breakIndicator.enabled = true;
            alertIndicator.enabled = false;
            zombieAnimator.SetBool("break", true);
                    breakIndicator.transform.position = screenPosition;
            breakIndicator.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
            return;
        }
        if (facingLeft)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (!alert)
        {
            Timer += Time.deltaTime;
            alert = checkPlayerInRange();
            animator.SetFloat("Speed", 1.5f);
            if (Timer < 3f && Timer >= 0f)
            {

                facingLeft = true;
            }
            else if (Timer > 3f && Timer < 6f)
            {
                facingLeft = false;
            }
            else if (Timer >= 6f)
            {
                Timer = 0f;
            }
            alertIndicator.color = Color.green;
        }
        else
        {
            alertIndicator.color = Color.red;
            animator.SetFloat("Speed", 3f);
            if (transform.position.x - knight.transform.position.x > 0)
            {
                if (transform.position.x - knight.transform.position.x > attackRange)
                {
                    zombieAnimator.SetBool("attack", false);

                    //transform.position += new Vector3(-alertSpeed, 0, 0);
                    facingLeft = true;
                }
                else
                {
                    zombieAnimator.SetBool("attack", true);
                }
            }
            else
            {
                if (transform.position.x - knight.transform.position.x < -attackRange)
                {
                    zombieAnimator.SetBool("attack", false);

                    //transform.position += new Vector3(alertSpeed, 0, 0);
                    facingLeft = false;
                }
                else
                {
                    zombieAnimator.SetBool("attack", true);
                }
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (hasCollided)
        {
            return;
        }

        if (playerAnimator.isSwinging && (other.gameObject.CompareTag("weaponR") || other.gameObject.CompareTag("weaponL")))
        {
            if(health <=0){
                Destroy(zombie);
            }
            health -= 100;
            hasCollided = true;
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("weaponR") || other.gameObject.CompareTag("weaponL")){
        hasCollided = false;}
    }
}
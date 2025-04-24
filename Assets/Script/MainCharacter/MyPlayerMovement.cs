using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public interface IDamageable
{
    Vector3 Position { get; }
    void Damage(float damage);
}
public interface IDamagable
{
    int MaxHealth { get; }
    int Health { get; set; }
    public void TakeDamage(int damage);
}

public class MyPlayerMovement : MonoBehaviour, IDamagable
{
    public Rigidbody rb;
    [SerializeField]
    private float movementSpeed = 10;
    Animator playerAnimator;
    PlayerAnimator playerAniamationScript;
    //private Canvas balanceIndicatorCanvas;

    public float balancePoint;
    public Slider b1, b2, b3;
    public bool isHoldingRight;

    public bool facingLeft;
    public Slider healthbar;
    public Camera mainCamera;
    public Vector3 HealthBarOffset;
    public Vector3 BalanceUIOffset;
    int _maxHealth = 100;
    int _health = 100;

    public bool hasCollided;
    public enemyAnimator enemyAnimatorScript;

    public int MaxHealth => _maxHealth;

    public int Health
    {
        get => _health;
        set => _health = value;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerAniamationScript = GetComponentInChildren<PlayerAnimator>();


        balancePoint = 0;
        isHoldingRight = true;
        facingLeft = false;


        HealthBarOffset = new Vector3(0, 1, 0);
        BalanceUIOffset = new Vector3(0, 1.3f, 0);
        Health = 100;
        initBalanceIndicator();
        
    }
    void initBalanceIndicator()
    {
        RectTransform rectTransform1 = b1.GetComponent<RectTransform>();
        //rectTransform1.anchoredPosition = new Vector2(0.05f*Screen.width, -0.2f * Screen.height);


        RectTransform rectTransform2 = b2.GetComponent<RectTransform>();
        //rectTransform2.anchoredPosition = new Vector2(0.1f*Screen.width, -0.2f * Screen.height);


        RectTransform rectTransform3 = b3.GetComponent<RectTransform>();
        //rectTransform3.anchoredPosition = new Vector2(0.15f*Screen.width, -0.2f * Screen.height);


        rectTransform1.sizeDelta = rectTransform2.sizeDelta = rectTransform3.sizeDelta = new Vector2(40, 20);


    }

    void updateBalancePointUI()
    {
        if(balancePoint > 3){
            b1.value =b2.value =b3.value = 1;
            playerAnimator.SetFloat("swingspeed",0.7f);
        }
        else if(balancePoint <=3 && balancePoint >=2){
            b1.value =b2.value= 1;
            
            b3.value = balancePoint - 2;
            playerAnimator.SetFloat("swingspeed",0.5f);
        }
        else if(balancePoint <2 && balancePoint >=1){
            b1.value = 1;

            b2.value = balancePoint - 1;
            b3.value = 0;
            playerAnimator.SetFloat("swingspeed",1);
        }
        else{

            b1.value = balancePoint;
            
            b2.value =b3.value = 0;
            playerAnimator.SetFloat("swingspeed",1.2f);
        }
    }
    void Update()
    {
        if(Health <=0){
            SceneManager.LoadScene("DEMO_v2");
        }
        if (balancePoint > 0)
        {
            if(!playerAniamationScript.isSwinging){
            balancePoint -= Time.deltaTime * 0.5f;}
        }
        else
        {
            balancePoint = 0;
        }
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + HealthBarOffset);
        healthbar.transform.position = screenPosition;
        healthbar.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        healthbar.value = Health;

        Vector3 screenPosition2 = mainCamera.WorldToScreenPoint(transform.position + BalanceUIOffset);
        b1.transform.position = screenPosition2+ new Vector3(-40, 0, 0);
        b1.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        b2.transform.position = screenPosition2 ;
        b2.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        b3.transform.position = screenPosition2 + new Vector3(40, 0, 0);
        b3.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

        if (Input.GetMouseButtonDown(0) && !playerAniamationScript.isSwinging && isHoldingRight)
        {
            //rb.velocity = Vector3.zero;

        }
        else if (playerAniamationScript.isSwinging)
        {
            //rb.velocity = Vector3.zero;
        }
        else if (horizontal != 0f)
        {

            //rb.velocity = new Vector3(horizontal, 0, 0) * movementSpeed;
            if (horizontal > 0)
            {
                facingLeft = false;
            }
            else
            {
                facingLeft = true;
            }
        }
        else
        {

            //rb.velocity = Vector3.zero;
        }

        //rb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
        updateBalancePointUI();

    }
    Transform FindParentByName(Transform child, string name)
    {
        if (child == null)
            return null;


        if (child.name == name)
            return child;


        return FindParentByName(child.parent, name);
    }
    void OnTriggerEnter(Collider other)
    {
        if (hasCollided)
        {
            return;
        }

        if (other.gameObject.CompareTag("enemyweapon"))
        {
            Transform swordEnemy = FindParentByName(other.gameObject.transform, "swordenemy");
            enemyAnimatorScript = swordEnemy.GetComponent<enemyAnimator>();
            if (enemyAnimatorScript.attack)
            {
                Health -= 40;
                hasCollided = true;
            }
        }

    }


    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("enemyweapon"))
        {
            hasCollided = false;
        }
    }

}
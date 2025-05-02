using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool alive;
    public int health;
    public Animator playerAnimator;
    public GameObject enemy;
    public GameObject player;
    private float hitTimer = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        //enemy = transform.gameObject;
        //enemy = GameObject.Find("Banana Man");
        //player =GameObject.FindWithTag("Player");

        alive = true;
        health = 100;
        hitTimer = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer += Time.deltaTime;

    }

    void OnTriggerEnter(Collider other)
    {


        if (hitTimer > 1 && (other.gameObject.CompareTag("weaponR") || other.gameObject.CompareTag("weaponL")))
        {
            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("swing normal") ||playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SecondAtt")) {
                health -= 5;
                hitTimer = 0;
            }
            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("FinalAttack")){
                health -= 100;
                hitTimer = 0;
            }
        }

    }

}

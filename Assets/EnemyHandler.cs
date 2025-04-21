using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    Animator enemyController;
    GameObject knight;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponentInChildren<Animator>();
        knight = GameObject.FindWithTag("Knight"); 
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(gameObject.transform.position, knight.transform.position);
        print(distance);
        if (distance< 1f){
            enemyController.SetBool("attack", true);
        }
        else
        {
            enemyController.SetBool("attack", false);
        }
    }
}

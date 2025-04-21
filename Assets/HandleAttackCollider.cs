using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAttackCollider : MonoBehaviour
{
    [SerializeField]
    string targetTag;

    private ZombieHandler zombie;

    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponentInParent<ZombieHandler>();
        
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == targetTag && zombie.currentState == ZombieHandler.AttackState.Started &&!zombie.hasDealtDamage) {
            print("has hit");
            Animator animator = collision.gameObject.GetComponentInChildren<Animator>();
            animator.SetBool("hit", true);

            collision.gameObject.GetComponent<IDamagable>().Health -= zombie.damage;

            zombie.hasDealtDamage = true;
            HitSlowMo.Instance.TriggerSlowmotion(20);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isStoppingAnimation = animator.GetBool("changeDirection") ||
                      animator.GetCurrentAnimatorStateInfo(0).IsName("stop") ||
                      animator.GetCurrentAnimatorStateInfo(0).IsName("stop backwards") ||
                      animator.GetCurrentAnimatorStateInfo(0).IsName("swing normal");
        if (isStoppingAnimation)
        {
            gameObject.GetComponent<MeshRenderer>().material = mat1;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = mat2;

        }
        
    }
}

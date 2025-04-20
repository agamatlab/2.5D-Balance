using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneRotationHandler : MonoBehaviour
{
    float rotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(rotation, -90, 0);
        rotation++;
        rotation %= 360;


    }
}

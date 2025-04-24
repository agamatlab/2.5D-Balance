using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keppz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 new_pos = new Vector3(transform.position.x, transform.position.y, 0);
        Debug.Log(transform.position + " " + new_pos);
        //transform.position = new_pos;
    }
}

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
        Vector3 new_pos = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        Debug.Log("LOCAL position : " + transform.localPosition + "Global " + transform.position );
        transform.transform.localPosition = new_pos;
    }
}

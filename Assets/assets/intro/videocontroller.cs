using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class videocontroller : MonoBehaviour
{
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
float height = mainCamera.orthographicSize * 2;
float width = height * mainCamera.aspect;

transform.localScale = new Vector3(width / 10f, 1, height / 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

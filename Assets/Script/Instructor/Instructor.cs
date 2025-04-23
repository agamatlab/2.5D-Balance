using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructor : MonoBehaviour
{
    public Transform playerTransform;
    public Transform instructorTransform;
    public bool inRange;
    public GameObject canvas;
    public RectTransform instructionTransform;
    public Vector3 offset = new Vector3(0, 3, 0);
    public Camera mainCamera;

    public bool IsWithinRange()
    {
        
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);


        float distance = Vector2.Distance(currentPosition, playerPosition);

        return distance <= 1.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 3, 0);
                float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        inRange = false;
        instructionTransform.sizeDelta = new Vector2(screenWidth * 0.4f, screenHeight * 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        inRange = IsWithinRange();
        if(inRange){
            canvas.SetActive(true);
        }else{
            canvas.SetActive(false);
        }

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(instructorTransform.position + offset);
        instructionTransform.position = screenPosition;
        instructionTransform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }
}

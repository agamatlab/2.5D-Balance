using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBarController : MonoBehaviour
{
    public Slider healthbar;
    public Transform enemyTransform;
    public Vector3 offset = new Vector3(0, 2, 0);
    public Camera mainCamera;
    public ZombieHandler zombieHandler;
    // Start is called before the first frame update
    void Start()
    {

        offset = new Vector3(0, 1.6f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemyTransform.position + offset);


        healthbar.transform.position = screenPosition;


        healthbar.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        healthbar.value = (float)(zombieHandler.health);
    }
}

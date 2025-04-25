using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextLevel : MonoBehaviour
{
        public Transform playerTransform;
    public Transform instructorTransform;


    public bool IsWithinRange()
    {
        
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);


        float distance = Vector2.Distance(currentPosition, playerPosition);

        return distance <= 1.5f;
    }


    // Update is called once per frame
    void Update()
    {
        if(IsWithinRange()&&Input.GetKeyDown(KeyCode.E)){
            SceneManager.LoadScene("lv2");
        }
    }
}

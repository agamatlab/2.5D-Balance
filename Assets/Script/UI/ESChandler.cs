using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESChandler : MonoBehaviour
{

    public RectTransform ESCtransform;
    public bool isPaused = false;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        ESCtransform.sizeDelta = new Vector2(screenWidth * 0.075f, screenHeight * 0.05f);
        ESCtransform.anchorMin = new Vector2(0, 1);
        ESCtransform.anchorMax = new Vector2(0, 1); 
        ESCtransform.anchoredPosition = new Vector2(screenWidth * 0.075f, -screenHeight * 0.05f);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
}

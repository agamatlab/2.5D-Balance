using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
        public RectTransform startButton;
    public RectTransform exitButton;
    public ESChandler esc;
    // Start is called before the first frame update


        public void resume()
    {
        
        esc.isPaused = false;
    }

    public void ExitGame()
    {
        
        Application.Quit();
    }    
    
    void Start()
    {
                float screenWidth = Screen.width;
        float screenHeight = Screen.height;
                startButton.sizeDelta = new Vector2(screenWidth * 0.3f, screenHeight * 0.1f);
        startButton.anchorMin = new Vector2(0.5f, 0.5f);
        startButton.anchorMax = new Vector2(0.5f, 0.5f); 
        startButton.anchoredPosition = new Vector2(0, screenHeight * 0.15f);

        exitButton.sizeDelta = new Vector2(screenWidth * 0.3f, screenHeight * 0.1f);
        exitButton.anchorMin = new Vector2(0.5f, 0.5f);
        exitButton.anchorMax = new Vector2(0.5f, 0.5f); 
        exitButton.anchoredPosition = new Vector2(0, 0);
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            resume();
        }
                if (Input.GetKeyDown(KeyCode.Q))
        {
            ExitGame();
        }
    }
}

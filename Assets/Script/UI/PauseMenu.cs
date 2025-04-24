using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
        public RectTransform startButton;
    public RectTransform exitButton;
    public RectTransform resetButton;
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
    public void ResetGame()
    {
        
        SceneManager.LoadScene("DEMO_v2");
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

        resetButton.sizeDelta = new Vector2(screenWidth * 0.3f, screenHeight * 0.1f);
        resetButton.anchorMin = new Vector2(0.5f, 0.5f);
        resetButton.anchorMax = new Vector2(0.5f, 0.5f); 
        resetButton.anchoredPosition = new Vector2(0, -screenHeight * 0.15f);
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public RectTransform title;
    public RectTransform logo;
    public RectTransform startButton;
    public RectTransform exitButton;
    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        title.sizeDelta = new Vector2(screenWidth * 0.5f, screenHeight * 0.2f);
        title.anchorMin = new Vector2(0.5f, 0.5f);
        title.anchorMax = new Vector2(0.5f, 0.5f); 
        title.anchoredPosition = new Vector2(0, screenHeight * 0.1f);

        logo.sizeDelta = new Vector2(screenHeight * 0.15f, screenHeight * 0.15f);
        logo.anchorMin = new Vector2(1, 1);
        logo.anchorMax = new Vector2(1, 1); 
        logo.anchoredPosition = new Vector2(-screenHeight * 0.1f, -screenHeight * 0.1f);

        startButton.sizeDelta = new Vector2(screenWidth * 0.3f, screenHeight * 0.1f);
        startButton.anchorMin = new Vector2(0.5f, 0.5f);
        startButton.anchorMax = new Vector2(0.5f, 0.5f); 
        startButton.anchoredPosition = new Vector2(0, 0);

        exitButton.sizeDelta = new Vector2(screenWidth * 0.3f, screenHeight * 0.1f);
        exitButton.anchorMin = new Vector2(0.5f, 0.5f);
        exitButton.anchorMax = new Vector2(0.5f, 0.5f); 
        exitButton.anchoredPosition = new Vector2(0, -screenHeight * 0.15f);
    }
    void Update()
    {}
    public void StartGame()
    {
        
        SceneManager.LoadScene("DEMO_v2");
    }

    public void ExitGame()
    {
        
        Application.Quit();
    }
}

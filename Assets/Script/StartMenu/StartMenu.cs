using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{


    public void StartGame()
    {
        
        SceneManager.LoadScene("DEMO_v2");
    }

    public void ExitGame()
    {
        
        Application.Quit();
    }
}

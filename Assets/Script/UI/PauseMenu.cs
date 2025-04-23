using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

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

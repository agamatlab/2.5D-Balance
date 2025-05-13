using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class introVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("StartMenu");
    }
}

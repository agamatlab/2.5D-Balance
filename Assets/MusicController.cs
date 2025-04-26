using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource effects;

    public AudioClip MenuBackgroundMusic;
    public AudioClip GameBackgroundMusic;
    public AudioClip ZombieDie;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = transform.GetChild(0).GetComponent<AudioSource>();
        backgroundMusic.clip = MenuBackgroundMusic;
        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

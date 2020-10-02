using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Eigenanteil
// Static Audio Player, der mehrere Sounds lädt, speichert und bei bedarf abspielen kann
public class AudioPlayer: MonoBehaviour
{
      private static AudioPlayer audioPlayer = null;

      [SerializeField]
      public static AudioSource menuMusic, gameplayMusic, collectSound;

      public AudioMixer Master;


    private void Awake()
    {
        if (audioPlayer == null)
        { 
            audioPlayer = this;
            DontDestroyOnLoad(gameObject);
            // load audio sources in order they are placed in the inspector
            AudioSource[] audio = GetComponents<AudioSource>();
            menuMusic = audio[0];
            gameplayMusic = audio[1];
            collectSound = audio[2];
            // some sounds need to loop
            menuMusic.loop = true;
            gameplayMusic.loop = true;
            return;
        }
        if (audioPlayer == this)
        {
            return;
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    AudioPlayer.PlayMenuMusic();
    }

    public static void PlayMenuMusic()
    {
        gameplayMusic.Stop();
        menuMusic.Play();
    }

    public static void PlayGameplayMusic()
    {
        menuMusic.Stop();
        gameplayMusic.Play();
    }

    public static void PlayCollectSound()
    {
        collectSound.Play();
    }

}
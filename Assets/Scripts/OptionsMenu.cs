using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

// Eigenanteil
// Options Menü, womit Asteroid-Outlines, volume und fullscreen getoggled werden können
public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public static bool asteroidTailOutline = false, collectableAsteroidOutline = false;

    public void SetVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }

    public void SetAsteroidTailOutline(bool useTailOutline)
    {
        OptionsMenu.asteroidTailOutline = useTailOutline;
    }

    public void SetCollectableAsteroidOutline(bool useAsteroidOutline)
    {
        OptionsMenu.collectableAsteroidOutline = useAsteroidOutline;
    }
}

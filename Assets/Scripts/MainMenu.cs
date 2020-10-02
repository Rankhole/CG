using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

// Eigenanteil
// Funktionen zum Starten der Szenen
public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game!");
        Application.Quit();
    }

    public void SetVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Eigenanteil
public class Score : MonoBehaviour
{

    public GameObject scoreText;
    public GameObject timeText;

    public static int score;
    public static float time;


    void Start()
    {
        // initialize score to be loaded, for example when Game Over screen appears
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().SetText(score.ToString());
        timeText.GetComponent<TMPro.TextMeshProUGUI>().SetText(Score.time.ToString("F2"));
    }
    public void increaseScore()
    {
        score++;
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().SetText(score.ToString());
    }

}

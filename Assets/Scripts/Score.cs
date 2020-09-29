using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    public GameObject scoreText;
    public GameObject timeText;

    public int score = 0;
    // Start is called before the first frame update
    public void increaseScore()
    {
        score++;
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().SetText(score.ToString());
    }

    void Update()
    {
        timeText.GetComponent<TMPro.TextMeshProUGUI>().SetText(Time.time.ToString("F2"));
    }

}

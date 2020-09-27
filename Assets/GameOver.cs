using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{

    void Start()
    {
        Debug.Log("START");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("EXPLODE");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Eigenanteil, sorgt für das schrumpfen der Sphere
public class ShrinkingSphere : MonoBehaviour
{

    public float sphereSize;
    public GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newSize = sphereSize - Time.timeSinceLevelLoad;
        // shrink sphere (scale) by 1 for every second that passes
        sphere.GetComponent<Transform>().localScale = new Vector3(newSize, newSize, newSize);
    }
}

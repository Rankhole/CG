using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        sphere.GetComponent<Transform>().localScale = new Vector3(newSize, newSize, newSize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    [SerializeField]
    public GameObject asteroidPrefab;
    private List<GameObject> asteroids;
    public float radiusOfSphere = 1000f;
    public Vector3 center = new Vector3(0, 0, 0);
    public bool spherical = false;
    public int maxAsteroidSize = 200;

    // Start is called before the first frame update
    void Start()
    {
        asteroids = new List<GameObject>();
        for(int i = 0; i < maxAsteroidSize; i++)
        {
            SpawnAsteroid();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAsteroid()
    {
        var spawnedAsteroid = Instantiate(asteroidPrefab) as GameObject;
        spawnedAsteroid.tag = "Collectable";
        spawnedAsteroid.GetComponent<Collider>().enabled = true;
        var vec = new Vector3();
        if (spherical)
        {
            // create random vector pointing anywhere
            var x = Random.Range(-1f,1f);
            var y = Random.Range(-1f,1f);
            var z = Random.Range(-1f,1f);
            //normalize * radius of the sphere
            vec = new Vector3(x,y,z).normalized * Random.Range(0.0f, radiusOfSphere);
        } else {
            vec = new Vector3(Random.Range(- radiusOfSphere / 2, radiusOfSphere / 2), Random.Range(- radiusOfSphere / 2, radiusOfSphere / 2), Random.Range(- radiusOfSphere / 2, radiusOfSphere / 2));
        }
        spawnedAsteroid.transform.position = vec + center;
        spawnedAsteroid.transform.rotation = Random.rotation;

        asteroids.Add(spawnedAsteroid);
    }
}

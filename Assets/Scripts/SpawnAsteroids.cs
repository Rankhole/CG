using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Eigenanteil
// Kümmert sich um das Spawnen von Asteroiden (random position)
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
        // initialize max numbers of asteroids on the map
        asteroids = new List<GameObject>();
        for(int i = 0; i < maxAsteroidSize; i++)
        {
            SpawnAsteroid();
        }
    }

    // Spawns an asteroid
    public void SpawnAsteroid()
    {
        var spawnedAsteroid = Instantiate(asteroidPrefab) as GameObject;
        spawnedAsteroid.layer = 2;
        spawnedAsteroid.tag = "Collectable";
        spawnedAsteroid.GetComponent<Collider>().enabled = true;
        spawnedAsteroid.GetComponent<Collider>().isTrigger = true;

        if (OptionsMenu.collectableAsteroidOutline)
        {
            var outline = spawnedAsteroid.GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.green;
            outline.OutlineWidth = 2f;
        }

        var vec = new Vector3();
        // spherical would spawn the asteroids within a sphere
        // problem: in the middle of the sphere, there are seemingly "more asteroids", because of the mathematics of it
        // -> default is non spherical (cubic)
        if (spherical)
        {
            // create random vector pointing anywhere
            var x = Random.Range(-1f,1f);
            var y = Random.Range(-1f,1f);
            var z = Random.Range(-1f,1f);
            // normalized random vector * radius of the sphere = vector pointing to new asteroid location from the middle of sphere
            vec = new Vector3(x,y,z).normalized * Random.Range(0.0f, radiusOfSphere);
        } else { // cubic spawning of asteroid
            vec = new Vector3(Random.Range(- radiusOfSphere / 2, radiusOfSphere / 2), Random.Range(- radiusOfSphere / 2, radiusOfSphere / 2), Random.Range(- radiusOfSphere / 2, radiusOfSphere / 2));
        }
        // set position and rotation (randomly)
        spawnedAsteroid.transform.position = vec + center;
        spawnedAsteroid.transform.rotation = Random.rotation;

        asteroids.Add(spawnedAsteroid);
    }
}

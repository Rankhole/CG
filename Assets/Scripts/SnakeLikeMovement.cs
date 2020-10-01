using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Eigenanteil
public class SnakeLikeMovement : MonoBehaviour
{

    public List<Transform> asteroidTail = new List<Transform>();

    public int beginSize = 1;
    
    public float speed = 1000;
    // public float rotationSpeed = 100;

    [SerializeField]
    public GameObject asteroidPrefab;
    public int maxTrailSize = 200;
    public float speedOfInterp = 1f;

    public Transform spaceship;

    private Vector3[] trailpath, velocities;
    private Quaternion[] trailrotation;

    public GameObject scoreText, timeText;
    private Score scoreScript;

    private SpawnAsteroids spawnAsteroidsScript;

    // Start is called before the first frame update
    void Start()
    {
        // get Score script from text object
        scoreScript = scoreText.GetComponent<Score>();

        spawnAsteroidsScript = this.GetComponent<SpawnAsteroids>();

        // initialize arrays for tracking spaceship movement over time
        trailpath = new Vector3[maxTrailSize];
        trailrotation = new Quaternion[maxTrailSize];
        velocities = new Vector3[maxTrailSize];

        // head of our "tail" is the spaceship
        asteroidTail.Add(spaceship);

        // add first position of our trailpath and rotation
        trailpath[0] = asteroidTail[0].position;
        trailrotation[0] = asteroidTail[0].rotation;

        // call method to record spaceship transformation every 1 second
        InvokeRepeating("UpdateSpaceshipPath", 1f, 1f);  //1s delay, repeat every 1s

        // optionally add beginSize many Asteroids to tail
        for (int i = 0; i < beginSize - 1; i++)
        {
            AddBodyPart();
        }
    }

    public void UpdateSpaceshipPath()
    {
        // "save" the spaceships position and trickle it down -> saved state for each second passing
        // now we effectively have a "trail" that the asteroids can follow
        for (int i = trailpath.Length - 1; i > 0; i--)
        {
            trailpath[i] = trailpath[i-1];
            trailrotation[i] = trailrotation[i-1];
        }
        trailpath[0] = asteroidTail[0].position;
        trailrotation[0] = asteroidTail[0].rotation;
        // Move();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            AddBodyPart();
        
        Score.time = Time.timeSinceLevelLoad;
        timeText.GetComponent<TMPro.TextMeshProUGUI>().SetText(Score.time.ToString("F2"));
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        float time_between_secs = Time.time - Mathf.Floor(Time.time);
        // Debug.Log(time_between_secs);

        // iterate over all the asteroids and update their positions
        for (int i = 1; i < asteroidTail.Count; i++)
        {
            Transform currentAsteroid = asteroidTail[i];
            // New position of our asteroid x will be the position of our spaceship x seconds ago -> position x in trail arrays
            Vector3 newpos = trailpath[i];
            Quaternion newrot = trailrotation[i];
            // set the position
            currentAsteroid.position = Vector3.SmoothDamp(currentAsteroid.position, newpos, ref velocities[i], speedOfInterp);
            currentAsteroid.rotation = Quaternion.Slerp(currentAsteroid.rotation, newrot, Time.deltaTime);
            // currentAsteroid.position = newpos;
        }
    }


    public void AddBodyPart()
    {
        if(asteroidTail.Count == maxTrailSize){
            Debug.Log("Maximum number of asteroids reached!");
            return;
        }
        // instantiate an asteroid object
        GameObject createdAsteroid = Instantiate (asteroidPrefab, trailpath[asteroidTail.Count - 1], trailrotation[asteroidTail.Count - 1]) as GameObject;
        createdAsteroid.layer = 2;
        var outline = createdAsteroid.GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.green;
        outline.OutlineWidth = 2f;
        // set it's tag to "Collected", so that it will lead to game over if ship collides with it
        createdAsteroid.tag = "Collected";
        // enable collider (mesh)
        createdAsteroid.GetComponent<Collider>().enabled = true;
        Transform newpart = createdAsteroid.transform;
        // newpart.SetParent(transform);
        // finally, add the new asteroid to our asteroidTail list
        asteroidTail.Add(newpart);

        // increase score when collecting asteroid
        scoreScript.increaseScore();
    }

    public void OnTriggerEnter(Collider other)
    {
        //If tag is "Collectable", it means we collect the asteroid, destroy it, add it to our "snake body" and increase our score
        if (other.tag == "Collectable")
        {
            Debug.Log("COLLECTED");
            Destroy(other.gameObject);
            AddBodyPart();
            spawnAsteroidsScript.SpawnAsteroid();
        } else if (other.tag == "Collected"){ // Colliding with our own snake body = Game Over!
            Debug.Log("GAME OVER!");
            // Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Debug.Log("Object not tagged properly!");
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        Debug.Log("GAME OVER!");
            // Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }

}
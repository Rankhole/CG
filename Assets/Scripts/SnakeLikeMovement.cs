using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Eigenanteil
// Regelt das meiste der Spielmechaniken (Asteroid-Tail, Collisionen etc.)
public class SnakeLikeMovement : MonoBehaviour
{
    // list of transforms that form our asteroid tail
    public List<Transform> asteroidTail = new List<Transform>();

    public int beginSize = 1;
    
    [SerializeField]
    public GameObject asteroidPrefab;
    public int maxTrailSize = 200;
    public float speedOfInterp = 1f;

    public Transform spaceship;

    //the "trail" of the spaceship - this is the path that the asteroid tail should follow
    private Vector3[] trailpath, velocities;
    private Quaternion[] trailrotation;

    public GameObject scoreText, timeText;
    private Score scoreScript;

    private SpawnAsteroids spawnAsteroidsScript;

    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer.PlayGameplayMusic();

        // get Score script from text object and set score and time to 0
        scoreScript = scoreText.GetComponent<Score>();
        Score.score = 0;
        Score.time = 0f;
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().SetText(Score.score.ToString());
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().SetText(Score.time.ToString("F2"));

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
        InvokeRepeating("UpdateSpaceshipPath", 0f, 1f);  //1s delay, repeat every 1s

        // optionally add beginSize many Asteroids to tail
        for (int i = 0; i < beginSize - 1; i++)
        {
            addAsteroidToTail();
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
    }

    // Update is called once per frame
    void Update()
    {
        // This is just for the demo of the game - real game version would not have this. Add asteroids to tail by pressing Q key.
        if (Input.GetKey(KeyCode.L))
            addAsteroidToTail();
        // update time
        Score.time = Time.timeSinceLevelLoad;
        timeText.GetComponent<TMPro.TextMeshProUGUI>().SetText(Score.time.ToString("F2"));
    }

    // Physics corrected update method
    void FixedUpdate()
    {
        Move();
    }

    // Function to make each asteroid in the tail move
    public void Move()
    {
        // iterate over all the asteroids and update their positions
        for (int i = 1; i < asteroidTail.Count; i++)
        {
            Transform currentAsteroid = asteroidTail[i];
            // New position of our asteroid x will be the position of our spaceship x seconds ago -> position x in trail arrays
            Vector3 newpos = trailpath[i];
            Quaternion newrot = trailrotation[i];
            // set the position by doing a "SmoothDamp" -> approach the wanted position within "speedOfInterp" seconds
            // since we update the interval every second, setting speedOfInterp to 1s means we have perfectly smooth motion between each position!
            currentAsteroid.position = Vector3.SmoothDamp(currentAsteroid.position, newpos, ref velocities[i], speedOfInterp);
            // Slerp is good for rotations
            currentAsteroid.rotation = Quaternion.Slerp(currentAsteroid.rotation, newrot, Time.deltaTime);
        }
    }

    // Function to add an asteroid to the asteroid tail
    public void addAsteroidToTail()
    {
        if(asteroidTail.Count == maxTrailSize){
            Debug.Log("Maximum number of asteroids reached!");
            return;
        }
        Debug.Log(asteroidTail.ToString());
        // instantiate an asteroid object
        GameObject createdAsteroid = Instantiate (asteroidPrefab, trailpath[asteroidTail.Count - 1], trailrotation[asteroidTail.Count - 1]) as GameObject;
        createdAsteroid.layer = 2;

        // Kein Eigenanteil - Das Script kommt vom Unity Asset Store - "Quick Outline"
        // Create an outline around the newly created asteroid for better visibility of the tail
        if (OptionsMenu.asteroidTailOutline)
        {
            var outline = createdAsteroid.GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 2f;
        }

        // set it's tag to "Collected", so that it will lead to game over if ship collides with it
        createdAsteroid.tag = "Collected";
        // enable collider (mesh) - now we want it to trigger a "Game Over"!
        createdAsteroid.GetComponent<Collider>().enabled = true;
        createdAsteroid.GetComponent<Collider>().isTrigger = true;
        Transform newpart = createdAsteroid.transform;
        // finally, add the new asteroid to our asteroidTail list
        asteroidTail.Add(newpart);
        // increase score when collecting asteroid
        scoreScript.increaseScore();
    }

    // Function gets called when we enter a "trigger" event (in this case: Collectable -> Collect the asteroid, Collected -> Game Over!)
    public void OnTriggerEnter(Collider other)
    {
        //If tag is "Collectable", it means we collect the asteroid, destroy it, add it to our "snake body" and increase our score
        if (other.tag == "Collectable")
        {
            CollectAsteroid(other);
        } else if (other.tag == "Collected"){ // Colliding with our own snake body = Game Over!
            GameOver();
        } else {
            Debug.Log("Object not tagged (properly?)!");
        }
    }

    // Function gets called when we enter a normal collision event
    public void OnCollisionEnter(Collision col)
    {
            GameOver();
    }

    public void CollectAsteroid(Collider other)
    {
        Debug.Log("COLLECTED");
        // destroy asteroid
        Destroy(other.gameObject);
        // add it to our tail
        addAsteroidToTail();
        // spawn a new asteroid on the map so we don't run out
        spawnAsteroidsScript.SpawnAsteroid();
        // play "collect sound effect"
        AudioPlayer.PlayCollectSound();
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER!");
        // load game over scene
        SceneManager.LoadScene("GameOver");
        // free cursor to select in menu again
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // play menu music
        AudioPlayer.PlayMenuMusic();
    }
}
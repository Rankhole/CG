using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLikeMovement : MonoBehaviour
{

    public List<Transform> bodyParts = new List<Transform>();

    public float minDistance = 50f;

    public int beginSize = 1;
    
    public float speed = 1000;
    // public float rotationSpeed = 100;

    [SerializeField]
    public GameObject bodyprefabs;

    private float dis;
    private Transform curBodyPart;
    private Transform PrevBodyPart;

    public int maxTrailSize = 200;
    public float speedOfInterp = 1f;

    private Vector3[] trailpath;
    private Quaternion[] trailrotation;

    private Vector3[] velocities;

    public GameObject scoreText;
    private Score scoreScript;

    // Start is called before the first frame update
    void Start()
    {
        // get Score script from text object
        scoreScript = scoreText.GetComponent<Score>();

        // initialize arrays for tracking spaceship movement over time
        trailpath = new Vector3[maxTrailSize];
        trailrotation = new Quaternion[maxTrailSize];
        velocities = new Vector3[maxTrailSize];

        // head of our "tail" is the spaceship
        trailpath[0] = bodyParts[0].position;
        trailrotation[0] = bodyParts[0].rotation;

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
        trailpath[0] = bodyParts[0].position;
        trailrotation[0] = bodyParts[0].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            AddBodyPart();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {

        float curspeed = speed;
        float time_between_secs = Time.time - Mathf.Floor(Time.time);
        // Debug.Log(time_between_secs);
        for (int i = 1; i < bodyParts.Count; i++)
        {

            curBodyPart = bodyParts[i];
            PrevBodyPart = bodyParts[i-1];
            Vector3 newpos = trailpath[i];
            Quaternion newrot = trailrotation[i];

            // dis = Vector3.Distance(newpos, curBodyPart.position);

            // float T = Time.deltaTime * dis / minDistance * curspeed;

            // if (T > 0.5f)
            //     T = 0.5f;
            curBodyPart.position = Vector3.SmoothDamp(curBodyPart.position, newpos, ref velocities[i], speedOfInterp);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, newrot, time_between_secs);



        }
    }


    public void AddBodyPart()
    {
        if(bodyParts.Count == maxTrailSize){
            Debug.Log("Maximum number of asteroids reached!");
            return;
        }
        GameObject createdAsteroid = Instantiate (bodyprefabs, trailpath[bodyParts.Count - 1], trailrotation[bodyParts.Count - 1]) as GameObject;
        createdAsteroid.tag = "Collected";
        createdAsteroid.GetComponent<Collider>().enabled = true;
        Transform newpart = createdAsteroid.transform;
        newpart.SetParent(transform);

        bodyParts.Add(newpart);

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
        } else if (other.tag == "Collected"){ // Colliding with our own snake body = Game Over!
            Debug.Log("GAME OVER!");
            // Destroy(gameObject);
        } else {
            Debug.Log("Object not tagged properly!");
        }
    }

}
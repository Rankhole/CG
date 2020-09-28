using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLikeMovement : MonoBehaviour
{

    public List<Transform> bodyParts = new List<Transform>();

    public float minDistance = 50f;

    public int beginSize;
    
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

    // Start is called before the first frame update
    void Start()
    {
        trailpath = new Vector3[maxTrailSize];
        trailrotation = new Quaternion[maxTrailSize];
        velocities = new Vector3[maxTrailSize];
        trailpath[0] = bodyParts[0].position;
        trailrotation[0] = bodyParts[0].rotation;
        InvokeRepeating("UpdateSpaceshipPath", 0.25f, 0.25f);  //1s delay, repeat every 1s
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
        Debug.Log(time_between_secs);
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
        Transform newpart = (Instantiate (bodyprefabs, trailpath[bodyParts.Count - 1], trailrotation[bodyParts.Count - 1]) as GameObject).transform;
        newpart.SetParent(transform);

        bodyParts.Add(newpart);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with Asteroid!");
    }

}
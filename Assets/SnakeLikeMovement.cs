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


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < beginSize - 1; i++)
        {

            AddBodyPart();

        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Q))
            AddBodyPart();
    }

 public void Move()
    {

        float curspeed = speed;

        for (int i = 1; i < bodyParts.Count; i++)
        {

            curBodyPart = bodyParts[i];
            PrevBodyPart = bodyParts[i - 1];

            dis = Vector3.Distance(PrevBodyPart.position,curBodyPart.position);

            Vector3 newpos = PrevBodyPart.position;

            newpos.y = bodyParts[0].position.y;

            float T = Time.deltaTime * dis / minDistance * curspeed;

            if (T > 0.5f)
                T = 0.5f;
            curBodyPart.position = Vector3.Slerp(curBodyPart.position, newpos, T);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);



        }
    }


    public void AddBodyPart()
    {
        Transform newpart = (Instantiate (bodyprefabs, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        newpart.SetParent(transform);

        bodyParts.Add(newpart);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with Asteroid!");
    }

}
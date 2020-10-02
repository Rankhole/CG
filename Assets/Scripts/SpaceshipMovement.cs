using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Eigenanteil mit "Tutorial", habe es stark an mein Spielkonzept angepasst
// Quelle: https://sharpcoderblog.com/blog/spaceship-controller-in-unity-3d
[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    public float cruiseSpeed = 25f;
    public float accelerationSpeed = 60f;
    public Transform spaceship;
    public float rotationSpeed = 30.0f;
    // value for smoothing
    public float cameraSmooth = 4f;
    float speed;
    Rigidbody r;
    // rotation in regards to the world space - not local
    Quaternion worldSpaceRotation;
    float rotationZ = 0;
    float horizontal = 0;
    float vertical = 0;
    // local ship rotation
    Vector3 localShipRotation;

    // Start is called before the first frame update
    void Start()
    {
        // disable gravity, we are in space!
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        
        worldSpaceRotation = transform.rotation;
        localShipRotation = spaceship.localEulerAngles;
        rotationZ = localShipRotation.z;
        
        // lock cursor in gameplay mode!
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //Thrust buttons (space, RMB) to be twice as fast! SPEED!
        if (Input.GetAxis("Thrust") != 0)
        {
            speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 3);
        } else // lame
        {
            speed = Mathf.Lerp(speed, cruiseSpeed, Time.deltaTime * 10);
        }
        
        // forward movement -> "where ever we are looking"
        Vector3 velocity = new Vector3(0, 0, speed);
        velocity = transform.TransformDirection(velocity);
        r.velocity = new Vector3(velocity.x, velocity.y, velocity.z);

        //Local Z rotation of the ship = barrel roll axis
        float rotationZTmp = Input.GetAxis("Rotation") * -1;
        //horizontal movement
        horizontal = Mathf.Lerp(horizontal, Input.GetAxis("Horizontal") * rotationSpeed, Time.deltaTime * cameraSmooth);
        //vertical movement
        vertical = Mathf.Lerp(vertical, Input.GetAxis("Vertical") * rotationSpeed * -1, Time.deltaTime * cameraSmooth);
        // local rotation 
        Quaternion localRotation = Quaternion.Euler(-vertical, horizontal, rotationZTmp * rotationSpeed);
        // our rotation in world space = our old world space rotation * our newly calculated local rotation
        worldSpaceRotation = worldSpaceRotation * localRotation;
        transform.rotation = worldSpaceRotation;
        rotationZ -= horizontal;
        // clamp (lock) the maximum rotation speed to 100...or it looks...stupid
        rotationZ = Mathf.Clamp(rotationZ, -100, 100);

        spaceship.transform.localEulerAngles = new Vector3(localShipRotation.x, localShipRotation.y, rotationZ);
        rotationZ = Mathf.Lerp(rotationZ, localShipRotation.z, Time.deltaTime * cameraSmooth);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Backup_SC_Controller : MonoBehaviour
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
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        worldSpaceRotation = transform.rotation;
        localShipRotation = spaceship.localEulerAngles;
        rotationZ = localShipRotation.z;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //Press Right Mouse Button to accelerate
        if (Input.GetMouseButton(1))
        {
            speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 3);
        }
        else
        {
            speed = Mathf.Lerp(speed, cruiseSpeed, Time.deltaTime * 10);
        }

        //Set moveDirection to the vertical axis (up and down keys) * speed
        Vector3 moveDirection = new Vector3(0, 0, speed);
        //Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        //Set the velocity, so you can move
        r.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        //Local Z rotation of the ship = barrel roll axis
        float rotationZTmp = Input.GetAxis("Rotation") * -1;
        //horizontal movement
        horizontal = Mathf.Lerp(horizontal, Input.GetAxis("Horizontal") * rotationSpeed, Time.deltaTime * cameraSmooth);
        //veritcal movement
        vertical = Mathf.Lerp(vertical, Input.GetAxis("Vertical") * rotationSpeed * -1, Time.deltaTime * cameraSmooth);
        Quaternion localRotation = Quaternion.Euler(-vertical, horizontal, rotationZTmp * rotationSpeed);
        worldSpaceRotation = worldSpaceRotation * localRotation;
        transform.rotation = worldSpaceRotation;
        rotationZ -= horizontal;
        rotationZ = Mathf.Clamp(rotationZ, -100, 100);
        spaceship.transform.localEulerAngles = new Vector3(localShipRotation.x, localShipRotation.y, rotationZ);
        rotationZ = Mathf.Lerp(rotationZ, localShipRotation.z, Time.deltaTime * cameraSmooth);
    }
}
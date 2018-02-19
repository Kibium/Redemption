using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamara : MonoBehaviour
{
    public Transform Player;
    public float lookSensivity;
    public float min;
    public float max;
    public float smoothTime;


    public float yRotation;
    public float lookSensivity1;
    public float xRotation;
    public Vector3 velocity = Vector3.zero;

    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    // Use this for initialization
    void Start()
    {
        lookSensivity1 = lookSensivity;

    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        yRotation += Input.GetAxis("Mouse X") * lookSensivity;
        xRotation -= Input.GetAxis("Mouse Y") * lookSensivity;

        xRotation = Mathf.Clamp(xRotation, -min, max);

        Vector3 targetPosition = Player.TransformPoint(new Vector3(0, 0.5f, 0));

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
     
    }
}


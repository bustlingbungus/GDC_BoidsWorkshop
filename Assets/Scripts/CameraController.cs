using System;
using System.Collections.Generic;
using System.Net.Sockets;
using NUnit.Framework.Constraints;
using UnityEditor.Search;
using UnityEngine;


/* Allows user to move the main camera sround in the scene, for convenienct viewing */
public class CameraController : MonoBehaviour
{
    /* Speed the camera moves around the scene */
    [SerializeField] float moveSpeed = 10f;

    /* Speed the mouse rotates the camera */
    [SerializeField] float sensitivity = 1f;

    /* Limit for how far up/down camera can look */
    [SerializeField] float lookAngleLimit = 100f;


    /* Camera's current x rotation (up/down look angle) */
    float curr_x_rotation = 0f;


    /* List of all game object, to facilitate locking onto boids */
    List<GameObject> boids = null;

    /* Whether or not we use manual camera control */
    bool manual_camera = false;

    /* How far to trail behind flock of boids */
    [SerializeField] float boidFollowDist = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // hide mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // initial camera rotation
        curr_x_rotation = transform.rotation.eulerAngles.x;

        // get references to all boids
        boids = new List<GameObject>(GameObject.FindGameObjectsWithTag("Boid"));
    }


    // Update is called once per frame
    void Update()
    {
        if (manual_camera)
        {
            LookAround();
            Move();
            UpDownMotion();
        }
        else FollowBoids();

        // toggle manual camera
        if (Input.GetMouseButtonDown(0)) manual_camera = !manual_camera;
    }


    /* Use the mouse X/Y axes to rotate the camera */
    void LookAround()
    {
        // Get mouse movement
        float XMouseMovement = Input.GetAxis("Mouse X");
        float YMouseMovement = Input.GetAxis("Mouse Y");

        // limit x rotation (up/down look angle)
        curr_x_rotation -= YMouseMovement * sensitivity;
        curr_x_rotation = Mathf.Clamp(curr_x_rotation, -lookAngleLimit, lookAngleLimit);

        // new y rotation
        float curr_y_rotation = transform.rotation.eulerAngles.y;
        curr_y_rotation += XMouseMovement * sensitivity;


        // assign new look angles
        transform.rotation = Quaternion.Euler(
            curr_x_rotation,
            curr_y_rotation,
            transform.rotation.eulerAngles.z
        );
    }


    /* Use horizontal and vertical axes to move the camera laterally */
    void Move()
    {
        // get lateral movement vector from input axes
        Vector3 playerInput = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        // convert movement vector to local space
        Vector3 movementVector = transform.TransformDirection(playerInput);

        // move position
        transform.position += movementVector * Time.deltaTime * moveSpeed;
    }


    /* Use Space/C to move the camera straight up/down */
    void UpDownMotion()
    {
        // move up when space is pressed, down when c is pressed
        // ignore look direction, just move up/down
        if (Input.GetKey(KeyCode.Space))
            transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
        if (Input.GetKey(KeyCode.C))
            transform.position -= new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    void FollowBoids()
    {
        // find average position of boids
        Vector3 avgPos = Vector3.zero;
        foreach (GameObject b in boids) avgPos += b.transform.position;
        avgPos /= boids.Count;

        Vector3 dir = (transform.position - avgPos).normalized;

        // move towards fixed distance from boids
        Vector3 targetPos = avgPos + (dir * boidFollowDist);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.9f);

        // look towards average position
        transform.forward = Vector3.Lerp(transform.forward, -dir, 0.9f);
    }
}

using NUnit.Framework.Constraints;
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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // hide mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // initial camera rotation
        curr_x_rotation = transform.rotation.eulerAngles.x;
    }


    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
        UpDownMotion();
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
}

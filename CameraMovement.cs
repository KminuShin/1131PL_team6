using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of camera movement

    public static int canMove = 1; // Accessible from anywhere

    void Update()
    {
        // if can move
        if (canMove == 1)
        {
            // Get input from the keyboard
            float horizontal = Input.GetAxis("Horizontal"); // A, D or Left, Right arrows
            float vertical = Input.GetAxis("Vertical");     // W, S or Up, Down arrows

            // print("Horizontal: " + horizontal + " Vertical: " + vertical);

            // Calculate the direction to move the camera
            // Vector3 direction = new Vector3(horizontal, 0, vertical);
            Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

            // Move the camera in the calculated direction
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
        /// else if (NPCInteraction.shouldRotate)
        /// {
        ///     NPCInteraction.shouldRotate = false;
        /// }
    }
}

/*
public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of camera movement

    void Update()
    {
        // Get input from the keyboard
        float horizontal = Input.GetAxis("Horizontal"); // A, D or Left, Right arrows
        float vertical = Input.GetAxis("Vertical");     // W, S or Up, Down arrows

        // Calculate the direction to move the camera
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        // Move the camera in the calculated direction
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
} 
*/
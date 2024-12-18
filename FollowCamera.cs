using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the Camera's Transform
    public Vector3 offset = new Vector3(0, -1, 0); // Offset to adjust player position relative to the camera

    void Update()
    {
        if (cameraTransform != null)
        {
            // Set the player's position to match the camera's position + offset
            transform.position = cameraTransform.position + offset;
        }
    }
}

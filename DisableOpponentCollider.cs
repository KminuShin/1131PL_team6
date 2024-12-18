using UnityEngine;

public class DisableOpponentCollider : MonoBehaviour
{
    private Collider opponentCollider;

    void Start()
    {
        // Get the Collider component attached to the opponent
        opponentCollider = GetComponent<Collider>();
    }

    public void DisableCollider()
    {
        if (opponentCollider != null)
        {
            opponentCollider.enabled = false; // Disable the collider
            Debug.Log("Opponent collider disabled.");
        }
        else
        {
            Debug.LogWarning("No Collider found on the opponent!");
        }
    }

    public void EnableCollider()
    {
        if (opponentCollider != null)
        {
            opponentCollider.enabled = true; // Enable the collider
            Debug.Log("Opponent collider enabled.");
        }
    }
}

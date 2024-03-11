using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public Transform teleportDestination; // Destination where the player will be teleported
    public TeleportScript otherTeleport; // Reference to the other teleport script
    public int teleporterID; // Unique identifier for this teleporter

    private bool isTeleporting = false; // Flag to prevent multiple teleportations

    public void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player") && !isTeleporting)
        {
            Debug.Log("Teleporting player...");
            isTeleporting = true;
            Teleport(other.gameObject);
        }
    }

    public void Teleport(GameObject player)
    {
        // Teleport the player to the destination
        player.transform.position = teleportDestination.position;

        // If the other teleport is set, activate it to allow bidirectional teleportation
        if (otherTeleport != null && otherTeleport.teleporterID != teleporterID)
        {
            Debug.Log("Activating other teleport...");
            otherTeleport.ActivateTeleport(player);
        }

        isTeleporting = false;
    }

    // Activate teleportation for the other teleport script
    public void ActivateTeleport(GameObject player)
    {
        if (!isTeleporting)
        {
            isTeleporting = true;
            Teleport(player);
        }
    }
}

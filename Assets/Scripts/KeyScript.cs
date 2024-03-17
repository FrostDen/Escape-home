using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class KeyScript : MonoBehaviour
{
    // Reference to the door script
    public AN_DoorScript doorScript;
    PlayerInteractions playerInteractions;
    public Collider keyCollider;

    private bool isUsed = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        // Store the initial position and rotation of the key
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (doorScript != null && doorScript.isOpened)
        {
            // Set the position of the key to the collider
            transform.position = keyCollider.transform.position;
            transform.rotation = keyCollider.transform.rotation;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (isUsed)
            {
                // Reset the key's state
                ResetKey();
            }
            else
            {
                isUsed = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the key collides with the door's key collider
        if (other == keyCollider && !isUsed)
        {
            // Unlock the door
            if (doorScript != null)
            {
                isUsed = true;
                doorScript.isOpened = true;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.DoorOpen, transform.position);
            }
        }
    }

    // Reset the key's state
    private void ResetKey()
    {
        isUsed = false;
    }
}

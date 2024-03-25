using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float waitOnPickup = 0.2f;
    public float breakForce = 20f;
    [HideInInspector] public PlayerInteractions playerInteractions;
    [SerializeField] public Vector3 specificRotation;
    public bool isGrabbed = false;

    OutlineSelection outlineSelection; // Reference to the Outline component

    private void Start()
    {
        // Get the Outline component attached to the object
        outlineSelection = GetComponent<OutlineSelection>();
        if (outlineSelection == null)
        {
            // If Outline component is not found, try to find it in children
            outlineSelection = GetComponentInChildren<OutlineSelection>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerInteractions != null && isGrabbed)
        {
            if (collision.relativeVelocity.magnitude > breakForce)
            {
                playerInteractions.BreakConnection();
            }
        }
    }

    // This is used to prevent the connection from breaking when you just picked up the object
    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(waitOnPickup);
        if (playerInteractions != null)
        {
            isGrabbed = true;

            // Disable the Outline component when the object is picked up
            if (outlineSelection != null)
            {
                outlineSelection.enabled = false;
            }
        }
    }
}

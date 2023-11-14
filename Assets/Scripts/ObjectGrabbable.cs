using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private Transform playerCameraTransform;
    private Quaternion initialRotation;
    [SerializeField] private Vector3 specificRotation;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
    }

    public void Grab(Transform objectGrabPointTransform, Transform playerCameraTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        if (CompareTag("Object"))
        {
            this.playerCameraTransform = playerCameraTransform;
            objectRigidbody.freezeRotation = true;
        }
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        this.playerCameraTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.freezeRotation = false;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);

            if (playerCameraTransform != null)
            {
                // Rotate the object to face the camera
                Vector3 lookAtPosition = new Vector3(playerCameraTransform.position.x, transform.position.y, playerCameraTransform.position.z);
                transform.LookAt(lookAtPosition);

                if (CompareTag("Object"))
                {

                    transform.Rotate(specificRotation);
                }
            }
        }
    }
}

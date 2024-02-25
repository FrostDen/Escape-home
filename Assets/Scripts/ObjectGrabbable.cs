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

    public bool isGrabbed = false;
    public GameObject playerObject;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
    }

    public void Grab(Transform objectGrabPointTransform, Transform playerCameraTransform)
    {
        isGrabbed = true;
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        if (CompareTag("Object"))
        {
            this.playerCameraTransform = playerCameraTransform;
            objectRigidbody.freezeRotation = true;
        }

        if (CompareTag("Phone"))
        {
            this.playerCameraTransform = playerCameraTransform;
            objectRigidbody.freezeRotation = true;
        }

        if (CompareTag("Flashlight"))
        {
            this.playerCameraTransform = playerCameraTransform;
            objectRigidbody.freezeRotation = true;
        }

        if (CompareTag("Charger"))
        {
            this.playerCameraTransform = playerCameraTransform;
            objectRigidbody.freezeRotation = true;
        }



    }

    public void Drop()
    {
        isGrabbed = false;
        this.objectGrabPointTransform = null;
        this.playerCameraTransform = null;
        objectRigidbody.useGravity = true;
        objectRigidbody.freezeRotation = false; // Ensure rotation is not frozen when not grabbed
    }

    private bool isInspecting = false;
    private float rotationSpeed = 10f;

    // Add this method for enabling object inspection
    public void StartInspecting()
    {
        isInspecting = true;
        objectRigidbody.freezeRotation = false; // Unfreeze rotation when inspecting
    }

    public void StopInspecting()
    {
        isInspecting = false;
        objectRigidbody.freezeRotation = true; // Freeze rotation when inspection stops
    }

    private void FixedUpdate()
    {
        #region Inspection
        if (isGrabbed)
        {
            if (CompareTag("Inspect"))
            {
                if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
                {
                    StartInspecting();
                    LockCameraRotation(isGrabbed);
                }

                if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
                {
                    StopInspecting();
                    LockCameraRotation(!isGrabbed);
                }
            }
            if (CompareTag("Inspect retrievable"))
            {
                if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
                {
                    StartInspecting();
                    LockCameraRotation(isGrabbed);
                }

                if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
                {
                    StopInspecting();
                    LockCameraRotation(!isGrabbed);
                }
            }

            if (CompareTag("Radio"))
            {
                if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
                {
                    StartInspecting();
                    LockCameraRotation(isGrabbed);
                }

                if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
                {
                    StopInspecting();
                    LockCameraRotation(!isGrabbed);
                }
            }

            if (isInspecting)
            {
                float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
                float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

                // Rotate the object around its local axes based on mouse input
                transform.Rotate(Vector3.up, -mouseX, Space.Self);
                transform.Rotate(Vector3.right, mouseY, Space.Self);
            }
        }

        if (isGrabbed)
        {
            if (CompareTag("Object"))
            {
                if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
                {
                    LockCameraRotation(isGrabbed);
                    Cursor.visible = true; // Cursor is visible while inspecting
                    Cursor.lockState = CursorLockMode.None; // Unlock cursor while inspecting
                }

                if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
                {
                    LockCameraRotation(!isGrabbed);
                    Cursor.visible = false; // Cursor is hidden when not inspecting
                    Cursor.lockState = CursorLockMode.Locked; // Lock cursor when not inspecting
                }
            }

            if (CompareTag("Phone"))
            {
                if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
                {
                    LockCameraRotation(isGrabbed);
                    Cursor.visible = true; // Cursor is visible while inspecting
                    Cursor.lockState = CursorLockMode.None; // Unlock cursor while inspecting
                }

                if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
                {
                    LockCameraRotation(!isGrabbed);
                    Cursor.visible = false; // Cursor is hidden when not inspecting
                    Cursor.lockState = CursorLockMode.Locked; // Lock cursor when not inspecting
                }
            }

            if (CompareTag("Charger"))
            {
                if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
                {
                    LockCameraRotation(isGrabbed);
                    Cursor.visible = true; // Cursor is visible while inspecting
                    Cursor.lockState = CursorLockMode.None; // Unlock cursor while inspecting
                }

                if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
                {
                    LockCameraRotation(!isGrabbed);
                    Cursor.visible = false; // Cursor is hidden when not inspecting
                    Cursor.lockState = CursorLockMode.Locked; // Lock cursor when not inspecting
                }
            }
        }
        #endregion

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
                    lerpSpeed = 30f;
                    newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed); // Reuse the existing newPosition variable
                    objectRigidbody.MovePosition(newPosition);
                    transform.Rotate(specificRotation);
                }

                if (CompareTag("Phone"))
                {
                    lerpSpeed = 30f;
                    newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed); // Reuse the existing newPosition variable
                    objectRigidbody.MovePosition(newPosition);
                    transform.Rotate(specificRotation);
                }

                if (CompareTag("Flashlight"))
                {
                    lerpSpeed = 30f;
                    newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed); // Reuse the existing newPosition variable
                    objectRigidbody.MovePosition(newPosition);
                    transform.Rotate(specificRotation);
                }

                if (CompareTag("Charger"))
                {
                    lerpSpeed = 30f;
                    newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed); // Reuse the existing newPosition variable
                    objectRigidbody.MovePosition(newPosition);
                    transform.Rotate(specificRotation);
                }
            }
        }
    }

    public void LockCameraRotation(bool isLocked)
    {
        if (playerObject != null)
        {
           GameObject playerController = GameObject.FindGameObjectWithTag("Player");
            if (playerController != null)
            {
                MonoBehaviour[] scripts = playerController.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                     if (script.GetType().Name.Equals("FirstPersonController"))
                     {
                          script.enabled = !isLocked;
                          break;
                     }
                }
            }
        }
    }

// for mobile x= -100; y= 0; z= 90;
// for flashlight x= -90; y= -90; z= 0;
// for key x= 0; y= 90; z= 90;
// for plug x= 200; y= 0; z= 0;


    [Tooltip("Feature for one using only")]
    public bool OneTime = false;
    [Tooltip("SocketObject with collider(shpere, box etc.) (is trigger = true)")]
    public Collider[] Places;

    private Collider place;


    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    public bool isConnected = false, youCan = true;

    public void Update()
    {
        if (isConnected && place != null) // Check if there are any sockets
        {
            foreach (Collider socket in Places)
            {
                this.transform.position = place.transform.position; // Position based on each socket
                this.transform.rotation = place.transform.rotation;
            }

            if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, Camera.main.transform.position) <= NearView())
            {
                isConnected = false;
                place = null;
            }
        }
    }

    float NearView() // it is true if you are near an interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        float maxDistance = 2f; // Define the maximum distance for the ray
        if (angleView < 1f && distance < maxDistance)
            return maxDistance; // Returning the maximum distance for the ray
        else
            return 0f; // Return 0 if the conditions are not met
    }

    // OnTriggerEnter method outside of Update
    private void OnTriggerEnter(Collider other)
    {
        foreach (Collider socket in Places)
        {
            if (other == socket)
            {
                isConnected = true;
                place = other;
            }
        }
        if (OneTime)
            youCan = false;
    }






}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.1f;
    public int interactableLayerIndex;
    private Vector3 raycastPos;
    public GameObject lookObject;
    public PhysicsObject physicsObject;
    private Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float maxSpeed = 2500f;
    [SerializeField] public float maxDistance = 2f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;
    [SerializeField] private float sensitivity = 2f;

    private bool isInspecting = false;

    public GameObject playerObject;

    public Transform flashlightPoint;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    //A simple visualization of the point we're following in the scene view
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(pickupParent.position, 0.5f);
    //}

    public bool isRightMouseButtonPressed = false;
    public bool isMiddleMouseButtonPressed = false;


    //Interactable Object detections and distance check
    void Update()
    {
        //Here we check if we're currently looking at an interactable object
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));       
        RaycastHit hit;
        if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, 1 << interactableLayerIndex, QueryTriggerInteraction.Ignore))
        {

           lookObject = hit.collider.transform.root.gameObject;

            // Draw a visual representation of the sphere cast
            Debug.DrawRay(raycastPos, mainCamera.transform.forward * hit.distance, Color.green);
            Debug.DrawRay(hit.point, hit.normal * 0.1f, Color.red);

        }
        else
        {
            lookObject = null;
        }


        //if we press the button of choice
        if (Input.GetButtonDown("Fire1"))
        {
            //and we're not holding anything
            if (currentlyPickedUpObject == null)
            {
                //and we are looking an interactable object
                if (lookObject != null)
                {

                    PickUpObject();
                    physicsObject.isGrabbed = true;
                }

            }
            //if we press the pickup button and have something, we drop it
            else if (!isRightMouseButtonPressed || !isMiddleMouseButtonPressed)
            {
                BreakConnection();
            }
        }

        isRightMouseButtonPressed = Input.GetMouseButton(1);
        isMiddleMouseButtonPressed = Input.GetMouseButton(2);

        if (physicsObject != null && physicsObject.isGrabbed)
        {
            // Check if the right mouse button is pressed
            if (Input.GetMouseButtonDown(2))
            {
                isMiddleMouseButtonPressed = true;
                StartInspecting();
                LockCameraRotation(physicsObject.isGrabbed);
            }

            // Check if the right mouse button is released
            if (Input.GetMouseButtonUp(2))
            {
                isMiddleMouseButtonPressed = false;
                StopInspecting();
                LockCameraRotation(!physicsObject.isGrabbed);
            }
        }

        if (currentlyPickedUpObject != null && currentlyPickedUpObject.CompareTag("Phone"))
        {
            // Check if the right mouse button is pressed
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(DelayedLockCameraRotation(physicsObject.isGrabbed));
            }

            // Check if the right mouse button is released
            if (Input.GetMouseButtonUp(1))
            {
                StartCoroutine(DelayedLockCameraRotation(!physicsObject.isGrabbed));
            }
        }

        if (currentlyPickedUpObject != null && currentlyPickedUpObject.CompareTag("CovidTest"))
        {
            // Check if the right mouse button is pressed
            if (Input.GetMouseButtonDown(1))
            {
                isRightMouseButtonPressed = true;
                StartCoroutine(DelayedLockCameraRotation(physicsObject.isGrabbed));
            }

            // Check if the right mouse button is released
            if (Input.GetMouseButtonUp(1))
            {
                isRightMouseButtonPressed = false;
                StartCoroutine(DelayedLockCameraRotation(!physicsObject.isGrabbed));
            }
        }

        // Check if the object is grabbed and the right mouse button is pressed
        if (physicsObject != null && physicsObject.isGrabbed && isRightMouseButtonPressed)
        {
            // Disable grabbing/dropping object functionality
            if (Input.GetMouseButtonDown(0))
            {
                return;
            }
        }



        // Check if the distance between the player and grabbed object is greater than 2f
        if (currentlyPickedUpObject != null && Vector3.Distance(pickupParent.transform.position, currentlyPickedUpObject.transform.position) > maxDistance)
        {
            BreakConnection();
        }

    }

    //Velocity movement toward pickup parent and rotation
    private void FixedUpdate()
    {
        if (currentlyPickedUpObject != null)
        {
            currentDist = Vector3.Distance(pickupParent.position, pickupRB.position);
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            currentSpeed *= Time.fixedDeltaTime;
            Vector3 direction = pickupParent.position - pickupRB.position;
            pickupRB.velocity = direction.normalized * currentSpeed;

            if (currentlyPickedUpObject.CompareTag("Cardbob"))
            {
                maxSpeed = Random.Range(300f, 1500f);
                currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            }

            if (currentlyPickedUpObject.CompareTag("Hinges") || currentlyPickedUpObject.CompareTag("Safe Door") || currentlyPickedUpObject.CompareTag("OpenedDoor") || currentlyPickedUpObject.CompareTag("Pantry Door"))
            {
                // Calculate mouse movement only on the Y-axis
                float mouseX = -Input.GetAxis("Mouse X") * sensitivity;
                float mouseY = 0f;

                // Apply rotation based on mouse movement
                Quaternion rotationDelta = Quaternion.Euler(mouseX, 0f, 0f);
                pickupRB.MoveRotation(pickupRB.rotation * rotationDelta);
            }

            if (currentlyPickedUpObject.CompareTag("Heavy") || currentlyPickedUpObject.CompareTag("Box"))
            {
                maxSpeed = 150f;
                currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            }

            if (currentlyPickedUpObject.CompareTag("Inspect"))
            {
                maxSpeed = 2500f;
                currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            }

            if (!currentlyPickedUpObject.CompareTag("Heavy") 
                && !currentlyPickedUpObject.CompareTag("Hinges") 
                && !currentlyPickedUpObject.CompareTag("Safe Door") 
                && !currentlyPickedUpObject.CompareTag("Pantry Door") 
                && !currentlyPickedUpObject.CompareTag("OpenedDoor"))
            {
                // Check if inspecting 
                if (isInspecting)
                {

                    // Calculate mouse movement
                    float mouseX = Input.GetAxis("Mouse X") * sensitivity;
                    float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

                    // Apply rotation based on mouse movement
                    Quaternion rotationDelta = Quaternion.Euler(-mouseY, mouseX, 0f);
                    pickupRB.MoveRotation(pickupRB.rotation * rotationDelta);
                }
                else
                {
                    if (!currentlyPickedUpObject.CompareTag("Hinges") 
                        && !currentlyPickedUpObject.CompareTag("Safe Door") 
                        && !currentlyPickedUpObject.CompareTag("OpenedDoor") 
                        && !currentlyPickedUpObject.CompareTag("Pantry Door") 
                        && !currentlyPickedUpObject.CompareTag("Heavy") 
                        && !currentlyPickedUpObject.CompareTag("Box") 
                        && !currentlyPickedUpObject.CompareTag("Inspect"))
                    {
                        //Rotation
                        lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                        lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
                        pickupRB.MoveRotation(lookRot);
                        maxSpeed = 2500f;

                        if (currentlyPickedUpObject.CompareTag("Phone"))
                        {
                            // Apply specific rotation
                            pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));
                            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                            lookRot = Quaternion.Euler(-100f, lookRot.eulerAngles.y, 90f); // Only face the camera on the y-axis
                            pickupRB.MoveRotation(lookRot);
                        }

                        if (currentlyPickedUpObject.CompareTag("Radio"))
                        {
                            // Apply specific rotation
                            pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));
                            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                            lookRot = Quaternion.Euler(0f, lookRot.eulerAngles.y - 90f, 0f); // Only face the camera on the y-axis
                            pickupRB.MoveRotation(lookRot);
                        }
                        if (currentlyPickedUpObject.CompareTag("Object"))
                        {
                            // Apply specific rotation
                            pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));
                            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                            lookRot = Quaternion.Euler(0f, lookRot.eulerAngles.y + 90f, 90f); // Only face the camera on the y-axis
                            pickupRB.MoveRotation(lookRot);
                        }
                        if (currentlyPickedUpObject.CompareTag("CovidTest"))
                        {
                            // Apply specific rotation
                            pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));
                            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                            lookRot = Quaternion.Euler(-100f, lookRot.eulerAngles.y, 90f); // Only face the camera on the y-axis
                            pickupRB.MoveRotation(lookRot);
                        }
                        if (currentlyPickedUpObject.CompareTag("Charger"))
                        {
                            // Apply specific rotation
                            pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));
                            lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                            lookRot = Quaternion.Euler(200f, lookRot.eulerAngles.y, 0f); // Only face the camera on the y-axis
                            pickupRB.MoveRotation(lookRot);
                        }
                        if (currentlyPickedUpObject.CompareTag("Flashlight"))
                        {
                            // Apply specific rotation (if necessary)
                            pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));

                            // Calculate the direction to the flashlight point
                            Vector3 directionToFlashlightPoint = flashlightPoint.position - pickupRB.position;

                            // Calculate the rotation to face the flashlight point
                            Quaternion lookRotation = Quaternion.LookRotation(directionToFlashlightPoint);

                            // Adjust the x-axis rotation to -90 degrees
                            lookRotation *= Quaternion.Euler(-90f, 0f, 0f);

                            // Get the mouse input on the y-axis
                            float mouseY = Input.GetAxis("Mouse Y");

                            // Calculate the rotation angle based on mouse input and sensitivity
                            float rotationAngle = mouseY * sensitivity * Time.deltaTime;

                            // Apply the rotation angle to the current rotation on the y-axis
                            Quaternion rotationDelta = Quaternion.Euler(rotationAngle, 0f, 0f);
                            lookRotation *= rotationDelta;

                            // Apply the rotation
                            pickupRB.MoveRotation(lookRotation);
                        }

                        // Crazy flashlight
                        //if (currentlyPickedUpObject.CompareTag("Flashlight"))
                        //{
                        //    // Apply specific rotation (if necessary)
                        //    pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));

                        //    // Calculate the direction to the flashlight point
                        //    Vector3 directionToFlashlightPoint = flashlightPoint.position - pickupRB.position;

                        //    // Calculate the rotation to face the flashlight point
                        //    Quaternion lookRotation = Quaternion.LookRotation(directionToFlashlightPoint);

                        //    // Adjust the x-axis rotation to -90 degrees
                        //    lookRotation *= Quaternion.Euler(-90f, 0f, 0f);

                        //    // Calculate the angle between the flashlight's forward direction and the direction to the flashlight point
                        //    float angle = Vector3.SignedAngle(pickupRB.transform.forward, directionToFlashlightPoint, pickupRB.transform.right);

                        //    // Apply the rotation around the flashlight's forward axis based on the angle
                        //    Quaternion zRotation = Quaternion.AngleAxis(angle, pickupRB.transform.forward);
                        //    lookRotation *= zRotation;

                        //    // Apply the rotation
                        //    pickupRB.MoveRotation(lookRotation);
                        //}

                    }
                }
            }
        }


    }



    // Add this method for enabling object inspection
    public void StartInspecting()
    {
        isInspecting = true;
        if (pickupRB != null)
        {
            pickupRB.constraints = RigidbodyConstraints.None; // Unfreeze rotation when inspecting
        }
    }

    public void StopInspecting()
    {
        isInspecting = false;
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation; // Freeze rotation when inspection stops
    }

    //Release the object
    public void BreakConnection()
    {
        // Check if the right mouse button is pressed
        if (isRightMouseButtonPressed || isMiddleMouseButtonPressed)
        {
            return; // Return early if the right mouse button is pressed
        }

        pickupRB.constraints = RigidbodyConstraints.None;
        currentlyPickedUpObject = null;
        physicsObject.isGrabbed = false;
        currentDist = 0;
        pickupRB.useGravity = true; // Enable gravity when connection is broken
    }

    public void PickUpObject()
    {

        physicsObject = lookObject.GetComponentInChildren<PhysicsObject>();
        currentlyPickedUpObject = lookObject;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        physicsObject.specificRotation = pickupRB.rotation.eulerAngles;
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp());
        //pickupRB.useGravity = false; // Disable gravity when picking up

    }

    public void LockCameraRotation(bool isLocked)
    {
        if (playerObject != null)
        {
            FirstPersonController firstPersonController = playerObject.GetComponent<FirstPersonController>();
            if (firstPersonController != null)
            {
                firstPersonController.enabled = !isLocked;
            }
        }
    }

    IEnumerator DelayedLockCameraRotation(bool lockRotation)
    {
        yield return new WaitForSeconds(0.7f); // Delay for 1 second
        LockCameraRotation(lockRotation);
    }

}
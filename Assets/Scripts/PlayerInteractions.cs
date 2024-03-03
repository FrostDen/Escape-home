using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableLayerIndex;
    private Vector3 raycastPos;
    public GameObject lookObject;
    private PhysicsObject physicsObject;
    private Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3000f;
    [SerializeField] private float maxDistance = 2f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;
    [SerializeField] private float sensitivity = 100f;

    private bool isInspecting = false;

    public GameObject playerObject;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    //A simple visualization of the point we're following in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pickupParent.position, 0.5f);
    }

    //Interactable Object detections and distance check
    void Update()
    {
        //Here we check if we're currently looking at an interactable object
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));       
        RaycastHit hit;
        if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, 1 << interactableLayerIndex))
        {

           lookObject = hit.collider.transform.root.gameObject;

        }
        else
        {
            lookObject = null;
            
        }

        //if we press the button of choice
        if (Input.GetButtonDown("Interact"))
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
            else 
            {
                BreakConnection();
                physicsObject.isGrabbed = false;
            }
        }

        if (physicsObject != null && physicsObject.isGrabbed)
        {
            // Check if the right mouse button is pressed
            if (Input.GetMouseButtonDown(2))
            {
                StartInspecting();
                LockCameraRotation(physicsObject.isGrabbed);
            }

            // Check if the right mouse button is released
            if (Input.GetMouseButtonUp(2))
            {
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
                //Rotation
                lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
                pickupRB.MoveRotation(lookRot);

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
                    // Apply specific rotation
                    pickupRB.MoveRotation(Quaternion.Euler(physicsObject.specificRotation));
                    lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
                    lookRot = Quaternion.Euler(-90f, lookRot.eulerAngles.y - 90f, 0f); // Only face the camera on the y-axis
                    pickupRB.MoveRotation(lookRot);
                }
            }
        }

        //#region Inspection


        //if (physicsObject != null && physicsObject.isGrabbed)
        //{

        //    if (CompareTag("Inspect") || CompareTag("Inspect retrievable") || CompareTag("Radio"))
        //    {
        //        if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
        //        {
        //            StartInspecting();
        //            LockCameraRotation(physicsObject.isGrabbed);
        //            Debug.Log("Inspecting");
        //        }

        //        if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
        //        {
        //            StopInspecting();
        //            LockCameraRotation(!physicsObject.isGrabbed);
        //            Debug.Log("Not inspecting");
        //        }
        //    }

        //    if (isInspecting && currentlyPickedUpObject != null)
        //    {
        //        float mouseX = 0f;
        //        float mouseY = 0f;

        //        mouseX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        //        mouseY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        //        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        //        currentlyPickedUpObject.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
        //    }


        //    if (CompareTag("Object") || CompareTag("Phone") || CompareTag("Charger"))
        //    {
        //        if (Input.GetMouseButtonDown(1)) // Check for right mouse button down
        //        {
        //            LockCameraRotation(physicsObject.isGrabbed);
        //            Cursor.visible = true; // Cursor is visible while inspecting
        //            Cursor.lockState = CursorLockMode.None; // Unlock cursor while inspecting
        //        }

        //        if (Input.GetMouseButtonUp(1)) // Check for right mouse button up
        //        {
        //            LockCameraRotation(!physicsObject.isGrabbed);
        //            Cursor.visible = false; // Cursor is hidden when not inspecting
        //            Cursor.lockState = CursorLockMode.Locked; // Lock cursor when not inspecting
        //        }
        //    }
        //}
        //#endregion

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
        pickupRB.constraints = RigidbodyConstraints.None;
        currentlyPickedUpObject = null;
        physicsObject.isGrabbed = false;
        currentDist = 0;
    }

    public void PickUpObject()
    {

        physicsObject = lookObject.GetComponentInChildren<PhysicsObject>();
        currentlyPickedUpObject = lookObject;
        //currentlyPickedUpObject.transform.Rotate(physicsObject.specificRotation);
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();
        physicsObject.specificRotation = pickupRB.rotation.eulerAngles;
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;
        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp());
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    private Vector3 specificRotation;

    private ObjectGrabbable objectGrabbable;
    private PlayerInteractions playerInteractions;
    public bool isGrabbed = false;

    float distance;
    float angleView;
    Vector3 direction;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            { //Not carrying an object, try to grab
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, NearView(), pickUpLayerMask, QueryTriggerInteraction.Ignore))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        CalculateGrabbedObjectRotation();

                        // Pass the player transform and rotation to the Grab method
                        playerInteractions.PickUpObject();
                    }
                }
            }
            else
            {
                //Currently carrying something, drop
                playerInteractions.BreakConnection();
                objectGrabbable.LockCameraRotation(isGrabbed);
                objectGrabbable = null;
            }
        }
    }

    float NearView()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        float maxDistance = 2f;
        if (angleView < 90f && distance < maxDistance)
            return maxDistance;
        else
            return 0f;
    }


    private void CalculateGrabbedObjectRotation()
    {
        Vector3 cameraForward = playerCameraTransform.forward;
        specificRotation = new Vector3(0f, Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectGrabbable objectGrabbable;

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
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }
            else
            {
                //Currently carrying something, drop
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }

    float NearView() // it is true if you are near an interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        float maxDistance = 2f; // Define the maximum distance for the ray
        if (angleView < 90f && distance < maxDistance)
            return maxDistance; // Returning the maximum distance for the ray
        else
            return 0f; // Return 0 if the conditions are not met
    }

}

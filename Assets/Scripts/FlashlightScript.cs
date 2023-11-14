using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightScript : MonoBehaviour
{

    float distance;
    float angleView;
    Vector3 direction;

    public Light flashlight;

    void Start()
    {
        // Ensure the flashlight starts in the off state
        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    void Update()
    {
        // Toggle flashlight on/off when the 'F' key is pressed
        if (Input.GetKeyDown(KeyCode.F) && NearView() > 0f)
        {
            ToggleFlashlight();
        }
    }

    void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            // Toggle the state of the flashlight
            flashlight.enabled = !flashlight.enabled;

            // You can add additional logic here, such as playing a sound or animating the flashlight model.
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
}


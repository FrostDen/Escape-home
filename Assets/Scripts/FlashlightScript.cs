using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FlashlightScript : MonoBehaviour
{
    public BatteryScript batteryScript;
    public ObjectGrabbable objectGrabbable;
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
        if (Input.GetKeyDown(KeyCode.F) && NearView() > 0f && objectGrabbable.isGrabbed)
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            // Toggle the state of the flashlight
            bool isFlashlightOn = !flashlight.enabled;
            flashlight.enabled = isFlashlightOn;

            // Play the corresponding FMOD sound based on the flashlight state
            if (isFlashlightOn && CompareTag("Flashlight"))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.FlashlightON, transform.position);
            }
            else
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.FlashlightOFF, transform.position);
            }
        }

        // You can add additional logic here, such as playing a sound or animating the flashlight model.
    }

    float NearView()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        float maxDistance = 2f;
        if (angleView < 10f && distance < maxDistance)
            return maxDistance;
        else
            return 0f;
    }
}


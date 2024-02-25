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

    private Color originalEmissionColor;
    [SerializeField] private Renderer emissionRenderer; // Reference to the renderer with the emission texture


    void Start()
    {
        // Ensure the flashlight starts in the off state
        if (flashlight != null)
        {
            flashlight.enabled = false;
            originalEmissionColor = GetOriginalEmissionColor();
            SetEmission(originalEmissionColor, false);
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
                SetEmission(originalEmissionColor, true);
            }
            else
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.FlashlightOFF, transform.position);
                SetEmission(originalEmissionColor, false);
            }
        }

        // You can add additional logic here, such as playing a sound or animating the flashlight model.
    }

    Color GetOriginalEmissionColor()
    {
        if (emissionRenderer != null)
        {
            Material material = emissionRenderer.material;

            // Assuming "_EmissionColor" is the emission property, modify it based on your material
            Color originalColor = material.GetColor("_EmissionColor");
            return originalColor;
        }

        return Color.black; // Default color if no emissionRenderer is set
    }

    void SetEmission(Color emissionColor, bool enableEmission)
    {
        if (emissionRenderer != null)
        {
            Material material = emissionRenderer.material;

            // Assuming "_EmissionColor" is the emission property, modify it based on your material
            material.SetColor("_EmissionColor", enableEmission ? emissionColor : Color.black);

            // Enable or disable emission
            material.EnableKeyword("_EMISSION");
        }
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


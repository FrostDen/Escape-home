using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FlashlightMobileScript : MonoBehaviour
{
    public BatteryScript batteryScript;
    public PhysicsObject physicsObject;
    float distance;
    float angleView;
    Vector3 direction;

    public Light flashlightMobile;

    public Color originalEmissionColorMobile;
    [SerializeField] private Renderer emissionRendererMobile; // Reference to the renderer with the emission texture

    void Start()
    {
        // Ensure the flashlight starts in the off state
        if (flashlightMobile != null)
        {
            flashlightMobile.enabled = false;
            originalEmissionColorMobile = GetOriginalEmissionColorMobile();
            SetEmission(originalEmissionColorMobile, false);
        }

        if (physicsObject == null)
        {
            physicsObject = GetComponent<PhysicsObject>();
        }
    }

    void Update()
    {
        // Toggle flashlight on/off when the 'F' key is pressed
        if (Input.GetKeyDown(KeyCode.F) && physicsObject.isGrabbed && CompareTag("Phone"))
        {
            ToggleFlashlight();
        }
    }

    public bool isFlashlightOn;

    public void ToggleFlashlight()
    {
        if (flashlightMobile != null)
        {
            // Toggle the state of the flashlight
            isFlashlightOn = !flashlightMobile.enabled;
            flashlightMobile.enabled = isFlashlightOn;

            // Play the corresponding FMOD sound based on the flashlight state
            if (isFlashlightOn && CompareTag("Phone"))
            {
                //AudioManager.instance.PlayOneShot(FMODEvents.instance.FlashlightON, transform.position);
                SetEmission(originalEmissionColorMobile, true);
            }
            else
            {
                //AudioManager.instance.PlayOneShot(FMODEvents.instance.FlashlightOFF, transform.position);
                SetEmission(originalEmissionColorMobile, false);
            }
        }

    }

    Color GetOriginalEmissionColorMobile()
    {
        if (emissionRendererMobile != null)
        {
            Material material = emissionRendererMobile.material;
            Color originalColor = material.GetColor("_EmissionColor");
            return originalColor;
        }
        else
        {
            Debug.LogError("emissionRendererMobile is not assigned.");
            return Color.black;
        }
    }

    public void SetEmission(Color emissionColor, bool enableEmission)
    {
        if (emissionRendererMobile != null)
        {
            Material material = emissionRendererMobile.material;

            material.SetColor("_EmissionColor", enableEmission ? emissionColor : Color.black);

            material.EnableKeyword("_EMISSION");
        }
    }
}

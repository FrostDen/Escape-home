using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FlashlightScript : MonoBehaviour
{
    public PhysicsObject physicsObject;
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

        if (physicsObject == null)
        {
            physicsObject = GetComponent<PhysicsObject>();
        }
    }

    void Update()
    {
        // Toggle flashlight on/off when the 'F' key is pressed
        if (Input.GetKeyDown(KeyCode.F) && physicsObject.isGrabbed && CompareTag("Flashlight"))
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

    }

    Color GetOriginalEmissionColor()
    {
        if (emissionRenderer != null)
        {
            Material material = emissionRenderer.material;
            Color originalColor1 = material.GetColor("_EmissionColor");
            return originalColor1;
        }
        else
        {
            Debug.LogError("emissionRenderer is not assigned.");
            return Color.black;
        }
    }

    void SetEmission(Color emissionColor, bool enableEmission)
    {
        if (emissionRenderer != null)
        {
            Material material = emissionRenderer.material;

            material.SetColor("_EmissionColor", enableEmission ? emissionColor : Color.black);

            material.EnableKeyword("_EMISSION");
        }
    }
}

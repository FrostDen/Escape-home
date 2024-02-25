using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]

public class RadioScript : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Renderer emissionRenderer; // Reference to the renderer with the emission texture

    [Header("Parameter Change")]
    [SerializeField] private string parameterName = "toggle_Radio";
    [SerializeField] [Range(0, 1)] private float parameterValue;

    float distance;
    float angleView;
    Vector3 direction;

    private StudioEventEmitter emitter;

    // To track if the radio is currently playing
    private bool isRadioPlaying = true;

    // Store the original emission color
    private Color originalEmissionColor;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the StudioEventEmitter
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.Radio, this.gameObject);

        // Store the original emission color
        originalEmissionColor = GetOriginalEmissionColor();

        // Set the initial playback position to a random time
        //SetRandomStartTime();

        //Start playing the radio
        emitter.Play();

        // Enable emission with the original color
        SetEmission(originalEmissionColor, true);
        AudioManager.instance.SetRadioParameter(parameterName, 1);
    }

    // Function to get the original emission color
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

    // Function to set emission on the specified renderer
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

    // Function to set the initial playback position to a random time
    //void SetRandomStartTime()
    //{
    //    FMOD.Studio.EventDescription eventDescription;
    //    emitter.EventInstance.getDescription(out eventDescription);

    //    int length;
    //    eventDescription.getLength(out length);

    //    int randomStartTime = Mathf.FloorToInt(Random.Range(1f, length / 1000f));
    //    emitter.EventInstance.setTimelinePosition(randomStartTime * 1000); // Convert seconds to milliseconds
    //}

    // Update is called once per frame
    void Update()
    {
        // Check for player input (e.g., pressing 'F') to toggle the radio on/off
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleRadio();
        }
    }

    void ToggleRadio()
    {
        // Check if the player is near the radio
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, NearView(), pickUpLayerMask, QueryTriggerInteraction.Ignore))
        {
            // Toggle the radio state
            if (isRadioPlaying)
            {
                emitter.Stop(); // Stop playing the radio
                SetEmission(originalEmissionColor, false); // Disable emission with the original color
                AudioManager.instance.SetRadioParameter(parameterName, 0);
            }
            else
            {
                FMOD.Studio.EventDescription eventDescription;
                emitter.EventInstance.getDescription(out eventDescription);

                int length;
                eventDescription.getLength(out length);

                //emitter.SetParameter("time", Random.Range(0f, length - 1f)); // Set a new random playback position
                emitter.Play(); // Start playing the radio
                SetEmission(originalEmissionColor, true); // Enable emission with the original color
                AudioManager.instance.SetRadioParameter(parameterName, 1);
            }

            // Update the radio state
            isRadioPlaying = !isRadioPlaying;
        }
    }

    //void ToggleRadio()
    //{
    //    // Check if the player is near the radio
    //    if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, NearView(), pickUpLayerMask, QueryTriggerInteraction.Ignore))
    //    {
    //        // Toggle the radio state
    //        if (isRadioPlaying)
    //        {
    //            Debug.Log("Toggling radio OFF");
    //            AudioManager.instance.SetRadioParameter(parameterName, 0);
    //            SetEmission(originalEmissionColor, false); // Disable emission with the original color
    //            emitter.start();
    //        }
    //        else
    //        {
    //            Debug.Log("Toggling radio ON");
    //            AudioManager.instance.SetRadioParameter(parameterName, 1);
    //            SetEmission(originalEmissionColor, true); // Enable emission with the original color
    //        }

    //        // Update the radio state
    //        isRadioPlaying = !isRadioPlaying;
    //    }
    //    else
    //    {
    //        Debug.Log("Player is not near the radio.");
    //    }
    //}

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

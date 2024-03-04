using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class RadioScript : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Renderer emissionRenderer;
    [SerializeField] private string parameterName = "toggle_Radio";

    private StudioEventEmitter emitter;
    private Color originalEmissionColor;
    private bool isRadioPlaying = true;

    private void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();

        if (emitter == null)
        {
            Debug.LogError("StudioEventEmitter component not found.");
            return;
        }

        // Initialize the StudioEventEmitter
        AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.Radio, this.gameObject);

        // Store the original emission color
        originalEmissionColor = GetOriginalEmissionColor();

        // Start playing the radio
        PlayRadio();
    }

    private Color GetOriginalEmissionColor()
    {
        if (emissionRenderer != null)
        {
            Material material = emissionRenderer.material;
            return material.GetColor("_EmissionColor");
        }

        return Color.black;
    }

    private void PlayRadio()
    {
        emitter.Play();
        SetEmission(originalEmissionColor, true);
        AudioManager.instance.SetRadioParameter(parameterName, 1);
    }

    private void StopRadio()
    {
        emitter.Stop();
        SetEmission(originalEmissionColor, false);
        AudioManager.instance.SetRadioParameter(parameterName, 0);
    }

    private void ToggleRadio()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, 2f, pickUpLayerMask))
        {
            if (isRadioPlaying)
                StopRadio();
            else
                PlayRadio();

            isRadioPlaying = !isRadioPlaying;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleRadio();
        }
    }

    private void SetEmission(Color emissionColor, bool enableEmission)
    {
        if (emissionRenderer != null)
        {
            Material material = emissionRenderer.material;
            material.SetColor("_EmissionColor", enableEmission ? emissionColor : Color.black);
            material.EnableKeyword("_EMISSION");
        }
    }
}

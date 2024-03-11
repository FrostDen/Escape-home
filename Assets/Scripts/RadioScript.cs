using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class RadioScript : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Renderer emissionRenderer;
    [SerializeField] private string parameterName = "toggle_Radio";
    [SerializeField] public float maxDistance = 2f;
    public int interactableLayerIndex;
    public float sphereCastRadius = 0.5f;
    private Vector3 raycastPos;
    public Camera mainCamera;
    public GameObject lookObject;

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
        //emitter.Play();
        SetEmission(originalEmissionColor, true);
        AudioManager.instance.SetParameter(parameterName, 1);
    }

    private void StopRadio()
    {
        //emitter.Stop();
        SetEmission(originalEmissionColor, false);
        AudioManager.instance.SetParameter(parameterName, 0);
    }

    private void ToggleRadio()
    {
        // Update the raycast position with the current position of the camera
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Perform a sphere cast to check for nearby interactable objects
        RaycastHit hit;
        if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, 1 << interactableLayerIndex))
        {
            lookObject = hit.collider.transform.root.gameObject;

            //Toggle the radio state based on whether it's currently playing or not
            if (isRadioPlaying)
                StopRadio();
            else
                PlayRadio();

            isRadioPlaying = !isRadioPlaying;
        }
        else
        {
            lookObject = null;
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

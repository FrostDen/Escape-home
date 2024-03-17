using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ItemController : MonoBehaviour
{
    public Item item;

    public bool isRedKey = true, Facemask = false;

    public InventoryItemController inventoryItemController;

    [SerializeField] public Transform playerCameraTransform;
    [SerializeField] public Transform objectGrabPointTransform;
    [SerializeField] public LayerMask pickUpLayerMask;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    private void Start()
    {
        inventoryItemController = FindObjectOfType<InventoryItemController>(); // key will get up and it will saved in "inventary"
        playerCameraTransform = Camera.main.transform;

        //volumeProfile = globalVolume.profile;

        //if (volumeProfile.TryGet(out bloomComponent))
        //{
        //    // Save the original intensity value
        //    originalBloomIntensity = bloomComponent.intensity.value;
        //}
    }

    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        InventoryManager.Instance.ListItems();
        if (isRedKey) inventoryItemController.RedKey = true;
        else if (Facemask) inventoryItemController.Facemask = true;
        else inventoryItemController.BlueKey = true;
        Destroy(gameObject);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.PickupSound, playerCameraTransform.transform.position);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= NearView())
        {
            if (Input.GetButtonDown("Interact") && item.itemName == "rúško")
            {
                Pickup();
            }

            if (Input.GetButtonDown("Interact") && (item.itemName == "pivo" || item.itemName == "plechovica piva"))
            {
                DrinkBeer();
            }
            if (Input.GetButtonDown("Interact") && item.itemName == "tabletky")
            {
                EatPills();
            }
        }
    }

    public void DrinkBeer()
    {
        HUD hud = FindObjectOfType<HUD>();
        float timeToAdd = 30f;

        if (item != null && (item.itemName == "pivo" || item.itemName == "plechovica piva") && item.itemType == ItemType.Consumable)
        {
            if (hud != null)
            {
                hud.AddTimeToTimer(timeToAdd);
                //InventoryManager.Instance.Remove(item);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.UseSound, objectGrabPointTransform.transform.position);
                Destroy(gameObject);
                Debug.Log("Drink");
            }
        }
    }

    public Volume globalVolume;
    public VolumeProfile volumeProfile;

    private Bloom bloomComponent; // Reference to the Bloom effect
    private float originalBloomIntensity = 1f; // Store the original Bloom intensity

    public void EatPills()
    {
        HUD hud = FindObjectOfType<HUD>();
        float timeToAdd = 20f;
        float bloomIntensityIncrease = 250f;
        float bloomEffectDuration = 20f;

        // Ensure globalVolume and volumeProfile are not null
        if (globalVolume != null && volumeProfile != null)
        {
            if (item != null && item.itemName == "tabletky" && item.itemType == ItemType.Consumable)
            {
                hud.AddTimeToTimer(timeToAdd);
                hud.TimeController();

                // Declare and initialize bloomComponent and originalBloomIntensity
                Bloom bloomComponent;
                float originalBloomIntensity = 1f;

                // Adjust Bloom intensity when eating pills
                if (volumeProfile.TryGet(out bloomComponent))
                {
                    // Calculate the target intensity
                    float targetIntensity = originalBloomIntensity + bloomIntensityIncrease;

                    StartCoroutine(ChangeBloomIntensity(originalBloomIntensity, targetIntensity, bloomEffectDuration, bloomComponent));
                }
                AudioManager.instance.PlayOneShot(FMODEvents.instance.UseSound, objectGrabPointTransform.transform.position);
                //InventoryManager.Instance.Remove(item);
                Destroy(gameObject);
                Debug.Log("Bloom intensity increased gradually");
            }
        }
        else
        {
            Debug.LogError("globalVolume or volumeProfile is not assigned in the Inspector!");
        }
    }

    public IEnumerator ChangeBloomIntensity(float startIntensity, float targetIntensity, float duration, Bloom bloomComponent)
    {
        float elapsedTime = 0f;
        float currentIntensity = startIntensity;

        while (elapsedTime < duration)
        {
            // Adjust intensity smoothly
            currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            bloomComponent.intensity.value = currentIntensity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the intensity is set to the target value
        bloomComponent.intensity.value = targetIntensity;

        yield return new WaitForSeconds(1f); // Wait for a moment at the increased intensity

        // Gradual decrease
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Adjust intensity smoothly
            currentIntensity = Mathf.Lerp(targetIntensity, startIntensity, elapsedTime / duration);
            bloomComponent.intensity.value = currentIntensity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the intensity is set to the original value
        bloomComponent.intensity.value = startIntensity;
        Debug.Log("Bloom intensity increased and decreased gradually");
    }

    //public Vector3 Pickposition;
    //public Vector3 PickRotation;

    //public void UseItem()
    //{
    //    transform.localPosition = Pickposition;
    //    transform.localEulerAngles = PickRotation;
    //}

    float NearView() // it is true if you are near an interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        float maxDistance = 2f; // Define the maximum distance for the ray
        if (angleView < 10f && distance < maxDistance)
            return maxDistance; // Returning the maximum distance for the ray
        else
            return 0f; // Return 0 if the conditions are not met
    }
}

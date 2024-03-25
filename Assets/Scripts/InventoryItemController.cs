using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using FMODUnity;

public class InventoryItemController : MonoBehaviour
{
    public TMP_Text itemQuantityText; // Reference to the UI text displaying item quantity
    public Item item;
    public Transform ObjectGrabPoint; // Reference to the ObjectGrabPoint GameObject

    private int itemQuantity = 0; // Keep track of the item quantity

    public bool RedKey = false, BlueKey = false, Facemask = false;

    public Volume globalVolume;
    public VolumeProfile volumeProfile;

    private Bloom bloomComponent; // Reference to the Bloom effect
    private float originalBloomIntensity; // Store the original Bloom intensity

    public HUD hud;



    void Start()
    {
        volumeProfile = globalVolume.profile;

        if (volumeProfile.TryGet(out bloomComponent))
        {
            // Save the original intensity value
            originalBloomIntensity = bloomComponent.intensity.value;
        }
    }

    public void RemoveItem()
    {
        hud.SetNextQuest(8);
        InventoryManager.Instance.Remove(item);
        SpawnDroppedItem();
        Facemask = false;
        //Destroy(gameObject);
    }

    private void SpawnDroppedItem()
    {
        if (item != null)
        {
            // Spawn the dropped item at the ObjectGrabPoint position
            if (item.prefab != null)
            {
                Debug.Log("Prefab found. Spawning...");
                GameObject spawnedItem = Instantiate(item.prefab.gameObject, GetSpawnPosition(), Quaternion.identity);
                // Set the name of the spawned item to match the original item's name
                spawnedItem.name = item.itemName;
                Debug.Log("Spawned item at " + spawnedItem.transform.position);
            }
            else
            {
                Debug.LogError("Prefab is null. Make sure the prefab is assigned in the Item scriptable object.");
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        if (ObjectGrabPoint != null)
        {
            return ObjectGrabPoint.position; // Use ObjectGrabPoint position as spawn position
        }
        else
        {
            Debug.LogError("ObjectGrabPoint is not assigned. Make sure to assign the ObjectGrabPoint GameObject.");
            return transform.position; // Fallback to the current position if ObjectGrabPoint is not assigned
        }
    }

    public void AddItem(Item newItem)
    {
        if (item != null && item.IsStackable && item.itemName == newItem.itemName)
        {
            itemQuantity++;
            //UpdateItemQuantityText();
        }
        else
        {
            item = newItem;
            itemQuantity = 1;
            itemQuantityText.gameObject.SetActive(false);
            //UpdateItemQuantityText();
            if (item != null && item.itemName == "k¾úè" && item.itemType == ItemType.Key)
            {
                RedKey = true;
            }
            if (item != null && item.itemName == "rúško")
            {
                Facemask = true;
                hud.SetNextQuest(12);
            }
        }
    }

    //public FirstPersonController playerController;

    //public void DrinkBeer()
    //{
    //    HUD hud = FindObjectOfType<HUD>();
    //    float timeToAdd = 30f;

    //    if (item != null && (item.itemName == "pivo" || item.itemName == "plechovica piva") && item.itemType == ItemType.Consumable)
    //    {
    //        if (hud != null)
    //        {
    //            hud.AddTimeToTimer(timeToAdd);
    //            //InventoryManager.Instance.Remove(item);
    //            AudioManager.instance.PlayOneShot(FMODEvents.instance.UseSound, ObjectGrabPoint.transform.position);
    //            Destroy(gameObject);
    //            Debug.Log("Drink");
    //        }
    //    }
    //}

    //public void EatPills()
    //{
    //    HUD hud = FindObjectOfType<HUD>();
    //    float timeToAdd = 20f;
    //    float bloomIntensityIncrease = 1.5f;
    //    float bloomEffectDuration = 20f;

    //    if (item != null && item.itemName == "tabletky" && item.itemType == ItemType.Consumable)
    //    {
    //        hud.AddTimeToTimer(timeToAdd);
    //        hud.TimeController();

    //        // Adjust Bloom intensity when eating pills
    //        if (volumeProfile.TryGet(out bloomComponent))
    //        {
    //            // Calculate the target intensity
    //            float targetIntensity = originalBloomIntensity + bloomIntensityIncrease;

    //            StartCoroutine(ChangeBloomIntensity(originalBloomIntensity, targetIntensity, bloomEffectDuration));
    //        }
    //        AudioManager.instance.PlayOneShot(FMODEvents.instance.UseSound, ObjectGrabPoint.transform.position);
    //        InventoryManager.Instance.Remove(item);
    //        Destroy(gameObject);
    //        Debug.Log("Bloom intensity increased gradually");
    //    }
    //}

    //public IEnumerator ChangeBloomIntensity(float startIntensity, float targetIntensity, float duration)
    //{
    //    float elapsedTime = 0f;
    //    float currentIntensity = startIntensity;

    //    while (elapsedTime < duration)
    //    {
    //        // Adjust intensity smoothly
    //        currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
    //        bloomComponent.intensity.value = currentIntensity;

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Ensure the intensity is set to the target value
    //    bloomComponent.intensity.value = targetIntensity;

    //    yield return new WaitForSeconds(1f); // Wait for a moment at the increased intensity

    //    // Gradual decrease
    //    elapsedTime = 0f;

    //    while (elapsedTime < duration)
    //    {
    //        // Adjust intensity smoothly
    //        currentIntensity = Mathf.Lerp(targetIntensity, startIntensity, elapsedTime / duration);
    //        bloomComponent.intensity.value = currentIntensity;

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Ensure the intensity is set to the original value
    //    bloomComponent.intensity.value = startIntensity;
    //    Debug.Log("Bloom intensity increased and decreased gradually");
    //}

    //private void UpdateItemQuantityText()
    //{
    //    if (itemQuantityText != null)
    //    {
    //        itemQuantityText.text = itemQuantity.ToString();
    //    }
    //}
}
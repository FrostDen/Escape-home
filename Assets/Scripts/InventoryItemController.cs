using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour
{
    public TMP_Text itemQuantityText; // Reference to the UI text displaying item quantity
    public Item item;
    public Transform ObjectGrabPoint; // Reference to the ObjectGrabPoint GameObject

    private int itemQuantity = 0; // Keep track of the item quantity

    public bool RedKey = false, BlueKey = false;

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        SpawnDroppedItem();
        Destroy(gameObject);
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
                UpdateItemQuantityText();
            }
            else
            {
                item = newItem;
                itemQuantity = 1;
                itemQuantityText.gameObject.SetActive(false);
                UpdateItemQuantityText();
                if (item != null && item.itemName == "Key" && item.itemType == ItemType.Key) 
                {
                RedKey = true;
                }
            }
    }

    public float timeToAdd = 30f;

    public void DrinkBeer()
    {
        HUD hud = FindObjectOfType<HUD>();
        if (item != null && item.itemName == "Beer" && item.itemType == ItemType.Consumable)
        {
            if (hud != null)
            {
                hud.AddTimeToTimer(timeToAdd);
                InventoryManager.Instance.Remove(item);
                Destroy(gameObject);
            }
        }
    }

    private void UpdateItemQuantityText()
    {
        if (itemQuantityText != null)
        {
            itemQuantityText.text = itemQuantity.ToString();
        }
    }
}

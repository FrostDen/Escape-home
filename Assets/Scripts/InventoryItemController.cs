using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{

    [SerializeField] private List<Item> Items = new List<Item>();

    Item Item;

    public Button RemoveItemBtn;

    //public void RemoveItem()
    //{
    //    InventoryManager.Instance.Remove(item);
    //    Destroy(gameObject);
    //}

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(Item);
        SpawnDroppedItem();
        Destroy(gameObject);
        Debug.Log("Removed! Means its working.");
    }

    private void SpawnDroppedItem()
    {
        if (Item != null)
        {
            // Spawn the dropped item in front of the player
            if (Item.prefab != null)
            {
                GameObject itemPrefabObject = Item.prefab.gameObject;
                if (itemPrefabObject != null)
                {
                    Debug.Log("Prefab found. Spawning...");
                    GameObject spawnedItem = Instantiate(itemPrefabObject, GetSpawnPosition(), Quaternion.identity);
                    Debug.Log("Spawned item at " + spawnedItem.transform.position);
                }
                else
                {
                    Debug.LogError("Prefab object is null. Make sure the prefab is assigned.");
                }
            }
            else
            {
                Debug.LogError("Prefab is null. Make sure the prefab is assigned in the Item scriptable object.");
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // Adjust the spawn position to be in front of the player
        Vector3 spawnPosition = transform.position + transform.forward * 2f; // 2 units in front of the player
        return spawnPosition;
    }




    public void AddItem(Item newItem)
    {
        Item = newItem;
    }

}

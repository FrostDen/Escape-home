using System;
using UnityEngine;

public class Item3DViewer : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    private Transform itemPrefab;

    private void OnEnable()
    {
        if (inventoryManager == null)
        {
            inventoryManager = InventoryManager.Instance;
        }

        if (inventoryManager != null)
        {
            inventoryManager.OnItemSelected += InventoryManager_OnItemSelected;
        }
    }

    private void OnDisable()
    {
        if (inventoryManager != null)
        {
            inventoryManager.OnItemSelected -= InventoryManager_OnItemSelected;
        }
    }

    private void InventoryManager_OnItemSelected(object sender, Item item)
    {
        if (itemPrefab != null)
        {
            Destroy(itemPrefab.gameObject);
        }
        itemPrefab = Instantiate(item.prefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
        Debug.Log("Selected item: " + item.itemName);
    }
}

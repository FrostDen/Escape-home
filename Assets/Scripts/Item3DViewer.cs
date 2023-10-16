using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3DViewer : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    private Transform itemPrefab;
    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        inventoryManager.OnItemSelected += InventoryManager_OnItemSelected;
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

    //private void OnDestroy()
    //{
    //    if (inventoryManager != null)
    //    {
    //        inventoryManager.OnItemSelected -= InventoryManager_OnItemSelected;
    //    }
    //}
}

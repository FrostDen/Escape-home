using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager_backup : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private bool isInventoryOpen = false; 

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen; 

            ItemContent.gameObject.SetActive(isInventoryOpen);

            Cursor.visible = isInventoryOpen;
            Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

            //LockCameraRotation(isInventoryOpen);
            ListItems();
        }
    }

    private void Awake()
    {
        //Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }
    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach ( Item item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            TextMeshProUGUI itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();
            //var itemName = obj.transform.Find("itemName").GetComponent<Text>(); //TextMeshProUGUI
            var itemIcon = obj.transform.Find("itemIcon").GetComponent<Image>();

            if (itemName != null)
            {
                itemName.text = item.itemName;
            }
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
            }

        }
    }

    // private void LockCameraRotation(bool isLocked)
    //{
    //    // Get the main camera (you might need to adjust this based on your setup)
    //    Camera mainCamera = Camera.main;

    //    if (mainCamera != null)
    //    {
    //        // Lock or unlock camera rotation based on the flag
    //        mainCamera.GetComponent<FirstPersonController>().enabled = !isLocked;
    //    }
    //}
}

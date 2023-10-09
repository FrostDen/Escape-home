using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
   
    public static InventoryManager Instance;
    [SerializeField] private List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Toggle EnableRemove;

    public InventoryItemController[] InventoryItems; 

    private bool isInventoryOpen = false; 

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen; 

            ItemContent.gameObject.SetActive(isInventoryOpen);

            Cursor.visible = isInventoryOpen;
            Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

            LockCameraRotation(isInventoryOpen);
            ListItems();
        }
    }

    private void Awake()
    {
        Instance = this;
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

        foreach (Item item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            TextMeshProUGUI itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();
            //var itemName = obj.transform.Find("itemName").GetComponent<Text>(); //TextMeshProUGUI //TMP_text
            var itemIcon = obj.transform.Find("itemIcon").GetComponent<Image>();

            var removeItemButton = obj.transform.Find("RemoveItemBtn").GetComponent<Button>();

            if (itemName != null)
            {
                itemName.text = item.itemName;
            }
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
            }
            if (EnableRemove.isOn)
                removeItemButton.gameObject.SetActive(true);

            SetInventoryItems();

        }
    }
    public void EnableItemsRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveItemBtn").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveItemBtn").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (i < Items.Count)
            {
                InventoryItems[i].AddItem(Items[i]);
            }
            else
            {
                InventoryItems[i].AddItem(null);
            }
        }
    }


    public Camera mainCamera;

    private void LockCameraRotation(bool isLocked)
    {
       
        if (mainCamera != null)
        {
            mainCamera.GetComponent<FirstPersonController>().enabled = !isLocked;
        }
    }
}

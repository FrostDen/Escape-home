using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    public event EventHandler<Item> OnItemSelected;

    public static InventoryManager Instance;
    [SerializeField] public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    private Dictionary<Item, Transform> itemTransformDic;
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
            EnableRemove.gameObject.SetActive(isInventoryOpen);
            //Item3DViewer.gameObject.SetActive(isInventoryOpen);

            Cursor.visible = isInventoryOpen;
            Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

            LockCameraRotation(isInventoryOpen);

            ListItems();
        }
    }

    private void Awake()
    {
        //ItemContent = transform.Find("Content");
        ItemContent.gameObject.SetActive(false);

        itemTransformDic = new Dictionary<Item, Transform>();

        foreach (Item item in Items)
        {
            Transform itemTransform = Instantiate(ItemContent, transform);
            itemTransform.gameObject.SetActive(true);
            ItemContent.Find("Image").GetComponent<Image>().sprite = item.icon;

            itemTransformDic[item] = itemTransform;

            itemTransform.GetComponent<Button>().onClick.AddListener(() =>
            { });
        }

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

    public void RemoveItem()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
    }

    public void ListItems()
    {
        RemoveItem();
        foreach (Item item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            TextMeshProUGUI itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();

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

    private void SelectItem(Item selectedItem)
    {
        foreach (Item item in itemTransformDic.Keys)
        {
            itemTransformDic[item].Find("Selected").gameObject.SetActive(false);
        }

        itemTransformDic[selectedItem].Find("Selected").gameObject.SetActive(true);

        OnItemSelected?.Invoke(this, selectedItem);
    }

    public GameObject playerObject; // Make sure to assign this in the Inspector

    private void LockCameraRotation(bool isLocked)
    {
        if (playerObject != null)
        {
            MonoBehaviour[] scripts = playerObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script.GetType().Name.Equals("FirstPersonController"))
                {
                    script.enabled = !isLocked;
                    break;
                }
            }
        }
    }
}

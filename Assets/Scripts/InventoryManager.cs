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

    public InventoryItemController[] InventoryItems; 

    private bool isInventoryOpen = false;


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;

            ItemContent.gameObject.SetActive(isInventoryOpen);
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

            itemTransform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectItem(item));
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
            InventoryItemController itemController = obj.GetComponent<InventoryItemController>();
            itemController.AddItem(item);
            TextMeshProUGUI itemName = obj.transform.Find("itemName")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI itemQuantity = obj.transform.Find("itemQuantityText")?.GetComponent<TextMeshProUGUI>();

            var itemIcon = obj.transform.Find("itemIcon").GetComponent<Image>();
            
            if (itemName != null)
            {
                itemName.text = item.itemName;
            }
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
            }
            //SetInventoryItems();
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

    #region Inspect item in inventory
    private void SelectItem(Item selectedItem)
    {
        foreach (Item item in itemTransformDic.Keys)
        {
            itemTransformDic[item].Find("Selected").gameObject.SetActive(false);
        }

        if (OnItemSelected != null)  // Add this null check
        {
            OnItemSelected.Invoke(this, selectedItem);
        }
    }
    #endregion

    public GameObject playerObject; // Make sure to assign this in the Inspector

    public void LockCameraRotation(bool isLocked)
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

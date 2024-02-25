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

    public bool isInventoryOpen = false;

    private Vector3 hiddenPosition;
    private bool hasInitialized = false; // Flag to track initialization


    private void Start()
    {
        if (!hasInitialized)
        {
            InitializeInventory();
            hasInitialized = true;
        }
    }

    private void InitializeInventory()
    {
        hiddenPosition = new Vector3(Screen.width * 2, 0, 0); // Calculate off-screen position
        ItemContent.localPosition = hiddenPosition; // Initialize inventory panel off-screen

        // Other initialization logic
        // ...
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape) && isInventoryOpen)
        {
            isInventoryOpen = !isInventoryOpen;
            ToggleInventoryPanel();
        //Item3DViewer.gameObject.SetActive(isInventoryOpen);

            Cursor.visible = isInventoryOpen;
            Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

            LockCameraRotation(isInventoryOpen);

            ListItems();
        }
    }

    public void ToggleInventoryPanel()
    {
        ItemContent.localPosition = isInventoryOpen ? Vector3.zero : hiddenPosition;
    }

    private void Awake()
    {
        hiddenPosition = new Vector3(Screen.width * 2, 0, 0); // Calculate off-screen position

        ItemContent.localPosition = hiddenPosition; // Initialize inventory panel off-screen

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

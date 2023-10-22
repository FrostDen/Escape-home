using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public Item item;

    public bool isRedKey = true;

    InventoryItemController inventoryItemController;

    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    public float pickUpDistance = 2f; // Adjust the pick up distance as needed

    private void Start()
    {
        inventoryItemController = FindObjectOfType<InventoryItemController>(); // key will get up and it will saved in "inventary"
    }

    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        InventoryManager.Instance.ListItems();
        if (isRedKey) inventoryItemController.RedKey = true;
        else inventoryItemController.BlueKey = true;
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= pickUpDistance)
        {
            if ((Input.GetMouseButtonDown(0)))
            {
                Pickup();
            }
        }
    }

    public Vector3 Pickposition;
    public Vector3 PickRotation;

    public void UseItem()
    {
        transform.localPosition = Pickposition;
        transform.localEulerAngles = PickRotation;
    }
}

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

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

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
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= NearView())
        {
            if (Input.GetMouseButtonDown(0))
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
    float NearView() // it is true if you are near an interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        float maxDistance = 2f; // Define the maximum distance for the ray
        if (angleView < 10f && distance < maxDistance)
            return maxDistance; // Returning the maximum distance for the ray
        else
            return 0f; // Return 0 if the conditions are not met
    }
}

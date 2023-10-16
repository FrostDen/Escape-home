using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public Item item;

    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    public float pickUpDistance = 2f; // Adjust the pick up distance as needed

    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        InventoryManager.Instance.ListItems();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerCameraTransform.position) <= pickUpDistance)
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

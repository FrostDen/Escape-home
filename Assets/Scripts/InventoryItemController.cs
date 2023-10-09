using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    
    [SerializeField] private List<Item> Items = new List<Item>();

    Item item;

    public Button RemoveItemBtn;

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    //public void UseItem()
    //{
    //    switch (item.itemType)
    //    {
    //        case item.itemType.Consumable:
    //            Timer.Instance.UseConsumable(); 
    //            break;
    //    }
    //    RemoveItem();
    //}
}

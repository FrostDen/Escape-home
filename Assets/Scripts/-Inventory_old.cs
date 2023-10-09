using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item_old> itemList;

    public Inventory()
    {
        itemList = new List<Item_old>();

        AddItem(new Item_old { itemType = Item_old.ItemType.Beer, amount = 1 });
		AddItem(new Item_old { itemType = Item_old.ItemType.Key, amount = 1 });
        AddItem(new Item_old { itemType = Item_old.ItemType.Facemask, amount = 1 });
        Debug.Log(itemList.Count);
    }

    public void AddItem(Item_old item)
    {
           itemList.Add(item);
    }

    public List<Item_old> GetItemList()
    {
        return itemList;
    }
}

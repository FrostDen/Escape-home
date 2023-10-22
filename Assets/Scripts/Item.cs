using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int amount = 1;
    public int value;
    public Sprite icon;
    public Transform prefab;
    public ItemType itemType;
    public bool IsStackable;
}

public enum ItemType
{
    Consumable,
    Object,
    Key
}
 
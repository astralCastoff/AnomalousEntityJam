using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "Item Name";
    [TextArea(3, 30)] public string itemDescription = string.Empty;
    public int saleValue = 100;
    public Texture2D inventorySprite;
    public bool canBePickedUp = true;
}

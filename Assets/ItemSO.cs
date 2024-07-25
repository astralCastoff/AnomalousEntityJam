using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item", order = 0, fileName = "Item")]
public class ItemSO : ScriptableObject
{
    //[Range(0.1f, 100f)] public float m_MaxSpeed = 0.1f;

    public string itemName = "Item Name";
    [TextArea(3, 30)] public string itemDescription = string.Empty;
    public int saleValue = 100;
    public Sprite inventorySprite;
}
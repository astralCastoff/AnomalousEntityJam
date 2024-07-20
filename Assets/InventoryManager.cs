using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform ItemPickupAreaOrigin;
    [SerializeField] float itemPickupRadius;
    [SerializeField] LayerMask itemsLayer;

    public Item[] items = new Item[24];

    [SerializeField] GameObject InventoryUI;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUI.SetActive(!InventoryUI.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] temp = Physics.OverlapSphere(ItemPickupAreaOrigin.position, itemPickupRadius, itemsLayer);
            float lowestDistance = Mathf.Infinity;
            GameObject closestItem = null;

            foreach (Collider c in temp)
            {
                float distance = Vector3.Distance(c.transform.position, ItemPickupAreaOrigin.position);
                if (distance < lowestDistance && c.GetComponent<Item>().canBePickedUp)
                {
                    lowestDistance = distance;
                    closestItem = c.gameObject;
                }
            }

            if (closestItem != null)
            {
                Debug.Log("not null");
                for (int i = 0; i < items.Length; i++)
                {
                    Debug.Log("iteration");
                    if (items[i] == null)
                    {
                        Debug.Log("found null spot");
                        items[i] = closestItem.GetComponent<Item>();
                        PickUpItem(closestItem);
                        //closestItem.SetActive(false);
                        break;
                    }
                }
            }
            closestItem = null;
        }
    }

    [SerializeField] float itemPickupDuration = 1f;
    [SerializeField] Transform itemPickupDestination;
    [SerializeField] AudioSource pickupSoundEffect;
    void PickUpItem(GameObject _item)//make a method that is activated when I'm done picking the item up to disable its renderer and set its scale back to 1
    {
        _item.GetComponent<Item>().canBePickedUp = false;
        pickupSoundEffect.Play();
        _item.transform.DOScale(0f, itemPickupDuration);
        _item.transform.DOMove(itemPickupDestination.position, itemPickupDuration); 
    }

    
}

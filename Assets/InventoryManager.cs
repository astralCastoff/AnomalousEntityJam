using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform ItemPickupAreaOrigin;
    [SerializeField] float itemPickupRadius;
    [SerializeField] LayerMask itemsLayer;

    public Item[] items = new Item[24];
    public Image[] itemIcons = new Image[24];

    [SerializeField] RectTransform itemSelectionImage;
    public Image selectedItemIcon;
    [SerializeField] Text selectedItemName;
    [SerializeField] Text selectedItemDescription;
    int itemSelectionIndex = 0;
    const int MAX_ITEM_INDEX = 23;
    const int ITEM_ROW_WIDTH = 6;

    [SerializeField] GameObject InventoryUI;


    private void Start()
    {
        UpdateInventoryUI();
        ChangeSelectedItem(0);
        MoveSelectionCursorToCurrentSelection(itemIcons[0].rectTransform);
    }

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

            if (closestItem != null && closestItem.GetComponent<Item>().canBePickedUp && InventoryNotFull())
            {
                PickUpItem(closestItem);
            }
            closestItem = null;
        }

        if (InventoryUI.activeInHierarchy)
        {
            int indexAtStartOfFrame = itemSelectionIndex;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (itemSelectionIndex > 0)
                {
                    itemSelectionIndex--;
                }
                else
                {
                    itemSelectionIndex = MAX_ITEM_INDEX;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (itemSelectionIndex < MAX_ITEM_INDEX)
                {
                    itemSelectionIndex++;
                }
                else
                {
                    itemSelectionIndex = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (itemSelectionIndex + ITEM_ROW_WIDTH <= MAX_ITEM_INDEX)
                {
                    itemSelectionIndex += ITEM_ROW_WIDTH;
                }
                else
                {
                    itemSelectionIndex = ((itemSelectionIndex + ITEM_ROW_WIDTH) - MAX_ITEM_INDEX)-1;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (itemSelectionIndex - ITEM_ROW_WIDTH >= 0)
                {
                    itemSelectionIndex -= ITEM_ROW_WIDTH;
                }
                else
                {
                    itemSelectionIndex = ((itemSelectionIndex - ITEM_ROW_WIDTH) + MAX_ITEM_INDEX)+1;
                }
            }

            if (indexAtStartOfFrame != itemSelectionIndex)
            {
                ChangeSelectedItem(itemSelectionIndex);
                MoveSelectionCursorToCurrentSelection(itemIcons[itemSelectionIndex].rectTransform);
            }
            
        }
    }

    void MoveSelectionCursorToCurrentSelection(RectTransform _target)
    {
        itemSelectionImage.position = _target.position;
        itemSelectionImage.Translate(0, 7, 0);
    }

    bool InventoryNotFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    int FirstEmptyInventorySlotIndex()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        return -1; //i hope this never happens
    }

    [SerializeField] float itemPickupDuration = 1f;
    [SerializeField] Transform itemPickupDestination;
    [SerializeField] AudioSource pickupSoundEffect;
    void PickUpItem(GameObject _item)
    {
        items[FirstEmptyInventorySlotIndex()] = _item.GetComponent<Item>();
        _item.GetComponent<Item>().canBePickedUp = false;
        pickupSoundEffect.Play();
        _item.transform.DOScale(0f, itemPickupDuration);
        _item.transform.DOMove(itemPickupDestination.position, itemPickupDuration);
        StartCoroutine(WhenFinishedTweening(_item));
    }

    Vector3 itemStoragePosition = new Vector3(0, -20f, 0);
    [SerializeField] Transform itemStorage;
    IEnumerator WhenFinishedTweening(GameObject _item) //technically this is a "bad" way to do things and it would be better to make this be called directly by a tween callback instead of simply delaying by the amount of time it should take the tween to finish but I really don't think it matters
    {
        yield return new WaitForSeconds(itemPickupDuration);
        _item.SetActive(false);
        _item.transform.localScale = Vector3.one;
        _item.transform.parent = itemStorage;
        _item.transform.localPosition = itemStoragePosition;

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < itemIcons.Length; i++)
        {
            if (items[i] == null)
            {
                itemIcons[i].sprite = null;
                itemIcons[i].enabled = false;
            }
            else
            {
                itemIcons[i].enabled = true;
                itemIcons[i].sprite = items[i].itemScriptableObject.inventorySprite;
            }
        }
        ChangeSelectedItem(itemSelectionIndex);
    }

    public void ChangeSelectedItem(int _index)
    {
        if (items[_index] == null)
        {
            selectedItemIcon.sprite = null;
            selectedItemIcon.color = new Color(0,0,0,0);

            selectedItemName.text = "";
            selectedItemDescription.text = "";

            return;
        }
        else
        {
            ItemSO _itemSO = items[_index].itemScriptableObject;

            selectedItemIcon.sprite = _itemSO.inventorySprite;
            selectedItemIcon.color = Color.white;

            selectedItemName.text = _itemSO.itemName;
            selectedItemDescription.text = _itemSO.itemDescription;
        }
    }

}

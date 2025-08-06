using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryHelper : Singleton<InventoryHelper>
{
    private InventoryDataItem[] inventoryDataItems;
    [SerializeField] public TextMeshProUGUI coinWonText;

    private Dictionary<InventoryType, List<Action<int>>> listeners = new Dictionary<InventoryType, List<Action<int>>>();

    void Start()
    {
        inventoryDataItems = GetComponentsInChildren<InventoryDataItem>();
        coinWonText.gameObject.SetActive(false);
    }

    public int GetQuantity(InventoryType itemType)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(data => data.itemType == itemType);
        if (item != null)
        {
            return item.quantity;
        }

        return 0;
    }

    public void SetQuantity(InventoryType itemType, int quantityToSet)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(data => data.itemType == itemType);
        if (item != null)
        {
            SetQuantity(item, quantityToSet);
        }
        else
        {
            InventoryDataItem newItem = gameObject.AddComponent<InventoryDataItem>();
            newItem.itemType = itemType;
            newItem.quantity = quantityToSet;
            listeners[itemType] = new List<Action<int>>();
        }
    }

    // use only when inventoryData is already pulled
    public void SetQuantity(InventoryDataItem item, int quantityToSet)
    {
        item.quantity = quantityToSet;
        Trigger(item.itemType, item.quantity);
    }
    
    public void AddItem(InventoryType itemType, int quantityToAdd)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(data => data.itemType == itemType);
        
        if (item != null)
        {
            SetQuantity(item, item.quantity + quantityToAdd);
        }
        else
        {
            InventoryDataItem newItem = gameObject.AddComponent<InventoryDataItem>();
            newItem.itemType = itemType;
            newItem.quantity = quantityToAdd;
        }
    }

    public void RemoveItem(InventoryType itemType, int quantityToRemove)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(data => data.itemType == itemType);
            
            if (item != null)
            {
                RemoveItem(item, quantityToRemove);
            }
            else
            {
                Debug.LogWarning("don't have this item");
            }
    }

    //only use when inventoryData is already pulled
    public void RemoveItem(InventoryDataItem item, int quantityToRemove)
    {
        item.quantity -= quantityToRemove;
        Trigger(item.itemType, item.quantity);
    }

    public void ResetOnLost(InventoryType itemType)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(item => item.itemType == itemType);

        if (item != null)
        {
            SetQuantity(item, item.quantityOnLevelStart);
        }
    }

    public void OnWin(InventoryType itemType)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(item => item.itemType == itemType);

        if (item != null)
        {
            item.quantityOnLevelStart = item.quantity;
        }
        else
        {
            Debug.Log("Coinless level ? Shouldnt be possible at this point");
        }
    }

    public void ShowCountOnLevelWon(InventoryType itemType)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(item => item.itemType == itemType);
        if (item != null)
        {
            int coinsWon = item.quantity - item.quantityOnLevelStart;
            coinWonText.text = $"You won {coinsWon.ToString()} coins!";
            coinWonText.gameObject.SetActive(true);
        }
    }

    public bool TrySpend(InventoryType itemType, int quantityToSpend)
    {
        InventoryDataItem item = inventoryDataItems.FirstOrDefault(data => data.itemType == itemType);

        if (item != null)
        {
            if (item.quantity >= quantityToSpend)
            {
                RemoveItem(item, quantityToSpend);
                return true;
            }
        }
        Debug.Log("Not enough items");
        return false;
    }
    
    public void AddListener(InventoryType itemType, Action<int> listener)
    {
        if (!listeners.ContainsKey(itemType))
        {
            listeners[itemType] = new List<Action<int>>();
        }
        listeners[itemType].Add(listener);
    }
    
    public void RemoveListener(InventoryType itemType, Action<int> listener)
    {
        if (listeners.ContainsKey(itemType))
        {
            listeners[itemType].Remove(listener);
            if (listeners[itemType].Count == 0)
            {
                listeners.Remove(itemType);
            }
        }
    }

    public void Trigger(InventoryType itemType, int quantity)
    {
        var _listeners = listeners[itemType];
        if (_listeners != null)
        {
            foreach(Action<int> listener in _listeners)
            {
                listener.Invoke(quantity);
            }
        }
    }

}

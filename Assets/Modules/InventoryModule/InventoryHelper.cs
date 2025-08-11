using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryHelper : Singleton<InventoryHelper>
{
    private Dictionary<InventoryType, List<Action<int>>> listeners = new Dictionary<InventoryType, List<Action<int>>>();

    private List<InventoryDataItem> _inventoryData;

    public List<InventoryDataItem> inventoryData
    {
        get
        {
            if (_inventoryData == null)
            {
                _inventoryData = Resources.Load<InventoryScriptableObjectScript>("CoinInventory").inventoryDataItems;
            }
            return _inventoryData;
        }
    }


    public int GetQuantity(InventoryType itemType)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
        if (item != null)
        {
            return item.quantity;
        }

        return 0;
    }

    public void SetQuantity(InventoryType itemType, int quantityToSet)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
        if (item != null)
        {
            SetQuantity(item, quantityToSet);
        }
        else
        {
            InventoryDataItem newItem = new InventoryDataItem();
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
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
        
        if (item != null)
        {
            SetQuantity(item, item.quantity + quantityToAdd);
        }
        else
        {
            InventoryDataItem newItem = new InventoryDataItem();
            newItem.itemType = itemType;
            newItem.quantity = quantityToAdd;
            listeners[itemType] = new List<Action<int>>();
        }
    }

    public void RemoveItem(InventoryType itemType, int quantityToRemove)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);
            
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
        InventoryDataItem item = inventoryData.FirstOrDefault(item => item.itemType == itemType);

        if (item != null)
        {
            SetQuantity(item, item.quantity);
        }
    }

    public bool TrySpend(InventoryType itemType, int quantityToSpend)
    {
        InventoryDataItem item = inventoryData.FirstOrDefault(data => data.itemType == itemType);

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

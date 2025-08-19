using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryScriptableObject", menuName = "ScriptableObjects/InventoryScriptableObject")]

public class InventoryScriptableObjectScript : ScriptableObject
{
    public List<InventoryDataItem> inventoryDataItems = new();
}

[SerializeField]
public class InventoryDataItem
{
    public InventoryType itemType;
    public int quantity;
}
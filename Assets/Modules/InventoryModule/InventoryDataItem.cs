using UnityEngine;

public class InventoryDataItem : MonoBehaviour
{
    public InventoryType itemType;
    public int quantity;
    public int quantityOnLevelStart;
    public InventoryScriptableObjectScript inventoryScriptableObject;

    void Awake()
    {
        itemType = inventoryScriptableObject.inventoryType;
        quantity = inventoryScriptableObject.quantity;
        quantityOnLevelStart = quantity;
    }
}

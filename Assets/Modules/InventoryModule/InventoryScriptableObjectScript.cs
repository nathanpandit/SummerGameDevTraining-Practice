using UnityEngine;

[CreateAssetMenu(fileName = "InventoryScriptableObject", menuName = "ScriptableObjects/InventoryScriptableObject")]

public class InventoryScriptableObjectScript : ScriptableObject
{
    public InventoryType inventoryType;
    public int quantity;
}
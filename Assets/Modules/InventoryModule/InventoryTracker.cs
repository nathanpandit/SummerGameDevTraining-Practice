using UnityEngine;

public class InventoryTracker : MonoBehaviour
{
    [SerializeField] public InventoryType itemType;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] public InventoryScriptableObjectScript inventoryScriptableObject;

    void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        text.text = inventoryScriptableObject.quantity.ToString();
        InventoryHelper.Instance().AddListener(itemType, UpdateText);
    }

    public void UpdateText(int quantity)
    {
        text.text = quantity.ToString();
    }
}

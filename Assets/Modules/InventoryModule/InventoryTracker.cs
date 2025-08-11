using UnityEngine;

public class InventoryTracker : MonoBehaviour
{
    [SerializeField] public InventoryType itemType;
    [SerializeField] private TMPro.TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        text.text = InventoryHelper.Instance().GetQuantity(itemType).ToString();
        InventoryHelper.Instance().AddListener(itemType, UpdateText);
    }

    public void UpdateText(int quantity)
    {
        text.text = quantity.ToString();
    }
}

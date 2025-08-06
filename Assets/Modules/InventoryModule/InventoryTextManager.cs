using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryTextManager : Singleton<InventoryTextManager>
{
    public TextMeshProUGUI quantityOfItemsWonText;
    public TextMeshProUGUI quantityOfItemsText;
    public TextMeshProUGUI quantityOfItemsTextTemp;
    
    public void ShowCountOnLevelWon(InventoryType itemType)
    {
        InventoryDataItem item = InventoryHelper.Instance().inventoryDataItems.FirstOrDefault(item => item.itemType == itemType);
        if (item != null)
        {
            int coinsWon = item.quantity - item.quantityOnLevelStart;
            quantityOfItemsWonText.text = coinsWon.ToString();
            quantityOfItemsWonText.gameObject.SetActive(true);
        }
    }

    public void GrantReward()
    {
        
    }


}

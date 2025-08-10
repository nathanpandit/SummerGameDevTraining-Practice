using UnityEngine;
using DG.Tweening;
using TMPro;

public class CurveController : Singleton<CurveController>
{
    [SerializeField] public GameObject objectToMove;
    [SerializeField] public GameObject objectToMovePrefab;
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform endPoint;
    [SerializeField] public Vector3 startPosition;
    [SerializeField] public Vector3 endPosition;
    [SerializeField] public float duration = 1f;
    [SerializeField] public TextMeshProUGUI totalCoinCountText;
    [SerializeField] public TextMeshProUGUI coinsWonCountText;
    public int numberOfMovements;

    public void Awake()
    {
        startPosition = startPoint != null ? startPoint.position : Vector3.zero;
        endPosition = endPoint != null ? endPoint.position : Vector3.zero;
        Debug.Log($"POS1 at awake: {startPosition}");
        Debug.Log($"POS2 at awake: {endPosition}");
    }
    public void MoveObjectAlongCurve()
    {
        if (objectToMove == null || startPoint == null || endPoint == null)
        {
            Debug.LogError("Object to move or start/end points are not set.");
            return;
        }

        GameObject newObject = Instantiate(objectToMovePrefab, startPosition, Quaternion.identity);
        newObject.transform.SetParent(transform, true);

        Vector3 controlPoint = (startPosition + endPosition) / 2 + Vector3.up * 2f; // Adjust the height of the curve

        for (int i = 0; i < numberOfMovements; i++)
        {
            int iterationIndex = i;
            GameObject objectInIteration = newObject;
            newObject.transform.DOPath(new Vector3[] { startPosition, controlPoint, endPosition },
                    duration, PathType.CatmullRom)
                .SetEase(Ease.InOutQuad)
                .SetDelay(i * 0.2f)
                .OnStart(() => coinsWonCountText.text =
                    (InventoryHelper.Instance().QuantityOfItemsWon(InventoryType.Coin) - (iterationIndex + 1)).ToString())
                .OnComplete(() => onCompleteTween(iterationIndex, objectInIteration));

            if (i != numberOfMovements - 1)
            {
                newObject = Instantiate(objectToMovePrefab, startPosition, Quaternion.identity);
                newObject.transform.SetParent(transform, true);
            }
        }
    }
    
    public void GrantReward()
    {
        numberOfMovements = InventoryTextManager.Instance().coinsWon;
        MoveObjectAlongCurve();
    }

    public void onCompleteTween(int i, GameObject gameObject)
    {
        Debug.Log(InventoryHelper.Instance().GetQuantityOnStart(InventoryType.Coin));
        Debug.Log(i);
        totalCoinCountText.text = (InventoryHelper.Instance().GetQuantityOnStart(InventoryType.Coin) + i + 1).ToString();
        Destroy(gameObject);
    }
}

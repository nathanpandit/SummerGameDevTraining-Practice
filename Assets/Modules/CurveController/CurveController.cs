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
            newObject.transform.DOPath(new Vector3[] { startPosition, controlPoint, endPosition },
                    duration, PathType.CatmullRom)
                .SetEase(Ease.InOutQuad)
                .SetDelay(i * 0.2f)
                .OnComplete(() => onCompleteTween(i, newObject));
            
            newObject = Instantiate(objectToMovePrefab, startPosition, Quaternion.identity);
            newObject.transform.SetParent(transform, true);
        }
    }
    
    public void GrantReward()
    {
        numberOfMovements = InventoryTextManager.Instance().coinsWon;
        MoveObjectAlongCurve();
    }

    public void onCompleteTween(int i, GameObject gameObject)
    {
        totalCoinCountText.text = (InventoryHelper.Instance().GetQuantityOnStart(InventoryType.Coin) + i + 1).ToString();
        Destroy(gameObject);
    }
}

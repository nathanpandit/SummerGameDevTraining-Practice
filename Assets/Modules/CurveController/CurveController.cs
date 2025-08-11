using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UfoPuzzle;

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
    [SerializeField] public AnimationCurve movementCurve;
    public int numberOfMovements;

    public void Awake()
    {
        startPosition = startPoint != null ? startPoint.position : Vector3.zero;
        endPosition = endPoint != null ? endPoint.position : Vector3.zero;
        Debug.Log($"POS1 at awake: {startPosition}");
        Debug.Log($"POS2 at awake: {endPosition}");
    }
    public void MoveObjectAlongCurve(Action onComplete = null)
    {
        if (objectToMove == null || startPoint == null || endPoint == null)
        {
            Debug.LogError("Object to move or start/end points are not set.");
            return;
        }

        GameObject newObject = Instantiate(objectToMovePrefab, startPosition, Quaternion.identity);
        newObject.transform.SetParent(transform, true);

        Vector3 controlPoint = (startPosition + endPosition) / 2 + Vector3.up * 2f; // Adjust the height of the curve

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < numberOfMovements; i++)
        {
            int iterationIndex = i;
            GameObject objectInIteration = newObject;
            seq.Insert(0.2f*i,newObject.transform.DOPath(new Vector3[] { startPosition, controlPoint, endPosition },
                    duration, PathType.CatmullRom)
                .SetEase(movementCurve)
                .OnStart(() => coinsWonCountText.text =
                    (GameManager.coinCount - (iterationIndex + 1)).ToString())
                .OnComplete(() => onCompleteTween(iterationIndex, objectInIteration)));

            if (i != numberOfMovements - 1)
            {
                newObject = Instantiate(objectToMovePrefab, startPosition, Quaternion.identity);
                newObject.transform.SetParent(transform, true);
            }
        }
        seq.OnComplete(() => 
        {
            Debug.Log("All movements completed.");
            onComplete?.Invoke();
        });
    }
    
    public void GrantReward(Action onComplete = null)
    {
        numberOfMovements = GameManager.coinCount;
        Debug.Log($"Number of movements: {numberOfMovements}");;
        MoveObjectAlongCurve(onComplete);
    }

    public void onCompleteTween(int i, GameObject gameObject)
    {
        totalCoinCountText.text = (InventoryHelper.Instance().GetQuantity(InventoryType.Coin) - GameManager.coinCount + i + 1).ToString();
        Destroy(gameObject);
    }
}

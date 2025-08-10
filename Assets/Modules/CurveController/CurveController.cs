using UnityEngine;
using DG.Tweening;

public class CurveController : MonoBehaviour
{
    [SerializeField] public GameObject objectToMove;
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform endPoint;
    [SerializeField] public float duration = 1f;
    public int numberOfMovements;
    
    public void MoveObjectAlongCurve()
    {
        if (objectToMove == null || startPoint == null || endPoint == null)
        {
            Debug.LogError("Object to move or start/end points are not set.");
            return;
        }

        Vector3 controlPoint = (startPoint.position + endPoint.position) / 2 + Vector3.up * 2f; // Adjust the height of the curve

        objectToMove.transform.DOPath(new Vector3[] { startPoint.position, controlPoint, endPoint.position }, duration, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Debug.Log("Movement along curve completed."));
    }
}
